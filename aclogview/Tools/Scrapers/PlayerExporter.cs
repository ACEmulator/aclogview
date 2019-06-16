using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

using ACE.Database.Models.Shard;

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
            public readonly List<Biota> WorldObjects = new List<Biota>();

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
            Player player = null;

            bool playerLoginCompleted = false;

            var rwLock = new ReaderWriterLockSlim();

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

                        if (messageCode == (uint) PacketOpcode.CHARACTER_EXIT_GAME_EVENT)
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

                        // If we've completed login, we no longer process player create/update messages
                        if (playerLoginCompleted)
                            continue;

                        if (messageCode == (uint)PacketOpcode.WEENIE_ORDERED_EVENT) // 0xF7B0 Game Event
                        {
                            /*var guid = */binaryReader.ReadUInt32();
                            /*var sequence = */binaryReader.ReadUInt32();
                            var opCode = binaryReader.ReadUInt32();

                            if (opCode == (uint) PacketOpcode.PLAYER_DESCRIPTION_EVENT)
                            {
                                var message = CM_Login.PlayerDescription.read(binaryReader);;

                                player.Biota.WeenieType = (int)message.CACQualities.CBaseQualities._weenie_type;

                                foreach (var value in message.CACQualities.CBaseQualities._intStatsTable.hashTable)
                                    player.Biota.SetProperty((ACE.Entity.Enum.Properties.PropertyInt)value.Key, value.Value, rwLock, out _);
                                foreach (var value in message.CACQualities.CBaseQualities._int64StatsTable.hashTable)
                                    player.Biota.SetProperty((ACE.Entity.Enum.Properties.PropertyInt64)value.Key, value.Value, rwLock, out _);
                                foreach (var value in message.CACQualities.CBaseQualities._boolStatsTable.hashTable)
                                    player.Biota.SetProperty((ACE.Entity.Enum.Properties.PropertyBool)value.Key, (value.Value != 0), rwLock, out _);
                                foreach (var value in message.CACQualities.CBaseQualities._floatStatsTable.hashTable)
                                    player.Biota.SetProperty((ACE.Entity.Enum.Properties.PropertyFloat)value.Key, value.Value, rwLock, out _);
                                foreach (var value in message.CACQualities.CBaseQualities._strStatsTable.hashTable)
                                    player.Biota.SetProperty((ACE.Entity.Enum.Properties.PropertyString)value.Key, value.Value.m_buffer, rwLock, out _);
                                foreach (var value in message.CACQualities.CBaseQualities._didStatsTable.hashTable)
                                    player.Biota.SetProperty((ACE.Entity.Enum.Properties.PropertyDataId)value.Key, value.Value, rwLock, out _);
                                foreach (var value in message.CACQualities.CBaseQualities._iidStatsTable.hashTable)
                                    player.Biota.SetProperty((ACE.Entity.Enum.Properties.PropertyInstanceId)value.Key, value.Value, rwLock, out _);
                                foreach (var value in message.CACQualities.CBaseQualities._posStatsTable.hashTable)
                                {
                                    var position = new ACE.Entity.Position(value.Value.objcell_id, value.Value.frame.m_fOrigin.x, value.Value.frame.m_fOrigin.y, value.Value.frame.m_fOrigin.z, value.Value.frame.qx, value.Value.frame.qy, value.Value.frame.qz, value.Value.frame.qw);
                                    player.Biota.SetPosition((ACE.Entity.Enum.Properties.PositionType)value.Key, position, rwLock, out _);
                                }

                                AddOrUpdateAttribute(player.Biota, ACE.Entity.Enum.Properties.PropertyAttribute.Strength, message.CACQualities._attribCache._strength);
                                AddOrUpdateAttribute(player.Biota, ACE.Entity.Enum.Properties.PropertyAttribute.Endurance, message.CACQualities._attribCache._endurance);
                                AddOrUpdateAttribute(player.Biota, ACE.Entity.Enum.Properties.PropertyAttribute.Quickness, message.CACQualities._attribCache._quickness);
                                AddOrUpdateAttribute(player.Biota, ACE.Entity.Enum.Properties.PropertyAttribute.Coordination, message.CACQualities._attribCache._coordination);
                                AddOrUpdateAttribute(player.Biota, ACE.Entity.Enum.Properties.PropertyAttribute.Focus, message.CACQualities._attribCache._focus);
                                AddOrUpdateAttribute(player.Biota, ACE.Entity.Enum.Properties.PropertyAttribute.Self, message.CACQualities._attribCache._self);

                                AddOrUpdateAttribute2nd(player.Biota, ACE.Entity.Enum.Properties.PropertyAttribute2nd.MaxHealth, message.CACQualities._attribCache._health);
                                AddOrUpdateAttribute2nd(player.Biota, ACE.Entity.Enum.Properties.PropertyAttribute2nd.MaxStamina, message.CACQualities._attribCache._stamina);
                                AddOrUpdateAttribute2nd(player.Biota, ACE.Entity.Enum.Properties.PropertyAttribute2nd.MaxMana, message.CACQualities._attribCache._mana);

                                foreach (var value in message.CACQualities._skillStatsTable.hashTable)
                                    AddOrUpdateSkill(player.Biota, value.Key, value.Value, rwLock);

                                foreach (var value in message.CACQualities._spell_book.hashTable)
                                {
                                    var entry = player.Biota.GetOrAddKnownSpell((int)value.Key, rwLock, out _);
                                    entry.Probability = value.Value;
                                }

                                if (message.CACQualities._enchantment_reg != null)
                                { 
                                    if (message.CACQualities._enchantment_reg._add_list != null)
                                    {
                                        foreach (var value in message.CACQualities._enchantment_reg._add_list.list)
                                            AddUpdateEnchantment(player.Biota, value, rwLock);
                                    }
                                    if (message.CACQualities._enchantment_reg._cooldown_list != null)
                                    {
                                        foreach (var value in message.CACQualities._enchantment_reg._cooldown_list.list)
                                            AddUpdateEnchantment(player.Biota, value, rwLock);
                                    }
                                    if (message.CACQualities._enchantment_reg._mult_list != null)
                                    {
                                        foreach (var value in message.CACQualities._enchantment_reg._mult_list.list)
                                            AddUpdateEnchantment(player.Biota, value, rwLock);
                                    }
                                    if (message.CACQualities._enchantment_reg._vitae != null && message.CACQualities._enchantment_reg._vitae.eid != null)
                                        AddUpdateEnchantment(player.Biota, message.CACQualities._enchantment_reg._vitae, rwLock);
                                }

                                player.Character.CharacterOptions1 = (int)message.PlayerModule.options_;

                                if (message.PlayerModule.shortcuts_ != null)
                                { 
                                    foreach (var value in message.PlayerModule.shortcuts_.shortCuts_)
                                        player.Character.CharacterPropertiesShortcutBar.Add(new CharacterPropertiesShortcutBar { ShortcutBarIndex = (uint)value.index_, ShortcutObjectId = value.objectID_});
                                }

                                for (uint i = 0; i < message.PlayerModule.favorite_spells_.Length; i++)
                                {
                                    if (message.PlayerModule.favorite_spells_[i] != null)
                                    { 
                                        for (uint j = 0 ; j < message.PlayerModule.favorite_spells_[i].list.Count ; j++)
                                            player.Character.CharacterPropertiesSpellBar.Add(new CharacterPropertiesSpellBar { SpellBarNumber = i, SpellBarIndex = j, SpellId = (uint)message.PlayerModule.favorite_spells_[i].list[(int)j] });
                                    }
                                }

                                foreach (var value in message.PlayerModule.desired_comps_.hashTable)
                                    player.Character.CharacterPropertiesFillCompBook.Add(new CharacterPropertiesFillCompBook { SpellComponentId = (int)value.Key, QuantityToRebuy = value.Value });

                                player.Character.SpellbookFilters = message.PlayerModule.spell_filters_;

                                player.Character.CharacterOptions2 = (int)message.PlayerModule.options2;

                                //message.PlayerModule.m_colGameplayOptions.
                                // TODO: player.Character.GameplayOptions

                                //message.clist todo: these are inventory items
                                //message.ilist todo: these are equipped items
                            }
                            else if (opCode == (uint) PacketOpcode.Evt_Social__FriendsUpdate_ID)
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

                            // TODO: view contents - Inventory, etc..
                        }

                        if (messageCode == (uint)PacketOpcode.Evt_Physics__CreateObject_ID)
                        {
                            var message = CM_Physics.CreateObject.read(binaryReader);

                            if (message.object_id == player.Biota.Id)
                            {
                                // TODO: Should we parse anything else from this message?

                                // JUMP SUIT START
                                // Enable if if you want to create players with default "jump suits"
                                // Normally, this information is automatically generated by ACE at runtime based on the equipped objects
                                // TODO: Why do the characters look black/white? This could be a bug in ACLogView.PlayerExporter or ACE HandleRestoreRetailCharacter or ACE Creature_Networking.CalculateObjDesc
                                /*
                                byte order = 0;
                                foreach (var value in message.objdesc.apChanges)
                                    player.Biota.BiotaPropertiesAnimPart.Add(new BiotaPropertiesAnimPart { AnimationId = value.part_id, Index = value.part_index, Order = order++ });

                                foreach (var value in message.objdesc.subpalettes)
                                    player.Biota.BiotaPropertiesPalette.Add(new BiotaPropertiesPalette { SubPaletteId = value.subID, Offset = value.offset, Length = value.numcolors });

                                order = 0;
                                foreach (var value in message.objdesc.tmChanges)
                                    player.Biota.BiotaPropertiesTextureMap.Add(new BiotaPropertiesTextureMap { Index = value.part_index, OldId = value.old_tex_id, NewId = value.new_tex_id, Order = order++ });
                                */
                                // JUMP SUIT END

                                var position = new ACE.Entity.Position(message.physicsdesc.pos.objcell_id, message.physicsdesc.pos.frame.m_fOrigin.x, message.physicsdesc.pos.frame.m_fOrigin.y, message.physicsdesc.pos.frame.m_fOrigin.z, message.physicsdesc.pos.frame.qx, message.physicsdesc.pos.frame.qy, message.physicsdesc.pos.frame.qz, message.physicsdesc.pos.frame.qw);
                                player.Biota.SetPosition(ACE.Entity.Enum.Properties.PositionType.Location, position, rwLock, out _);
                            }
                        }

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
                            AddOrUpdateSkill(player.Biota, message.stype, message.val, rwLock);
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
                            AddOrUpdateAttribute(player.Biota, (ACE.Entity.Enum.Properties.PropertyAttribute)message.stype, message.val);
                        }
                        else if (messageCode == (uint)PacketOpcode.Evt_Qualities__PrivateUpdateAttributeLevel_ID)
                        {
                            // This doesn't happen in retail
                            throw new Exception("This shouldn't happen");
                        }
                        else if (messageCode == (uint)PacketOpcode.Evt_Qualities__PrivateUpdateAttribute2nd_ID)
                        {
                            var message = CM_Qualities.PrivateUpdateQualityEvent<STypeAttribute2nd, SecondaryAttribute>.read(0, binaryReader);
                            AddOrUpdateAttribute2nd(player.Biota, (ACE.Entity.Enum.Properties.PropertyAttribute2nd)message.stype, message.val);
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

        private void AddOrUpdateAttribute(Biota biota, ACE.Entity.Enum.Properties.PropertyAttribute attributeType, Attribute attribute)
        {
            var entity = biota.BiotaPropertiesAttribute.ToList().FirstOrDefault(r => r.Type == (ushort)attributeType);
            if (entity == null)
            {
                entity = new BiotaPropertiesAttribute { Type = (ushort)attributeType };
                biota.BiotaPropertiesAttribute.Add(entity);
            }
            entity.CPSpent = attribute._cp_spent;
            entity.InitLevel = attribute._init_level;
            entity.LevelFromCP = attribute._level_from_cp;
        }

        private void AddOrUpdateAttribute2nd(Biota biota, ACE.Entity.Enum.Properties.PropertyAttribute2nd attributeType, SecondaryAttribute attribute)
        {
            var entity = biota.BiotaPropertiesAttribute2nd.ToList().FirstOrDefault(r => r.Type == (ushort)attributeType);
            if (entity == null)
            {
                entity = new BiotaPropertiesAttribute2nd { Type = (ushort)attributeType };
                biota.BiotaPropertiesAttribute2nd.Add(entity);
            }
            entity.CPSpent = attribute._cp_spent;
            entity.CurrentLevel = attribute._current_level;
            entity.InitLevel = attribute._init_level;
            entity.LevelFromCP = attribute._level_from_cp;
        }

        private void AddOrUpdateSkill(Biota biota, STypeSkill skillType, Skill skill, ReaderWriterLockSlim rwLock)
        {
            var entity = biota.GetOrAddSkill((ushort)skillType, rwLock, out _);
            entity.InitLevel = skill._init_level;
            entity.LastUsedTime = skill._last_used_time;
            entity.LevelFromPP = (ushort)skill._level_from_pp;
            entity.PP = skill._pp;
            entity.ResistanceAtLastCheck = skill._resistance_of_last_check;
            entity.SAC = (uint)skill._sac;
        }

        private void AddUpdateEnchantment(Biota biota, CM_Magic.Enchantment enchantment, ReaderWriterLockSlim rwLock)
        {
            var entity = new BiotaPropertiesEnchantmentRegistry();
            entity.SpellId = enchantment.eid.i_spell_id;
            entity.LayerId = enchantment.eid.layer;
            entity.SpellCategory = enchantment.spell_category;
            entity.HasSpellSetId = (enchantment.has_spell_set_id != 0);
            entity.PowerLevel = enchantment.power_level;
            entity.StartTime = enchantment.start_time;
            entity.Duration = enchantment.duration;
            entity.CasterObjectId = enchantment.caster;
            entity.DegradeModifier = enchantment.degrade_modifier;
            entity.DegradeLimit = enchantment.degrade_limit;
            entity.LastTimeDegraded = enchantment.last_time_degraded;
            entity.StatModType = enchantment.smod.type;
            entity.StatModKey = enchantment.smod.key;
            entity.StatModValue = enchantment.smod.val;
            entity.SpellSetId = enchantment.spell_set_id;

            biota.AddEnchantment(entity, rwLock);
        }

        public override void WriteOutput(string destinationRoot)
        {
            var biotaWriter = new ACE.Database.SQLFormatters.Shard.BiotaSQLWriter();
            var characterWriter = new ACE.Database.SQLFormatters.Shard.CharacterSQLWriter();

            foreach (var kvp in players)
            {
                if (!Directory.Exists(Path.Combine(destinationRoot, "Player Exports", kvp.Value.ServerName)))
                    Directory.CreateDirectory(Path.Combine(destinationRoot, "Player Exports", kvp.Value.ServerName));

                // Biota
                {
                    var defaultFileName = biotaWriter.GetDefaultFileName(kvp.Value.Biota);

                    var fileName = Path.Combine(destinationRoot, "Player Exports", kvp.Value.ServerName, defaultFileName);

                    using (StreamWriter outputFile = new StreamWriter(fileName, false))
                        biotaWriter.CreateSQLINSERTStatement(kvp.Value.Biota, outputFile);
                }

                // Character
                {
                    var name = kvp.Value.Biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name);

                    var defaultFileName = kvp.Value.Character.Id.ToString("X8") + " " + name + " - Character.sql";

                    var fileName = Path.Combine(destinationRoot, "Player Exports", kvp.Value.ServerName, defaultFileName);

                    using (StreamWriter outputFile = new StreamWriter(fileName, false))
                        characterWriter.CreateSQLINSERTStatement(kvp.Value.Character, outputFile);
                }
            }
        }
    }
}
