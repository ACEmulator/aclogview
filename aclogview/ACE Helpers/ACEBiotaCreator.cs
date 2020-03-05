using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using ACE.Database.Models.Shard;

namespace aclogview.ACE_Helpers
{
    static class ACEBiotaCreator
    {
        public static void AddOrUpdateAttribute(Biota biota, ACE.Entity.Enum.Properties.PropertyAttribute attributeType, Attribute attribute)
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

        public static void AddOrUpdateAttribute2nd(Biota biota, ACE.Entity.Enum.Properties.PropertyAttribute2nd attributeType, SecondaryAttribute attribute)
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

        public static void AddOrUpdateSkill(Biota biota, STypeSkill skillType, Skill skill, ReaderWriterLockSlim rwLock)
        {
            var entity = biota.GetOrAddSkill((ushort)skillType, rwLock, out _);
            entity.InitLevel = skill._init_level;
            entity.LastUsedTime = skill._last_used_time;
            entity.LevelFromPP = (ushort)skill._level_from_pp;
            entity.PP = skill._pp;
            entity.ResistanceAtLastCheck = skill._resistance_of_last_check;
            entity.SAC = (uint)skill._sac;
        }

        private static void AddEnchantment(Biota biota, CM_Magic.Enchantment enchantment, ReaderWriterLockSlim rwLock)
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


