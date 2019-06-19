using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

using ACE.Database.Models.Shard;

using aclogview.ACE_Helpers;

namespace aclogview.Tools.Scrapers
{
    class PlayerExporter : Scraper
    {
        public override string Description => "Exports players and their inventory";

        class Player
        {
            public readonly string ServerName;

            public readonly Biota Biota = new Biota();
            public readonly Character Character = new Character();
            public readonly Dictionary<uint, Biota> WorldObjects = new Dictionary<uint, Biota>();

            public readonly List<(uint guid, uint containerProperties)> Inventory = new List<(uint guid, uint containerProperties)>();
            public readonly List<(uint guid, uint location, uint priority)> Equipment = new List<(uint guid, uint location, uint priority)>();

            public bool IsPossedItem(uint guid)
            {
                foreach (var entry in Inventory)
                    if (entry.guid == guid)
                        return true;

                foreach (var entry in Equipment)
                    if (entry.guid == guid)
                        return true;

                return false;
            }

            public Player(string serverName, uint guid)
            {
                ServerName = serverName;

                Biota.Id = guid;
                Character.Id = guid;
            }

            // TODO: player.Character.CharacterPropertiesQuestRegistry
            // TODO: player.Character.HairTexture
            // TODO: player.Character.DefaultHairTexture

            // TODO: Verify attributes, they seem to be not exporting/importing correctly
            // TODO: Some exports seem to have trouble in import with the EchnantmentRegistry failing the Database Save()

            // Notes: In current ACE shard, no biota stores data in anim_part, pallete, or texture_map
            // I'm still not sure why we have those tables in shard.
            // The information for the player that comes from the physics message comes from where?
        }

        private readonly ConcurrentDictionary<uint, Player> players = new ConcurrentDictionary<uint, Player>();

        public override void Reset()
        {
            players.Clear();
        }

