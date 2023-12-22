using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;

using aclogview.ACE_Helpers;

namespace aclogview.Tools.Scrapers
{
    class PlayerExporter : Scraper
    {
        public override string Description => "Exports players and their inventory";

        // TODO: player.Character.CharacterPropertiesQuestRegistry
        // TODO: player.Character.HairTexture
        // TODO: player.Character.DefaultHairTexture

        class WorldObjectItem
        {
            public readonly Biota Biota = new Biota();
            public readonly string Name;

            public bool AppraiseInfoReceived;

            public WorldObjectItem(uint guid, string name)
            {
                Biota.Id = guid;
                Name = name;

                Biota.PropertiesPosition = new Dictionary<PositionType, PropertiesPosition>();

                Biota.PropertiesSpellBook = new Dictionary<int, float>();

                Biota.PropertiesAnimPart = new List<PropertiesAnimPart>();
                Biota.PropertiesPalette = new List<PropertiesPalette>();
                Biota.PropertiesTextureMap = new List<PropertiesTextureMap>();

                // Biota additions over Weenie
                Biota.PropertiesEnchantmentRegistry = new Collection<PropertiesEnchantmentRegistry>();
            }
        }

        class LoginEvent
        {
            public readonly string FileName;
            public readonly uint TSec;

            public readonly Biota Biota = new Biota();
            public readonly ACE.Database.Models.Shard.Character Character = new ACE.Database.Models.Shard.Character();

            public readonly List<(uint guid, uint containerProperties)> Inventory = new List<(uint guid, uint containerProperties)>();
            public readonly List<(uint guid, uint location, uint priority)> Equipment = new List<(uint guid, uint location, uint priority)>();

            public readonly Dictionary<uint, List<uint>> ViewContentsEvents = new Dictionary<uint, List<uint>>();
            public bool PlayerLoginCompleted;

            public readonly Dictionary<uint, WorldObjectItem> WorldObjects = new Dictionary<uint, WorldObjectItem>();

            public LoginEvent(string fileName, uint tsec, uint guid)
            {
                FileName = fileName;
                TSec = tsec;

                Biota.Id = guid;
                Character.Id = guid;

                Biota.PropertiesPosition = new Dictionary<PositionType, PropertiesPosition>();

                Biota.PropertiesSpellBook = new Dictionary<int, float>();

                Biota.PropertiesAnimPart = new List<PropertiesAnimPart>();
                Biota.PropertiesPalette = new List<PropertiesPalette>();
                Biota.PropertiesTextureMap = new List<PropertiesTextureMap>();

                // Properties for creatures
                Biota.PropertiesAttribute = new Dictionary<PropertyAttribute, PropertiesAttribute>();
                Biota.PropertiesAttribute2nd = new Dictionary<PropertyAttribute2nd, PropertiesAttribute2nd>();
                Biota.PropertiesBodyPart = new Dictionary<CombatBodyPart, PropertiesBodyPart>();
                Biota.PropertiesSkill = new Dictionary<ACE.Entity.Enum.Skill, PropertiesSkill>();

                // Biota additions over Weenie
                Biota.PropertiesEnchantmentRegistry = new Collection<PropertiesEnchantmentRegistry>();
            }

            public bool IsPossessedItem(uint guid)
            {
                foreach (var entry in Inventory)
                {
                    if (entry.guid == guid)
                        return true;

                    if (ViewContentsEvents.TryGetValue(entry.guid, out var value))
                    {
                        if (value.Contains(guid))
                            return true;
                    }
                }

                foreach (var entry in Equipment)
                {
                    if (entry.guid == guid)
                        return true;
                }

                return false;
            }
        }

        class PlayerLogins
        {
            public readonly List<LoginEvent> LoginEvents = new List<LoginEvent>();

            /// <summary>
            /// Searches all possessions to find the last one recorded that also has AppraiseInfoReceived
            /// </summary>
            public WorldObjectItem GetBestPossession(uint guid, string name = null)
            {
                WorldObjectItem best = null;

                var sortedLoginEvents = LoginEvents.OrderBy(r => r.TSec).ToList();

                foreach (var loginEvent in sortedLoginEvents)
                {
                    if (loginEvent.WorldObjects.TryGetValue(guid, out var woi))
                    {
                        if (woi.Name != name)
                            continue;

                        if (best == null || woi.AppraiseInfoReceived)
                            best = woi;
                    }
                }

                return best;
            }
        }

