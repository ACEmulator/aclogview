using aclogview;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class CM_Examine : MessageProcessor {

    public override bool acceptMessageData(BinaryReader messageDataReader, TreeView outputTreeView) {
        bool handled = true;

        PacketOpcode opcode = Util.readOpcode(messageDataReader);
        switch (opcode) {
            case PacketOpcode.APPRAISAL_INFO_EVENT: {
                    SetAppraiseInfo message = SetAppraiseInfo.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            default: {
                    handled = false;
                    break;
                }
        }

        return handled;
    }

    public class CreatureAppraisalProfile {
        public enum CreatureAppraisalProfilePackHeader {
            Packed_None = 0,
            Packed_Enchantments = (1 << 0),
            // NOTE: Skip 2
            Packed_Attributes = (1 << 3)
        }

        public enum Enchantment_BFIndex {
            BF_STRENGTH = (1 << 0),
            BF_ENDURANCE = (1 << 1),
            BF_QUICKNESS = (1 << 2),
            BF_COORDINATION = (1 << 3),
            BF_FOCUS = (1 << 4),
            BF_SELF = (1 << 5),
            BF_MAX_HEALTH = (1 << 6),
            BF_MAX_STAMINA = (1 << 7),
            BF_MAX_MANA = (1 << 8),
            // NOTE: Skip 7
            BF_STRENGTH_HI = (1 << 16),
            BF_ENDURANCE_HI = (1 << 17),
            BF_QUICKNESS_HI = (1 << 18),
            BF_COORDINATION_HI = (1 << 19),
            BF_FOCUS_HI = (1 << 20),
            BF_SELF_HI = (1 << 21),
            BF_MAX_HEALTH_HI = (1 << 22),
            BF_MAX_STAMINA_HI = (1 << 23),
            BF_MAX_MANA_HI = (1 << 24)
        }

        public uint _header;
        public uint _strength;
        public uint _endurance;
        public uint _quickness;
        public uint _coordination;
        public uint _focus;
        public uint _self;
        public uint _stamina;
        public uint _health;
        public uint _mana;
        public uint _max_health;
        public uint _max_stamina;
        public uint _max_mana;
        public uint enchantment_bitfield;
        public List<string> packedItems = new List<string>();
        public int Length;

        public static CreatureAppraisalProfile read(BinaryReader binaryReader)
        {
            CreatureAppraisalProfile newObj = new CreatureAppraisalProfile();
            var startPosition = binaryReader.BaseStream.Position;
            newObj._header = binaryReader.ReadUInt32();
            newObj._health = binaryReader.ReadUInt32();
            newObj._max_health = binaryReader.ReadUInt32();
            if ((newObj._header & (uint)CreatureAppraisalProfilePackHeader.Packed_Attributes) != 0)
            {
                newObj._strength = binaryReader.ReadUInt32();
                newObj._endurance = binaryReader.ReadUInt32();
                newObj._quickness = binaryReader.ReadUInt32();
                newObj._coordination = binaryReader.ReadUInt32();
                newObj._focus = binaryReader.ReadUInt32();
                newObj._self = binaryReader.ReadUInt32();
                newObj._stamina = binaryReader.ReadUInt32();
                newObj._mana = binaryReader.ReadUInt32();
                newObj._max_stamina = binaryReader.ReadUInt32();
                newObj._max_mana = binaryReader.ReadUInt32();
                newObj.packedItems.Add(CreatureAppraisalProfilePackHeader.Packed_Attributes.ToString());
            }
            if ((newObj._header & (uint)CreatureAppraisalProfilePackHeader.Packed_Enchantments) != 0)
            {
                newObj.enchantment_bitfield = binaryReader.ReadUInt32();
                newObj.packedItems.Add(CreatureAppraisalProfilePackHeader.Packed_Enchantments.ToString());
            }
            newObj.Length = (int)(binaryReader.BaseStream.Position - startPosition);
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
           var headerNode = node.Nodes.Add("_header = " + Utility.FormatHex(_header));
            ContextInfo.AddToList(new ContextInfo { Length = 4 }, updateDataIndex: false);
            for (int i = 0; i < packedItems.Count; i++)
            {
                headerNode.Nodes.Add(packedItems[i]);
                ContextInfo.AddToList(new ContextInfo { Length = 4 }, updateDataIndex: false);
            }
            // skip header
            ContextInfo.DataIndex += 4;
            node.Nodes.Add("_health = " + _health);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_max_health = " + _max_health);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            if ((_header & (uint)CreatureAppraisalProfilePackHeader.Packed_Attributes) != 0)
            {
                node.Nodes.Add("_strength = " + _strength);
                ContextInfo.AddToList(new ContextInfo { Length = 4 });
                node.Nodes.Add("_endurance = " + _endurance);
                ContextInfo.AddToList(new ContextInfo { Length = 4 });
                node.Nodes.Add("_quickness = " + _quickness);
                ContextInfo.AddToList(new ContextInfo { Length = 4 });
                node.Nodes.Add("_coordination = " + _coordination);
                ContextInfo.AddToList(new ContextInfo { Length = 4 });
                node.Nodes.Add("_focus = " + _focus);
                ContextInfo.AddToList(new ContextInfo { Length = 4 });
                node.Nodes.Add("_self = " + _self);
                ContextInfo.AddToList(new ContextInfo { Length = 4 });
                node.Nodes.Add("_stamina = " + _stamina);
                ContextInfo.AddToList(new ContextInfo { Length = 4 });
                node.Nodes.Add("_mana = " + _mana);
                ContextInfo.AddToList(new ContextInfo { Length = 4 });
                node.Nodes.Add("_max_stamina = " + _max_stamina);
                ContextInfo.AddToList(new ContextInfo { Length = 4 });
                node.Nodes.Add("_max_mana = " + _max_mana);
                ContextInfo.AddToList(new ContextInfo { Length = 4 });
            }
            if ((_header & (uint)CreatureAppraisalProfilePackHeader.Packed_Enchantments) != 0)
            {
                var enchantmentNode = node.Nodes.Add("enchantment_bitfield = " + Utility.FormatHex(enchantment_bitfield));
                ContextInfo.AddToList(new ContextInfo { Length = 4 }, updateDataIndex: false);
                // Loop over the enum types and add all the applicable ones
                foreach (Enchantment_BFIndex enchantmentType in Enum.GetValues(typeof(Enchantment_BFIndex)))
                {
                    if ((enchantment_bitfield & (uint)enchantmentType) != 0)
                    {
                            enchantmentNode.Nodes.Add(Enum.GetName(typeof(Enchantment_BFIndex), enchantmentType));
                            ContextInfo.AddToList(new ContextInfo { Length = 4 }, updateDataIndex: false);
                    }
                }
                ContextInfo.DataIndex += 4;
            }
        }
    }

    public class ArmorLevels
    {
        public uint _base_armor_head;
        public uint _base_armor_chest;
        public uint _base_armor_groin;
        public uint _base_armor_bicep;
        public uint _base_armor_wrist;
        public uint _base_armor_hand;
        public uint _base_armor_thigh;
        public uint _base_armor_shin;
        public uint _base_armor_foot;

        public static ArmorLevels read(BinaryReader binaryReader)
        {
            ArmorLevels newObj = new ArmorLevels();
            newObj._base_armor_head = binaryReader.ReadUInt32();
            newObj._base_armor_chest = binaryReader.ReadUInt32();
            newObj._base_armor_groin = binaryReader.ReadUInt32();
            newObj._base_armor_bicep = binaryReader.ReadUInt32();
            newObj._base_armor_wrist = binaryReader.ReadUInt32();
            newObj._base_armor_hand = binaryReader.ReadUInt32();
            newObj._base_armor_thigh = binaryReader.ReadUInt32();
            newObj._base_armor_shin = binaryReader.ReadUInt32();
            newObj._base_armor_foot = binaryReader.ReadUInt32();
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
            node.Nodes.Add("_base_armor_head = " + _base_armor_head);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_base_armor_chest = " + _base_armor_chest);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_base_armor_groin = " + _base_armor_groin);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_base_armor_bicep = " + _base_armor_bicep);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_base_armor_wrist = " + _base_armor_wrist);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_base_armor_hand = " + _base_armor_hand);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_base_armor_thigh = " + _base_armor_thigh);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_base_armor_shin = " + _base_armor_shin);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_base_armor_foot = " + _base_armor_foot);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
        }
    }

    public class ArmorProfile
    {
        public float _mod_vs_slash;
        public float _mod_vs_pierce;
        public float _mod_vs_bludgeon;
        public float _mod_vs_cold;
        public float _mod_vs_fire;
        public float _mod_vs_acid;
        public float _mod_vs_electric;
        public float _mod_vs_nether;

        public static ArmorProfile read(BinaryReader binaryReader)
        {
            ArmorProfile newObj = new ArmorProfile();
            newObj._mod_vs_slash = binaryReader.ReadSingle();
            newObj._mod_vs_pierce = binaryReader.ReadSingle();
            newObj._mod_vs_bludgeon = binaryReader.ReadSingle();
            newObj._mod_vs_cold = binaryReader.ReadSingle();
            newObj._mod_vs_fire = binaryReader.ReadSingle();
            newObj._mod_vs_acid = binaryReader.ReadSingle();
            newObj._mod_vs_nether = binaryReader.ReadSingle();
            newObj._mod_vs_electric = binaryReader.ReadSingle();
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
            node.Nodes.Add("_mod_vs_slash = " + _mod_vs_slash);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_mod_vs_pierce = " + _mod_vs_pierce);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_mod_vs_bludgeon = " + _mod_vs_bludgeon);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_mod_vs_cold = " + _mod_vs_cold);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_mod_vs_fire = " + _mod_vs_fire);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_mod_vs_acid = " + _mod_vs_acid);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_mod_vs_nether = " + _mod_vs_nether);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_mod_vs_electric = " + _mod_vs_electric);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
        }
    }

    public class HookAppraisalProfile
    {
        public uint _bitField;
        public uint _validLocations;
        public AMMO_TYPE _ammoType;
        public List<string> packedItems = new List<string>();
        public int Length;

        public static HookAppraisalProfile read(BinaryReader binaryReader)
        {
            HookAppraisalProfile newObj = new HookAppraisalProfile();
            var startPosition = binaryReader.BaseStream.Position;
            newObj._bitField = binaryReader.ReadUInt32();
            newObj._validLocations = binaryReader.ReadUInt32();
            newObj._ammoType = (AMMO_TYPE)binaryReader.ReadUInt32();

            if ((newObj._bitField & (uint) HookAppraisal_BF.BF_INSCRIBABLE) != 0)
                newObj.packedItems.Add(HookAppraisal_BF.BF_INSCRIBABLE.ToString());
            if ((newObj._bitField & (uint) HookAppraisal_BF.BF_HEALER) != 0)
                newObj.packedItems.Add(HookAppraisal_BF.BF_HEALER.ToString());
            if ((newObj._bitField & (uint) HookAppraisal_BF.BF_FOOD) != 0)
                newObj.packedItems.Add(HookAppraisal_BF.BF_FOOD.ToString());
            if ((newObj._bitField & (uint) HookAppraisal_BF.BF_LOCKPICK) != 0)
                newObj.packedItems.Add(HookAppraisal_BF.BF_LOCKPICK.ToString());

            newObj.Length = (int)(binaryReader.BaseStream.Position - startPosition);
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
            var bitfieldNode = node.Nodes.Add("_bitField = " + Utility.FormatHex(_bitField));
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            foreach (string item in packedItems)
            {
                bitfieldNode.Nodes.Add(item);
            }
            var locationsNode = node.Nodes.Add("_validLocations = " + Utility.FormatHex(_validLocations));
            ContextInfo.AddToList(new ContextInfo { Length = 4 }, updateDataIndex: false);
            CM_Inventory.InventoryLocation.contributeToTreeNode(locationsNode, _validLocations);
            // Skip valid locations
            ContextInfo.DataIndex += 4;
            node.Nodes.Add("_ammoType = " + _ammoType);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
        }
    }

    public class WeaponProfile
    {
        public uint _damage_type;
        public uint _weapon_time;
        public uint _weapon_skill;
        public uint _weapon_damage;
        public double _damage_variance;
        public double _damage_mod;
        public double _weapon_length;
        public double _max_velocity;
        public double _weapon_offense;
        public uint _max_velocity_estimated;

        public static WeaponProfile read(BinaryReader binaryReader)
        {
            WeaponProfile newObj = new WeaponProfile();
            newObj._damage_type = binaryReader.ReadUInt32();
            newObj._weapon_time = binaryReader.ReadUInt32();
            newObj._weapon_skill = binaryReader.ReadUInt32();
            newObj._weapon_damage = binaryReader.ReadUInt32();
            newObj._damage_variance = binaryReader.ReadDouble();
            newObj._damage_mod = binaryReader.ReadDouble();
            newObj._weapon_length = binaryReader.ReadDouble();
            newObj._max_velocity = binaryReader.ReadDouble();
            newObj._weapon_offense = binaryReader.ReadDouble();
            newObj._max_velocity_estimated = binaryReader.ReadUInt32();
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
            node.Nodes.Add("_damage_type = " + _damage_type);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_weapon_time = " + _weapon_time);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_weapon_skill = " + _weapon_skill);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_weapon_damage = " + _weapon_damage);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_damage_variance = " + _damage_variance);
            ContextInfo.AddToList(new ContextInfo { Length = 8 });
            node.Nodes.Add("_damage_mod = " + _damage_mod);
            ContextInfo.AddToList(new ContextInfo { Length = 8 });
            node.Nodes.Add("_weapon_length = " + _weapon_length);
            ContextInfo.AddToList(new ContextInfo { Length = 8 });
            node.Nodes.Add("_max_velocity = " + _max_velocity);
            ContextInfo.AddToList(new ContextInfo { Length = 8 });
            node.Nodes.Add("_weapon_offense = " + _weapon_offense);
            ContextInfo.AddToList(new ContextInfo { Length = 8 });
            node.Nodes.Add("_max_velocity_estimated = " + _max_velocity_estimated);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
        }
    }

    public class AppraisalProfile {
        public enum AppraisalProfilePackHeader {
            Packed_None = 0,
            Packed_IntStats = (1 << 0),
            Packed_BoolStats = (1 << 1),
            Packed_FloatStats = (1 << 2),
            Packed_StringStats = (1 << 3),
            Packed_SpellList = (1 << 4),
            Packed_WeaponProfile = (1 << 5),
            Packed_HookProfile = (1 << 6),
            Packed_ArmorProfile = (1 << 7),
            Packed_CreatureProfile = (1 << 8),
            Packed_ArmorEnchant = (1 << 9),
            Packed_ResistEnchant = (1 << 10),
            Packed_WeaponEnchant = (1 << 11),
            Packed_DataIDStats = (1 << 12),
            Packed_Int64Stats = (1 << 13),
            Packed_ArmorLevels = (1 << 14)
        }

        public enum ResistanceEnchantment_BFIndex {
            BF_RESIST_SLASH = (1 << 0),
            BF_RESIST_PIERCE = (1 << 1),
            BF_RESIST_BLUDGEON = (1 << 2),
            BF_RESIST_FIRE = (1 << 3),
            BF_RESIST_COLD = (1 << 4),
            BF_RESIST_ACID = (1 << 5),
            BF_RESIST_ELECTRIC = (1 << 6),
            BF_RESIST_HEALTH_BOOST = (1 << 7),
            BF_RESIST_STAMINA_DRAIN = (1 << 8),
            BF_RESIST_STAMINA_BOOST = (1 << 9),
            BF_RESIST_MANA_DRAIN = (1 << 10),
            BF_RESIST_MANA_BOOST = (1 << 11),
            BF_MANA_CON_MOD = (1 << 12),
            BF_ELE_DAMAGE_MOD = (1 << 13),
            BF_RESIST_NETHER = (1 << 14),
            // NOTE: Skip 1
            BF_RESIST_SLASH_HI = (1 << 16),
            BF_RESIST_PIERCE_HI = (1 << 17),
            BF_RESIST_BLUDGEON_HI = (1 << 18),
            BF_RESIST_FIRE_HI = (1 << 19),
            BF_RESIST_COLD_HI = (1 << 20),
            BF_RESIST_ACID_HI = (1 << 21),
            BF_RESIST_ELECTRIC_HI = (1 << 22),
            BF_RESIST_HEALTH_BOOST_HI = (1 << 23),
            BF_RESIST_STAMINA_DRAIN_HI = (1 << 24),
            BF_RESIST_STAMINA_BOOST_HI = (1 << 25),
            BF_RESIST_MANA_DRAIN_HI = (1 << 26),
            BF_RESIST_MANA_BOOST_HI = (1 << 27),
            BF_MANA_CON_MOD_HI = (1 << 28),
            BF_ELE_DAMAGE_MOD_HI = (1 << 29),
            BF_RESIST_NETHER_HI = (1 << 30)
        }

        public enum WeaponEnchantment_BFIndex {
            BF_WEAPON_OFFENSE = (1 << 0),
            BF_WEAPON_DEFENSE = (1 << 1),
            BF_WEAPON_TIME = (1 << 2),
            BF_DAMAGE = (1 << 3),
            BF_DAMAGE_VARIANCE = (1 << 4),
            BF_DAMAGE_MOD = (1 << 5),
            // NOTE: Skip 10
            BF_WEAPON_OFFENSE_HI = (1 << 16),
            BF_WEAPON_DEFENSE_HI = (1 << 17),
            BF_WEAPON_TIME_HI = (1 << 18),
            BF_DAMAGE_HI = (1 << 19),
            BF_DAMAGE_VARIANCE_HI = (1 << 20),
            BF_DAMAGE_MOD_HI = (1 << 21)
        }

        public enum ArmorEnchantment_BFIndex {
            BF_ARMOR_LEVEL = (1 << 0),
            BF_ARMOR_MOD_VS_SLASH = (1 << 1),
            BF_ARMOR_MOD_VS_PIERCE = (1 << 2),
            BF_ARMOR_MOD_VS_BLUDGEON = (1 << 3),
            BF_ARMOR_MOD_VS_COLD = (1 << 4),
            BF_ARMOR_MOD_VS_FIRE = (1 << 5),
            BF_ARMOR_MOD_VS_ACID = (1 << 6),
            BF_ARMOR_MOD_VS_ELECTRIC = (1 << 7),
            BF_ARMOR_MOD_VS_NETHER = (1 << 8),
            // NOTE: Skip 7
            BF_ARMOR_LEVEL_HI = (1 << 16),
            BF_ARMOR_MOD_VS_SLASH_HI = (1 << 17),
            BF_ARMOR_MOD_VS_PIERCE_HI = (1 << 18),
            BF_ARMOR_MOD_VS_BLUDGEON_HI = (1 << 19),
            BF_ARMOR_MOD_VS_COLD_HI = (1 << 20),
            BF_ARMOR_MOD_VS_FIRE_HI = (1 << 21),
            BF_ARMOR_MOD_VS_ACID_HI = (1 << 22),
            BF_ARMOR_MOD_VS_ELECTRIC_HI = (1 << 23),
            BF_ARMOR_MOD_VS_NETHER_HI = (1 << 24)
        }

        public uint header;
        public uint success_flag;
        public PackableHashTable<STypeInt, int> _intStatsTable = new PackableHashTable<STypeInt, int>();
        public PackableHashTable<STypeInt64, long> _int64StatsTable = new PackableHashTable<STypeInt64, long>();
        public PackableHashTable<STypeBool, int> _boolStatsTable = new PackableHashTable<STypeBool, int>();
        public PackableHashTable<STypeFloat, double> _floatStatsTable = new PackableHashTable<STypeFloat, double>();
        public PackableHashTable<STypeString, PStringChar> _strStatsTable = new PackableHashTable<STypeString, PStringChar>();
        public PackableHashTable<STypeDID, uint> _didStatsTable = new PackableHashTable<STypeDID, uint>();
        public PList<uint> _spellsTable = new PList<uint>();
        public ArmorProfile _armorProfileTable = new ArmorProfile();
        public CreatureAppraisalProfile _creatureProfileTable = new CreatureAppraisalProfile();
        public WeaponProfile _weaponProfileTable = new WeaponProfile();
        public HookAppraisalProfile _hookProfileTable = new HookAppraisalProfile();
        public uint _armorEnchantment;
        public uint _weaponEnchantment;
        public uint _resistEnchantment;
        public ArmorLevels _armorLevelsTable = new ArmorLevels();
        public List<string> packedItems = new List<string>();
        public int Length;

        public static AppraisalProfile read(BinaryReader binaryReader) {
            AppraisalProfile newObj = new AppraisalProfile();
            var startPosition = binaryReader.BaseStream.Position;
            newObj.header = binaryReader.ReadUInt32();
            newObj.success_flag = binaryReader.ReadUInt32();
            if ((newObj.header & (uint)AppraisalProfilePackHeader.Packed_IntStats) != 0) {
                newObj._intStatsTable = PackableHashTable<STypeInt, int>.read(binaryReader);
                newObj.packedItems.Add(AppraisalProfilePackHeader.Packed_IntStats.ToString());
            }
            if ((newObj.header & (uint)AppraisalProfilePackHeader.Packed_Int64Stats) != 0) {
                newObj._int64StatsTable = PackableHashTable<STypeInt64, long>.read(binaryReader);
                newObj.packedItems.Add(AppraisalProfilePackHeader.Packed_Int64Stats.ToString());
            }
            if ((newObj.header & (uint)AppraisalProfilePackHeader.Packed_BoolStats) != 0) {
                newObj._boolStatsTable = PackableHashTable<STypeBool, int>.read(binaryReader);
                newObj.packedItems.Add(AppraisalProfilePackHeader.Packed_BoolStats.ToString());
            }
            if ((newObj.header & (uint)AppraisalProfilePackHeader.Packed_FloatStats) != 0) {
                newObj._floatStatsTable = PackableHashTable<STypeFloat, double>.read(binaryReader);
                newObj.packedItems.Add(AppraisalProfilePackHeader.Packed_FloatStats.ToString());
            }
            if ((newObj.header & (uint)AppraisalProfilePackHeader.Packed_StringStats) != 0) {
                newObj._strStatsTable = PackableHashTable<STypeString, PStringChar>.read(binaryReader);
                newObj.packedItems.Add(AppraisalProfilePackHeader.Packed_StringStats.ToString());
            }
            if ((newObj.header & (uint)AppraisalProfilePackHeader.Packed_DataIDStats) != 0) {
                newObj._didStatsTable = PackableHashTable<STypeDID, uint>.read(binaryReader);
                newObj.packedItems.Add(AppraisalProfilePackHeader.Packed_DataIDStats.ToString());
            }
            if ((newObj.header & (uint)AppraisalProfilePackHeader.Packed_SpellList) != 0)
            {
                newObj._spellsTable = PList<uint>.read(binaryReader);
                newObj.packedItems.Add(AppraisalProfilePackHeader.Packed_SpellList.ToString());
            }
            if ((newObj.header & (uint)AppraisalProfilePackHeader.Packed_ArmorProfile) != 0)
            {
                newObj._armorProfileTable = ArmorProfile.read(binaryReader);
                newObj.packedItems.Add(AppraisalProfilePackHeader.Packed_ArmorProfile.ToString());
            }
            if ((newObj.header & (uint)AppraisalProfilePackHeader.Packed_CreatureProfile) != 0)
            {
                newObj._creatureProfileTable = CreatureAppraisalProfile.read(binaryReader);
                newObj.packedItems.Add(AppraisalProfilePackHeader.Packed_CreatureProfile.ToString());
            }
            if ((newObj.header & (uint)AppraisalProfilePackHeader.Packed_WeaponProfile) != 0)
            {
                newObj._weaponProfileTable = WeaponProfile.read(binaryReader);
                newObj.packedItems.Add(AppraisalProfilePackHeader.Packed_WeaponProfile.ToString());
            }
            if ((newObj.header & (uint)AppraisalProfilePackHeader.Packed_HookProfile) != 0)
            {
                newObj._hookProfileTable = HookAppraisalProfile.read(binaryReader);
                newObj.packedItems.Add(AppraisalProfilePackHeader.Packed_HookProfile.ToString());
            }
            if ((newObj.header & (uint)AppraisalProfilePackHeader.Packed_ArmorEnchant) != 0)
            {
                newObj._armorEnchantment = binaryReader.ReadUInt32();
                newObj.packedItems.Add(AppraisalProfilePackHeader.Packed_ArmorEnchant.ToString());
            }
            if ((newObj.header & (uint)AppraisalProfilePackHeader.Packed_WeaponEnchant) != 0)
            {
                newObj._weaponEnchantment = binaryReader.ReadUInt32();
                newObj.packedItems.Add(AppraisalProfilePackHeader.Packed_WeaponEnchant.ToString());
            }
            if ((newObj.header & (uint)AppraisalProfilePackHeader.Packed_ResistEnchant) != 0)
            {
                newObj._resistEnchantment = binaryReader.ReadUInt32();
                newObj.packedItems.Add(AppraisalProfilePackHeader.Packed_ResistEnchant.ToString());
            }
            if ((newObj.header & (uint)AppraisalProfilePackHeader.Packed_ArmorLevels) != 0)
            {
                newObj._armorLevelsTable = ArmorLevels.read(binaryReader);
                newObj.packedItems.Add(AppraisalProfilePackHeader.Packed_ArmorLevels.ToString());
            }
            newObj.Length = (int)(binaryReader.BaseStream.Position - startPosition);
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node) {
            var headerNode = node.Nodes.Add("header = " + Utility.FormatHex(header));
            ContextInfo.AddToList(new ContextInfo { Length = 4 }, updateDataIndex: false);
            for (int i = 0; i < packedItems.Count; i++)
            {
                headerNode.Nodes.Add(packedItems[i]);
                ContextInfo.AddToList(new ContextInfo { Length = 4 }, updateDataIndex: false);
            }
            // skip header
            ContextInfo.DataIndex += 4;
            node.Nodes.Add("success_flag = " + success_flag);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            TreeNode intStatsNode = node.Nodes.Add("_intStatsTable = ");
            if ((header & (uint)AppraisalProfilePackHeader.Packed_IntStats) != 0)
            {
                ContextInfo.AddToList(new ContextInfo { Length = _intStatsTable.Length }, updateDataIndex: false);
                _intStatsTable.contributeToTreeNode(intStatsNode);
                // skip header
                ContextInfo.DataIndex += 4;
                for (int i = 0; i < _intStatsTable.hashTable.Count; i++)
                {
                    ContextInfo.AddToList(new ContextInfo { Length = 8 });
                }
            }
            else
            {
                ContextInfo.applyNonSerializedContextInfo(node);
            }

            TreeNode int64StatsNode = node.Nodes.Add("_int64StatsTable = ");
            
            if ((header & (uint)AppraisalProfilePackHeader.Packed_Int64Stats) != 0)
            {
                ContextInfo.AddToList(new ContextInfo { Length = _int64StatsTable.Length }, updateDataIndex: false);
                _int64StatsTable.contributeToTreeNode(int64StatsNode);
                // skip header
                ContextInfo.DataIndex += 4;
                for (int i = 0; i < _int64StatsTable.hashTable.Count; i++)
                {
                    ContextInfo.AddToList(new ContextInfo { Length = 12 });
                }
            }
            else
            {
                ContextInfo.applyNonSerializedContextInfo(node);
            }

            TreeNode boolStatsNode = node.Nodes.Add("_boolStatsTable = ");
            if ((header & (uint)AppraisalProfilePackHeader.Packed_BoolStats) != 0)
            {
                ContextInfo.AddToList(new ContextInfo { Length = _boolStatsTable.Length }, updateDataIndex: false);
                _boolStatsTable.contributeToTreeNode(boolStatsNode);
                // skip header
                ContextInfo.DataIndex += 4;
                for (int i = 0; i < _boolStatsTable.hashTable.Count; i++)
                {
                    ContextInfo.AddToList(new ContextInfo { Length = 8 });
                }
            }
            else
            {
                ContextInfo.applyNonSerializedContextInfo(node);
            }

            TreeNode floatStatsNode = node.Nodes.Add("_floatStatsTable = ");
            if ((header & (uint)AppraisalProfilePackHeader.Packed_FloatStats) != 0)
            {
                ContextInfo.AddToList(new ContextInfo { Length = _floatStatsTable.Length }, updateDataIndex: false);
                _floatStatsTable.contributeToTreeNode(floatStatsNode);
                // skip header
                ContextInfo.DataIndex += 4;
                for (int i = 0; i < _floatStatsTable.hashTable.Count; i++)
                {
                    ContextInfo.AddToList(new ContextInfo { Length = 12 });
                }
            }
            else
            {
                ContextInfo.applyNonSerializedContextInfo(node);
            }

            TreeNode strStatsNode = node.Nodes.Add("_strStatsTable = ");
            if ((header & (uint)AppraisalProfilePackHeader.Packed_StringStats) != 0)
            {
                ContextInfo.AddToList(new ContextInfo { Length = _strStatsTable.Length }, updateDataIndex: false);
                _strStatsTable.contributeToTreeNode(strStatsNode);
                // skip header
                ContextInfo.DataIndex += 4;
                foreach (KeyValuePair<STypeString,PStringChar> element in _strStatsTable.hashTable)
                {
                    ContextInfo.AddToList(new ContextInfo { Length = element.Value.Length + 4 });
                }
            }
            else
            {
                ContextInfo.applyNonSerializedContextInfo(node);
            }

            TreeNode didStatsNode = node.Nodes.Add("_didStatsTable = ");
            if ((header & (uint)AppraisalProfilePackHeader.Packed_DataIDStats) != 0)
            {
                ContextInfo.AddToList(new ContextInfo { Length = _didStatsTable.Length }, updateDataIndex: false);
                _didStatsTable.contributeToTreeNode(didStatsNode);
                // skip header
                ContextInfo.DataIndex += 4;
                for (int i = 0; i < _didStatsTable.hashTable.Count; i++)
                {
                    ContextInfo.AddToList(new ContextInfo { Length = 8 });
                }
            }
            else
            {
                ContextInfo.applyNonSerializedContextInfo(node);
            }

            TreeNode spellsNode = node.Nodes.Add("_spellBook = ");
            if ((header & (uint)AppraisalProfilePackHeader.Packed_SpellList) != 0)
            {
                ContextInfo.AddToList(new ContextInfo { Length = _spellsTable.Length }, updateDataIndex: false);
                // Skip count header
                ContextInfo.DataIndex += 4;
                for (int i = 0; i < _spellsTable.list.Count; i++)
                {
                    uint i_spell_id = _spellsTable.list[i] & 0x7FFFFFFF;
                    uint enchantment_flag = _spellsTable.list[i] & 0x80000000;
                    TreeNode spellIDNode = spellsNode.Nodes.Add($"({i_spell_id}) " + (SpellID)i_spell_id);
                    ContextInfo.AddToList(new ContextInfo { Length = 4 }, updateDataIndex: false);
                    if (enchantment_flag != 0)
                    {
                        spellIDNode.Nodes.Add("enchantment_flag = On");
                    }
                    else
                    {
                        spellIDNode.Nodes.Add("enchantment_flag = Off");
                    }
                    ContextInfo.AddToList(new ContextInfo { Length = 4 }, updateDataIndex: false);
                    ContextInfo.DataIndex += 4;
                }
            }
            else
            {
                ContextInfo.applyNonSerializedContextInfo(node);
            }

            TreeNode armorProfileNode = node.Nodes.Add("_armorProfile = ");
            if ((header & (uint)AppraisalProfilePackHeader.Packed_ArmorProfile) != 0)
            {
                ContextInfo.AddToList(new ContextInfo { Length = 32 }, updateDataIndex: false);
                _armorProfileTable.contributeToTreeNode(armorProfileNode);
            }
            else
            {
                ContextInfo.applyNonSerializedContextInfo(node);
            }

            TreeNode creatureProfileNode = node.Nodes.Add("_creatureProfile = ");
            if ((header & (uint)AppraisalProfilePackHeader.Packed_CreatureProfile) != 0)
            {
                ContextInfo.AddToList(new ContextInfo { Length = _creatureProfileTable.Length }, updateDataIndex: false);
                _creatureProfileTable.contributeToTreeNode(creatureProfileNode);
            }
            else
            {
                ContextInfo.applyNonSerializedContextInfo(node);
            }

            TreeNode weaponProfileNode = node.Nodes.Add("_weaponProfile = ");
            if ((header & (uint)AppraisalProfilePackHeader.Packed_WeaponProfile) != 0)
            {
                ContextInfo.AddToList(new ContextInfo { Length = 60 }, updateDataIndex: false);
                _weaponProfileTable.contributeToTreeNode(weaponProfileNode);
            }
            else
            {
                ContextInfo.applyNonSerializedContextInfo(node);
            }

            TreeNode hooksNode = node.Nodes.Add("_hookProfile = ");
            if ((header & (uint)AppraisalProfilePackHeader.Packed_HookProfile) != 0)
            {
                ContextInfo.AddToList(new ContextInfo { Length = _hookProfileTable.Length }, updateDataIndex: false);
                _hookProfileTable.contributeToTreeNode(hooksNode);
            }
            else
            {
                ContextInfo.applyNonSerializedContextInfo(node);
            }

            TreeNode armorEnchantmentNode = node.Nodes.Add("_armorEnchantments = ");
            if ((header & (uint)AppraisalProfilePackHeader.Packed_ArmorEnchant) != 0)
            {
                ContextInfo.AddToList(new ContextInfo { Length = 4 }, updateDataIndex: false);
                armorEnchantmentNode.Nodes.Add("bitfield = " + Utility.FormatHex(_armorEnchantment));
                ContextInfo.AddToList(new ContextInfo { Length = 4 }, updateDataIndex: false);
                // Loop over the enum types and add all the applicable ones
                foreach (ArmorEnchantment_BFIndex armorEnchantmentType in Enum.GetValues(typeof(ArmorEnchantment_BFIndex)))
                {
                    if ((_armorEnchantment & (uint)armorEnchantmentType) != 0)
                    {
                        armorEnchantmentNode.Nodes.Add(Enum.GetName(typeof(ArmorEnchantment_BFIndex), armorEnchantmentType));
                        ContextInfo.AddToList(new ContextInfo { Length = 4 }, updateDataIndex: false);
                    }
                }
                ContextInfo.DataIndex += 4;
            }
            else
            {
                ContextInfo.applyNonSerializedContextInfo(node);
            }

            TreeNode weaponEnchantmentNode = node.Nodes.Add("_weaponEnchanments = ");
            if ((header & (uint)AppraisalProfilePackHeader.Packed_WeaponEnchant) != 0)
            {
                ContextInfo.AddToList(new ContextInfo { Length = 4 }, updateDataIndex: false);
                weaponEnchantmentNode.Nodes.Add("bitfield = " + Utility.FormatHex(_weaponEnchantment));
                ContextInfo.AddToList(new ContextInfo { Length = 4 }, updateDataIndex: false);
                // Loop over the enum types and add all the applicable ones
                foreach (WeaponEnchantment_BFIndex weaponEnchantmentType in Enum.GetValues(typeof(WeaponEnchantment_BFIndex)))
                {
                    if ((_weaponEnchantment & (uint)weaponEnchantmentType) != 0)
                    {
                        weaponEnchantmentNode.Nodes.Add(Enum.GetName(typeof(WeaponEnchantment_BFIndex), weaponEnchantmentType));
                        ContextInfo.AddToList(new ContextInfo { Length = 4 }, updateDataIndex: false);
                    }
                }
                ContextInfo.DataIndex += 4;
            }
            else
            {
                ContextInfo.applyNonSerializedContextInfo(node);
            }

            TreeNode resistEnchantmentNode = node.Nodes.Add("_resistEnchantments = ");
            if ((header & (uint)AppraisalProfilePackHeader.Packed_ResistEnchant) != 0)
            {
                ContextInfo.AddToList(new ContextInfo { Length = 4 }, updateDataIndex: false);
                resistEnchantmentNode.Nodes.Add("bitfield = " + Utility.FormatHex(_resistEnchantment));
                ContextInfo.AddToList(new ContextInfo { Length = 4 }, updateDataIndex: false);
                // Loop over the enum types and add all the applicable ones
                foreach (ResistanceEnchantment_BFIndex resistEnchantmentType in Enum.GetValues(typeof(ResistanceEnchantment_BFIndex)))
                {
                    if ((_resistEnchantment & (uint)resistEnchantmentType) != 0)
                    {
                        resistEnchantmentNode.Nodes.Add(Enum.GetName(typeof(ResistanceEnchantment_BFIndex), resistEnchantmentType));
                        ContextInfo.AddToList(new ContextInfo { Length = 4 }, updateDataIndex: false);
                    }
                }
                ContextInfo.DataIndex += 4;
            }
            else
            {
                ContextInfo.applyNonSerializedContextInfo(node);
            }

            TreeNode armorLevelsNode = node.Nodes.Add("_armorLevels = ");
            if ((header & (uint)AppraisalProfilePackHeader.Packed_ArmorLevels) != 0)
            {
                ContextInfo.AddToList(new ContextInfo { Length = 36 }, updateDataIndex: false);
                _armorLevelsTable.contributeToTreeNode(armorLevelsNode);
            }
            else
            {
                ContextInfo.applyNonSerializedContextInfo(node);
            }
        }
    }

    public class SetAppraiseInfo : Message {
        public uint i_objid;
        public AppraisalProfile i_prof;

        public static SetAppraiseInfo read(BinaryReader binaryReader) {
            SetAppraiseInfo newObj = new SetAppraiseInfo();
            newObj.i_objid = binaryReader.ReadUInt32();
            newObj.i_prof = AppraisalProfile.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ServerToClientHeader });
            rootNode.Nodes.Add("i_objid = " + Utility.FormatHex(i_objid));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            TreeNode profileNode = rootNode.Nodes.Add("i_prof = ");
            ContextInfo.AddToList(new ContextInfo { Length = i_prof.Length }, updateDataIndex: false);
            i_prof.contributeToTreeNode(profileNode);
            treeView.Nodes.Add(rootNode);
            profileNode.Expand();
        }
    }

}
