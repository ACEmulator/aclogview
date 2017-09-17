using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class CM_Login : MessageProcessor {

    public override bool acceptMessageData(BinaryReader messageDataReader, TreeView outputTreeView) {
        bool handled = true;

        PacketOpcode opcode = Util.readOpcode(messageDataReader);
        switch (opcode) {
            case PacketOpcode.Evt_Login__CharacterSet_ID: {
                    Login__CharacterSet message = Login__CharacterSet.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Login__WorldInfo_ID: {
                    WorldInfo message = WorldInfo.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.PLAYER_DESCRIPTION_EVENT:
                {
                    PlayerDescription message = PlayerDescription.read(messageDataReader);
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

    public class CharacterIdentity {
        public uint gid_;
        public PStringChar name_;
        public uint secondsGreyedOut_;

        public static CharacterIdentity read(BinaryReader binaryReader) {
            CharacterIdentity newObj = new CharacterIdentity();
            newObj.gid_ = binaryReader.ReadUInt32();
            newObj.name_ = PStringChar.read(binaryReader);
            newObj.secondsGreyedOut_ = binaryReader.ReadUInt32();

            return newObj;
        }

        public void contributeToTreeNode(TreeNode node) {
            node.Nodes.Add("gid_ = " + gid_);
            node.Nodes.Add("name_ = " + name_.m_buffer);
            node.Nodes.Add("secondsGreyedOut_ = " + secondsGreyedOut_);
        }
    }

    public class Login__CharacterSet : Message {
        public uint status_;
        public List<CharacterIdentity> set_ = new List<CharacterIdentity>();
        public List<CharacterIdentity> delSet_ = new List<CharacterIdentity>();
        public uint numAllowedCharacters_;
        public PStringChar account_;
        public uint m_fUseTurbineChat;
        public uint m_fHasThroneofDestiny;

        public static Login__CharacterSet read(BinaryReader binaryReader) {
            Login__CharacterSet newObj = new Login__CharacterSet();
            newObj.status_ = binaryReader.ReadUInt32();
            uint setNum = binaryReader.ReadUInt32();
            for (uint i = 0; i < setNum; ++i) {
                newObj.set_.Add(CharacterIdentity.read(binaryReader));
            }
            uint delSetNum = binaryReader.ReadUInt32();
            for (uint i = 0; i < delSetNum; ++i) {
                newObj.delSet_.Add(CharacterIdentity.read(binaryReader));
            }
            newObj.numAllowedCharacters_ = binaryReader.ReadUInt32();
            newObj.account_ = PStringChar.read(binaryReader);
            newObj.m_fUseTurbineChat = binaryReader.ReadUInt32();
            newObj.m_fHasThroneofDestiny = binaryReader.ReadUInt32();

            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("status_ = " + status_);
            TreeNode setNode = rootNode.Nodes.Add("set_ = ");
            foreach (CharacterIdentity identity in set_) {
                identity.contributeToTreeNode(setNode);
            }
            TreeNode delSetNode = rootNode.Nodes.Add("delSet_ = ");
            foreach (CharacterIdentity identity in delSet_) {
                identity.contributeToTreeNode(delSetNode);
            }
            rootNode.Nodes.Add("numAllowedCharacters_ = " + numAllowedCharacters_);
            rootNode.Nodes.Add("account_ = " + account_.m_buffer);
            rootNode.Nodes.Add("m_fUseTurbineChat = " + m_fUseTurbineChat);
            rootNode.Nodes.Add("m_fHasThroneofDestiny = " + m_fHasThroneofDestiny);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class WorldInfo : Message {
        public int cConnections;
        public int cMaxConnections;
        public PStringChar strWorldName;

        public static WorldInfo read(BinaryReader binaryReader) {
            WorldInfo newObj = new WorldInfo();
            newObj.cConnections = binaryReader.ReadInt32();
            newObj.cMaxConnections = binaryReader.ReadInt32();
            newObj.strWorldName = PStringChar.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("cConnections = " + cConnections);
            rootNode.Nodes.Add("cMaxConnections = " + cMaxConnections);
            rootNode.Nodes.Add("strWorldName = " + strWorldName.m_buffer);
            treeView.Nodes.Add(rootNode);
        }
    }


    public class PlayerDescription
    {
        public CACQualities CACQualities;
        public CM_Character.PlayerModule PlayerModule;
        public PList<ContentProfile> clist;
        public PList<InventoryPlacement> ilist;

        public static PlayerDescription read(BinaryReader binaryReader)
        {
            PlayerDescription newObj = new PlayerDescription();
            newObj.CACQualities = CACQualities.read(binaryReader);
            newObj.PlayerModule = CM_Character.PlayerModule.read(binaryReader);
            Util.readToAlign(binaryReader);
            newObj.clist = PList<ContentProfile>.read(binaryReader);
            newObj.ilist = PList<InventoryPlacement>.read(binaryReader);
            return newObj;
        }

        public void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            TreeNode CACQualitiesNode = rootNode.Nodes.Add("CACQualities = ");
            CACQualities.contributeToTreeNode(CACQualitiesNode);
            TreeNode PlayerModuleNode = rootNode.Nodes.Add("PlayerModule = ");
            PlayerModule.contributeToTreeNode(PlayerModuleNode);
            
            TreeNode ContentProfileNode = rootNode.Nodes.Add("clist = ");
            foreach (ContentProfile element in clist.list)
            {
                TreeNode clistItemNode = ContentProfileNode.Nodes.Add("item");
                TreeNode clistIIDNode = clistItemNode.Nodes.Add("m_iid = " + element.m_iid);
                TreeNode clistContainerNode = clistItemNode.Nodes.Add("m_uContainerProperties = " + element.m_uContainerProperties);
            }
            TreeNode InventoryPlacementProfileNode = rootNode.Nodes.Add("ilist = ");
            foreach (InventoryPlacement element in ilist.list)
            {
                TreeNode ilistItemNode = InventoryPlacementProfileNode.Nodes.Add("item");
                TreeNode ilistIIDNode = ilistItemNode.Nodes.Add("iid_ = " + element.iid_);
                TreeNode ilistLocNode = ilistItemNode.Nodes.Add("loc_ = " + element.loc_);
                TreeNode ilistPriorityNode = ilistItemNode.Nodes.Add("priority_ = " + element.priority_);
            }
            
            treeView.Nodes.Add(rootNode);
        }
    }

    public class CACQualities
    {
        public enum QualitiesPackHeader
        {
            Packed_None = 0,
            Packed_AttributeCache = (1 << 0),  //0x01
            Packed_SkillHashTable = (1 << 1), //0x02
            Packed_Body = (1 << 2), //0x04
            Packed_Attributes = (1 << 3), //0x08
            Packed_EmoteTable = (1 << 4), //0x10
            Packed_CreationProfileList = (1 << 5), //0x20
            Packed_PageDataList = (1 << 6), //0x40
            Packed_GeneratorTable = (1 << 7), //0x80
            Packed_SpellBook = (1 << 8), //0x0100
            Packed_EnchantmentRegistry = (1 << 9), //0x0200
            Packed_GeneratorRegistry = (1 << 10), //0x0400
            Packed_GeneratorQueue = (1 << 11), //0x0800
        }

        public CBaseQualities CBaseQualities;
        public uint header;
        public WeenieType _weenie_type;
        public AttributeCache _attribCache;
        public PackableHashTable<STypeSkill, Skill> _skillStatsTable = new PackableHashTable<STypeSkill, Skill>();
        public PackableHashTable<SpellID, float> _spell_book = new PackableHashTable<SpellID, float>();
        public EnchantmentRegistry _enchantment_reg;

        public static CACQualities read(BinaryReader binaryReader)
        {
            CACQualities newObj = new CACQualities();
            newObj.CBaseQualities = CBaseQualities.read(binaryReader);
            newObj.header = binaryReader.ReadUInt32();
            newObj._weenie_type = (WeenieType)binaryReader.ReadUInt32();
            if ((newObj.header & (uint)QualitiesPackHeader.Packed_AttributeCache) != 0)
            {
                newObj._attribCache = AttributeCache.read(binaryReader);
            }
            if ((newObj.header & (uint)QualitiesPackHeader.Packed_SkillHashTable) != 0)
            {
                newObj._skillStatsTable = PackableHashTable<STypeSkill, Skill>.read(binaryReader);
            }
            if ((newObj.header & (uint)QualitiesPackHeader.Packed_SpellBook) != 0)
            {
                // the float value represents a SpellBookPage which appears to be "_casting_likelihood".
                newObj._spell_book = PackableHashTable<SpellID, float>.read(binaryReader);
            }
            if ((newObj.header & (uint)QualitiesPackHeader.Packed_EnchantmentRegistry) != 0)
            {
                newObj._enchantment_reg = EnchantmentRegistry.read(binaryReader);
            }
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
            node.Expand();
            TreeNode CBaseQualitiesNode = node.Nodes.Add("CBaseQualities = ");
            CBaseQualities.contributeToTreeNode(CBaseQualitiesNode);

            node.Nodes.Add("header = " + header);
            node.Nodes.Add("_weenie_type = " + _weenie_type);

            TreeNode attribCacheNode = node.Nodes.Add("_attribCache = ");
            if ((header & (uint)QualitiesPackHeader.Packed_AttributeCache) != 0)
            {
                _attribCache.contributeToTreeNode(attribCacheNode);
            }

            TreeNode skillStatsNode = node.Nodes.Add("_skillStatsTable = ");
            if ((header & (uint)QualitiesPackHeader.Packed_SkillHashTable) != 0)
            {
                foreach (KeyValuePair<STypeSkill, Skill> element in _skillStatsTable.hashTable)
                {
                    TreeNode thisStatNode = skillStatsNode.Nodes.Add(element.Key + " = ");
                    Skill thisSkill = element.Value;
                    thisSkill.contributeToTreeNode(thisStatNode);
                    //node.Nodes.Add(element.Key + " -> " + element.Value);
                }
            }

            TreeNode spellBookNode = node.Nodes.Add("_spell_book = ");
            if ((header & (uint)QualitiesPackHeader.Packed_SpellBook) != 0)
            {
                _spell_book.contributeToTreeNode(spellBookNode);
            }
            TreeNode enchantmentRegNode = node.Nodes.Add("_enchantment_reg = ");
            if ((header & (uint)QualitiesPackHeader.Packed_EnchantmentRegistry) != 0)
            {
                _enchantment_reg.contributeToTreeNode(enchantmentRegNode);
            }
        }
    }

    public class CBaseQualities
    {
        public enum BaseQualitiesPackHeader
        {
            Packed_None = 0,
            Packed_IntStats = (1 << 0),  //0x01
            Packed_BoolStats = (1 << 1), //0x02
            Packed_FloatStats = (1 << 2), //0x04
            Packed_DataIDStats = (1 << 3), //0x08
            Packed_StringStats = (1 << 4), //0x10
            Packed_PositionHashTable = (1 << 5), //0x20
            Packed_IIDStats = (1 << 6), //0x40
            Packed_Int64Stats = (1 << 7), //0x80
        }

        public uint header;
        public WeenieType _weenie_type;
        public PackableHashTable<STypeInt, int> _intStatsTable = new PackableHashTable<STypeInt, int>();
        public PackableHashTable<STypeInt64, long> _int64StatsTable = new PackableHashTable<STypeInt64, long>();
        public PackableHashTable<STypeBool, int> _boolStatsTable = new PackableHashTable<STypeBool, int>();
        public PackableHashTable<STypeFloat, double> _floatStatsTable = new PackableHashTable<STypeFloat, double>();
        public PackableHashTable<STypeString, PStringChar> _strStatsTable = new PackableHashTable<STypeString, PStringChar>();
        public PackableHashTable<STypeDID, uint> _didStatsTable = new PackableHashTable<STypeDID, uint>();
        public PackableHashTable<STypeIID, uint> _iidStatsTable = new PackableHashTable<STypeIID, uint>();
        public PackableHashTable<STypePosition, Position> _posStatsTable = new PackableHashTable<STypePosition, Position>();

        public static CBaseQualities read(BinaryReader binaryReader)
        {
            CBaseQualities newObj = new CBaseQualities();
            newObj.header = binaryReader.ReadUInt32();
            newObj._weenie_type = (WeenieType)binaryReader.ReadUInt32();
            if ((newObj.header & (uint)BaseQualitiesPackHeader.Packed_IntStats) != 0)
            {
                newObj._intStatsTable = PackableHashTable<STypeInt, int>.read(binaryReader);
            }
            if ((newObj.header & (uint)BaseQualitiesPackHeader.Packed_Int64Stats) != 0)
            {
                newObj._int64StatsTable = PackableHashTable<STypeInt64, long>.read(binaryReader);
            }
            if ((newObj.header & (uint)BaseQualitiesPackHeader.Packed_BoolStats) != 0)
            {
                newObj._boolStatsTable = PackableHashTable<STypeBool, int>.read(binaryReader);
            }
            if ((newObj.header & (uint)BaseQualitiesPackHeader.Packed_FloatStats) != 0)
            {
                newObj._floatStatsTable = PackableHashTable<STypeFloat, double>.read(binaryReader);
            }
            if ((newObj.header & (uint)BaseQualitiesPackHeader.Packed_StringStats) != 0)
            {
                newObj._strStatsTable = PackableHashTable<STypeString, PStringChar>.read(binaryReader);
            }
            if ((newObj.header & (uint)BaseQualitiesPackHeader.Packed_DataIDStats) != 0)
            {
                newObj._didStatsTable = PackableHashTable<STypeDID, uint>.read(binaryReader);
            }
            if ((newObj.header & (uint)BaseQualitiesPackHeader.Packed_IIDStats) != 0)
            {
                newObj._iidStatsTable = PackableHashTable<STypeIID, uint>.read(binaryReader);
            }
            if ((newObj.header & (uint)BaseQualitiesPackHeader.Packed_PositionHashTable) != 0)
            {
                newObj._posStatsTable = PackableHashTable<STypePosition, Position>.read(binaryReader);
            }

            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
            node.Nodes.Add("header = " + header);
            node.Nodes.Add("_weenie_type = " + _weenie_type);

            TreeNode intStatsNode = node.Nodes.Add("_intStatsTable = ");
            if ((header & (uint)BaseQualitiesPackHeader.Packed_IntStats) != 0)
            {
                _intStatsTable.contributeToTreeNode(intStatsNode);
            }
            TreeNode int64StatsNode = node.Nodes.Add("_int64StatsTable = ");
            if ((header & (uint)BaseQualitiesPackHeader.Packed_Int64Stats) != 0)
            {
                _int64StatsTable.contributeToTreeNode(int64StatsNode);
            }
            TreeNode boolStatsNode = node.Nodes.Add("_boolStatsTable = ");
            if ((header & (uint)BaseQualitiesPackHeader.Packed_BoolStats) != 0)
            {
                _boolStatsTable.contributeToTreeNode(boolStatsNode);
            }
            TreeNode floatStatsNode = node.Nodes.Add("_floatStatsTable = ");
            if ((header & (uint)BaseQualitiesPackHeader.Packed_FloatStats) != 0)
            {
                _floatStatsTable.contributeToTreeNode(floatStatsNode);
            }
            TreeNode strStatsNode = node.Nodes.Add("_strStatsTable = ");
            if ((header & (uint)BaseQualitiesPackHeader.Packed_StringStats) != 0)
            {
                _strStatsTable.contributeToTreeNode(strStatsNode);
            }
            TreeNode didStatsNode = node.Nodes.Add("_didStatsTable = ");
            if ((header & (uint)BaseQualitiesPackHeader.Packed_DataIDStats) != 0)
            {
                _didStatsTable.contributeToTreeNode(didStatsNode);
            }
            TreeNode iidStatsNode = node.Nodes.Add("_iidStatsTable = ");
            if ((header & (uint)BaseQualitiesPackHeader.Packed_IIDStats) != 0)
            {
                _iidStatsTable.contributeToTreeNode(iidStatsNode);
            }
            TreeNode posStatsNode = node.Nodes.Add("_posStatsTable = ");
            if ((header & (uint)BaseQualitiesPackHeader.Packed_PositionHashTable) != 0)
            {
                foreach (KeyValuePair<STypePosition, Position> element in _posStatsTable.hashTable)
                {
                    TreeNode thisPosNode = posStatsNode.Nodes.Add(element.Key + " = ");
                    Position thisPos = element.Value;
                    thisPos.contributeToTreeNode(thisPosNode);
                }
            }
        }
    }

    public class AttributeCache
    {
        public uint header;
        public Attribute _strength;
        public Attribute _endurance;
        public Attribute _quickness;
        public Attribute _coordination;
        public Attribute _focus;
        public Attribute _self;
        public SecondaryAttribute _health;
        public SecondaryAttribute _stamina;
        public SecondaryAttribute _mana;

        public static AttributeCache read(BinaryReader binaryReader)
        {
            AttributeCache newObj = new AttributeCache();
            newObj.header = binaryReader.ReadUInt32();
            newObj._strength = Attribute.read(binaryReader);
            newObj._endurance = Attribute.read(binaryReader);
            newObj._quickness = Attribute.read(binaryReader);
            newObj._coordination = Attribute.read(binaryReader);
            newObj._focus = Attribute.read(binaryReader);
            newObj._self = Attribute.read(binaryReader);
            newObj._health = SecondaryAttribute.read(binaryReader);
            newObj._stamina = SecondaryAttribute.read(binaryReader);
            newObj._mana = SecondaryAttribute.read(binaryReader);
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
            node.Nodes.Add("header = " + header);
            TreeNode strengthNode = node.Nodes.Add("_strength = ");
            _strength.contributeToTreeNode(strengthNode);
            TreeNode enduranceNode = node.Nodes.Add("_endurance = ");
            _endurance.contributeToTreeNode(enduranceNode);
            TreeNode quicknessNode = node.Nodes.Add("_quickness = ");
            _quickness.contributeToTreeNode(quicknessNode);
            TreeNode coordinationNode = node.Nodes.Add("_coordination = ");
            _coordination.contributeToTreeNode(coordinationNode);
            TreeNode focusNode = node.Nodes.Add("_focus = ");
            _focus.contributeToTreeNode(focusNode);
            TreeNode selfNode = node.Nodes.Add("_self = ");
            _self.contributeToTreeNode(selfNode);
            TreeNode healthNode = node.Nodes.Add("_health = ");
            _health.contributeToTreeNode(healthNode);
            TreeNode staminaNode = node.Nodes.Add("_stamina = ");
            _stamina.contributeToTreeNode(staminaNode);
            TreeNode manaNode = node.Nodes.Add("_mana = ");
            _mana.contributeToTreeNode(manaNode);
        }
    }

    public class EnchantmentRegistry
    {
        public uint header;
        public PList<CM_Magic.Enchantment> _mult_list = new PList<CM_Magic.Enchantment>();
        public PList<CM_Magic.Enchantment> _add_list = new PList<CM_Magic.Enchantment>();
        public PList<CM_Magic.Enchantment> _cooldown_list = new PList<CM_Magic.Enchantment>();
        public CM_Magic.Enchantment _vitae = new CM_Magic.Enchantment();

        public static EnchantmentRegistry read(BinaryReader binaryReader)
        {
            EnchantmentRegistry newObj = new EnchantmentRegistry();
            newObj.header = binaryReader.ReadUInt32();
            if ((newObj.header & (uint)EnchantmentRegistryPackHeader.Packed_MultList) != 0)
            {
                newObj._mult_list = PList<CM_Magic.Enchantment>.read(binaryReader);
            }
            if ((newObj.header & (uint)EnchantmentRegistryPackHeader.Packed_AddList) != 0)
            {
                newObj._add_list = PList<CM_Magic.Enchantment>.read(binaryReader);
            }
            if ((newObj.header & (uint)EnchantmentRegistryPackHeader.Packed_Cooldown) != 0)
            {
                newObj._cooldown_list = PList<CM_Magic.Enchantment>.read(binaryReader);
            }
            if ((newObj.header & (uint)EnchantmentRegistryPackHeader.Packed_Vitae) != 0)
            {
                newObj._vitae = CM_Magic.Enchantment.read(binaryReader);
            }

            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
            node.Nodes.Add("header = " + header);

            TreeNode multListNode = node.Nodes.Add("_mult_list = ");
            if ((header & (uint)EnchantmentRegistryPackHeader.Packed_MultList) != 0)
            {
                foreach (CM_Magic.Enchantment element in _mult_list.list)
                {
                    CM_Magic.Enchantment thisEnchantment = element;
                    ushort spell_id = (ushort)element._id;
                    TreeNode thisEnchantmentNode = multListNode.Nodes.Add("Enchantment = " + (SpellID)spell_id);
                    thisEnchantment.contributeToTreeNode(thisEnchantmentNode);
                }
            }
            TreeNode addListNode = node.Nodes.Add("_add_list = ");
            if ((header & (uint)EnchantmentRegistryPackHeader.Packed_AddList) != 0)
            {
                foreach (CM_Magic.Enchantment element in _add_list.list)
                {
                    CM_Magic.Enchantment thisEnchantment = element;
                    ushort spell_id = (ushort)element._id;
                    TreeNode thisEnchantmentNode = addListNode.Nodes.Add("Enchantment = " + (SpellID)spell_id);
                    thisEnchantment.contributeToTreeNode(thisEnchantmentNode);
                }
            }
            TreeNode cooldownListNode = node.Nodes.Add("_cooldown_list = ");
            if ((header & (uint)EnchantmentRegistryPackHeader.Packed_Cooldown) != 0)
            {
                foreach (CM_Magic.Enchantment element in _cooldown_list.list)
                {
                    CM_Magic.Enchantment thisEnchantment = element;
                    ushort spell_id = (ushort)element._id;
                    TreeNode thisEnchantmentNode = cooldownListNode.Nodes.Add("Enchantment = " + (SpellID)spell_id);
                    thisEnchantment.contributeToTreeNode(thisEnchantmentNode);
                }
            }
            TreeNode vitaeNode = node.Nodes.Add("_vitae = ");
            if ((header & (uint)EnchantmentRegistryPackHeader.Packed_Vitae) != 0)
            {
                _vitae.contributeToTreeNode(vitaeNode);
            }

        }
    }

    public class ContentProfile
    {
        public uint m_iid;
        public uint m_uContainerProperties;

        public static ContentProfile read(BinaryReader binaryReader)
        {
            ContentProfile newObj = new ContentProfile();
            newObj.m_iid = binaryReader.ReadUInt32();
            newObj.m_uContainerProperties = binaryReader.ReadUInt32();
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
            node.Nodes.Add("m_iid = " + m_iid);
            node.Nodes.Add("m_uContainerProperties = " + m_uContainerProperties);
        }
    }

    public class InventoryPlacement
    {
        public uint iid_;
        public uint loc_;
        public uint priority_;

        public static InventoryPlacement read(BinaryReader binaryReader)
        {
            InventoryPlacement newObj = new InventoryPlacement();
            newObj.iid_ = binaryReader.ReadUInt32();
            newObj.loc_ = binaryReader.ReadUInt32();
            newObj.priority_ = binaryReader.ReadUInt32();
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
            node.Nodes.Add("iid_ = " + iid_);
            node.Nodes.Add("loc_ = " + loc_);
            node.Nodes.Add("priority_ = " + priority_);
        }
    }


}