        private readonly Dictionary<string, Dictionary<uint, PlayerLogins>> playerLoginsByServer = new Dictionary<string, Dictionary<uint, PlayerLogins>>();

        class BiotaEx
        {
            public Position LastPosition;
            public uint LastPositionTSec;

            public CM_Physics.CreateObject LastCreateObject;
            public uint LastCreateObjectTSec;

            public CM_Examine.SetAppraiseInfo LastAppraisalProfile;
            public uint LastAppraisalProfileTSec;
        }

        private readonly Dictionary<string, Dictionary<uint, BiotaEx>> biotasByServer = new Dictionary<string, Dictionary<uint, BiotaEx>>();

        public override void Reset()
        {
            playerLoginsByServer.Clear();
            biotasByServer.Clear();
        }

        public override (int hits, int messageExceptions) ProcessFileRecords(string fileName, List<PacketRecord> records, ref bool searchAborted)
        {
            int hits = 0;
            int messageExceptions = 0;

            string serverName = "Unknown";

            LoginEvent loginEvent = null;

            var rwLock = new ReaderWriterLockSlim();

            // Determine the server name using the Server List.
            // This will be overriden if a Evt_Login__WorldInfo_ID is received
            if (records.Count > 0)
            {
                var servers = ServerList.FindBy(records[0].ipHeader, records[0].isSend);

                if (servers.Count == 1 && servers[0].IsRetail)
                    serverName = servers[0].Name;
            }

            foreach (PacketRecord record in records)
            {
                if (searchAborted)
                    return (hits, messageExceptions);

                try
                {
                    if (record.data.Length <= 4)
                        continue;

                    using (var memoryStream = new MemoryStream(record.data))
                    using (var binaryReader = new BinaryReader(memoryStream))
                    {
                        var messageCode = binaryReader.ReadUInt32();

                        if (messageCode == (uint)PacketOpcode.Evt_Login__WorldInfo_ID) // 0xF7E1
                        {
                            var message = CM_Login.WorldInfo.read(binaryReader);
                            serverName = message.strWorldName.m_buffer;
                            continue;
                        }

                        if (messageCode == (uint)PacketOpcode.CHARACTER_EXIT_GAME_EVENT) // 0xF653
                        {
                            loginEvent = null;
                            continue;
                        }

                        // This could be seen multiple times if the first time the player tries to enter, they get a "Your character is already in world" message
                        if (messageCode == (uint)PacketOpcode.CHARACTER_ENTER_GAME_EVENT) // 0xF657
                        {
                            var message = Proto_UI.EnterWorld.read(binaryReader);

                            loginEvent = new LoginEvent(fileName, record.tsSec, message.gid);
                            loginEvent.Biota.SetProperty(PropertyString.PCAPRecordedServerName, serverName, rwLock, out _);
                            continue;
                        }

                        if (messageCode == (uint)PacketOpcode.Evt_Physics__CreateObject_ID) // 0xF745
                        {
                            var message = CM_Physics.CreateObject.read(binaryReader);

                            if (loginEvent != null)
                            {
                                // We only process player create/update messages for player biotas during the login process
                                if (!loginEvent.PlayerLoginCompleted && message.object_id == loginEvent.Biota.Id)
                                {
                                    ACEBiotaCreator.Update(message, loginEvent.Biota, rwLock, true);

                                    var position = new ACE.Entity.Position(message.physicsdesc.pos.objcell_id, message.physicsdesc.pos.frame.m_fOrigin.x, message.physicsdesc.pos.frame.m_fOrigin.y, message.physicsdesc.pos.frame.m_fOrigin.z, message.physicsdesc.pos.frame.qx, message.physicsdesc.pos.frame.qy, message.physicsdesc.pos.frame.qz, message.physicsdesc.pos.frame.qw);
                                    loginEvent.Biota.SetPosition(PositionType.Location, position, rwLock);
                                }

                                // Record inventory items
                                if (!loginEvent.WorldObjects.ContainsKey(message.object_id) && loginEvent.IsPossessedItem(message.object_id))
                                {
                                    var item = new WorldObjectItem(message.object_id, message.wdesc._name.m_buffer);
                                    ACEBiotaCreator.Update(message, item.Biota, rwLock, true);
                                    loginEvent.WorldObjects[message.object_id] = item;
                                }
                            }

                            if (message.object_id >= 0x50000000 && message.object_id <= 0x5FFFFFFF) // Make sure it's a player GUID
                            {
                                // Update the global biota infos
                                lock (biotasByServer)
                                {
                                    if (!biotasByServer.TryGetValue(serverName, out var biotaServer))
                                    {
                                        biotaServer = new Dictionary<uint, BiotaEx>();
                                        biotasByServer[serverName] = biotaServer;
                                    }

                                    if (!biotaServer.TryGetValue(message.object_id, out var biotaEx))
                                    {
                                        biotaEx = new BiotaEx();
                                        biotaServer[message.object_id] = biotaEx;
                                    }

                                    if (biotaEx.LastCreateObjectTSec < record.tsSec)
                                    {
                                        biotaEx.LastCreateObject = message;
                                        biotaEx.LastCreateObjectTSec = record.tsSec;
                                    }
                                }
                            }

                            continue;
                        }

                        if (messageCode == (uint)PacketOpcode.Evt_Movement__UpdatePosition_ID) // 0xF748
                        {
                            var message = CM_Movement.UpdatePosition.read(binaryReader);

                            if (message.object_id >= 0x50000000 && message.object_id <= 0x5FFFFFFF) // Make sure it's a player GUID
                            {
                                // Update the global biota infos
                                lock (biotasByServer)
                                {
                                    if (!biotasByServer.TryGetValue(serverName, out var biotaServer))
                                    {
                                        biotaServer = new Dictionary<uint, BiotaEx>();
                                        biotasByServer[serverName] = biotaServer;
                                    }

                                    if (!biotaServer.TryGetValue(message.object_id, out var biotaEx))
                                    {
                                        biotaEx = new BiotaEx();
                                        biotaServer[message.object_id] = biotaEx;
                                    }

                                    if (biotaEx.LastPositionTSec < record.tsSec)
                                    {
                                        biotaEx.LastPosition = message.positionPack.position;
                                        biotaEx.LastPositionTSec = record.tsSec;
                                    }
                                }
                            }

                            continue;
                        }

                        if (messageCode == (uint)PacketOpcode.ORDERED_EVENT) // 0xF7B1 Game Action
                        {
                            /*var sequence = */binaryReader.ReadUInt32();
                            var opCode = binaryReader.ReadUInt32();

                            if (opCode == (uint)PacketOpcode.Evt_Character__LoginCompleteNotification_ID)
                            {
                                // At this point, we should stop building/updating the player/character and only update the possessed items
                                if (loginEvent != null)
                                    loginEvent.PlayerLoginCompleted = true;
                            }

                            continue;
                        }

                        if (messageCode == (uint)PacketOpcode.WEENIE_ORDERED_EVENT) // 0xF7B0 Game Event
                        {
                            /*var guid = */binaryReader.ReadUInt32();
                            /*var sequence = */binaryReader.ReadUInt32();
                            var opCode = binaryReader.ReadUInt32();

                            if (opCode == (uint)PacketOpcode.PLAYER_DESCRIPTION_EVENT)
                            {
                                var message = CM_Login.PlayerDescription.read(binaryReader);

                                // We only process player create/update messages for player biotas during the login process
                                if (loginEvent != null && !loginEvent.PlayerLoginCompleted)
                                {
                                    hits++;

                                    ACECharacterCreator.Update(message, loginEvent.Character);
                                    ACEBiotaCreator.Update(message, loginEvent.Biota, loginEvent.Inventory, loginEvent.Equipment, rwLock);

                                    lock (playerLoginsByServer)
                                    {
                                        if (!playerLoginsByServer.TryGetValue(serverName, out var server))
                                        {
                                            server = new Dictionary<uint, PlayerLogins>();
                                            playerLoginsByServer[serverName] = server;
                                        }

                                        if (!server.TryGetValue(loginEvent.Biota.Id, out var player))
                                        {
                                            player = new PlayerLogins();
                                            server[loginEvent.Biota.Id] = player;
                                        }

                                        player.LoginEvents.Add(loginEvent);
                                    }
                                }
                            }
                            else if (opCode == (uint)PacketOpcode.Evt_Social__FriendsUpdate_ID)
                            {
                                // Skip this
                                // player.Character.CharacterPropertiesFriendList
                            }
                            else if (opCode == (uint)PacketOpcode.Evt_Social__CharacterTitleTable_ID)
                            {
                                var message = CM_Social.CharacterTitleTable.read(binaryReader);

                                // We only process player create/update messages for player biotas during the login process
                                if (loginEvent != null && !loginEvent.PlayerLoginCompleted)
                                {
                                    loginEvent.Biota.SetProperty(PropertyInt.CharacterTitleId, (int)message.mDisplayTitle, rwLock, out _);

                                    foreach (var value in message.mTitleList.list)
                                        loginEvent.Character.CharacterPropertiesTitleBook.Add(new ACE.Database.Models.Shard.CharacterPropertiesTitleBook { TitleId = (uint)value });
                                }
                            }
                            else if (opCode == (uint)PacketOpcode.Evt_Social__SendClientContractTrackerTable_ID)
                            {
                                // Skip this
                                // player.Character.CharacterPropertiesContractRegistry
                            }
                            else if (opCode == (uint)PacketOpcode.ALLEGIANCE_UPDATE_EVENT)
                            {
                                // Skip this
                            }
                            else if (opCode == (uint)PacketOpcode.VIEW_CONTENTS_EVENT)
                            {
                                var message = CM_Inventory.ViewContents.read(binaryReader);

                                // We only process player create/update messages for player biotas during the login process
                                if (loginEvent != null && !loginEvent.PlayerLoginCompleted)
                                {
                                    var list = new List<uint>();

                                    foreach (var value in message.contents_list.list)
                                        list.Add(value.m_iid); // We don't use m_uContainerProperties

                                    if (!loginEvent.ViewContentsEvents.ContainsKey(message.i_container)) // We only store the first ViewContentsEvent
                                        loginEvent.ViewContentsEvents[message.i_container] = list;
                                }
                            }

                            if (opCode == (uint)PacketOpcode.APPRAISAL_INFO_EVENT)
                            {
                                var message = CM_Examine.SetAppraiseInfo.read(binaryReader);

                                if (loginEvent != null)
                                {
                                    if (message.i_objid == loginEvent.Biota.Id)
                                        ACEBiotaCreator.Update(message, loginEvent.Biota, rwLock);

                                    // If this is an inventory item, update it
                                    if (loginEvent.WorldObjects.TryGetValue(message.i_objid, out var value))
                                    {
                                        ACEBiotaCreator.Update(message, value.Biota, rwLock);
                                        value.AppraiseInfoReceived = true;
                                    }
                                }

                                if (message.i_objid >= 0x50000000 && message.i_objid <= 0x5FFFFFFF) // Make sure it's a player GUID
                                {
                                    // Update the global biota infos
                                    lock (biotasByServer)
                                    {
                                        if (!biotasByServer.TryGetValue(serverName, out var biotaServer))
                                        {
                                            biotaServer = new Dictionary<uint, BiotaEx>();
                                            biotasByServer[serverName] = biotaServer;
                                        }

                                        if (!biotaServer.TryGetValue(message.i_objid, out var biotaEx))
                                        {
                                            biotaEx = new BiotaEx();
                                            biotaServer[message.i_objid] = biotaEx;
                                        }

                                        if (biotaEx.LastAppraisalProfileTSec < record.tsSec)
                                        {
                                            biotaEx.LastAppraisalProfile = message;
                                            biotaEx.LastAppraisalProfileTSec = record.tsSec;
                                        }
                                    }
                                }
                            }

                            continue;
                        }
                    }
                }
                catch (InvalidDataException)
                {
                    // This is a pcap parse error
                }
                catch (Exception)
                {
                    messageExceptions++;
                    // Do something with the exception maybe
                }
            }

            return (hits, messageExceptions);
        }

