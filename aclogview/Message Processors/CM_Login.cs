using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using aclogview;
using static CM_Inventory;

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
        public int Length;

        public static CharacterIdentity read(BinaryReader binaryReader) {
            CharacterIdentity newObj = new CharacterIdentity();
            var startPosition = binaryReader.BaseStream.Position;
            newObj.gid_ = binaryReader.ReadUInt32();
            newObj.name_ = PStringChar.read(binaryReader);
            newObj.secondsGreyedOut_ = binaryReader.ReadUInt32();
            newObj.Length = (int)(binaryReader.BaseStream.Position - startPosition);
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node) {
            node.Nodes.Add("gid_ = " + Utility.FormatHex(gid_));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            node.Nodes.Add("name_ = " + name_.m_buffer);
            ContextInfo.AddToList(new ContextInfo { Length = name_.Length, DataType = DataType.Serialized_AsciiString });
            node.Nodes.Add("secondsGreyedOut_ = " + secondsGreyedOut_);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
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
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Opcode });
            rootNode.Nodes.Add("status_ = " + status_);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            TreeNode setNode = rootNode.Nodes.Add("set_ = ");
            // Calculate character set size
            var charSetSize = 4;
            for (int i = 0; i < set_.Count; i++)
            {
                charSetSize += set_[i].Length;
            }
            ContextInfo.AddToList(new ContextInfo { Length = charSetSize }, updateDataIndex: false);
            // Skip character list count uint
            ContextInfo.DataIndex += 4;
            for (int i = 0; i < set_.Count; i++) {
                TreeNode characterNode = setNode.Nodes.Add($"character {i+1} = ");
                ContextInfo.AddToList(new ContextInfo { Length = set_[i].Length }, updateDataIndex: false);
                set_[i].contributeToTreeNode(characterNode);
            }
            TreeNode delSetNode = rootNode.Nodes.Add("delSet_ = ");
            // Calculate deleted character set size
            charSetSize = 4;
            for (int i = 0; i < delSet_.Count; i++)
            {
                charSetSize += delSet_[i].Length;
            }
            ContextInfo.AddToList(new ContextInfo { Length = charSetSize }, updateDataIndex: false);
            // Skip character list count uint
            ContextInfo.DataIndex += 4;
            for (int i = 0; i < delSet_.Count; i++)
            {
                TreeNode characterNode = delSetNode.Nodes.Add($"character {i+1} = ");
                ContextInfo.AddToList(new ContextInfo { Length = delSet_[i].Length }, updateDataIndex: false);
                delSet_[i].contributeToTreeNode(characterNode);
            }
            rootNode.Nodes.Add("numAllowedCharacters_ = " + numAllowedCharacters_);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            rootNode.Nodes.Add("account_ = " + account_.m_buffer);
            ContextInfo.AddToList(new ContextInfo { Length = account_.Length, DataType = DataType.Serialized_AsciiString });
            rootNode.Nodes.Add("m_fUseTurbineChat = " + m_fUseTurbineChat);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            rootNode.Nodes.Add("m_fHasThroneofDestiny = " + m_fHasThroneofDestiny);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
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
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Opcode });
            rootNode.Nodes.Add("cConnections = " + cConnections);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            rootNode.Nodes.Add("cMaxConnections = " + cMaxConnections);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            rootNode.Nodes.Add("strWorldName = " + strWorldName.m_buffer);
            ContextInfo.AddToList(new ContextInfo { Length = strWorldName.Length, DataType = DataType.Serialized_AsciiString });
            treeView.Nodes.Add(rootNode);
        }
    }


    public class PlayerDescription : Message
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
            newObj.clist = PList<ContentProfile>.read(binaryReader);
            newObj.ilist = PList<InventoryPlacement>.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Header16Bytes });
            TreeNode CACQualitiesNode = rootNode.Nodes.Add("CACQualities = ");
            ContextInfo.AddToList(new ContextInfo { Length = CACQualities.Length }, updateDataIndex: false);
            CACQualities.contributeToTreeNode(CACQualitiesNode);
            TreeNode PlayerModuleNode = rootNode.Nodes.Add("PlayerModule = ");
            ContextInfo.AddToList(new ContextInfo { Length = PlayerModule.Length }, updateDataIndex: false);
            PlayerModule.contributeToTreeNode(PlayerModuleNode);
            TreeNode ContentProfileNode = rootNode.Nodes.Add("clist = ");
            ContextInfo.AddToList(new ContextInfo { Length = clist.Length }, updateDataIndex: false);
            // Skip Plist count dword
            ContextInfo.DataIndex += 4;
            for (int i = 0; i < clist.list.Count; i++)
            {
                TreeNode clistItemNode = ContentProfileNode.Nodes.Add($"item {i + 1}");
                ContextInfo.AddToList(new ContextInfo { Length = clist.list[i].Length }, updateDataIndex: false);
                clist.list[i].contributeToTreeNode(clistItemNode);
            }
            TreeNode InventoryPlacementProfileNode = rootNode.Nodes.Add("ilist = ");
            ContextInfo.AddToList(new ContextInfo { Length = ilist.Length }, updateDataIndex: false);
            // Skip Plist count dword
            ContextInfo.DataIndex += 4;
            for (int i = 0; i < ilist.list.Count; i++)
            {
                TreeNode ilistItemNode = InventoryPlacementProfileNode.Nodes.Add($"item {i + 1}");
                ContextInfo.AddToList(new ContextInfo { Length = ilist.list[i].Length }, updateDataIndex: false);
                ilist.list[i].contributeToTreeNode(ilistItemNode);
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
        public PackableHashTable<uint, float> _spell_book = new PackableHashTable<uint, float>();
        public EnchantmentRegistry _enchantment_reg;
        public int Length;
        public List<string> packedItems = new List<string>(); // Display purposes

        public static CACQualities read(BinaryReader binaryReader)
        {
            CACQualities newObj = new CACQualities();
            var startPosition = binaryReader.BaseStream.Position;
            newObj.CBaseQualities = CBaseQualities.read(binaryReader);
            newObj.header = binaryReader.ReadUInt32();
            newObj._weenie_type = (WeenieType)binaryReader.ReadUInt32();
            if ((newObj.header & (uint)QualitiesPackHeader.Packed_AttributeCache) != 0)
            {
                newObj._attribCache = AttributeCache.read(binaryReader);
                newObj.packedItems.Add(QualitiesPackHeader.Packed_AttributeCache.ToString());
            }
            if ((newObj.header & (uint)QualitiesPackHeader.Packed_SkillHashTable) != 0)
            {
                newObj._skillStatsTable = PackableHashTable<STypeSkill, Skill>.read(binaryReader);
                newObj.packedItems.Add(QualitiesPackHeader.Packed_SkillHashTable.ToString());
            }
            if ((newObj.header & (uint)QualitiesPackHeader.Packed_SpellBook) != 0)
            {
                newObj._spell_book = PackableHashTable<uint, float>.read(binaryReader);
                newObj.packedItems.Add(QualitiesPackHeader.Packed_SpellBook.ToString());
            }
            if ((newObj.header & (uint)QualitiesPackHeader.Packed_EnchantmentRegistry) != 0)
            {
                newObj._enchantment_reg = EnchantmentRegistry.read(binaryReader);
                newObj.packedItems.Add(QualitiesPackHeader.Packed_EnchantmentRegistry.ToString());
            }
            newObj.Length = (int)(binaryReader.BaseStream.Position - startPosition);
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
            node.Expand();
            TreeNode CBaseQualitiesNode = node.Nodes.Add("CBaseQualities = ");
            ContextInfo.AddToList(new ContextInfo { Length = CBaseQualities.Length }, updateDataIndex: false);
            CBaseQualities.contributeToTreeNode(CBaseQualitiesNode);

            TreeNode headerNode = node.Nodes.Add("header = " + Utility.FormatHex(header));
            ContextInfo.AddToList(new ContextInfo { Length = 4 }, updateDataIndex: false );
            for (int i = 0; i < packedItems.Count; i++)
            {
                headerNode.Nodes.Add(packedItems[i]);
                ContextInfo.AddToList(new ContextInfo { Length = 4 }, updateDataIndex: false);
            }
            // Now skip the header
            ContextInfo.DataIndex += 4;
            node.Nodes.Add("_weenie_type = " + _weenie_type);
            ContextInfo.AddToList(new ContextInfo { Length = 4 }) ;

            if ((header & (uint)QualitiesPackHeader.Packed_AttributeCache) != 0)
            {
                TreeNode attribCacheNode = node.Nodes.Add("_attribCache = ");
                ContextInfo.AddToList(new ContextInfo { Length = _attribCache.Length }, updateDataIndex: false);
                _attribCache.contributeToTreeNode(attribCacheNode);
            }

            if ((header & (uint)QualitiesPackHeader.Packed_SkillHashTable) != 0)
            {
                TreeNode skillStatsNode = node.Nodes.Add("_skillStatsTable = ");
                ContextInfo.AddToList(new ContextInfo { Length = _skillStatsTable.Length }, updateDataIndex: false);
                // Skip PackableHashTable count dword
                ContextInfo.DataIndex += 4;
                foreach (KeyValuePair<STypeSkill, Skill> element in _skillStatsTable.hashTable)
                {
                    TreeNode thisStatNode = skillStatsNode.Nodes.Add(element.Key + " = ");
                    Skill thisSkill = element.Value;
                    ContextInfo.AddToList(new ContextInfo { Length = sizeof(STypeSkill) + thisSkill.Length }, updateDataIndex: false);
                    // Skip STypeSkill dword
                    ContextInfo.DataIndex += 4;
                    thisSkill.contributeToTreeNode(thisStatNode);
                }
            }

            if ((header & (uint)QualitiesPackHeader.Packed_SpellBook) != 0)
            {
                TreeNode spellBookNode = node.Nodes.Add("_spell_book = ");
                ContextInfo.AddToList(new ContextInfo { Length = _spell_book.Length }, updateDataIndex: false);
                // Skip PackableHashTable count dword
                ContextInfo.DataIndex += 4;
                foreach (KeyValuePair<uint, float> element in _spell_book.hashTable)
                {
                    TreeNode spellNode = spellBookNode.Nodes.Add($"({element.Key}) {(SpellID)element.Key} = ");
                    ContextInfo.AddToList(new ContextInfo { DataType = DataType.SpellID_uint });
                    spellNode.Nodes.Add("_casting_likelihood = " + element.Value);
                    ContextInfo.AddToList(new ContextInfo { Length = sizeof(float) });
                }
            }
            
            if ((header & (uint)QualitiesPackHeader.Packed_EnchantmentRegistry) != 0)
            {
                TreeNode enchantmentRegNode = node.Nodes.Add("_enchantment_reg = ");
                ContextInfo.AddToList(new ContextInfo { Length = _enchantment_reg.Length }, updateDataIndex: false);
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
        public int Length;
        public List<string> packedItems = new List<string>(); // Display purposes

        public static CBaseQualities read(BinaryReader binaryReader)
        {
            CBaseQualities newObj = new CBaseQualities();
            var startPosition = binaryReader.BaseStream.Position;
            newObj.header = binaryReader.ReadUInt32();
            newObj._weenie_type = (WeenieType)binaryReader.ReadUInt32();
            if ((newObj.header & (uint)BaseQualitiesPackHeader.Packed_IntStats) != 0)
            {
                newObj._intStatsTable = PackableHashTable<STypeInt, int>.read(binaryReader);
                newObj.packedItems.Add(BaseQualitiesPackHeader.Packed_IntStats.ToString());
            }
            if ((newObj.header & (uint)BaseQualitiesPackHeader.Packed_Int64Stats) != 0)
            {
                newObj._int64StatsTable = PackableHashTable<STypeInt64, long>.read(binaryReader);
                newObj.packedItems.Add(BaseQualitiesPackHeader.Packed_Int64Stats.ToString());
            }
            if ((newObj.header & (uint)BaseQualitiesPackHeader.Packed_BoolStats) != 0)
            {
                newObj._boolStatsTable = PackableHashTable<STypeBool, int>.read(binaryReader);
                newObj.packedItems.Add(BaseQualitiesPackHeader.Packed_BoolStats.ToString());
            }
            if ((newObj.header & (uint)BaseQualitiesPackHeader.Packed_FloatStats) != 0)
            {
                newObj._floatStatsTable = PackableHashTable<STypeFloat, double>.read(binaryReader);
                newObj.packedItems.Add(BaseQualitiesPackHeader.Packed_FloatStats.ToString());
            }
            if ((newObj.header & (uint)BaseQualitiesPackHeader.Packed_StringStats) != 0)
            {
                newObj._strStatsTable = PackableHashTable<STypeString, PStringChar>.read(binaryReader);
                newObj.packedItems.Add(BaseQualitiesPackHeader.Packed_StringStats.ToString());
            }
            if ((newObj.header & (uint)BaseQualitiesPackHeader.Packed_DataIDStats) != 0)
            {
                newObj._didStatsTable = PackableHashTable<STypeDID, uint>.read(binaryReader);
                newObj.packedItems.Add(BaseQualitiesPackHeader.Packed_DataIDStats.ToString());
            }
            if ((newObj.header & (uint)BaseQualitiesPackHeader.Packed_IIDStats) != 0)
            {
                newObj._iidStatsTable = PackableHashTable<STypeIID, uint>.read(binaryReader);
                newObj.packedItems.Add(BaseQualitiesPackHeader.Packed_IIDStats.ToString());
            }
            if ((newObj.header & (uint)BaseQualitiesPackHeader.Packed_PositionHashTable) != 0)
            {
                newObj._posStatsTable = PackableHashTable<STypePosition, Position>.read(binaryReader);
                newObj.packedItems.Add(BaseQualitiesPackHeader.Packed_PositionHashTable.ToString());
            }
            newObj.Length = (int)(binaryReader.BaseStream.Position - startPosition);
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
            TreeNode headerNode = node.Nodes.Add("header = " + Utility.FormatHex(header));
            ContextInfo.AddToList(new ContextInfo { Length = 4 }, updateDataIndex: false);
            for (int i = 0; i < packedItems.Count; i++)
            {
                headerNode.Nodes.Add(packedItems[i]);
                ContextInfo.AddToList(new ContextInfo { Length = 4 }, updateDataIndex: false);
            }
            // Now skip over the header
            ContextInfo.DataIndex += 4;
            node.Nodes.Add("_weenie_type = " + _weenie_type);
            ContextInfo.AddToList(new ContextInfo { Length = 4 } );
            
            if ((header & (uint)BaseQualitiesPackHeader.Packed_IntStats) != 0)
            {
                TreeNode intStatsNode = node.Nodes.Add("_intStatsTable = ");
                ContextInfo.AddToList(new ContextInfo { Length = _intStatsTable.Length }, updateDataIndex: false);
                _intStatsTable.contributeToTreeNode(intStatsNode);
                // Skip PackableHashTable count dword
                ContextInfo.DataIndex += 4;
                for (int i = 0; i < _intStatsTable.hashTable.Count; i++)
                {
                    ContextInfo.AddToList(new ContextInfo { Length = sizeof(STypeInt) + sizeof(int) } );
                }
            }
            if ((header & (uint)BaseQualitiesPackHeader.Packed_Int64Stats) != 0)
            {
                TreeNode int64StatsNode = node.Nodes.Add("_int64StatsTable = ");
                ContextInfo.AddToList(new ContextInfo { Length = _int64StatsTable.Length }, updateDataIndex: false);
                _int64StatsTable.contributeToTreeNode(int64StatsNode);
                // Skip PackableHashTable count dword
                ContextInfo.DataIndex += 4;
                for (int i = 0; i < _int64StatsTable.hashTable.Count; i++)
                {
                    ContextInfo.AddToList(new ContextInfo { Length = sizeof(STypeInt64) + sizeof(long) });
                }
            }
            if ((header & (uint)BaseQualitiesPackHeader.Packed_BoolStats) != 0)
            {
                TreeNode boolStatsNode = node.Nodes.Add("_boolStatsTable = ");
                ContextInfo.AddToList(new ContextInfo { Length = _boolStatsTable.Length }, updateDataIndex: false);
                _boolStatsTable.contributeToTreeNode(boolStatsNode);
                // Skip PackableHashTable count dword
                ContextInfo.DataIndex += 4;
                for (int i = 0; i < _boolStatsTable.hashTable.Count; i++)
                {
                    ContextInfo.AddToList(new ContextInfo { Length = sizeof(STypeBool) + sizeof(int) });
                }
            }
            if ((header & (uint)BaseQualitiesPackHeader.Packed_FloatStats) != 0)
            {
                TreeNode floatStatsNode = node.Nodes.Add("_floatStatsTable = ");
                ContextInfo.AddToList(new ContextInfo { Length = _floatStatsTable.Length }, updateDataIndex: false);
                _floatStatsTable.contributeToTreeNode(floatStatsNode);
                // Skip PackableHashTable count dword
                ContextInfo.DataIndex += 4;
                for (int i = 0; i < _floatStatsTable.hashTable.Count; i++)
                {
                    ContextInfo.AddToList(new ContextInfo { Length = sizeof(STypeFloat) + sizeof(double) });
                }
            }
            if ((header & (uint)BaseQualitiesPackHeader.Packed_StringStats) != 0)
            {
                // TODO: Possibly separate the string type and string into different tree nodes
                // so context info can be added to the string.
                TreeNode strStatsNode = node.Nodes.Add("_strStatsTable = ");
                ContextInfo.AddToList(new ContextInfo { Length = _strStatsTable.Length }, updateDataIndex: false);
                _strStatsTable.contributeToTreeNode(strStatsNode);
                // Skip PackableHashTable count dword
                ContextInfo.DataIndex += 4;
                foreach (KeyValuePair<STypeString, PStringChar> element in _strStatsTable.hashTable)
                {
                    ContextInfo.AddToList(new ContextInfo { Length = sizeof(STypeString) + element.Value.Length });
                }
            }
            if ((header & (uint)BaseQualitiesPackHeader.Packed_DataIDStats) != 0)
            {
                TreeNode didStatsNode = node.Nodes.Add("_didStatsTable = ");
                ContextInfo.AddToList(new ContextInfo { Length = _didStatsTable.Length }, updateDataIndex: false);
                // Skip PackableHashTable count dword
                ContextInfo.DataIndex += 4;
                foreach (KeyValuePair<STypeDID, uint> element in _didStatsTable.hashTable)
                {
                    didStatsNode.Nodes.Add(element.Key + " = " + Utility.FormatHex(element.Value));
                    ContextInfo.AddToList(new ContextInfo { Length = sizeof(STypeDID) + sizeof(uint) });
                }
            }
            if ((header & (uint)BaseQualitiesPackHeader.Packed_IIDStats) != 0)
            {
                TreeNode iidStatsNode = node.Nodes.Add("_iidStatsTable = ");
                ContextInfo.AddToList(new ContextInfo { Length = _iidStatsTable.Length }, updateDataIndex: false);
                // Skip PackableHashTable count dword
                ContextInfo.DataIndex += 4;
                foreach (KeyValuePair<STypeIID, uint> element in _iidStatsTable.hashTable)
                {
                    iidStatsNode.Nodes.Add(element.Key + " = " + Utility.FormatHex(element.Value));
                    ContextInfo.AddToList(new ContextInfo { Length = sizeof(STypeIID) + sizeof(uint) });
                }
            }
            if ((header & (uint)BaseQualitiesPackHeader.Packed_PositionHashTable) != 0)
            {
                TreeNode posStatsNode = node.Nodes.Add("_posStatsTable = ");
                ContextInfo.AddToList(new ContextInfo { Length = _posStatsTable.Length }, updateDataIndex: false);
                // Skip PackableHashTable count dword
                ContextInfo.DataIndex += 4;
                foreach (KeyValuePair<STypePosition, Position> element in _posStatsTable.hashTable)
                {
                    TreeNode thisPosNode = posStatsNode.Nodes.Add(element.Key + " = ");
                    Position thisPos = element.Value;
                    ContextInfo.AddToList(new ContextInfo { Length = sizeof(STypePosition) + thisPos.Length }, updateDataIndex: false);
                    // Skip STypePosition count dword
                    ContextInfo.DataIndex += 4;
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
        public int Length;

        public static AttributeCache read(BinaryReader binaryReader)
        {
            AttributeCache newObj = new AttributeCache();
            var startPosition = binaryReader.BaseStream.Position;
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
            newObj.Length = (int)(binaryReader.BaseStream.Position - startPosition);
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
            TreeNode headerNode = node.Nodes.Add("header = " + Utility.FormatHex(header));
            ContextInfo.AddToList(new ContextInfo { Length = 4 }, updateDataIndex: false);
            foreach (ATTRIBUTE_CACHE_MASK element in Enum.GetValues(typeof(ATTRIBUTE_CACHE_MASK))) {
                if ((header & (uint)element) != 0)
                {
                    headerNode.Nodes.Add(Enum.GetName(typeof(ATTRIBUTE_CACHE_MASK), element));
                    ContextInfo.AddToList(new ContextInfo { Length = 4 }, updateDataIndex: false);
                }
            }
            // Now skip the header
            ContextInfo.DataIndex += 4;
            TreeNode strengthNode = node.Nodes.Add("_strength = ");
            ContextInfo.AddToList(new ContextInfo { Length = _strength.Length }, updateDataIndex: false);
            _strength.contributeToTreeNode(strengthNode);
            TreeNode enduranceNode = node.Nodes.Add("_endurance = ");
            ContextInfo.AddToList(new ContextInfo { Length = _endurance.Length }, updateDataIndex: false);
            _endurance.contributeToTreeNode(enduranceNode);
            TreeNode quicknessNode = node.Nodes.Add("_quickness = ");
            ContextInfo.AddToList(new ContextInfo { Length = _quickness.Length }, updateDataIndex: false);
            _quickness.contributeToTreeNode(quicknessNode);
            TreeNode coordinationNode = node.Nodes.Add("_coordination = ");
            ContextInfo.AddToList(new ContextInfo { Length = _coordination.Length }, updateDataIndex: false);
            _coordination.contributeToTreeNode(coordinationNode);
            TreeNode focusNode = node.Nodes.Add("_focus = ");
            ContextInfo.AddToList(new ContextInfo { Length = _focus.Length }, updateDataIndex: false);
            _focus.contributeToTreeNode(focusNode);
            TreeNode selfNode = node.Nodes.Add("_self = ");
            ContextInfo.AddToList(new ContextInfo { Length = _self.Length }, updateDataIndex: false);
            _self.contributeToTreeNode(selfNode);
            TreeNode healthNode = node.Nodes.Add("_health = ");
            ContextInfo.AddToList(new ContextInfo { Length = _health.Length }, updateDataIndex: false);
            _health.contributeToTreeNode(healthNode);
            TreeNode staminaNode = node.Nodes.Add("_stamina = ");
            ContextInfo.AddToList(new ContextInfo { Length = _stamina.Length }, updateDataIndex: false);
            _stamina.contributeToTreeNode(staminaNode);
            TreeNode manaNode = node.Nodes.Add("_mana = ");
            ContextInfo.AddToList(new ContextInfo { Length = _mana.Length }, updateDataIndex: false);
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
        public int Length;
        public List<string> packedItems = new List<string>(); // Display purposes

        public static EnchantmentRegistry read(BinaryReader binaryReader)
        {
            EnchantmentRegistry newObj = new EnchantmentRegistry();
            var startPosition = binaryReader.BaseStream.Position;
            newObj.header = binaryReader.ReadUInt32();
            if ((newObj.header & (uint)EnchantmentRegistryPackHeader.Packed_MultList) != 0)
            {
                newObj._mult_list = PList<CM_Magic.Enchantment>.read(binaryReader);
                newObj.packedItems.Add(EnchantmentRegistryPackHeader.Packed_MultList.ToString());
            }
            if ((newObj.header & (uint)EnchantmentRegistryPackHeader.Packed_AddList) != 0)
            {
                newObj._add_list = PList<CM_Magic.Enchantment>.read(binaryReader);
                newObj.packedItems.Add(EnchantmentRegistryPackHeader.Packed_AddList.ToString());
            }
            if ((newObj.header & (uint)EnchantmentRegistryPackHeader.Packed_Cooldown) != 0)
            {
                newObj._cooldown_list = PList<CM_Magic.Enchantment>.read(binaryReader);
                newObj.packedItems.Add(EnchantmentRegistryPackHeader.Packed_Cooldown.ToString());
            }
            if ((newObj.header & (uint)EnchantmentRegistryPackHeader.Packed_Vitae) != 0)
            {
                newObj._vitae = CM_Magic.Enchantment.read(binaryReader);
                newObj.packedItems.Add(EnchantmentRegistryPackHeader.Packed_Vitae.ToString());
            }
            newObj.Length = (int)(binaryReader.BaseStream.Position - startPosition);
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
            TreeNode headerNode = node.Nodes.Add("header = " + Utility.FormatHex(header));
            ContextInfo.AddToList(new ContextInfo { Length = 4 }, updateDataIndex: false);
            for (int i = 0; i < packedItems.Count; i++)
            {
                headerNode.Nodes.Add(packedItems[i]);
                ContextInfo.AddToList(new ContextInfo { Length = 4 }, updateDataIndex: false);
            }
            // Now skip the header
            ContextInfo.DataIndex += 4;
            
            if ((header & (uint)EnchantmentRegistryPackHeader.Packed_MultList) != 0)
            {
                TreeNode multListNode = node.Nodes.Add("_mult_list = ");
                ContextInfo.AddToList(new ContextInfo { Length = _mult_list.Length }, updateDataIndex: false);
                // Skip PList count dword
                ContextInfo.DataIndex += 4;
                foreach (CM_Magic.Enchantment element in _mult_list.list)
                {
                    CM_Magic.Enchantment thisEnchantment = element;
                    TreeNode thisEnchantmentNode = multListNode.Nodes.Add("Enchantment = " + (SpellID)element.eid.i_spell_id);
                    ContextInfo.AddToList(new ContextInfo { Length = thisEnchantment.Length }, updateDataIndex: false);
                    thisEnchantment.contributeToTreeNode(thisEnchantmentNode);
                }
            }
            
            if ((header & (uint)EnchantmentRegistryPackHeader.Packed_AddList) != 0)
            {
                TreeNode addListNode = node.Nodes.Add("_add_list = ");
                ContextInfo.AddToList(new ContextInfo { Length = _add_list.Length }, updateDataIndex: false);
                // Skip PList count dword
                ContextInfo.DataIndex += 4;
                foreach (CM_Magic.Enchantment element in _add_list.list)
                {
                    CM_Magic.Enchantment thisEnchantment = element;
                    TreeNode thisEnchantmentNode = addListNode.Nodes.Add("Enchantment = " + (SpellID)element.eid.i_spell_id);
                    ContextInfo.AddToList(new ContextInfo { Length = thisEnchantment.Length }, updateDataIndex: false);
                    thisEnchantment.contributeToTreeNode(thisEnchantmentNode);
                }
            }
            
            if ((header & (uint)EnchantmentRegistryPackHeader.Packed_Cooldown) != 0)
            {
                TreeNode cooldownListNode = node.Nodes.Add("_cooldown_list = ");
                ContextInfo.AddToList(new ContextInfo { Length = _cooldown_list.Length }, updateDataIndex: false);
                // Skip PList count dword
                ContextInfo.DataIndex += 4;
                foreach (CM_Magic.Enchantment element in _cooldown_list.list)
                {
                    CM_Magic.Enchantment thisEnchantment = element;
                    TreeNode thisEnchantmentNode = cooldownListNode.Nodes.Add("Enchantment = " + (SpellID)element.eid.i_spell_id);
                    ContextInfo.AddToList(new ContextInfo { Length = thisEnchantment.Length }, updateDataIndex: false);
                    thisEnchantment.contributeToTreeNode(thisEnchantmentNode);
                }
            }
            
            if ((header & (uint)EnchantmentRegistryPackHeader.Packed_Vitae) != 0)
            {
                TreeNode vitaeNode = node.Nodes.Add("_vitae = ");
                ContextInfo.AddToList(new ContextInfo { Length = _vitae.Length }, updateDataIndex: false);
                _vitae.contributeToTreeNode(vitaeNode);
            }

        }
    }

    public class InventoryPlacement
    {
        public uint iid_;
        public uint loc_;
        public uint priority_;
        public int Length = 12;

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
            TreeNode ilistIIDNode = node.Nodes.Add("iid_ = " + Utility.FormatHex(iid_));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            TreeNode ilistLocNode = node.Nodes.Add("loc_ = " + Utility.FormatHex(loc_));
            ContextInfo.AddToList(new ContextInfo { Length = 4 }, updateDataIndex: false);
            foreach (INVENTORY_LOC e in Enum.GetValues(typeof(INVENTORY_LOC)))
            {
                if ((loc_ & (uint)e) == (uint)e && (uint)e != 0)
                {
                    ilistLocNode.Nodes.Add($"{Enum.GetName(typeof(INVENTORY_LOC), e)}");
                    ContextInfo.AddToList(new ContextInfo { Length = 4 }, updateDataIndex: false);
                }
            }
            // Now skip _loc dword
            ContextInfo.DataIndex += 4;
            TreeNode ilistPriorityNode = node.Nodes.Add("priority_ = " + priority_);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
        }
    }


}
