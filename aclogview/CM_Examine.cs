using aclogview;
using System;
using System.Collections.Generic;
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

        public static CreatureAppraisalProfile read(BinaryReader binaryReader)
        {
            CreatureAppraisalProfile newObj = new CreatureAppraisalProfile();
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
            }
            if ((newObj._header & (uint)CreatureAppraisalProfilePackHeader.Packed_Enchantments) != 0)
            {
                // TODO: Decode this message further using Enchantment_BFIndex
                newObj.enchantment_bitfield = binaryReader.ReadUInt32();
            }
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
            node.Nodes.Add("_header = " + Utility.FormatHex(_header));
            node.Nodes.Add("_health = " + _health);
            node.Nodes.Add("_max_health = " + _max_health);
            if ((_header & (uint)CreatureAppraisalProfilePackHeader.Packed_Attributes) != 0)
            {
                node.Nodes.Add("_strength = " + _strength);
                node.Nodes.Add("_endurance = " + _endurance);
                node.Nodes.Add("_quickness = " + _quickness);
                node.Nodes.Add("_coordination = " + _coordination);
                node.Nodes.Add("_focus = " + _focus);
                node.Nodes.Add("_self = " + _self);
                node.Nodes.Add("_stamina = " + _stamina);
                node.Nodes.Add("_mana = " + _mana);
                node.Nodes.Add("_max_stamina = " + _max_stamina);
                node.Nodes.Add("_max_mana = " + _max_mana);
            }
            if ((_header & (uint)CreatureAppraisalProfilePackHeader.Packed_Enchantments) != 0)
            {
                node.Nodes.Add("enchantment_bitfield = " + Utility.FormatHex(enchantment_bitfield));
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
            node.Nodes.Add("_base_armor_chest = " + _base_armor_chest);
            node.Nodes.Add("_base_armor_groin = " + _base_armor_groin);
            node.Nodes.Add("_base_armor_bicep = " + _base_armor_bicep);
            node.Nodes.Add("_base_armor_wrist = " + _base_armor_wrist);
            node.Nodes.Add("_base_armor_hand = " + _base_armor_hand);
            node.Nodes.Add("_base_armor_thigh = " + _base_armor_thigh);
            node.Nodes.Add("_base_armor_shin = " + _base_armor_shin);
            node.Nodes.Add("_base_armor_foot = " + _base_armor_foot);
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
            node.Nodes.Add("_mod_vs_pierce = " + _mod_vs_pierce);
            node.Nodes.Add("_mod_vs_bludgeon = " + _mod_vs_bludgeon);
            node.Nodes.Add("_mod_vs_cold = " + _mod_vs_cold);
            node.Nodes.Add("_mod_vs_fire = " + _mod_vs_fire);
            node.Nodes.Add("_mod_vs_acid = " + _mod_vs_acid);
            node.Nodes.Add("_mod_vs_nether = " + _mod_vs_nether);
            node.Nodes.Add("_mod_vs_electric = " + _mod_vs_electric);
        }
    }

    public class HookAppraisalProfile
    {
        public uint _bitField;
        public uint _validLocations;
        public AMMO_TYPE _ammoType;
        public bool isInscribable;
        public bool isHealer;
        public bool isLockpick;

        public static HookAppraisalProfile read(BinaryReader binaryReader)
        {
            HookAppraisalProfile newObj = new HookAppraisalProfile();
            newObj._bitField = binaryReader.ReadUInt32();
            newObj._validLocations = binaryReader.ReadUInt32();
            newObj._ammoType = (AMMO_TYPE)binaryReader.ReadUInt32();

            if ((newObj._bitField & (uint)Enchantment_BFIndex.BF_INSCRIBABLE) != 0)
                newObj.isInscribable = true;
            if ((newObj._bitField & (uint)Enchantment_BFIndex.BF_HEALER) != 0)
                newObj.isHealer = true;
            if ((newObj._bitField & (uint)Enchantment_BFIndex.BF_LOCKPICK) != 0)
                newObj.isLockpick = true;

            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
            node.Nodes.Add("_bitField = " + Utility.FormatHex(_bitField));
            node.Nodes.Add("_validLocations = " + _validLocations);
            node.Nodes.Add("_ammoType = " + _ammoType);
            node.Nodes.Add("isInscribable = " + isInscribable);
            node.Nodes.Add("isHealer = " + isHealer);
            node.Nodes.Add("isLockpick = " + isLockpick);
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
            node.Nodes.Add("_weapon_time = " + _weapon_time);
            node.Nodes.Add("_weapon_skill = " + _weapon_skill);
            node.Nodes.Add("_weapon_damage = " + _weapon_damage);
            node.Nodes.Add("_damage_variance = " + _damage_variance);
            node.Nodes.Add("_damage_mod = " + _damage_mod);
            node.Nodes.Add("_weapon_length = " + _weapon_length);
            node.Nodes.Add("_max_velocity = " + _max_velocity);
            node.Nodes.Add("_weapon_offense = " + _weapon_offense);
            node.Nodes.Add("_max_velocity_estimated = " + _max_velocity_estimated);
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

        public static AppraisalProfile read(BinaryReader binaryReader) {
            AppraisalProfile newObj = new AppraisalProfile();
            newObj.header = binaryReader.ReadUInt32();
            newObj.success_flag = binaryReader.ReadUInt32();
            if ((newObj.header & (uint)AppraisalProfilePackHeader.Packed_IntStats) != 0) {
                newObj._intStatsTable = PackableHashTable<STypeInt, int>.read(binaryReader);
            }
            if ((newObj.header & (uint)AppraisalProfilePackHeader.Packed_Int64Stats) != 0) {
                newObj._int64StatsTable = PackableHashTable<STypeInt64, long>.read(binaryReader);
            }
            if ((newObj.header & (uint)AppraisalProfilePackHeader.Packed_BoolStats) != 0) {
                newObj._boolStatsTable = PackableHashTable<STypeBool, int>.read(binaryReader);
            }
            if ((newObj.header & (uint)AppraisalProfilePackHeader.Packed_FloatStats) != 0) {
                newObj._floatStatsTable = PackableHashTable<STypeFloat, double>.read(binaryReader);
            }
            if ((newObj.header & (uint)AppraisalProfilePackHeader.Packed_StringStats) != 0) {
                newObj._strStatsTable = PackableHashTable<STypeString, PStringChar>.read(binaryReader);
            }
            if ((newObj.header & (uint)AppraisalProfilePackHeader.Packed_DataIDStats) != 0) {
                newObj._didStatsTable = PackableHashTable<STypeDID, uint>.read(binaryReader);
            }
            if ((newObj.header & (uint)AppraisalProfilePackHeader.Packed_SpellList) != 0)
            {
                newObj._spellsTable = PList<uint>.read(binaryReader);
            }
            if ((newObj.header & (uint)AppraisalProfilePackHeader.Packed_ArmorProfile) != 0)
            {
                newObj._armorProfileTable = ArmorProfile.read(binaryReader);
            }
            if ((newObj.header & (uint)AppraisalProfilePackHeader.Packed_CreatureProfile) != 0)
            {
                newObj._creatureProfileTable = CreatureAppraisalProfile.read(binaryReader);
            }
            if ((newObj.header & (uint)AppraisalProfilePackHeader.Packed_WeaponProfile) != 0)
            {
                newObj._weaponProfileTable = WeaponProfile.read(binaryReader);
            }
            if ((newObj.header & (uint)AppraisalProfilePackHeader.Packed_HookProfile) != 0)
            {
                newObj._hookProfileTable = HookAppraisalProfile.read(binaryReader);
            }
            if ((newObj.header & (uint)AppraisalProfilePackHeader.Packed_ArmorEnchant) != 0)
            {
                newObj._armorEnchantment = binaryReader.ReadUInt32();
            }
            if ((newObj.header & (uint)AppraisalProfilePackHeader.Packed_WeaponEnchant) != 0)
            {
                newObj._weaponEnchantment = binaryReader.ReadUInt32();
            }
            if ((newObj.header & (uint)AppraisalProfilePackHeader.Packed_ResistEnchant) != 0)
            {
                newObj._resistEnchantment = binaryReader.ReadUInt32();
            }
            if ((newObj.header & (uint)AppraisalProfilePackHeader.Packed_ArmorLevels) != 0)
            {
                newObj._armorLevelsTable = ArmorLevels.read(binaryReader);
            }
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node) {
            node.Nodes.Add("header = " + Utility.FormatHex(header));
            node.Nodes.Add("success_flag = " + success_flag);
            TreeNode intStatsNode = node.Nodes.Add("_intStatsTable = ");
            if ((header & (uint)AppraisalProfilePackHeader.Packed_IntStats) != 0)
            {
                _intStatsTable.contributeToTreeNode(intStatsNode);
            }
            TreeNode int64StatsNode = node.Nodes.Add("_int64StatsTable = ");
            if ((header & (uint)AppraisalProfilePackHeader.Packed_Int64Stats) != 0)
            {
                _int64StatsTable.contributeToTreeNode(int64StatsNode);
            }
            TreeNode boolStatsNode = node.Nodes.Add("_boolStatsTable = ");
            if ((header & (uint)AppraisalProfilePackHeader.Packed_BoolStats) != 0)
            {
                _boolStatsTable.contributeToTreeNode(boolStatsNode);
            }
            TreeNode floatStatsNode = node.Nodes.Add("_floatStatsTable = ");
            if ((header & (uint)AppraisalProfilePackHeader.Packed_FloatStats) != 0)
            {
                _floatStatsTable.contributeToTreeNode(floatStatsNode);
            }
            TreeNode strStatsNode = node.Nodes.Add("_strStatsTable = ");
            if ((header & (uint)AppraisalProfilePackHeader.Packed_StringStats) != 0)
            {
                _strStatsTable.contributeToTreeNode(strStatsNode);
            }
            TreeNode didStatsNode = node.Nodes.Add("_didStatsTable = ");
            if ((header & (uint)AppraisalProfilePackHeader.Packed_DataIDStats) != 0)
            {
                _didStatsTable.contributeToTreeNode(didStatsNode);
            }
            TreeNode spellsNode = node.Nodes.Add("_spellBook = ");
            if ((header & (uint)AppraisalProfilePackHeader.Packed_SpellList) != 0)
            {
                for (int i = 0; i < _spellsTable.list.Count; i++)
                {
                    uint i_spell_id = _spellsTable.list[i] & 0x7FFFFFFF;
                    uint enchantment_flag = _spellsTable.list[i] & 0x80000000;
                    TreeNode spellIDNode = spellsNode.Nodes.Add($"({i_spell_id}) " + (SpellID)i_spell_id);
                    if (enchantment_flag != 0)
                    {
                        spellIDNode.Nodes.Add("enchantment_flag = On");
                    }
                    else
                    {
                        spellIDNode.Nodes.Add("enchantment_flag = Off");
                    }
                }
            }

            TreeNode armorProfileNode = node.Nodes.Add("_armorProfile = ");
            if ((header & (uint)AppraisalProfilePackHeader.Packed_ArmorProfile) != 0)
            {
                _armorProfileTable.contributeToTreeNode(armorProfileNode);
            }
            TreeNode creatureProfileNode = node.Nodes.Add("_creatureProfile = ");
            if ((header & (uint)AppraisalProfilePackHeader.Packed_CreatureProfile) != 0)
            {
                _creatureProfileTable.contributeToTreeNode(creatureProfileNode);
            }
            TreeNode weaponProfileNode = node.Nodes.Add("_weaponProfile = ");
            if ((header & (uint)AppraisalProfilePackHeader.Packed_WeaponProfile) != 0)
            {
                _weaponProfileTable.contributeToTreeNode(weaponProfileNode);
            }
            TreeNode hooksNode = node.Nodes.Add("_hookProfile = ");
            if ((header & (uint)AppraisalProfilePackHeader.Packed_HookProfile) != 0)
            {
                _hookProfileTable.contributeToTreeNode(hooksNode);
            }

            TreeNode armorEnchantmentNode = node.Nodes.Add("_armorEnchantments = ");
            if ((header & (uint)AppraisalProfilePackHeader.Packed_ArmorEnchant) != 0)
            {
                armorEnchantmentNode.Nodes.Add("bitfield = " + Utility.FormatHex(_armorEnchantment));
                // Loop over the enum types and add all the applicable ones
                foreach (ArmorEnchantment_BFIndex armorEnchantmentType in Enum.GetValues(typeof(ArmorEnchantment_BFIndex)))
                {
                    if ((_armorEnchantment & (uint)armorEnchantmentType) != 0)
                    {
                        armorEnchantmentNode.Nodes.Add(Enum.GetName(typeof(ArmorEnchantment_BFIndex), armorEnchantmentType));
                    }
                }
            }

            TreeNode weaponEnchantmentNode = node.Nodes.Add("_weaponEnchanments = ");
            if ((header & (uint)AppraisalProfilePackHeader.Packed_WeaponEnchant) != 0)
            {
                weaponEnchantmentNode.Nodes.Add("bitfield = " + Utility.FormatHex(_weaponEnchantment));
                // Loop over the enum types and add all the applicable ones
                foreach (WeaponEnchantment_BFIndex weaponEnchantmentType in Enum.GetValues(typeof(WeaponEnchantment_BFIndex)))
                {
                    if ((_weaponEnchantment & (uint)weaponEnchantmentType) != 0)
                    {
                        weaponEnchantmentNode.Nodes.Add(Enum.GetName(typeof(WeaponEnchantment_BFIndex), weaponEnchantmentType));
                    }
                }
            }

            TreeNode resistEnchantmentNode = node.Nodes.Add("_resistEnchantments = ");
            if ((header & (uint)AppraisalProfilePackHeader.Packed_ResistEnchant) != 0)
            {
                resistEnchantmentNode.Nodes.Add("bitfield = " + Utility.FormatHex(_resistEnchantment));
                // Loop over the enum types and add all the applicable ones
                foreach (ResistanceEnchantment_BFIndex resistEnchantmentType in Enum.GetValues(typeof(ResistanceEnchantment_BFIndex)))
                {
                    if ((_resistEnchantment & (uint)resistEnchantmentType) != 0)
                    {
                        resistEnchantmentNode.Nodes.Add(Enum.GetName(typeof(ResistanceEnchantment_BFIndex), resistEnchantmentType));
                    }
                }
            }

            TreeNode armorLevelsNode = node.Nodes.Add("_armorLevels = ");
            if ((header & (uint)AppraisalProfilePackHeader.Packed_ArmorLevels) != 0)
            {
                _armorLevelsTable.contributeToTreeNode(armorLevelsNode);
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
            rootNode.Nodes.Add("i_objid = " + Utility.FormatHex(i_objid));
            TreeNode profileNode = rootNode.Nodes.Add("i_prof = ");
            i_prof.contributeToTreeNode(profileNode);
            treeView.Nodes.Add(rootNode);
            profileNode.Expand();
        }
    }
}