        public override void WriteOutput(string destinationRoot, ref bool writeOutputAborted)
        {
            var playerExportsFolder = Path.Combine(destinationRoot, "Player Exports");

            if (!Directory.Exists(playerExportsFolder))
                Directory.CreateDirectory(playerExportsFolder);


            var notes = new StringBuilder();
            notes.AppendLine("The following Windows command will import all the sql files into your retail shard. It can take many hours.");
            notes.AppendLine("for /f \"delims=\" %f in ('dir /b /s \"C:\\ACLogView Output\\Player Exports\\Darktide\\*.sql\"') do \"C:\\Program Files\\MySQL\\MySQL Server 8.0\\bin\\mysql\" --user=root --password=password ace_shard_retail_dt < \"%f\"");
            notes.AppendLine("for /f \"delims=\" %f in ('dir /b /s \"C:\\ACLogView Output\\Player Exports\\Frostfell\\*.sql\"') do \"C:\\Program Files\\MySQL\\MySQL Server 8.0\\bin\\mysql\" --user=root --password=password ace_shard_retail_ff < \"%f\"");
            notes.AppendLine("for /f \"delims=\" %f in ('dir /b /s \"C:\\ACLogView Output\\Player Exports\\Harvestgain\\*.sql\"') do \"C:\\Program Files\\MySQL\\MySQL Server 8.0\\bin\\mysql\" --user=root --password=password ace_shard_retail_hg < \"%f\"");
            notes.AppendLine("for /f \"delims=\" %f in ('dir /b /s \"C:\\ACLogView Output\\Player Exports\\Leafcull\\*.sql\"') do \"C:\\Program Files\\MySQL\\MySQL Server 8.0\\bin\\mysql\" --user=root --password=password ace_shard_retail_lc < \"%f\"");
            notes.AppendLine("for /f \"delims=\" %f in ('dir /b /s \"C:\\ACLogView Output\\Player Exports\\Morningthaw\\*.sql\"') do \"C:\\Program Files\\MySQL\\MySQL Server 8.0\\bin\\mysql\" --user=root --password=password ace_shard_retail_mt < \"%f\"");
            notes.AppendLine("for /f \"delims=\" %f in ('dir /b /s \"C:\\ACLogView Output\\Player Exports\\Solclaim\\*.sql\"') do \"C:\\Program Files\\MySQL\\MySQL Server 8.0\\bin\\mysql\" --user=root --password=password ace_shard_retail_sc < \"%f\"");
            notes.AppendLine("for /f \"delims=\" %f in ('dir /b /s \"C:\\ACLogView Output\\Player Exports\\Thistledown\\*.sql\"') do \"C:\\Program Files\\MySQL\\MySQL Server 8.0\\bin\\mysql\" --user=root --password=password ace_shard_retail_td < \"%f\"");
            notes.AppendLine("for /f \"delims=\" %f in ('dir /b /s \"C:\\ACLogView Output\\Player Exports\\Verdantine\\*.sql\"') do \"C:\\Program Files\\MySQL\\MySQL Server 8.0\\bin\\mysql\" --user=root --password=password ace_shard_retail_vt < \"%f\"");
            notes.AppendLine("for /f \"delims=\" %f in ('dir /b /s \"C:\\ACLogView Output\\Player Exports\\WintersEbb\\*.sql\"') do \"C:\\Program Files\\MySQL\\MySQL Server 8.0\\bin\\mysql\" --user=root --password=password ace_shard_retail_we < \"%f\"");
            notes.AppendLine("The following Linux command will import all the sql files into your retail shard. It can take many hours...");
            notes.AppendLine("Make sure you're in the Player Exports root folder.");
            notes.AppendLine("for i in *.sql Darktide/* Darktide/*/* Darktide/*/*/*; do mysql --user=root --password=password ace_shard_retail_dt < \"$i\"; done");
            notes.AppendLine("for i in *.sql Frostfell/* Frostfell/*/* Frostfell/*/*/*; do mysql --user=root --password=password ace_shard_retail_ff < \"$i\"; done");
            notes.AppendLine("for i in *.sql Harvestgain/* Harvestgain/*/* Harvestgain/*/*/*; do mysql --user=root --password=password ace_shard_retail_hg < \"$i\"; done");
            notes.AppendLine("for i in *.sql Leafcull/* Leafcull/*/* Leafcull/*/*/*; do mysql --user=root --password=password ace_shard_retail_lc < \"$i\"; done");
            notes.AppendLine("for i in *.sql Morningthaw/* Morningthaw/*/* Morningthaw/*/*/*; do mysql --user=root --password=password ace_shard_retail_mt < \"$i\"; done");
            notes.AppendLine("for i in *.sql Solclaim/* Solclaim/*/* Solclaim/*/*/*; do mysql --user=root --password=password ace_shard_retail_sc < \"$i\"; done");
            notes.AppendLine("for i in *.sql Thistledown/* Thistledown/*/* Thistledown/*/*/*; do mysql --user=root --password=password ace_shard_retail_td < \"$i\"; done");
            notes.AppendLine("for i in *.sql Verdantine/* Verdantine/*/* Verdantine/*/*/*; do mysql --user=root --password=password ace_shard_retail_vt < \"$i\"; done");
            notes.AppendLine("for i in *.sql WintersEbb/* WintersEbb/*/* WintersEbb/*/*/*; do mysql --user=root --password=password ace_shard_retail_we < \"$i\"; done");


            // Find guid collisions across servers
            Dictionary<string, HashSet<uint>> guidsByServer = new Dictionary<string, HashSet<uint>>();
            foreach (var server in playerLoginsByServer)
            {
                var guids = new HashSet<uint>();
                guidsByServer.Add(server.Key, guids);

                foreach (var player in server.Value)
                {
                    guids.Add(player.Key);
                    foreach (var loginEvent in player.Value.LoginEvents)
                    {
                        foreach (var wo in loginEvent.WorldObjects)
                            guids.Add(wo.Key);
                    }
                }
            }

            var keys = guidsByServer.Keys.ToList();
            keys.Sort();
            for (int i = 0; i < keys.Count - 1; i++)
            {
                for (int j = i + 1; j < keys.Count; j++)
                {
                    var intersections = new HashSet<uint>(guidsByServer[keys[i]]);
                    intersections.IntersectWith(guidsByServer[keys[j]]);
                    if (intersections.Count > 0)
                    {
                        notes.AppendLine();
                        notes.AppendLine(keys[i] + " IntersectWith " + keys[j]);
                        foreach (var intersect in intersections)
                            notes.AppendLine(intersect.ToString("X8"));
                    }
                }
            }

            var notesFileName = Path.Combine(playerExportsFolder, "notes.txt");
            File.WriteAllText(notesFileName, notes.ToString());


            var biotaWriter = new ACE.Database.SQLFormatters.Shard.BiotaSQLWriter();
            var characterWriter = new ACE.Database.SQLFormatters.Shard.CharacterSQLWriter();

            var rwLock = new ReaderWriterLockSlim();

            // Export players by login event
            foreach (var server in playerLoginsByServer)
            {
                var serverDirectory = Path.Combine(playerExportsFolder, server.Key);

                foreach (var player in server.Value)
                {
                    if (writeOutputAborted)
                        return;

                    // We only export the last login event
                    var loginEvent = player.Value.LoginEvents.Where(r => r.Biota.PropertiesDID.Count > 0).OrderBy(r => r.TSec).LastOrDefault();

                    // no valid result
                    if (loginEvent == null)
                        continue;

                    var name = loginEvent.Biota.GetProperty(PropertyString.Name, rwLock);

                    var playerDirectoryRoot = Path.Combine(serverDirectory, name);

                    var loginEventDirectory = Path.Combine(playerDirectoryRoot, loginEvent.TSec.ToString());

                    if (!Directory.Exists(loginEventDirectory))
                        Directory.CreateDirectory(loginEventDirectory);

                    var sb = new StringBuilder();

                    sb.AppendLine("Source: ");
                    sb.AppendLine(loginEvent.FileName);

                    if (player.Value.LoginEvents.Count > 1)
                    {
                        sb.AppendLine();
                        sb.AppendLine("Alternate sources:");
                        foreach (var value in player.Value.LoginEvents.OrderBy(r => r.TSec).ThenBy(r => r.FileName))
                        {
                            if (loginEvent == value)
                                continue;
                            sb.AppendLine(value.FileName);
                        }
                    }

                    var failedExportsUnknownWeenie = new HashSet<string>();
                    var partialExportsNoAppraisalInfo = new HashSet<string>();

                    // Biota
                    {
                        var defaultFileName = ACE.Database.SQLFormatters.Shard.BiotaSQLWriter.GetDefaultFileName(loginEvent.Biota.Id, loginEvent.Biota.GetName());

                        var fileName = Path.Combine(loginEventDirectory, defaultFileName);

                        // Update to the latest position seen
                        if (biotasByServer.TryGetValue(server.Key, out var biotaServer) && biotaServer.TryGetValue(player.Key, out var biotaEx) && biotaEx.LastPosition != null)
                            ACEBiotaCreator.Update(PositionType.Location, biotaEx.LastPosition, loginEvent.Biota, rwLock);

                        loginEvent.Biota.WeenieType = ACEBiotaCreator.DetermineWeenieType(loginEvent.Biota, rwLock);

                        var databaseBiota = ACE.Database.Adapter.BiotaConverter.ConvertFromEntityBiota(loginEvent.Biota);

                        ACE.Database.ShardDatabase.SetBiotaPopulatedCollections(databaseBiota);

                        using (StreamWriter outputFile = new StreamWriter(fileName, false))
                            biotaWriter.CreateSQLINSERTStatement(databaseBiota, outputFile);
                    }

                    // Character
                    {
                        loginEvent.Character.Name = name;

                        var defaultFileName = loginEvent.Character.Id.ToString("X8") + " " + name + " - Character.sql";

                        var fileName = Path.Combine(loginEventDirectory, defaultFileName);

                        using (StreamWriter outputFile = new StreamWriter(fileName, false))
                            characterWriter.CreateSQLINSERTStatement(loginEvent.Character, outputFile);
                    }

                    // Possessions
                    foreach (var woi in loginEvent.WorldObjects)
                    {
                        var woiBeingUsed = woi.Value;

                        // If we don't have appraise info for this WO, try to find one that does
                        if (!woi.Value.AppraiseInfoReceived)
                        {
                            var result = player.Value.GetBestPossession(woi.Key, woi.Value.Name);

                            if (result != woiBeingUsed)
                            {
                                // todo log that the item was replaced with a better match from a different session
                                woiBeingUsed = result;
                            }
                        }

                        // Update the InventoryOrder and Container
                        for (int i = 0; i < loginEvent.Inventory.Count; i++)
                        {
                            if (loginEvent.Inventory[i].guid == woiBeingUsed.Biota.Id)
                            {
                                woiBeingUsed.Biota.SetProperty(PropertyInstanceId.Owner, loginEvent.Biota.Id, rwLock, out _);
                                woiBeingUsed.Biota.SetProperty(PropertyInstanceId.Container, loginEvent.Biota.Id, rwLock, out _);
                                woiBeingUsed.Biota.SetProperty(PropertyInt.InventoryOrder, i, rwLock, out _);

                                woiBeingUsed.Biota.TryRemoveProperty(PropertyInt.CurrentWieldedLocation, rwLock);
                                woiBeingUsed.Biota.TryRemoveProperty(PropertyInstanceId.Wielder, rwLock);

                                goto processed;
                            }
                        }

                        foreach (var container in loginEvent.ViewContentsEvents)
                        {
                            var index = container.Value.IndexOf(woiBeingUsed.Biota.Id);
                            if (index != -1)
                            {
                                woiBeingUsed.Biota.SetProperty(PropertyInstanceId.Owner, container.Key, rwLock, out _);
                                woiBeingUsed.Biota.SetProperty(PropertyInstanceId.Container, container.Key, rwLock, out _);
                                woiBeingUsed.Biota.SetProperty(PropertyInt.InventoryOrder, index, rwLock, out _);

                                woiBeingUsed.Biota.TryRemoveProperty(PropertyInt.CurrentWieldedLocation, rwLock);
                                woiBeingUsed.Biota.TryRemoveProperty(PropertyInstanceId.Wielder, rwLock);

                                goto processed;
                            }
                        }

                        processed:

                        var defaultFileName = ACE.Database.SQLFormatters.Shard.BiotaSQLWriter.GetDefaultFileName(woiBeingUsed.Biota.Id, woiBeingUsed.Biota.GetName());

                        defaultFileName = String.Concat(defaultFileName.Split(Path.GetInvalidFileNameChars()));

                        var fileName = Path.Combine(loginEventDirectory, defaultFileName);

                        woiBeingUsed.Biota.WeenieType = ACEBiotaCreator.DetermineWeenieType(woiBeingUsed.Biota, rwLock);

                        if (woiBeingUsed.Biota.WeenieType == 0)
                        {
                            failedExportsUnknownWeenie.Add($"{woiBeingUsed.Biota.Id:X8}:{woiBeingUsed.Name}");
                            continue;
                        }

                        if (!woiBeingUsed.AppraiseInfoReceived)
                            partialExportsNoAppraisalInfo.Add($"{woiBeingUsed.Biota.Id:X8}:{woiBeingUsed.Name}");

                        var databaseBiota = ACE.Database.Adapter.BiotaConverter.ConvertFromEntityBiota(woiBeingUsed.Biota);

                        ACE.Database.ShardDatabase.SetBiotaPopulatedCollections(databaseBiota);

                        using (StreamWriter outputFile = new StreamWriter(fileName, false))
                            biotaWriter.CreateSQLINSERTStatement(databaseBiota, outputFile);
                    }

                    if (failedExportsUnknownWeenie.Count > 0)
                    {
                        sb.AppendLine();
                        sb.AppendLine("Failed Exports - Unable to determine weenie type:");
                        foreach (var value in failedExportsUnknownWeenie)
                            sb.AppendLine(value);
                    }

                    if (partialExportsNoAppraisalInfo.Count > 0)
                    {
                        sb.AppendLine();
                        sb.AppendLine("Partial Exports - Missing appraisal info:");
                        foreach (var value in partialExportsNoAppraisalInfo)
                            sb.AppendLine(value);
                    }

                    // Determine missing possessions
                    var possessions = new HashSet<uint>();
                    foreach (var value in loginEvent.Inventory)
                        possessions.Add(value.guid);
                    foreach (var value in loginEvent.Equipment)
                        possessions.Add(value.guid);
                    foreach (var container in loginEvent.ViewContentsEvents)
                    {
                        if (possessions.Contains(container.Key))
                        {
                            foreach (var child in container.Value)
                                possessions.Add(child);
                        }
                    }

                    sb.AppendLine();
                    sb.AppendLine("Missing Exports - Possessed items that were not found:");
                    foreach (var value in possessions)
                    {
                        if (!loginEvent.WorldObjects.ContainsKey(value))
                            sb.AppendLine($"{value:X8}");
                    }


                    var resutlsFileName = Path.Combine(loginEventDirectory, "results.txt");
                    File.WriteAllText(resutlsFileName, sb.ToString());
                }
            }


            // Export player biotas that don't have login events
            foreach (var server in biotasByServer)
            {
                var serverDirectory = Path.Combine(playerExportsFolder, server.Key);

                foreach (var biotaEx in server.Value)
                {
                    if (writeOutputAborted)
                        return;

                    // Was this biota captured by a login event?
                    if (playerLoginsByServer.TryGetValue(server.Key, out var playerLoginServer) && playerLoginServer.ContainsKey(biotaEx.Key))
                        continue;

                    if (biotaEx.Value.LastCreateObject == null)
                        continue;

                    var biota = new Biota();

                    biota.Id = biotaEx.Key;

                    biota.PropertiesPosition = new Dictionary<PositionType, PropertiesPosition>();

                    biota.PropertiesAnimPart = new List<PropertiesAnimPart>();
                    biota.PropertiesPalette = new List<PropertiesPalette>();
                    biota.PropertiesTextureMap = new List<PropertiesTextureMap>();

                    // Properties for creatures
                    biota.PropertiesAttribute = new Dictionary<PropertyAttribute, PropertiesAttribute>();
                    biota.PropertiesAttribute2nd = new Dictionary<PropertyAttribute2nd, PropertiesAttribute2nd>();
                    biota.PropertiesBodyPart = new Dictionary<CombatBodyPart, PropertiesBodyPart>();
                    biota.PropertiesSkill = new Dictionary<ACE.Entity.Enum.Skill, PropertiesSkill>();

                    // Biota additions over Weenie
                    biota.PropertiesEnchantmentRegistry = new Collection<PropertiesEnchantmentRegistry>();

                    ACEBiotaCreator.Update(biotaEx.Value.LastCreateObject, biota, rwLock, true);

                    if (biotaEx.Value.LastAppraisalProfile != null)
                        ACEBiotaCreator.Update(biotaEx.Value.LastAppraisalProfile, biota, rwLock);

                    var name = biota.GetProperty(PropertyString.Name, rwLock);

                    var playerDirectoryRoot = Path.Combine(serverDirectory, name);

                    var playerDirectoryInstance = Path.Combine(playerDirectoryRoot, "0");

                    if (!Directory.Exists(playerDirectoryInstance))
                        Directory.CreateDirectory(playerDirectoryInstance);

                    // Biota
                    {
                        var defaultFileName = ACE.Database.SQLFormatters.Shard.BiotaSQLWriter.GetDefaultFileName(biota.Id, biota.GetName());

                        var fileName = Path.Combine(playerDirectoryInstance, defaultFileName);

                        // Update to the latest position seen
                        if (biotaEx.Value.LastPosition != null)
                            ACEBiotaCreator.Update(PositionType.Location, biotaEx.Value.LastPosition, biota, rwLock);

                        biota.WeenieType = ACEBiotaCreator.DetermineWeenieType(biota, rwLock);

                        var databaseBiota = ACE.Database.Adapter.BiotaConverter.ConvertFromEntityBiota(biota);

                        ACE.Database.ShardDatabase.SetBiotaPopulatedCollections(databaseBiota);

                        using (StreamWriter outputFile = new StreamWriter(fileName, false))
                            biotaWriter.CreateSQLINSERTStatement(databaseBiota, outputFile);
                    }
                }
            }
        }
    }
}