        /// <summary>
        /// Do not call this twice for the same  Character or Biota
        /// This should be the first message you parse when constructiong a player
        /// </summary>
        public static void Update(CM_Login.PlayerDescription message, Character character, Biota biota, List<(uint guid, uint containerProperties)> inventory, List<(uint guid, uint location, uint priority)> equipment, ReaderWriterLockSlim rwLock)
        {
            biota.WeenieType = (int)message.CACQualities.CBaseQualities._weenie_type;

            foreach (var kvp in message.CACQualities.CBaseQualities._intStatsTable.hashTable)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyInt)kvp.Key, kvp.Value, rwLock, out _);
            foreach (var kvp in message.CACQualities.CBaseQualities._int64StatsTable.hashTable)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyInt64)kvp.Key, kvp.Value, rwLock, out _);
            foreach (var kvp in message.CACQualities.CBaseQualities._boolStatsTable.hashTable)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyBool)kvp.Key, (kvp.Value != 0), rwLock, out _);
            foreach (var kvp in message.CACQualities.CBaseQualities._floatStatsTable.hashTable)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyFloat)kvp.Key, kvp.Value, rwLock, out _);
            foreach (var kvp in message.CACQualities.CBaseQualities._strStatsTable.hashTable)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyString)kvp.Key, kvp.Value.m_buffer, rwLock, out _);
            foreach (var kvp in message.CACQualities.CBaseQualities._didStatsTable.hashTable)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyDataId)kvp.Key, kvp.Value, rwLock, out _);
            foreach (var kvp in message.CACQualities.CBaseQualities._iidStatsTable.hashTable)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyInstanceId)kvp.Key, kvp.Value, rwLock, out _);
            foreach (var kvp in message.CACQualities.CBaseQualities._posStatsTable.hashTable)
            {
                var position = new ACE.Entity.Position(kvp.Value.objcell_id, kvp.Value.frame.m_fOrigin.x, kvp.Value.frame.m_fOrigin.y, kvp.Value.frame.m_fOrigin.z, kvp.Value.frame.qx, kvp.Value.frame.qy, kvp.Value.frame.qz, kvp.Value.frame.qw);
                biota.SetPosition((ACE.Entity.Enum.Properties.PositionType)kvp.Key, position, rwLock, out _);
            }

            var name = biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name);
            biota.SetProperty(ACE.Entity.Enum.Properties.PropertyString.PCAPRecordedCharacterName, name, rwLock, out _);

            AddOrUpdateAttribute(biota, ACE.Entity.Enum.Properties.PropertyAttribute.Strength, message.CACQualities._attribCache._strength);
            AddOrUpdateAttribute(biota, ACE.Entity.Enum.Properties.PropertyAttribute.Endurance, message.CACQualities._attribCache._endurance);
            AddOrUpdateAttribute(biota, ACE.Entity.Enum.Properties.PropertyAttribute.Quickness, message.CACQualities._attribCache._quickness);
            AddOrUpdateAttribute(biota, ACE.Entity.Enum.Properties.PropertyAttribute.Coordination, message.CACQualities._attribCache._coordination);
            AddOrUpdateAttribute(biota, ACE.Entity.Enum.Properties.PropertyAttribute.Focus, message.CACQualities._attribCache._focus);
            AddOrUpdateAttribute(biota, ACE.Entity.Enum.Properties.PropertyAttribute.Self, message.CACQualities._attribCache._self);

            AddOrUpdateAttribute2nd(biota, ACE.Entity.Enum.Properties.PropertyAttribute2nd.Health, message.CACQualities._attribCache._health);
            AddOrUpdateAttribute2nd(biota, ACE.Entity.Enum.Properties.PropertyAttribute2nd.Stamina, message.CACQualities._attribCache._stamina);
            AddOrUpdateAttribute2nd(biota, ACE.Entity.Enum.Properties.PropertyAttribute2nd.Mana, message.CACQualities._attribCache._mana);

            foreach (var value in message.CACQualities._skillStatsTable.hashTable)
                AddOrUpdateSkill(biota, value.Key, value.Value, rwLock);

            foreach (var value in message.CACQualities._spell_book.hashTable)
            {
                var entry = biota.GetOrAddKnownSpell((int)value.Key, rwLock, out _);
                entry.Probability = value.Value;
            }

            if (message.CACQualities._enchantment_reg != null)
            {
                if (message.CACQualities._enchantment_reg._add_list != null)
                {
                    foreach (var value in message.CACQualities._enchantment_reg._add_list.list)
                        AddEnchantment(biota, value, rwLock);
                }
                if (message.CACQualities._enchantment_reg._cooldown_list != null)
                {
                    foreach (var value in message.CACQualities._enchantment_reg._cooldown_list.list)
                        AddEnchantment(biota, value, rwLock);
                }
                if (message.CACQualities._enchantment_reg._mult_list != null)
                {
                    foreach (var value in message.CACQualities._enchantment_reg._mult_list.list)
                        AddEnchantment(biota, value, rwLock);
                }
                if (message.CACQualities._enchantment_reg._vitae != null && message.CACQualities._enchantment_reg._vitae.eid != null)
                    AddEnchantment(biota, message.CACQualities._enchantment_reg._vitae, rwLock);
            }

            character.CharacterOptions1 = (int)message.PlayerModule.options_;

            if (message.PlayerModule.shortcuts_ != null)
            {
                foreach (var value in message.PlayerModule.shortcuts_.shortCuts_)
                    character.CharacterPropertiesShortcutBar.Add(new CharacterPropertiesShortcutBar { ShortcutBarIndex = (uint)value.index_, ShortcutObjectId = value.objectID_ });
            }

            for (uint i = 0; i < message.PlayerModule.favorite_spells_.Length; i++)
            {
                if (message.PlayerModule.favorite_spells_[i] != null)
                {
                    for (uint j = 0; j < message.PlayerModule.favorite_spells_[i].list.Count; j++)
                        character.CharacterPropertiesSpellBar.Add(new CharacterPropertiesSpellBar { SpellBarNumber = i, SpellBarIndex = j, SpellId = (uint)message.PlayerModule.favorite_spells_[i].list[(int)j] });
                }
            }

            foreach (var value in message.PlayerModule.desired_comps_.hashTable)
                character.CharacterPropertiesFillCompBook.Add(new CharacterPropertiesFillCompBook { SpellComponentId = (int)value.Key, QuantityToRebuy = value.Value });

            character.SpellbookFilters = message.PlayerModule.spell_filters_;

            character.CharacterOptions2 = (int)message.PlayerModule.options2;

            // This is just window placement. For now, we don't bother exporting it
            // TODO: message.PlayerModule.m_colGameplayOptions -> player.Character.GameplayOptions

            foreach (var value in message.clist.list)
                inventory.Add((value.m_iid, value.m_uContainerProperties));

            foreach (var value in message.ilist.list)
                equipment.Add((value.iid_, value.loc_, value.priority_));
        }

        public static void Update(CM_Physics.CreateObject message, Biota biota, ReaderWriterLockSlim rwLock, bool includeMetaData)
        {
            biota.WeenieClassId = message.wdesc._wcid;

            if (includeMetaData)
            {
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyInstanceId)8000, message.object_id, rwLock, out _);

                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyDataId)8001, message.wdesc.header, rwLock, out _);

                if ((message.wdesc._bitfield & (uint)CM_Physics.PublicWeenieDesc.BitfieldIndex.BF_INCLUDES_SECOND_HEADER) != 0)
                    biota.SetProperty((ACE.Entity.Enum.Properties.PropertyDataId)8002, message.wdesc.header2, rwLock, out _);
            }

            biota.SetProperty((ACE.Entity.Enum.Properties.PropertyString)STypeString.NAME_STRING, message.wdesc._name.m_buffer, rwLock, out _);

            biota.SetProperty((ACE.Entity.Enum.Properties.PropertyDataId)STypeDID.ICON_DID, message.wdesc._iconID, rwLock, out _);

            biota.SetProperty((ACE.Entity.Enum.Properties.PropertyInt)STypeInt.ITEM_TYPE_INT, (int)message.wdesc._type, rwLock, out _);

            if (includeMetaData)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyDataId)8003, message.wdesc._bitfield, rwLock, out _);

            if ((message.wdesc.header & (uint)CM_Physics.PublicWeenieDesc.PublicWeenieDescPackHeader.PWD_Packed_PluralName) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyString)STypeString.PLURAL_NAME_STRING, message.wdesc._plural_name.m_buffer, rwLock, out _);

            if ((message.wdesc.header & (uint)CM_Physics.PublicWeenieDesc.PublicWeenieDescPackHeader.PWD_Packed_ItemsCapacity) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyInt)STypeInt.ITEMS_CAPACITY_INT, message.wdesc._itemsCapacity, rwLock, out _);

            if ((message.wdesc.header & (uint)CM_Physics.PublicWeenieDesc.PublicWeenieDescPackHeader.PWD_Packed_ContainersCapacity) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyInt)STypeInt.CONTAINERS_CAPACITY_INT, message.wdesc._containersCapacity, rwLock, out _);

            if ((message.wdesc.header & (uint)CM_Physics.PublicWeenieDesc.PublicWeenieDescPackHeader.PWD_Packed_AmmoType) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyInt)STypeInt.AMMO_TYPE_INT, (int)message.wdesc._ammoType, rwLock, out _);

            if ((message.wdesc.header & (uint)CM_Physics.PublicWeenieDesc.PublicWeenieDescPackHeader.PWD_Packed_Value) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyInt)STypeInt.VALUE_INT, (int)message.wdesc._value, rwLock, out _);

            if ((message.wdesc.header & (uint)CM_Physics.PublicWeenieDesc.PublicWeenieDescPackHeader.PWD_Packed_Useability) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyInt)STypeInt.ITEM_USEABLE_INT, (int)message.wdesc._useability, rwLock, out _);

            if ((message.wdesc.header & (uint)CM_Physics.PublicWeenieDesc.PublicWeenieDescPackHeader.PWD_Packed_UseRadius) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyFloat)STypeFloat.USE_RADIUS_FLOAT, message.wdesc._useRadius, rwLock, out _);

            if ((message.wdesc.header & (uint)CM_Physics.PublicWeenieDesc.PublicWeenieDescPackHeader.PWD_Packed_TargetType) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyInt)STypeInt.TARGET_TYPE_INT, (int)message.wdesc._targetType, rwLock, out _);

            if ((message.wdesc.header & (uint)CM_Physics.PublicWeenieDesc.PublicWeenieDescPackHeader.PWD_Packed_UIEffects) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyInt)STypeInt.UI_EFFECTS_INT, (int)message.wdesc._effects, rwLock, out _);

            if ((message.wdesc.header & (uint)CM_Physics.PublicWeenieDesc.PublicWeenieDescPackHeader.PWD_Packed_CombatUse) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyInt)STypeInt.COMBAT_USE_INT, message.wdesc._combatUse, rwLock, out _);

            if ((message.wdesc.header & (uint)CM_Physics.PublicWeenieDesc.PublicWeenieDescPackHeader.PWD_Packed_Structure) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyInt)STypeInt.STRUCTURE_INT, message.wdesc._structure, rwLock, out _);

            if ((message.wdesc.header & (uint)CM_Physics.PublicWeenieDesc.PublicWeenieDescPackHeader.PWD_Packed_MaxStructure) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyInt)STypeInt.MAX_STRUCTURE_INT, message.wdesc._maxStructure, rwLock, out _);

            if ((message.wdesc.header & (uint)CM_Physics.PublicWeenieDesc.PublicWeenieDescPackHeader.PWD_Packed_StackSize) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyInt)STypeInt.STACK_SIZE_INT, message.wdesc._stackSize, rwLock, out _);

            if ((message.wdesc.header & (uint)CM_Physics.PublicWeenieDesc.PublicWeenieDescPackHeader.PWD_Packed_MaxStackSize) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyInt)STypeInt.MAX_STACK_SIZE_INT, message.wdesc._maxStackSize, rwLock, out _);

            if ((message.wdesc.header & (uint)CM_Physics.PublicWeenieDesc.PublicWeenieDescPackHeader.PWD_Packed_ContainerID) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyInstanceId)STypeIID.CONTAINER_IID, message.wdesc._containerID, rwLock, out _);

            if ((message.wdesc.header & (uint)CM_Physics.PublicWeenieDesc.PublicWeenieDescPackHeader.PWD_Packed_WielderID) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyInstanceId)STypeIID.WIELDER_IID, message.wdesc._wielderID, rwLock, out _);

            if ((message.wdesc.header & (uint)CM_Physics.PublicWeenieDesc.PublicWeenieDescPackHeader.PWD_Packed_ValidLocations) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyInt)STypeInt.LOCATIONS_INT, (int)message.wdesc._valid_locations, rwLock, out _);

            if ((message.wdesc.header & (uint)CM_Physics.PublicWeenieDesc.PublicWeenieDescPackHeader.PWD_Packed_Location) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyInt)STypeInt.CURRENT_WIELDED_LOCATION_INT, (int)message.wdesc._location, rwLock, out _);

            if ((message.wdesc.header & (uint)CM_Physics.PublicWeenieDesc.PublicWeenieDescPackHeader.PWD_Packed_Priority) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyInt)STypeInt.CLOTHING_PRIORITY_INT, (int)message.wdesc._priority, rwLock, out _);

            if ((message.wdesc.header & (uint)CM_Physics.PublicWeenieDesc.PublicWeenieDescPackHeader.PWD_Packed_BlipColor) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyInt)STypeInt.RADARBLIP_COLOR_INT, message.wdesc._blipColor, rwLock, out _);

            if ((message.wdesc.header & (uint)CM_Physics.PublicWeenieDesc.PublicWeenieDescPackHeader.PWD_Packed_RadarEnum) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyInt)STypeInt.SHOWABLE_ON_RADAR_INT, (int)message.wdesc._radar_enum, rwLock, out _);

            if ((message.wdesc.header & (uint)CM_Physics.PublicWeenieDesc.PublicWeenieDescPackHeader.PWD_Packed_PScript) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyDataId)STypeDID.PHYSICS_SCRIPT_DID, message.wdesc._pscript, rwLock, out _);

            if (includeMetaData)
            {
                if ((message.wdesc.header & (uint)CM_Physics.PublicWeenieDesc.PublicWeenieDescPackHeader.PWD_Packed_Workmanship) != 0)
                    biota.SetProperty((ACE.Entity.Enum.Properties.PropertyFloat)8004, message.wdesc._workmanship, rwLock, out _);
            }

            if ((message.wdesc.header & (uint)CM_Physics.PublicWeenieDesc.PublicWeenieDescPackHeader.PWD_Packed_Burden) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyInt)STypeInt.ENCUMB_VAL_INT, message.wdesc._burden, rwLock, out _);

            if ((message.wdesc.header & (uint)CM_Physics.PublicWeenieDesc.PublicWeenieDescPackHeader.PWD_Packed_SpellID) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyDataId)STypeDID.SPELL_DID, message.wdesc._spellID, rwLock, out _);

            if ((message.wdesc.header & (uint)CM_Physics.PublicWeenieDesc.PublicWeenieDescPackHeader.PWD_Packed_HouseOwner) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyInstanceId)STypeIID.HOUSE_OWNER_IID, message.wdesc._house_owner_iid, rwLock, out _);

            //if ((message.wdesc.header & (uint)CM_Physics.PublicWeenieDesc.PublicWeenieDescPackHeader.PWD_Packed_HouseRestrictions) != 0)
            //    result.SetProperty((ACE.Entity.Enum.Properties.PropertyInstanceId)(int)STypeIID.WIELDER_IID, Value = message.wdesc._wielderID });

            if ((message.wdesc.header & (uint)CM_Physics.PublicWeenieDesc.PublicWeenieDescPackHeader.PWD_Packed_HookItemTypes) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyInt)STypeInt.HOOK_ITEM_TYPE_INT, (int)message.wdesc._hook_item_types, rwLock, out _);

            if ((message.wdesc.header & (uint)CM_Physics.PublicWeenieDesc.PublicWeenieDescPackHeader.PWD_Packed_Monarch) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyInstanceId)STypeIID.MONARCH_IID, message.wdesc._monarch, rwLock, out _);

            if ((message.wdesc.header & (uint)CM_Physics.PublicWeenieDesc.PublicWeenieDescPackHeader.PWD_Packed_HookType) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyInt)STypeInt.HOOK_TYPE_INT, message.wdesc._hook_type, rwLock, out _);

            if ((message.wdesc.header & (uint)CM_Physics.PublicWeenieDesc.PublicWeenieDescPackHeader.PWD_Packed_IconOverlay) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyDataId)STypeDID.ICON_OVERLAY_DID, message.wdesc._iconOverlayID, rwLock, out _);

            if ((message.wdesc.header2 & (uint)CM_Physics.PublicWeenieDesc.PublicWeenieDescPackHeader2.PWD2_Packed_IconUnderlay) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyDataId)STypeDID.ICON_UNDERLAY_DID, message.wdesc._iconUnderlayID, rwLock, out _);

            if ((message.wdesc.header & unchecked((uint)CM_Physics.PublicWeenieDesc.PublicWeenieDescPackHeader.PWD_Packed_MaterialType)) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyInt)STypeInt.MATERIAL_TYPE_INT, (int)message.wdesc._material_type, rwLock, out _);

            if ((message.wdesc.header2 & (uint)CM_Physics.PublicWeenieDesc.PublicWeenieDescPackHeader2.PWD2_Packed_CooldownID) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyInt)STypeInt.SHARED_COOLDOWN_INT, (int)message.wdesc._cooldown_id, rwLock, out _);

            if ((message.wdesc.header2 & (uint)CM_Physics.PublicWeenieDesc.PublicWeenieDescPackHeader2.PWD2_Packed_CooldownDuration) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyFloat)STypeFloat.COOLDOWN_DURATION_FLOAT, message.wdesc._cooldown_duration, rwLock, out _);

            if ((message.wdesc.header2 & (uint)CM_Physics.PublicWeenieDesc.PublicWeenieDescPackHeader2.PWD2_Packed_PetOwner) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyInstanceId)STypeIID.PET_OWNER_IID, message.wdesc._pet_owner, rwLock, out _);

            if ((message.wdesc._bitfield & (uint)CM_Physics.PublicWeenieDesc.BitfieldIndex.BF_ADMIN) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyBool)STypeBool.IS_ADMIN_BOOL, true, rwLock, out _);
            if ((message.wdesc._bitfield & (uint)CM_Physics.PublicWeenieDesc.BitfieldIndex.BF_ATTACKABLE) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyBool)STypeBool.ATTACKABLE_BOOL, true, rwLock, out _);
            else
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyBool)STypeBool.ATTACKABLE_BOOL, false, rwLock, out _);
            if ((message.wdesc._bitfield & (uint)CM_Physics.PublicWeenieDesc.BitfieldIndex.BF_HIDDEN_ADMIN) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyBool)STypeBool.HIDDEN_ADMIN_BOOL, true, rwLock, out _);
            if ((message.wdesc._bitfield & (uint)CM_Physics.PublicWeenieDesc.BitfieldIndex.BF_IMMUNE_CELL_RESTRICTIONS) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyBool)STypeBool.IGNORE_HOUSE_BARRIERS_BOOL, true, rwLock, out _);
            if ((message.wdesc._bitfield & (uint)CM_Physics.PublicWeenieDesc.BitfieldIndex.BF_INSCRIBABLE) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyBool)STypeBool.INSCRIBABLE_BOOL, true, rwLock, out _);
            if ((message.wdesc._bitfield & (uint)CM_Physics.PublicWeenieDesc.BitfieldIndex.BF_REQUIRES_PACKSLOT) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyBool)STypeBool.REQUIRES_BACKPACK_SLOT_BOOL, true, rwLock, out _);
            if ((message.wdesc._bitfield & (uint)CM_Physics.PublicWeenieDesc.BitfieldIndex.BF_RETAINED) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyBool)STypeBool.RETAINED_BOOL, true, rwLock, out _);
            if ((message.wdesc._bitfield & (uint)CM_Physics.PublicWeenieDesc.BitfieldIndex.BF_STUCK) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyBool)STypeBool.STUCK_BOOL, true, rwLock, out _);
            else
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyBool)STypeBool.STUCK_BOOL, false, rwLock, out _);
            if ((message.wdesc._bitfield & (uint)CM_Physics.PublicWeenieDesc.BitfieldIndex.BF_UI_HIDDEN) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyBool)STypeBool.UI_HIDDEN_BOOL, true, rwLock, out _);
            if ((message.wdesc._bitfield & (uint)CM_Physics.PublicWeenieDesc.BitfieldIndex.BF_WIELD_LEFT) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyBool)STypeBool.AUTOWIELD_LEFT_BOOL, true, rwLock, out _);
            if ((message.wdesc._bitfield & (uint)CM_Physics.PublicWeenieDesc.BitfieldIndex.BF_WIELD_ON_USE) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyBool)STypeBool.WIELD_ON_USE_BOOL, true, rwLock, out _);

            if (message.objdesc.subpalettes.Count > 0)
            {
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyDataId)STypeDID.PALETTE_BASE_DID, message.objdesc.paletteID, rwLock, out _);

                biota.BiotaPropertiesPalette.Clear();
                foreach (var subpalette in message.objdesc.subpalettes)
                    biota.BiotaPropertiesPalette.Add(new BiotaPropertiesPalette { SubPaletteId = subpalette.subID, Offset = subpalette.offset, Length = subpalette.numcolors });
            }

            if (message.objdesc.tmChanges.Count > 0)
            {
                byte order = 0;
                biota.BiotaPropertiesTextureMap.Clear();
                foreach (var texture in message.objdesc.tmChanges)
                    biota.BiotaPropertiesTextureMap.Add(new BiotaPropertiesTextureMap { Index = texture.part_index, OldId = texture.old_tex_id, NewId = texture.new_tex_id, Order = order++ });
            }

            if (message.objdesc.apChanges.Count > 0)
            {
                byte order = 0;
                biota.BiotaPropertiesAnimPart.Clear();
                foreach (var animPart in message.objdesc.apChanges)
                    biota.BiotaPropertiesAnimPart.Add(new BiotaPropertiesAnimPart { Index = animPart.part_index, AnimationId = animPart.part_id, Order = order++ });
            }

            if (includeMetaData)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyDataId)8005, message.physicsdesc.bitfield, rwLock, out _);

            biota.SetProperty((ACE.Entity.Enum.Properties.PropertyInt)STypeInt.PHYSICS_STATE_INT, (int)message.physicsdesc.state, rwLock, out _);

            if (includeMetaData)
            {
                if ((message.physicsdesc.bitfield & (uint)CM_Physics.PhysicsDesc.PhysicsDescInfo.MOVEMENT) != 0)
                {
                    // message.physicsdesc.CMS is not implemented
                    //result.SetProperty((ACE.Entity.Enum.Properties.PropertyString)8006, Value = ConvertMovementBufferToString(message.physicsdesc.CMS) });
                    biota.SetProperty((ACE.Entity.Enum.Properties.PropertyInt)8007, message.physicsdesc.autonomous_movement, rwLock, out _);
                }
            }

            if ((message.physicsdesc.bitfield & (uint)CM_Physics.PhysicsDesc.PhysicsDescInfo.ANIMFRAME_ID) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyInt)STypeInt.PLACEMENT_INT, (int)message.physicsdesc.animframe_id, rwLock, out _);

            if (includeMetaData)
            {
                if ((message.physicsdesc.bitfield & (uint)CM_Physics.PhysicsDesc.PhysicsDescInfo.POSITION) != 0)
                    biota.BiotaPropertiesPosition.Add(
                        new BiotaPropertiesPosition
                        {
                            PositionType = 8040,
                            ObjCellId = message.physicsdesc.pos.objcell_id,
                            OriginX = message.physicsdesc.pos.frame.m_fOrigin.x,
                            OriginY = message.physicsdesc.pos.frame.m_fOrigin.y,
                            OriginZ = message.physicsdesc.pos.frame.m_fOrigin.z,
                            AnglesW = message.physicsdesc.pos.frame.qw,
                            AnglesX = message.physicsdesc.pos.frame.qx,
                            AnglesY = message.physicsdesc.pos.frame.qy,
                            AnglesZ = message.physicsdesc.pos.frame.qz
                        });
            }

            if ((message.physicsdesc.bitfield & (uint)CM_Physics.PhysicsDesc.PhysicsDescInfo.MTABLE) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyDataId)STypeDID.MOTION_TABLE_DID, message.physicsdesc.mtable_id, rwLock, out _);

            if ((message.physicsdesc.bitfield & (uint)CM_Physics.PhysicsDesc.PhysicsDescInfo.STABLE) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyDataId)STypeDID.SOUND_TABLE_DID, message.physicsdesc.stable_id, rwLock, out _);

            if ((message.physicsdesc.bitfield & (uint)CM_Physics.PhysicsDesc.PhysicsDescInfo.PETABLE) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyDataId)STypeDID.PHYSICS_EFFECT_TABLE_DID, message.physicsdesc.phstable_id, rwLock, out _);

            if ((message.physicsdesc.bitfield & (uint)CM_Physics.PhysicsDesc.PhysicsDescInfo.CSetup) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyDataId)STypeDID.SETUP_DID, message.physicsdesc.setup_id, rwLock, out _);

            if (includeMetaData)
            {
                if ((message.physicsdesc.bitfield & (uint)CM_Physics.PhysicsDesc.PhysicsDescInfo.PARENT) != 0)
                {
                    biota.SetProperty((ACE.Entity.Enum.Properties.PropertyInstanceId)8008, message.physicsdesc.parent_id, rwLock, out _);
                    biota.SetProperty((ACE.Entity.Enum.Properties.PropertyDataId)8009, message.physicsdesc.location_id, rwLock, out _);
                }

                if ((message.physicsdesc.bitfield & (uint)CM_Physics.PhysicsDesc.PhysicsDescInfo.CHILDREN) != 0)
                {
                    //result.SetProperty((ACE.Entity.Enum.Properties.PropertyInstanceId)8008, Value = message.physicsdesc.parent_id });
                    //result.SetProperty((ACE.Entity.Enum.Properties.PropertyDataId)8009, Value = message.physicsdesc.location_id });
                }
            }

            if ((message.physicsdesc.bitfield & (uint)CM_Physics.PhysicsDesc.PhysicsDescInfo.OBJSCALE) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyFloat)STypeFloat.DEFAULT_SCALE_FLOAT, message.physicsdesc.object_scale, rwLock, out _);

            if ((message.physicsdesc.bitfield & (uint)CM_Physics.PhysicsDesc.PhysicsDescInfo.FRICTION) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyFloat)STypeFloat.FRICTION_FLOAT, message.physicsdesc.friction, rwLock, out _);

            if ((message.physicsdesc.bitfield & (uint)CM_Physics.PhysicsDesc.PhysicsDescInfo.ELASTICITY) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyFloat)STypeFloat.ELASTICITY_FLOAT, message.physicsdesc.elasticity, rwLock, out _);

            if ((message.physicsdesc.bitfield & (uint)CM_Physics.PhysicsDesc.PhysicsDescInfo.TRANSLUCENCY) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyFloat)STypeFloat.TRANSLUCENCY_FLOAT, message.physicsdesc.translucency, rwLock, out _);

            if (includeMetaData)
            {
                if ((message.physicsdesc.bitfield & (uint)CM_Physics.PhysicsDesc.PhysicsDescInfo.VELOCITY) != 0)
                {
                    biota.SetProperty((ACE.Entity.Enum.Properties.PropertyFloat)8010, message.physicsdesc.velocity.x, rwLock, out _);
                    biota.SetProperty((ACE.Entity.Enum.Properties.PropertyFloat)8011, message.physicsdesc.velocity.y, rwLock, out _);
                    biota.SetProperty((ACE.Entity.Enum.Properties.PropertyFloat)8012, message.physicsdesc.velocity.z, rwLock, out _);
                }

                if ((message.physicsdesc.bitfield & (uint)CM_Physics.PhysicsDesc.PhysicsDescInfo.ACCELERATION) != 0)
                {
                    biota.SetProperty((ACE.Entity.Enum.Properties.PropertyFloat)8013, message.physicsdesc.acceleration.x, rwLock, out _);
                    biota.SetProperty((ACE.Entity.Enum.Properties.PropertyFloat)8014, message.physicsdesc.acceleration.y, rwLock, out _);
                    biota.SetProperty((ACE.Entity.Enum.Properties.PropertyFloat)8015, message.physicsdesc.acceleration.z, rwLock, out _);
                }

                if ((message.physicsdesc.bitfield & (uint)CM_Physics.PhysicsDesc.PhysicsDescInfo.OMEGA) != 0)
                {
                    biota.SetProperty((ACE.Entity.Enum.Properties.PropertyFloat)8016, message.physicsdesc.omega.x, rwLock, out _);
                    biota.SetProperty((ACE.Entity.Enum.Properties.PropertyFloat)8017, message.physicsdesc.omega.y, rwLock, out _);
                    biota.SetProperty((ACE.Entity.Enum.Properties.PropertyFloat)8018, message.physicsdesc.omega.z, rwLock, out _);
                }

                if ((message.physicsdesc.bitfield & (uint)CM_Physics.PhysicsDesc.PhysicsDescInfo.DEFAULT_SCRIPT) != 0)
                    biota.SetProperty((ACE.Entity.Enum.Properties.PropertyDataId)8019, (uint)message.physicsdesc.default_script, rwLock, out _);
            }

            if ((message.physicsdesc.bitfield & (uint)CM_Physics.PhysicsDesc.PhysicsDescInfo.DEFAULT_SCRIPT_INTENSITY) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyFloat)STypeFloat.PHYSICS_SCRIPT_INTENSITY_FLOAT, message.physicsdesc.default_script_intensity, rwLock, out _);

            if (includeMetaData)
            {
                if ((message.physicsdesc.bitfield & (uint)CM_Physics.PhysicsDesc.PhysicsDescInfo.TIMESTAMPS) != 0)
                {
                    for (int i = 0; i < message.physicsdesc.timestamps.Length; ++i)
                        biota.SetProperty((ACE.Entity.Enum.Properties.PropertyDataId)(8020 + i), message.physicsdesc.timestamps[i - 1], rwLock, out _);
                }
            }

            if ((message.physicsdesc.state & (uint)PhysicsState.STATIC_PS) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyBool)STypeBool.STUCK_BOOL, true, rwLock, out _);
            if ((message.physicsdesc.state & (uint)PhysicsState.ETHEREAL_PS) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyBool)STypeBool.ETHEREAL_BOOL, true, rwLock, out _);
            else
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyBool)STypeBool.ETHEREAL_BOOL, false, rwLock, out _);
            if ((message.physicsdesc.state & (uint)PhysicsState.REPORT_COLLISIONS_PS) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyBool)STypeBool.REPORT_COLLISIONS_BOOL, true, rwLock, out _);
            if ((message.physicsdesc.state & (uint)PhysicsState.IGNORE_COLLISIONS_PS) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyBool)STypeBool.IGNORE_COLLISIONS_BOOL, true, rwLock, out _);
            if ((message.physicsdesc.state & (uint)PhysicsState.NODRAW_PS) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyBool)STypeBool.NODRAW_BOOL, true, rwLock, out _);
            if ((message.physicsdesc.state & (uint)PhysicsState.GRAVITY_PS) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyBool)STypeBool.GRAVITY_STATUS_BOOL, true, rwLock, out _);
            if ((message.physicsdesc.state & (uint)PhysicsState.LIGHTING_ON_PS) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyBool)STypeBool.LIGHTS_STATUS_BOOL, true, rwLock, out _);
            //if ((message.physicsdesc.state & (uint)PhysicsState.HIDDEN_PS) != 0)
            //    result.SetProperty((ACE.Entity.Enum.Properties.PropertyBool)STypeBool.VISIBILITY_BOOL, true, rwLock, out _);
            if ((message.physicsdesc.state & (uint)PhysicsState.SCRIPTED_COLLISION_PS) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyBool)STypeBool.SCRIPTED_COLLISION_BOOL, true, rwLock, out _);
            if ((message.physicsdesc.state & (uint)PhysicsState.INELASTIC_PS) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyBool)STypeBool.INELASTIC_BOOL, true, rwLock, out _);
            //if ((message.physicsdesc.state & (uint)PhysicsState.CLOAKED_PS) != 0)
            //    result.SetProperty((ACE.Entity.Enum.Properties.PropertyBool)STypeBool.HIDDEN_ADMIN_BOOL, true, rwLock, out _);
            if ((message.physicsdesc.state & (uint)PhysicsState.REPORT_COLLISIONS_AS_ENVIRONMENT_PS) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyBool)STypeBool.REPORT_COLLISIONS_AS_ENVIRONMENT_BOOL, true, rwLock, out _);
            if ((message.physicsdesc.state & (uint)PhysicsState.EDGE_SLIDE_PS) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyBool)STypeBool.ALLOW_EDGE_SLIDE_BOOL, true, rwLock, out _);
            if ((message.physicsdesc.state & (uint)PhysicsState.FROZEN_PS) != 0)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyBool)STypeBool.IS_FROZEN_BOOL, true, rwLock, out _);
        }

        public static void Update(CM_Examine.SetAppraiseInfo message, Biota biota, ReaderWriterLockSlim rwLock)
        {
            foreach (var kvp in message.i_prof._intStatsTable.hashTable)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyInt)kvp.Key, kvp.Value, rwLock, out _);
            foreach (var kvp in message.i_prof._int64StatsTable.hashTable)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyInt64)kvp.Key, kvp.Value, rwLock, out _);
            foreach (var kvp in message.i_prof._boolStatsTable.hashTable)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyBool)kvp.Key, (kvp.Value != 0), rwLock, out _);
            foreach (var kvp in message.i_prof._floatStatsTable.hashTable)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyFloat)kvp.Key, kvp.Value, rwLock, out _);
            foreach (var kvp in message.i_prof._strStatsTable.hashTable)
            {
                if (kvp.Value.m_buffer == null) continue; // Not sure why some strings are null
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyString) kvp.Key, kvp.Value.m_buffer, rwLock, out _);
            }

            foreach (var kvp in message.i_prof._didStatsTable.hashTable)
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyDataId)kvp.Key, kvp.Value, rwLock, out _);

            foreach (var spell in message.i_prof._spellsTable.list)
            {
                if ((spell & 0x80000000) != 0) // These are enchantments
                {
                    var enchantment = (spell & 0x7FFFFFFF);
                    if (biota.BiotaPropertiesEnchantmentRegistry.All(y => y.SpellId != (int)enchantment))
                        biota.BiotaPropertiesEnchantmentRegistry.Add(new BiotaPropertiesEnchantmentRegistry{ SpellId = (int)enchantment });
                }
                else
                {
                    if (biota.BiotaPropertiesSpellBook.All(y => y.Spell != (int)spell))
                        biota.BiotaPropertiesSpellBook.Add(new BiotaPropertiesSpellBook { Spell = (int)spell, Probability = 2f });
                }
            }

            if ((message.i_prof.header & (uint)CM_Examine.AppraisalProfile.AppraisalProfilePackHeader.Packed_ArmorProfile) != 0)
            {
                biota.SetProperty(ACE.Entity.Enum.Properties.PropertyFloat.ArmorModVsSlash, message.i_prof._armorProfileTable._mod_vs_slash, rwLock, out _);
                biota.SetProperty(ACE.Entity.Enum.Properties.PropertyFloat.ArmorModVsPierce, message.i_prof._armorProfileTable._mod_vs_pierce, rwLock, out _);
                biota.SetProperty(ACE.Entity.Enum.Properties.PropertyFloat.ArmorModVsBludgeon, message.i_prof._armorProfileTable._mod_vs_bludgeon, rwLock, out _);
                biota.SetProperty(ACE.Entity.Enum.Properties.PropertyFloat.ArmorModVsCold, message.i_prof._armorProfileTable._mod_vs_cold, rwLock, out _);
                biota.SetProperty(ACE.Entity.Enum.Properties.PropertyFloat.ArmorModVsFire, message.i_prof._armorProfileTable._mod_vs_fire, rwLock, out _);
                biota.SetProperty(ACE.Entity.Enum.Properties.PropertyFloat.ArmorModVsAcid, message.i_prof._armorProfileTable._mod_vs_acid, rwLock, out _);
                biota.SetProperty(ACE.Entity.Enum.Properties.PropertyFloat.ArmorModVsNether, message.i_prof._armorProfileTable._mod_vs_nether, rwLock, out _);
                biota.SetProperty(ACE.Entity.Enum.Properties.PropertyFloat.ArmorModVsElectric, message.i_prof._armorProfileTable._mod_vs_electric, rwLock, out _);
            }

            // todo
            /*if ((message.i_prof.header & (uint)CM_Examine.AppraisalProfile.AppraisalProfilePackHeader.Packed_CreatureProfile) != 0)
            {
                if (!weenies[worldIDQueue[currentWorld][message.i_objid]].WeeniePropertiesAttribute2nd.Any(y => y.Type == (ushort)ACE.Entity.Enum.Properties.PropertyAttribute2nd.MaxHealth))
                    weenies[worldIDQueue[currentWorld][message.i_objid]].WeeniePropertiesAttribute2nd.Add(new ACE.Database.Models.World.WeeniePropertiesAttribute2nd { Type = (ushort)ACE.Entity.Enum.Properties.PropertyAttribute2nd.MaxHealth, CurrentLevel = message.i_prof._creatureProfileTable._health, InitLevel = 10 });

                if ((message.i_prof._creatureProfileTable._header & (uint)CM_Examine.CreatureAppraisalProfile.CreatureAppraisalProfilePackHeader.Packed_Attributes) != 0)
                {
                    if (!weenies[worldIDQueue[currentWorld][message.i_objid]].WeeniePropertiesAttribute.Any(y => y.Type == (ushort)ACE.Entity.Enum.Properties.PropertyAttribute.Strength))
                        weenies[worldIDQueue[currentWorld][message.i_objid]].WeeniePropertiesAttribute.Add(new ACE.Database.Models.World.WeeniePropertiesAttribute { Type = (ushort)ACE.Entity.Enum.Properties.PropertyAttribute.Strength, InitLevel = message.i_prof._creatureProfileTable._strength });
                    if (!weenies[worldIDQueue[currentWorld][message.i_objid]].WeeniePropertiesAttribute.Any(y => y.Type == (ushort)ACE.Entity.Enum.Properties.PropertyAttribute.Endurance))
                        weenies[worldIDQueue[currentWorld][message.i_objid]].WeeniePropertiesAttribute.Add(new ACE.Database.Models.World.WeeniePropertiesAttribute { Type = (ushort)ACE.Entity.Enum.Properties.PropertyAttribute.Endurance, InitLevel = message.i_prof._creatureProfileTable._endurance });
                    if (!weenies[worldIDQueue[currentWorld][message.i_objid]].WeeniePropertiesAttribute.Any(y => y.Type == (ushort)ACE.Entity.Enum.Properties.PropertyAttribute.Quickness))
                        weenies[worldIDQueue[currentWorld][message.i_objid]].WeeniePropertiesAttribute.Add(new ACE.Database.Models.World.WeeniePropertiesAttribute { Type = (ushort)ACE.Entity.Enum.Properties.PropertyAttribute.Quickness, InitLevel = message.i_prof._creatureProfileTable._quickness });
                    if (!weenies[worldIDQueue[currentWorld][message.i_objid]].WeeniePropertiesAttribute.Any(y => y.Type == (ushort)ACE.Entity.Enum.Properties.PropertyAttribute.Coordination))
                        weenies[worldIDQueue[currentWorld][message.i_objid]].WeeniePropertiesAttribute.Add(new ACE.Database.Models.World.WeeniePropertiesAttribute { Type = (ushort)ACE.Entity.Enum.Properties.PropertyAttribute.Coordination, InitLevel = message.i_prof._creatureProfileTable._coordination });
                    if (!weenies[worldIDQueue[currentWorld][message.i_objid]].WeeniePropertiesAttribute.Any(y => y.Type == (ushort)ACE.Entity.Enum.Properties.PropertyAttribute.Focus))
                        weenies[worldIDQueue[currentWorld][message.i_objid]].WeeniePropertiesAttribute.Add(new ACE.Database.Models.World.WeeniePropertiesAttribute { Type = (ushort)ACE.Entity.Enum.Properties.PropertyAttribute.Focus, InitLevel = message.i_prof._creatureProfileTable._focus });
                    if (!weenies[worldIDQueue[currentWorld][message.i_objid]].WeeniePropertiesAttribute.Any(y => y.Type == (ushort)ACE.Entity.Enum.Properties.PropertyAttribute.Self))
                        weenies[worldIDQueue[currentWorld][message.i_objid]].WeeniePropertiesAttribute.Add(new ACE.Database.Models.World.WeeniePropertiesAttribute { Type = (ushort)ACE.Entity.Enum.Properties.PropertyAttribute.Self, InitLevel = message.i_prof._creatureProfileTable._self });

                    if (!weenies[worldIDQueue[currentWorld][message.i_objid]].WeeniePropertiesAttribute2nd.Any(y => y.Type == (ushort)ACE.Entity.Enum.Properties.PropertyAttribute2nd.MaxStamina))
                        weenies[worldIDQueue[currentWorld][message.i_objid]].WeeniePropertiesAttribute2nd.Add(new ACE.Database.Models.World.WeeniePropertiesAttribute2nd { Type = (ushort)ACE.Entity.Enum.Properties.PropertyAttribute2nd.MaxStamina, CurrentLevel = message.i_prof._creatureProfileTable._stamina, InitLevel = 10 });
                    if (!weenies[worldIDQueue[currentWorld][message.i_objid]].WeeniePropertiesAttribute2nd.Any(y => y.Type == (ushort)ACE.Entity.Enum.Properties.PropertyAttribute2nd.MaxMana))
                        weenies[worldIDQueue[currentWorld][message.i_objid]].WeeniePropertiesAttribute2nd.Add(new ACE.Database.Models.World.WeeniePropertiesAttribute2nd { Type = (ushort)ACE.Entity.Enum.Properties.PropertyAttribute2nd.MaxMana, CurrentLevel = message.i_prof._creatureProfileTable._mana, InitLevel = 10 });
                }
            }*/

            if ((message.i_prof.header & (uint)CM_Examine.AppraisalProfile.AppraisalProfilePackHeader.Packed_WeaponProfile) != 0)
            {
                biota.SetProperty(ACE.Entity.Enum.Properties.PropertyInt.DamageType, (int)message.i_prof._weaponProfileTable._damage_type, rwLock, out _);
                biota.SetProperty(ACE.Entity.Enum.Properties.PropertyInt.WeaponTime, (int)message.i_prof._weaponProfileTable._weapon_time, rwLock, out _);
                biota.SetProperty(ACE.Entity.Enum.Properties.PropertyInt.WeaponSkill, (int)message.i_prof._weaponProfileTable._weapon_skill, rwLock, out _);
                biota.SetProperty(ACE.Entity.Enum.Properties.PropertyInt.Damage, (int)message.i_prof._weaponProfileTable._weapon_damage, rwLock, out _);
                biota.SetProperty(ACE.Entity.Enum.Properties.PropertyFloat.DamageVariance, message.i_prof._weaponProfileTable._damage_variance, rwLock, out _);
                biota.SetProperty(ACE.Entity.Enum.Properties.PropertyFloat.DamageMod, message.i_prof._weaponProfileTable._damage_mod, rwLock, out _);
                biota.SetProperty(ACE.Entity.Enum.Properties.PropertyFloat.WeaponLength, message.i_prof._weaponProfileTable._weapon_length, rwLock, out _);
                biota.SetProperty(ACE.Entity.Enum.Properties.PropertyFloat.MaximumVelocity, message.i_prof._weaponProfileTable._max_velocity, rwLock, out _);
                biota.SetProperty(ACE.Entity.Enum.Properties.PropertyFloat.WeaponOffense, message.i_prof._weaponProfileTable._weapon_offense, rwLock, out _);
                biota.SetProperty((ACE.Entity.Enum.Properties.PropertyInt)8030, (int)message.i_prof._weaponProfileTable._max_velocity_estimated, rwLock, out _);
            }

            if ((message.i_prof.header & (uint)CM_Examine.AppraisalProfile.AppraisalProfilePackHeader.Packed_HookProfile) != 0)
            {
            }

            // todo message.i_prof._armorEnchantment

            // todo message.i_prof._weaponEnchantment

            // todo message.i_prof._resistEnchantment

            if ((message.i_prof.header & (uint)CM_Examine.AppraisalProfile.AppraisalProfilePackHeader.Packed_ArmorLevels) != 0)
            {
                // todo
            }
        }

        public static void Update(ACE.Entity.Enum.Properties.PositionType positionType, Position position, Biota biota, ReaderWriterLockSlim rwLock)
        {
            var newPosition = new ACE.Entity.Position(position.objcell_id, position.frame.m_fOrigin.x, position.frame.m_fOrigin.y, position.frame.m_fOrigin.z, position.frame.qx, position.frame.qy, position.frame.qz, position.frame.qw);

            biota.SetPosition(positionType, newPosition, rwLock, out _);
        }

        public static ACE.Entity.Enum.WeenieType DetermineWeenieType(Biota biota, ReaderWriterLockSlim rwLock)
        {
            var objectDescriptionFlagProperty = biota.GetProperty((ACE.Entity.Enum.Properties.PropertyDataId) 8003, rwLock);

            if (objectDescriptionFlagProperty == null)
                return ACE.Entity.Enum.WeenieType.Undef;

            var objectDescriptionFlag = (ACE.Entity.Enum.ObjectDescriptionFlag)objectDescriptionFlagProperty;

            if (objectDescriptionFlag.HasFlag(ACE.Entity.Enum.ObjectDescriptionFlag.LifeStone))
                return ACE.Entity.Enum.WeenieType.LifeStone;
            if (objectDescriptionFlag.HasFlag(ACE.Entity.Enum.ObjectDescriptionFlag.BindStone))
                return ACE.Entity.Enum.WeenieType.AllegianceBindstone;
            if (objectDescriptionFlag.HasFlag(ACE.Entity.Enum.ObjectDescriptionFlag.PkSwitch))
                return ACE.Entity.Enum.WeenieType.PKModifier;
            if (objectDescriptionFlag.HasFlag(ACE.Entity.Enum.ObjectDescriptionFlag.NpkSwitch))
                return ACE.Entity.Enum.WeenieType.PKModifier;
            if (objectDescriptionFlag.HasFlag(ACE.Entity.Enum.ObjectDescriptionFlag.Lockpick))
                return ACE.Entity.Enum.WeenieType.Lockpick;
            if (objectDescriptionFlag.HasFlag(ACE.Entity.Enum.ObjectDescriptionFlag.Food))
                return ACE.Entity.Enum.WeenieType.Food;
            if (objectDescriptionFlag.HasFlag(ACE.Entity.Enum.ObjectDescriptionFlag.Healer))
                return ACE.Entity.Enum.WeenieType.Healer;
            if (objectDescriptionFlag.HasFlag(ACE.Entity.Enum.ObjectDescriptionFlag.Book))
                return ACE.Entity.Enum.WeenieType.Book;
            if (objectDescriptionFlag.HasFlag(ACE.Entity.Enum.ObjectDescriptionFlag.Portal))
                return ACE.Entity.Enum.WeenieType.Portal;
            if (objectDescriptionFlag.HasFlag(ACE.Entity.Enum.ObjectDescriptionFlag.Door))
                return ACE.Entity.Enum.WeenieType.Door;
            if (objectDescriptionFlag.HasFlag(ACE.Entity.Enum.ObjectDescriptionFlag.Vendor))
                return ACE.Entity.Enum.WeenieType.Vendor;
            if (objectDescriptionFlag.HasFlag(ACE.Entity.Enum.ObjectDescriptionFlag.Admin))
                return ACE.Entity.Enum.WeenieType.Admin;
            if (objectDescriptionFlag.HasFlag(ACE.Entity.Enum.ObjectDescriptionFlag.Corpse))
                return ACE.Entity.Enum.WeenieType.Corpse;

            if (biota.GetProperty(ACE.Entity.Enum.Properties.PropertyInt.ValidLocations, rwLock) == (int)ACE.Entity.Enum.EquipMask.MissileAmmo)
                return ACE.Entity.Enum.WeenieType.Ammunition;

            var itemTypeProperty = biota.GetProperty(ACE.Entity.Enum.Properties.PropertyInt.ItemType, rwLock);

            if (itemTypeProperty == null)
                return ACE.Entity.Enum.WeenieType.Undef;

            var itemType = (ACE.Entity.Enum.ItemType)itemTypeProperty;

            switch (itemType)
            {
                case ACE.Entity.Enum.ItemType.Misc:
                    if (
                           biota.WeenieClassId == 9548 || // W_HOUSE_CLASS
                           biota.WeenieClassId >= 9693 && biota.WeenieClassId <= 10492 || // W_HOUSECOTTAGE1_CLASS to W_HOUSECOTTAGE800_CLASS
                           biota.WeenieClassId >= 10493 && biota.WeenieClassId <= 10662 || // W_HOUSEVILLA801_CLASS to W_HOUSEVILLA970_CLASS
                           biota.WeenieClassId >= 10663 && biota.WeenieClassId <= 10692 || // W_HOUSEMANSION971_CLASS to W_HOUSEMANSION1000_CLASS
                           biota.WeenieClassId >= 10746 && biota.WeenieClassId <= 10750 || // W_HOUSETEST1_CLASS to W_HOUSETEST5_CLASS
                           biota.WeenieClassId >= 10829 && biota.WeenieClassId <= 10839 || // W_HOUSETEST6_CLASS to W_HOUSETEST16_CLASS
                           biota.WeenieClassId >= 11677 && biota.WeenieClassId <= 11682 || // W_HOUSETEST17_CLASS to W_HOUSETEST22_CLASS
                           biota.WeenieClassId >= 12311 && biota.WeenieClassId <= 12460 || // W_HOUSECOTTAGE1001_CLASS to W_HOUSECOTTAGE1150_CLASS
                           biota.WeenieClassId >= 12775 && biota.WeenieClassId <= 13024 || // W_HOUSECOTTAGE1151_CLASS to W_HOUSECOTTAGE1400_CLASS
                           biota.WeenieClassId >= 13025 && biota.WeenieClassId <= 13064 || // W_HOUSEVILLA1401_CLASS to W_HOUSEVILLA1440_CLASS
                           biota.WeenieClassId >= 13065 && biota.WeenieClassId <= 13074 || // W_HOUSEMANSION1441_CLASS to W_HOUSEMANSION1450_CLASS
                           biota.WeenieClassId == 13234 || // W_HOUSECOTTAGETEST10000_CLASS
                           biota.WeenieClassId == 13235 || // W_HOUSEVILLATEST10001_CLASS
                           biota.WeenieClassId >= 13243 && biota.WeenieClassId <= 14042 || // W_HOUSECOTTAGE1451_CLASS to W_HOUSECOTTAGE2350_CLASS
                           biota.WeenieClassId >= 14043 && biota.WeenieClassId <= 14222 || // W_HOUSEVILLA1851_CLASS to W_HOUSEVILLA2440_CLASS
                           biota.WeenieClassId >= 14223 && biota.WeenieClassId <= 14242 || // W_HOUSEMANSION1941_CLASS to W_HOUSEMANSION2450_CLASS
                           biota.WeenieClassId >= 14938 && biota.WeenieClassId <= 15087 || // W_HOUSECOTTAGE2451_CLASS to W_HOUSECOTTAGE2600_CLASS
                           biota.WeenieClassId >= 15088 && biota.WeenieClassId <= 15127 || // W_HOUSEVILLA2601_CLASS to W_HOUSEVILLA2640_CLASS
                           biota.WeenieClassId >= 15128 && biota.WeenieClassId <= 15137 || // W_HOUSEMANSION2641_CLASS to W_HOUSEMANSION2650_CLASS
                           biota.WeenieClassId >= 15452 && biota.WeenieClassId <= 15457 || // W_HOUSEAPARTMENT2851_CLASS to W_HOUSEAPARTMENT2856_CLASS
                           biota.WeenieClassId >= 15458 && biota.WeenieClassId <= 15607 || // W_HOUSECOTTAGE2651_CLASS to W_HOUSECOTTAGE2800_CLASS
                           biota.WeenieClassId >= 15612 && biota.WeenieClassId <= 15661 || // W_HOUSEVILLA2801_CLASS to W_HOUSEVILLA2850_CLASS
                           biota.WeenieClassId >= 15897 && biota.WeenieClassId <= 16890 || // W_HOUSEAPARTMENT2857_CLASS to W_HOUSEAPARTMENT3850_CLASS
                           biota.WeenieClassId >= 16923 && biota.WeenieClassId <= 18923 || // W_HOUSEAPARTMENT4051_CLASS to W_HOUSEAPARTMENT6050_CLASS
                           biota.WeenieClassId >= 18924 && biota.WeenieClassId <= 19073 || // W_HOUSECOTTAGE3851_CLASS to W_HOUSECOTTAGE4000_CLASS
                           biota.WeenieClassId >= 19077 && biota.WeenieClassId <= 19126 || // W_HOUSEVILLA4001_CLASS to W_HOUSEVILLA4050_CLASS
                           biota.WeenieClassId >= 20650 && biota.WeenieClassId <= 20799 || // W_HOUSECOTTAGE6051_CLASS to W_HOUSECOTTAGE6200_CLASS
                           biota.WeenieClassId >= 20800 && biota.WeenieClassId <= 20839 || // W_HOUSEVILLA6201_CLASS to W_HOUSEVILLA6240_CLASS
                           biota.WeenieClassId >= 20840 && biota.WeenieClassId <= 20849    // W_HOUSEMANSION6241_CLASS to W_HOUSEMANSION6250_CLASS
                           )
                        return ACE.Entity.Enum.WeenieType.House;
                    else if (biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Deed"))
                        return ACE.Entity.Enum.WeenieType.Deed;
                    else if (biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Button") ||
                        biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Lever") && !biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Broken")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Candle") && !biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Floating") && !biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Bronze")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Torch") && biota.WeenieClassId != 293
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Plant") && !biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Fertilized")
                        )
                        return ACE.Entity.Enum.WeenieType.Switch;
                    else if (biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Essence") && biota.GetProperty(ACE.Entity.Enum.Properties.PropertyInt.MaxStructure, rwLock) == 50)
                        return ACE.Entity.Enum.WeenieType.PetDevice;
                    else if (biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Mag-Ma!")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name) == "Acid"
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Vent")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Steam")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Electric Floor")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Refreshing")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name) == "Sewer"
                        //|| parsed.wdesc._name.m_buffer.Contains("Ice") && !parsed.wdesc._name.m_buffer.Contains("Box")
                        //|| parsed.wdesc._name.m_buffer.Contains("Firespurt")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Flames")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Plume")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("The Black Breath")
                        //|| parsed.wdesc._name.m_buffer.Contains("Bonfire")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Geyser")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Magma")
                        || biota.WeenieClassId == 14805
                        //|| parsed.wdesc._name.m_buffer.Contains("Pool") && !parsed.wdesc._name.m_buffer.Contains("of")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Firespurt")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Bonfire")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Pool") && !biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("of")
                        )
                        return ACE.Entity.Enum.WeenieType.HotSpot;
                    else
                        goto default;

                case ACE.Entity.Enum.ItemType.Caster:
                    return ACE.Entity.Enum.WeenieType.Caster;
                case ACE.Entity.Enum.ItemType.Jewelry:
                    return ACE.Entity.Enum.WeenieType.Generic;
                case ACE.Entity.Enum.ItemType.Armor:
                case ACE.Entity.Enum.ItemType.Clothing:
                    return ACE.Entity.Enum.WeenieType.Clothing;

                case ACE.Entity.Enum.ItemType.Container:
                    if (
                        biota.WeenieClassId == 9686 || // W_HOOK_CLASS
                        biota.WeenieClassId == 11697 || // W_HOOK_FLOOR_CLASS
                        biota.WeenieClassId == 11698 || // W_HOOK_CEILING_CLASS
                        biota.WeenieClassId == 12678 || // W_HOOK_ROOF_CLASS
                        biota.WeenieClassId == 12679    // W_HOOK_YARD_CLASS
                        )
                        return ACE.Entity.Enum.WeenieType.Hook;
                    else if (
                        biota.WeenieClassId == 9687     // W_STORAGE_CLASS
                        )
                        return ACE.Entity.Enum.WeenieType.Storage;
                    else if (
                        biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Pack")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Backpack")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Sack")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Pouch")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Basket")
                        )
                        return ACE.Entity.Enum.WeenieType.Container;
                    else
                        return ACE.Entity.Enum.WeenieType.Chest;

                case ACE.Entity.Enum.ItemType.None:
                    if (
                        biota.WeenieClassId == 9621 || // W_SLUMLORD_CLASS
                        biota.WeenieClassId == 10752 || // W_SLUMLORDTESTCHEAP_CLASS
                        biota.WeenieClassId == 10753 || // W_SLUMLORDTESTEXPENSIVE_CLASS
                        biota.WeenieClassId == 10754 || // W_SLUMLORDTESTMODERATE_CLASS
                        biota.WeenieClassId == 11711 || // W_SLUMLORDCOTTAGECHEAP_CLASS
                        biota.WeenieClassId == 11712 || // W_SLUMLORDCOTTAGEEXPENSIVE_CLASS
                        biota.WeenieClassId == 11713 || // W_SLUMLORDCOTTAGEMODERATE_CLASS
                        biota.WeenieClassId == 11714 || // W_SLUMLORDMANSIONCHEAP_CLASS
                        biota.WeenieClassId == 11715 || // W_SLUMLORDMANSIONEXPENSIVE_CLASS
                        biota.WeenieClassId == 11716 || // W_SLUMLORDMANSIONMODERATE_CLASS
                        biota.WeenieClassId == 11717 || // W_SLUMLORDVILLACHEAP_CLASS
                        biota.WeenieClassId == 11718 || // W_SLUMLORDVILLAEXPENSIVE_CLASS
                        biota.WeenieClassId == 11719 || // W_SLUMLORDVILLAMODERATE_CLASS
                        biota.WeenieClassId == 11977 || // W_SLUMLORDCOTTAGES349_579_CLASS
                        biota.WeenieClassId == 11978 || // W_SLUMLORDVILLA851_925_CLASS
                        biota.WeenieClassId == 11979 || // W_SLUMLORDCOTTAGE580_800_CLASS
                        biota.WeenieClassId == 11980 || // W_SLUMLORDVILLA926_970_CLASS
                        biota.WeenieClassId == 11980 || // W_SLUMLORDVILLA926_970_CLASS
                        biota.WeenieClassId == 12461 || // W_SLUMLORDCOTTAGE1001_1075_CLASS
                        biota.WeenieClassId == 12462 || // W_SLUMLORDCOTTAGE1076_1150_CLASS
                        biota.WeenieClassId == 13078 || // W_SLUMLORDCOTTAGE1151_1275_CLASS
                        biota.WeenieClassId == 13079 || // W_SLUMLORDCOTTAGE1276_1400_CLASS
                        biota.WeenieClassId == 13080 || // W_SLUMLORDVILLA1401_1440_CLASS
                        biota.WeenieClassId == 13081 || // W_SLUMLORDMANSION1441_1450_CLASS
                        biota.WeenieClassId == 14243 || // W_SLUMLORDCOTTAGE1451_1650_CLASS
                        biota.WeenieClassId == 14244 || // W_SLUMLORDCOTTAGE1651_1850_CLASS
                        biota.WeenieClassId == 14245 || // W_SLUMLORDVILLA1851_1940_CLASS
                        biota.WeenieClassId == 14246 || // W_SLUMLORDMANSION1941_1950_CLASS
                        biota.WeenieClassId == 14247 || // W_SLUMLORDCOTTAGE1951_2150_CLASS
                        biota.WeenieClassId == 14248 || // W_SLUMLORDCOTTAGE2151_2350_CLASS
                        biota.WeenieClassId == 14249 || // W_SLUMLORDVILLA2351_2440_CLASS
                        biota.WeenieClassId == 14250 || // W_SLUMLORDMANSION2441_2450_CLASS
                        biota.WeenieClassId == 14934 || // W_SLUMLORDCOTTAGE2451_2525_CLASS
                        biota.WeenieClassId == 14935 || // W_SLUMLORDCOTTAGE2526_2600_CLASS
                        biota.WeenieClassId == 14936 || // W_SLUMLORDVILLA2601_2640_CLASS
                        biota.WeenieClassId == 14937 || // W_SLUMLORDMANSION2641_2650_CLASS
                                                        // wo.WeenieClassId == 15273 || // W_SLUMLORDFAKENUHMUDIRA_CLASS
                        biota.WeenieClassId == 15608 || // W_SLUMLORDAPARTMENT_CLASS
                        biota.WeenieClassId == 15609 || // W_SLUMLORDCOTTAGE2651_2725_CLASS
                        biota.WeenieClassId == 15610 || // W_SLUMLORDCOTTAGE2726_2800_CLASS
                        biota.WeenieClassId == 15611 || // W_SLUMLORDVILLA2801_2850_CLASS
                        biota.WeenieClassId == 19074 || // W_SLUMLORDCOTTAGE3851_3925_CLASS
                        biota.WeenieClassId == 19075 || // W_SLUMLORDCOTTAGE3926_4000_CLASS
                        biota.WeenieClassId == 19076 || // W_SLUMLORDVILLA4001_4050_CLASS
                        biota.WeenieClassId == 20850 || // W_SLUMLORDCOTTAGE6051_6125_CLASS
                        biota.WeenieClassId == 20851 || // W_SLUMLORDCOTTAGE6126_6200_CLASS
                        biota.WeenieClassId == 20852 || // W_SLUMLORDVILLA6201_6240_CLASS
                        biota.WeenieClassId == 20853    // W_SLUMLORDMANSION6241_6250_CLASS
                                                        // wo.WeenieClassId == 22118 || // W_SLUMLORDHAUNTEDMANSION_CLASS
                        )
                        return ACE.Entity.Enum.WeenieType.SlumLord;
                    else if (
                        biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Bolt")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("wave")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Wave")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Blast")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Ring")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Stream")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Fist")
                        // || wo.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Missile")
                        // || wo.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Egg")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Death")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Fury")
                         || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Wind")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Flaming Skull")
                         || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Edge")
                        // || wo.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Snowball")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Bomb")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Blade")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Stalactite")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Boulder")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Whirlwind")
                        )
                        return ACE.Entity.Enum.WeenieType.ProjectileSpell;
                    else if (
                        biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Missile")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Egg")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Snowball")
                        )
                        return ACE.Entity.Enum.WeenieType.Missile;
                    else
                        goto default;

                case ACE.Entity.Enum.ItemType.Creature:
                    var weenieHeaderFlag2 = (ACE.Entity.Enum.WeenieHeaderFlag2)(biota.GetProperty((ACE.Entity.Enum.Properties.PropertyDataId)8002, rwLock) ?? (uint)ACE.Entity.Enum.WeenieHeaderFlag2.None);
                    if (weenieHeaderFlag2.HasFlag(ACE.Entity.Enum.WeenieHeaderFlag2.PetOwner))
                        if (biota.GetProperty(ACE.Entity.Enum.Properties.PropertyInt.RadarBlipColor, rwLock).HasValue && biota.GetProperty(ACE.Entity.Enum.Properties.PropertyInt.RadarBlipColor, rwLock) == (int)ACE.Entity.Enum.RadarColor.Yellow)
                            return ACE.Entity.Enum.WeenieType.Pet;
                        else
                            return ACE.Entity.Enum.WeenieType.CombatPet;
                    else if (
                        biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Pet")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Wind-up")
                        || biota.WeenieClassId == 48881
                        || biota.WeenieClassId == 34902
                        || biota.WeenieClassId == 48891
                        || biota.WeenieClassId == 48879
                        || biota.WeenieClassId == 34906
                        || biota.WeenieClassId == 48887
                        || biota.WeenieClassId == 48889
                        || biota.WeenieClassId == 48883
                        || biota.WeenieClassId == 34900
                        || biota.WeenieClassId == 34901
                        || biota.WeenieClassId == 34908
                        || biota.WeenieClassId == 34898
                        )
                        return ACE.Entity.Enum.WeenieType.Pet;
                    else if (
                        biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Cow")
                        && !biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Auroch")
                        && !biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Snowman")
                        )
                        return ACE.Entity.Enum.WeenieType.Cow;
                    else if (
                        biota.WeenieClassId >= 14342 && biota.WeenieClassId <= 14347
                        || biota.WeenieClassId >= 14404 && biota.WeenieClassId <= 14409
                        )
                        return ACE.Entity.Enum.WeenieType.GamePiece;
                    else
                        return ACE.Entity.Enum.WeenieType.Creature;

                case ACE.Entity.Enum.ItemType.Gameboard:
                    return ACE.Entity.Enum.WeenieType.Game;

                case ACE.Entity.Enum.ItemType.Portal:
                    if (
                        biota.WeenieClassId == 9620 || // W_PORTALHOUSE_CLASS
                        biota.WeenieClassId == 10751 || // W_PORTALHOUSETEST_CLASS
                        biota.WeenieClassId == 11730    // W_HOUSEPORTAL_CLASS
                        )
                        return ACE.Entity.Enum.WeenieType.HousePortal;
                    else
                        return ACE.Entity.Enum.WeenieType.Portal;

                case ACE.Entity.Enum.ItemType.MeleeWeapon:
                    return ACE.Entity.Enum.WeenieType.MeleeWeapon;

                case ACE.Entity.Enum.ItemType.MissileWeapon:
                    if (biota.GetProperty(ACE.Entity.Enum.Properties.PropertyInt.AmmoType, rwLock).HasValue)
                        return ACE.Entity.Enum.WeenieType.MissileLauncher;
                    else
                        return ACE.Entity.Enum.WeenieType.Missile;

                case ACE.Entity.Enum.ItemType.Money:
                    return ACE.Entity.Enum.WeenieType.Coin;
                case ACE.Entity.Enum.ItemType.Gem:
                    return ACE.Entity.Enum.WeenieType.Gem;
                case ACE.Entity.Enum.ItemType.SpellComponents:
                    return ACE.Entity.Enum.WeenieType.SpellComponent;
                case ACE.Entity.Enum.ItemType.ManaStone:
                    return ACE.Entity.Enum.WeenieType.ManaStone;
                case ACE.Entity.Enum.ItemType.TinkeringTool:
                    return ACE.Entity.Enum.WeenieType.CraftTool;
                case ACE.Entity.Enum.ItemType.Key:
                    return ACE.Entity.Enum.WeenieType.Key;
                case ACE.Entity.Enum.ItemType.PromissoryNote:
                    return ACE.Entity.Enum.WeenieType.Stackable;
                case ACE.Entity.Enum.ItemType.Writable:
                    return ACE.Entity.Enum.WeenieType.Scroll;

                default:
                    if (biota.GetProperty(ACE.Entity.Enum.Properties.PropertyInt.MaxStructure, rwLock).HasValue || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyInt.TargetType, rwLock).HasValue)
                        return ACE.Entity.Enum.WeenieType.CraftTool;
                    else if (biota.GetProperty(ACE.Entity.Enum.Properties.PropertyInt.MaxStackSize, rwLock).HasValue)
                        return ACE.Entity.Enum.WeenieType.Stackable;
                    else if (biota.BiotaPropertiesSpellBook.Count > 0)
                        return ACE.Entity.Enum.WeenieType.Switch;
                    else
                        return ACE.Entity.Enum.WeenieType.Generic;
            }
        }
    }
}