        public override void ProcessFileRecords(string fileName, List<PacketRecord> records, ref bool searchAborted)
        {
            string serverName = null;
            bool playerLoginCompleted = false;

            Player player = null;
            var rwLock = new ReaderWriterLockSlim();

            var viewContentsEvents = new Dictionary<uint, HashSet<uint>>();

            foreach (PacketRecord record in records)
            {
                if (searchAborted)
                    return;

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

                        if (serverName == null)
                            continue;

                        // This could be seen multiple times if the first time the player tries to enter, they get a "Your character is already in world" message
                        if (messageCode == (uint)PacketOpcode.CHARACTER_ENTER_GAME_EVENT) // 0xF657
                        {
                            var message = Proto_UI.EnterWorld.read(binaryReader);

                            if (player != null && player.Biota.Id != message.gid)
                            {
                                player = null;
                                playerLoginCompleted = false;
                                throw new Exception("This shouldn't happen");
                            }

                            player = new Player(serverName, message.gid);
                            playerLoginCompleted = false;
                            continue;
                        }

                        if (player == null)
                            continue;

                        if (messageCode == (uint)PacketOpcode.CHARACTER_EXIT_GAME_EVENT)
                        {
                            var message = Proto_UI.LogOff.read(binaryReader);

                            if (message.gid == player.Biota.Id)
                                players[player.Biota.Id] = player;

                            player = null;
                            playerLoginCompleted = false;
                            continue;
                        }

                        if (messageCode == (uint)PacketOpcode.ORDERED_EVENT) // 0xF7B1 Game Action
                        {
                            /*var sequence = */
                            binaryReader.ReadUInt32();
                            var opCode = binaryReader.ReadUInt32();

                            if (opCode == (uint)PacketOpcode.Evt_Character__LoginCompleteNotification_ID)
                            {
                                // At this point, we should stop building/updating the player/character and only update the possessed items
                                playerLoginCompleted = true;
                            }
                        }

                        if (messageCode == (uint)PacketOpcode.WEENIE_ORDERED_EVENT) // 0xF7B0 Game Event
                        {
                            /*var guid = */
                            binaryReader.ReadUInt32();
                            /*var sequence = */
                            binaryReader.ReadUInt32();
                            var opCode = binaryReader.ReadUInt32();

                            // We only process player create/update messages for player biotas during the login process
                            if (!playerLoginCompleted)
                            {
                                if (opCode == (uint)PacketOpcode.PLAYER_DESCRIPTION_EVENT)
                                {
                                    var message = CM_Login.PlayerDescription.read(binaryReader);
                                    ;

                                    ACEBiotaCreator.Update(message, player.Character, player.Biota, player.Inventory,
                                        player.Equipment, rwLock);

                                }
                                else if (opCode == (uint)PacketOpcode.Evt_Social__FriendsUpdate_ID)
                                {
                                    // Skip this
                                    // player.Character.CharacterPropertiesFriendList
                                }
                                else if (opCode == (uint)PacketOpcode.Evt_Social__CharacterTitleTable_ID)
                                {
                                    var message = CM_Social.CharacterTitleTable.read(binaryReader);

                                    player.Character.CharacterPropertiesTitleBook.Add(new CharacterPropertiesTitleBook { TitleId = (uint)message.mDisplayTitle });
                                    foreach (var value in message.mTitleList.list)
                                        player.Character.CharacterPropertiesTitleBook.Add(new CharacterPropertiesTitleBook { TitleId = (uint)value });
                                }
                                else if (opCode == (uint)PacketOpcode.Evt_Social__SendClientContractTrackerTable_ID)
                                {
                                    var message = CM_Social.SendClientContractTrackerTable.read(binaryReader);

                                    foreach (var value in message._contractTrackerHash.hashTable)
                                    {
                                        var contract = new CharacterPropertiesContract
                                        {
                                            // value.Key what's this?
                                            ContractId = value.Value._contract_id,
                                            Stage = value.Value._contract_stage,
                                            TimeWhenDone = (ulong)value.Value._time_when_done,
                                            TimeWhenRepeats = (ulong)value.Value._time_when_repeats,
                                            Version = value.Value._version,
                                        };
                                        player.Character.CharacterPropertiesContract.Add(contract);
                                    }
                                }
                                else if (opCode == (uint)PacketOpcode.ALLEGIANCE_UPDATE_EVENT)
                                {
                                    // Skip this
                                }
                                else if (opCode == (uint)PacketOpcode.VIEW_CONTENTS_EVENT)
                                {
                                    var message = CM_Inventory.ViewContents.read(binaryReader);

                                    var hashSet = new HashSet<uint>();

                                    foreach (var value in message.contents_list.list)
                                        hashSet.Add(value.m_iid); // We don't use m_uContainerProperties

                                    if (!viewContentsEvents.ContainsKey(message.i_container)) // We only store the first ViewContentsEvent
                                        viewContentsEvents[message.i_container] = hashSet;
                                }
                            }

                            if (opCode == (uint)PacketOpcode.APPRAISAL_INFO_EVENT)
                            {
                                var message = CM_Examine.SetAppraiseInfo.read(binaryReader);

                                if (message.i_objid == player.Biota.Id)
                                    ACEBiotaCreator.Update(message, player.Biota, rwLock);

                                if (player.WorldObjects.TryGetValue(message.i_objid, out var value))
                                    ACEBiotaCreator.Update(message, value, rwLock);
                            }
                        }

                        if (messageCode == (uint)PacketOpcode.Evt_Physics__CreateObject_ID)
                        {
                            var message = CM_Physics.CreateObject.read(binaryReader);

                            // We only process player create/update messages for player biotas during the login process
                            if (!playerLoginCompleted)
                            {
                                if (message.object_id == player.Biota.Id)
                                {
                                    ACEBiotaCreator.Update(message, player.Biota, rwLock, true);

                                    var position = new ACE.Entity.Position(message.physicsdesc.pos.objcell_id, message.physicsdesc.pos.frame.m_fOrigin.x, message.physicsdesc.pos.frame.m_fOrigin.y, message.physicsdesc.pos.frame.m_fOrigin.z, message.physicsdesc.pos.frame.qx, message.physicsdesc.pos.frame.qy, message.physicsdesc.pos.frame.qz, message.physicsdesc.pos.frame.qw);
                                    player.Biota.SetPosition(ACE.Entity.Enum.Properties.PositionType.Location, position, rwLock, out _);
                                }
                            }

                            if (!player.WorldObjects.ContainsKey(message.object_id) && (player.IsPossedItem(message.object_id) || viewContentsEvents.Any(r => r.Value.Contains(message.object_id))))
                            {
                                var item = new Biota { Id = message.object_id };
                                ACEBiotaCreator.Update(message, item, rwLock, true);
                                player.WorldObjects[message.object_id] = item;
                            }
                        }

                        // We only process player create/update messages for player biotas during the login process
                        if (!playerLoginCompleted)
                        {
                            if (messageCode == (uint)PacketOpcode.Evt_Qualities__PrivateUpdateInt_ID)
                            {
                                var message = CM_Qualities.PrivateUpdateQualityEvent<STypeInt, int>.read(0, binaryReader);
                                player.Biota.SetProperty((ACE.Entity.Enum.Properties.PropertyInt)message.stype, message.val, rwLock, out _);
                            }
                            else if (messageCode == (uint)PacketOpcode.Evt_Qualities__PrivateUpdateInt64_ID)
                            {
                                var message = CM_Qualities.PrivateUpdateQualityEvent<STypeInt64, long>.read(0, binaryReader);
                                player.Biota.SetProperty((ACE.Entity.Enum.Properties.PropertyInt64)message.stype, message.val, rwLock, out _);
                            }
                            else if (messageCode == (uint)PacketOpcode.Evt_Qualities__PrivateUpdateBool_ID)
                            {
                                var message = CM_Qualities.PrivateUpdateQualityEvent<STypeBool, int>.read(0, binaryReader);
                                player.Biota.SetProperty((ACE.Entity.Enum.Properties.PropertyBool)message.stype, (message.val != 0), rwLock, out _);
                            }
                            else if (messageCode == (uint)PacketOpcode.Evt_Qualities__PrivateUpdateFloat_ID)
                            {
                                var message = CM_Qualities.PrivateUpdateQualityEvent<STypeFloat, double>.read(0, binaryReader);
                                player.Biota.SetProperty((ACE.Entity.Enum.Properties.PropertyFloat)message.stype, message.val, rwLock, out _);
                            }
                            else if (messageCode == (uint)PacketOpcode.Evt_Qualities__PrivateUpdateString_ID)
                            {
                                var message = CM_Qualities.PrivateUpdateStringEvent.read(0, binaryReader);
                                player.Biota.SetProperty((ACE.Entity.Enum.Properties.PropertyString)message.stype, message.val.m_buffer, rwLock, out _);
                            }
                            else if (messageCode == (uint)PacketOpcode.Evt_Qualities__PrivateUpdateDataID_ID)
                            {
                                var message = CM_Qualities.PrivateUpdateQualityEvent<STypeDID, uint>.read(0, binaryReader);
                                player.Biota.SetProperty((ACE.Entity.Enum.Properties.PropertyDataId)message.stype, message.val, rwLock, out _);
                            }
                            else if (messageCode == (uint)PacketOpcode.Evt_Qualities__PrivateUpdateInstanceID_ID)
                            {
                                var message = CM_Qualities.PrivateUpdateQualityEvent<STypeIID, uint>.read(0, binaryReader);
                                player.Biota.SetProperty((ACE.Entity.Enum.Properties.PropertyInstanceId)message.stype, message.val, rwLock, out _);
                            }
                            else if (messageCode == (uint)PacketOpcode.Evt_Qualities__PrivateUpdatePosition_ID)
                            {
                                var message = CM_Qualities.PrivateUpdateQualityEvent<STypePosition, Position>.read(0, binaryReader);
                                var position = new ACE.Entity.Position(message.val.objcell_id, message.val.frame.m_fOrigin.x, message.val.frame.m_fOrigin.y, message.val.frame.m_fOrigin.z, message.val.frame.qx, message.val.frame.qy, message.val.frame.qz, message.val.frame.qw);
                                player.Biota.SetPosition((ACE.Entity.Enum.Properties.PositionType)message.stype, position, rwLock, out _);
                            }
                            else if (messageCode == (uint)PacketOpcode.Evt_Qualities__PrivateUpdateSkill_ID)
                            {
                                var message = CM_Qualities.PrivateUpdateQualityEvent<STypeSkill, Skill>.read(0, binaryReader);
                                ACEBiotaCreator.AddSOrUpdatekill(player.Biota, message.stype, message.val, rwLock);
                            }
                            else if (messageCode == (uint)PacketOpcode.Evt_Qualities__PrivateUpdateSkillLevel_ID)
                            {
                                var message = CM_Qualities.PrivateUpdateQualityEvent<STypeSkill, int>.read(0, binaryReader);
                                var entity = player.Biota.GetOrAddSkill((ushort)message.stype, rwLock, out _);
                                entity.PP = (uint)message.val; // TODO is this setting PP?
                            }
                            else if (messageCode == (uint)PacketOpcode.Evt_Qualities__PrivateUpdateSkillAC_ID)
                            {
                                var message = CM_Qualities.PrivateUpdateQualityEvent<STypeSkill, SKILL_ADVANCEMENT_CLASS>.read(0, binaryReader);
                                var entity = player.Biota.GetOrAddSkill((ushort)message.stype, rwLock, out _);
                                entity.SAC = (uint)message.val;
                            }
                            else if (messageCode == (uint)PacketOpcode.Evt_Qualities__PrivateUpdateAttribute_ID)
                            {
                                var message = CM_Qualities.PrivateUpdateQualityEvent<STypeAttribute, Attribute>.read(0, binaryReader);
                                ACEBiotaCreator.AddOrUpdateAttribute(player.Biota, (ACE.Entity.Enum.Properties.PropertyAttribute)message.stype, message.val);
                            }
                            else if (messageCode == (uint)PacketOpcode.Evt_Qualities__PrivateUpdateAttributeLevel_ID)
                            {
                                // This doesn't happen in retail
                                throw new Exception("This shouldn't happen");
                            }
                            else if (messageCode == (uint)PacketOpcode.Evt_Qualities__PrivateUpdateAttribute2nd_ID)
                            {
                                var message = CM_Qualities.PrivateUpdateQualityEvent<STypeAttribute2nd, SecondaryAttribute>.read(0, binaryReader);
                                ACEBiotaCreator.AddOrUpdateAttribute2nd(player.Biota, (ACE.Entity.Enum.Properties.PropertyAttribute2nd)message.stype, message.val);
                            }
                            else if (messageCode == (uint)PacketOpcode.Evt_Qualities__PrivateUpdateAttribute2ndLevel_ID)
                            {
                                var message = CM_Qualities.PrivateUpdateQualityEvent<STypeAttribute2nd, int>.read(0, binaryReader);
                                var entity = player.Biota.BiotaPropertiesAttribute2nd.ToList().FirstOrDefault(r => r.Type == (ushort)message.stype);
                                if (entity == null)
                                {
                                    entity = new BiotaPropertiesAttribute2nd { Type = (ushort)message.stype };
                                    player.Biota.BiotaPropertiesAttribute2nd.Add(entity);
                                }
                                entity.CurrentLevel = (uint)message.val;
                            }
                        }
                    }
                }
                catch (InvalidDataException)
                {
                    // This is a pcap parse error
                }
                catch (Exception ex)
                {
                    // Do something with the exception maybe
                }
            }

            if (player != null)
                players[player.Biota.Id] = player;
        }

        public override void WriteOutput(string destinationRoot, ref bool searchAborted)
        {
            var biotaWriter = new ACE.Database.SQLFormatters.Shard.BiotaSQLWriter();
            var characterWriter = new ACE.Database.SQLFormatters.Shard.CharacterSQLWriter();

            foreach (var kvp in players)
            {
                if (searchAborted)
                    return;

                // biota is corrupt
                if (kvp.Value.Biota.BiotaPropertiesDID.Count == 0)
                    continue;

                var name = kvp.Value.Biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name);

                var directory = Path.Combine(destinationRoot, "Player Exports", kvp.Value.ServerName, name);

                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                // Biota
                {
                    var defaultFileName = biotaWriter.GetDefaultFileName(kvp.Value.Biota);

                    var fileName = Path.Combine(directory, defaultFileName);

                    kvp.Value.Biota.WeenieType = (int)ACEBiotaCreator.DetermineWeenieType(kvp.Value.Biota, new ReaderWriterLockSlim());

                    using (StreamWriter outputFile = new StreamWriter(fileName, false))
                        biotaWriter.CreateSQLINSERTStatement(kvp.Value.Biota, outputFile);
                }

                // Character
                {
                    var defaultFileName = kvp.Value.Character.Id.ToString("X8") + " " + name + " - Character.sql";

                    var fileName = Path.Combine(directory, defaultFileName);

                    using (StreamWriter outputFile = new StreamWriter(fileName, false))
                        characterWriter.CreateSQLINSERTStatement(kvp.Value.Character, outputFile);
                }

                foreach (var wo in kvp.Value.WorldObjects)
                {
                    var defaultFileName = biotaWriter.GetDefaultFileName(wo.Value);

                    defaultFileName = String.Concat(defaultFileName.Split(Path.GetInvalidFileNameChars()));

                    var fileName = Path.Combine(directory, defaultFileName);

                   //wo.Value.WeenieType = (int)ACEBiotaCreator.DetermineWeenieType(wo.Value, new ReaderWriterLockSlim());

                    using (StreamWriter outputFile = new StreamWriter(fileName, false))
                        biotaWriter.CreateSQLINSERTStatement(wo.Value, outputFile);
                }
            }
        }
    }
}
