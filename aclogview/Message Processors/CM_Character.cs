using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using aclogview;

public class CM_Character : MessageProcessor {

    public override bool acceptMessageData(BinaryReader messageDataReader, TreeView outputTreeView) {
        bool handled = true;

        PacketOpcode opcode = Util.readOpcode(messageDataReader);
        switch (opcode) {
            case PacketOpcode.Evt_Character__TeleToPKLArena_ID:
            case PacketOpcode.Evt_Character__TeleToPKArena_ID:
            case PacketOpcode.Evt_Character__TeleToLifestone_ID:
            case PacketOpcode.Evt_Character__LoginCompleteNotification_ID:
            case PacketOpcode.Evt_Character__RequestPing_ID:
            case PacketOpcode.Evt_Character__ReturnPing_ID:
            case PacketOpcode.Evt_Character__ClearPlayerConsentList_ID:
            case PacketOpcode.Evt_Character__DisplayPlayerConsentList_ID:
            case PacketOpcode.Evt_Character__Suicide_ID:
            case PacketOpcode.Evt_Character__TeleToMarketplace_ID:
            case PacketOpcode.Evt_Character__EnterPKLite_ID:
            case PacketOpcode.Evt_Character__EnterGame_ServerReady_ID: {
                    EmptyMessage message = new EmptyMessage(opcode);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Character__PlayerOptionChangedEvent_ID: {
                    PlayerOptionChangedEvent message = PlayerOptionChangedEvent.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Character__StartBarber_ID: {
                    StartBarber message = StartBarber.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Character__AbuseLogRequest_ID: {
                    AbuseLogRequest message = AbuseLogRequest.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Character__AddShortCut_ID: {
                    AddShortCut message = AddShortCut.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Character__RemoveShortCut_ID: {
                    RemoveShortCut message = RemoveShortCut.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Character__CharacterOptionsEvent_ID: {
                    CharacterOptionsEvent message = CharacterOptionsEvent.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Character__QueryAge_ID: {
                    QueryAge message = QueryAge.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Character__QueryAgeResponse_ID: {
                    QueryAgeResponse message = QueryAgeResponse.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Character__QueryBirth_ID: {
                    QueryBirth message = QueryBirth.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Character__AddSpellFavorite_ID: {
                    AddSpellFavorite message = AddSpellFavorite.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Character__RemoveSpellFavorite_ID: {
                    RemoveSpellFavorite message = RemoveSpellFavorite.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Character__RemoveFromPlayerConsentList_ID: {
                    RemoveFromPlayerConsentList message = RemoveFromPlayerConsentList.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Character__AddPlayerPermission_ID: {
                    AddPlayerPermission message = AddPlayerPermission.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Character__RemovePlayerPermission_ID: {
                    RemovePlayerPermission message = RemovePlayerPermission.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Character__SetDesiredComponentLevel_ID: {
                    SetDesiredComponentLevel message = SetDesiredComponentLevel.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Character__ConfirmationRequest_ID: {
                    ConfirmationRequest message = ConfirmationRequest.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Character__ConfirmationResponse_ID: {
                    ConfirmationResponse message = ConfirmationResponse.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Character__ConfirmationDone_ID: {
                    ConfirmationDone message = ConfirmationDone.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Character__SpellbookFilterEvent_ID: {
                    SpellbookFilterEvent message = SpellbookFilterEvent.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            // TODO: Missing Evt_Character__Rename_ID
            case PacketOpcode.Evt_Character__FinishBarber_ID: {
                    FinishBarber message = FinishBarber.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.CHARACTER_ERROR_EVENT:
                {
                    CharacterErrorEvent message = CharacterErrorEvent.read(messageDataReader);
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

    public class PlayerOptionChangedEvent : Message {
        PlayerOption i_po;
        int i_value;

        public static PlayerOptionChangedEvent read(BinaryReader binaryReader) {
            PlayerOptionChangedEvent newObj = new PlayerOptionChangedEvent();
            newObj.i_po = (PlayerOption)binaryReader.ReadUInt32();
            // TODO: These is some more logic right here - need to handle it correctly
            newObj.i_value = binaryReader.ReadInt32();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_po = " + i_po);
            rootNode.Nodes.Add("i_value = " + i_value);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class ShortCutData {
        public int index_;
        public uint objectID_;
        public uint spellID_;

        public static ShortCutData read(BinaryReader binaryReader) {
            ShortCutData newObj = new ShortCutData();
            newObj.index_ = binaryReader.ReadInt32();
            newObj.objectID_ = binaryReader.ReadUInt32();
            newObj.spellID_ = binaryReader.ReadUInt32();
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node) {
            node.Nodes.Add("index_ = " + index_);
            node.Nodes.Add("objectID_ = " + Utility.FormatHex(objectID_));
            node.Nodes.Add("spellID_ = " + "(" + spellID_ + ") " + (SpellID)spellID_);
        }
    }

    public class ShortCutManager {
        public List<ShortCutData> shortCuts_ = new List<ShortCutData>();

        public static ShortCutManager read(BinaryReader binaryReader) {
            uint numShortcuts = binaryReader.ReadUInt32();
            ShortCutManager newObj = new ShortCutManager();
            for (int i = 0; i < numShortcuts; ++i) {
                newObj.shortCuts_.Add(ShortCutData.read(binaryReader));
            }
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node) {
            TreeNode shortcutsNode = node.Nodes.Add("shortCuts_ = ");
            foreach (ShortCutData shortcut in shortCuts_) {
                shortcut.contributeToTreeNode(shortcutsNode);
            }
        }
    }

    public class PlayerModule {
        public enum PlayerModulePackHeader {
            PM_Packed_None = 0,
            PM_Packed_ShortCutManager = (1 << 0),
            PM_Packed_SquelchList = (1 << 1),
            PM_Packed_MultiSpellLists = (1 << 2),
            PM_Packed_DesiredComps = (1 << 3),
            PM_Packed_ExtendedMultiSpellLists = (1 << 4),
            PM_Packed_SpellbookFilters = (1 << 5),
            PM_Packed_2ndCharacterOptions = (1 << 6),
            PM_Packed_TimeStampFormat = (1 << 7),
            PM_Packed_GenericQualitiesData = (1 << 8),
            PM_Packed_GameplayOptions = (1 << 9),
            PM_Packed_8_SpellLists = (1 << 10)
        }

        public uint header;
        public uint options_;
        public ShortCutManager shortcuts_;
        public PList<SpellID>[] favorite_spells_ = new PList<SpellID>[8];
        public PackableHashTable<uint, int> desired_comps_ = new PackableHashTable<uint, int>();
        public uint spell_filters_;
        public uint options2;
        public PStringChar m_TimeStampFormat;
        public GenericQualitiesData m_pPlayerOptionsData;
        public PackObjPropertyCollection m_colGameplayOptions;
        public List<string> packedItems; // For display purposes

        public static PlayerModule read(BinaryReader binaryReader) {
            PlayerModule newObj = new PlayerModule();
            newObj.packedItems = new List<string>();
            newObj.header = binaryReader.ReadUInt32();
            newObj.options_ = binaryReader.ReadUInt32();
            if ((newObj.header & (uint)PlayerModulePackHeader.PM_Packed_ShortCutManager) != 0) {
                newObj.shortcuts_ = ShortCutManager.read(binaryReader);
                newObj.packedItems.Add(PlayerModulePackHeader.PM_Packed_ShortCutManager.ToString());
            }

            newObj.favorite_spells_[0] = PList<SpellID>.read(binaryReader);
            if ((newObj.header & (uint)PlayerModulePackHeader.PM_Packed_MultiSpellLists) != 0) {
                for (int i = 1; i < 5; ++i) {
                    newObj.favorite_spells_[i] = PList<SpellID>.read(binaryReader);
                }
                newObj.packedItems.Add(PlayerModulePackHeader.PM_Packed_MultiSpellLists.ToString());
            } else if ((newObj.header & (uint)PlayerModulePackHeader.PM_Packed_ExtendedMultiSpellLists) != 0) {
                for (int i = 1; i < 7; ++i) {
                    newObj.favorite_spells_[i] = PList<SpellID>.read(binaryReader);
                }
                newObj.packedItems.Add(PlayerModulePackHeader.PM_Packed_ExtendedMultiSpellLists.ToString());
            } else if ((newObj.header & (uint)PlayerModulePackHeader.PM_Packed_8_SpellLists) != 0) {
                for (int i = 1; i < 8; ++i) {
                    newObj.favorite_spells_[i] = PList<SpellID>.read(binaryReader);
                }
                newObj.packedItems.Add(PlayerModulePackHeader.PM_Packed_8_SpellLists.ToString());
            }
            if ((newObj.header & (uint)PlayerModulePackHeader.PM_Packed_DesiredComps) != 0) {
                newObj.desired_comps_ = PackableHashTable<uint, int>.read(binaryReader);
                newObj.packedItems.Add(PlayerModulePackHeader.PM_Packed_DesiredComps.ToString());
            }
            if ((newObj.header & (uint)PlayerModulePackHeader.PM_Packed_SpellbookFilters) != 0) {
                newObj.spell_filters_ = binaryReader.ReadUInt32();
                newObj.packedItems.Add(PlayerModulePackHeader.PM_Packed_SpellbookFilters.ToString());
            } else {
                newObj.spell_filters_ = 0x3FFF;
            }
            if ((newObj.header & (uint)PlayerModulePackHeader.PM_Packed_2ndCharacterOptions) != 0) {
                newObj.options2 = binaryReader.ReadUInt32();
                newObj.packedItems.Add(PlayerModulePackHeader.PM_Packed_2ndCharacterOptions.ToString());
            } else {
                newObj.options2 = 0x948700;
            }
            if ((newObj.header & (uint)PlayerModulePackHeader.PM_Packed_TimeStampFormat) != 0) {
                newObj.m_TimeStampFormat = PStringChar.read(binaryReader);
                newObj.packedItems.Add(PlayerModulePackHeader.PM_Packed_TimeStampFormat.ToString());
            }
            if ((newObj.header & (uint)PlayerModulePackHeader.PM_Packed_GenericQualitiesData) != 0)
            {
                newObj.m_pPlayerOptionsData = GenericQualitiesData.read(binaryReader);
                newObj.packedItems.Add(PlayerModulePackHeader.PM_Packed_GenericQualitiesData.ToString());
            }
            if ((newObj.header & (uint)PlayerModulePackHeader.PM_Packed_GameplayOptions) != 0)
            {
                newObj.m_colGameplayOptions = PackObjPropertyCollection.read(binaryReader);
                Util.readToAlign(binaryReader); // Align to dword boundary
                newObj.packedItems.Add(PlayerModulePackHeader.PM_Packed_GameplayOptions.ToString());
            }
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node) {
            TreeNode headerNode = node.Nodes.Add("header = " + Utility.FormatHex(header));
            for (int i = 0; i < packedItems.Count; i++)
            {
                headerNode.Nodes.Add(packedItems[i]);
            }
            node.Nodes.Add("options_ = " + Utility.FormatHex(options_));
            TreeNode shortcutsNode = node.Nodes.Add("shortcuts_ = ");
            if (shortcuts_ != null) {
                shortcuts_.contributeToTreeNode(shortcutsNode);
            }
            TreeNode favoritesNode = node.Nodes.Add("favorite_spells_ = ");
            for (int i = 0; i < favorite_spells_.Count(); i++) {
                PList<SpellID> favoritesList = favorite_spells_[i];
                if (favoritesList != null) {
                    TreeNode favoritesSubNode = favoritesNode.Nodes.Add($"Spelltab {i+1} = ");
                    favoritesList.contributeToTreeNode(favoritesSubNode);
                }
            }
            TreeNode desiredCompsNode = node.Nodes.Add("desired_comps_ = ");
            if ((header & (uint)PlayerModulePackHeader.PM_Packed_DesiredComps) != 0)
            {
                foreach (KeyValuePair<uint, int> element in desired_comps_.hashTable)
                {
                    desiredCompsNode.Nodes.Add(element.Key + " = " + element.Value);
                }
            }
            node.Nodes.Add("spell_filters_ = " + Utility.FormatHex(spell_filters_));
            node.Nodes.Add("options2 = " + Utility.FormatHex(options2));
            node.Nodes.Add("m_TimeStampFormat = " + m_TimeStampFormat);

            TreeNode playerOptionsDataNode = node.Nodes.Add("m_pPlayerOptionsData = ");
            if ((header & (uint)PlayerModulePackHeader.PM_Packed_GenericQualitiesData) != 0)
                m_pPlayerOptionsData.contributeToTreeNode(playerOptionsDataNode);

            // DISABLED TREE NODE FORMATTING
            TreeNode colGameplayOptionsNode = node.Nodes.Add("m_colGameplayOptions = ");
            if ((header & (uint)PlayerModulePackHeader.PM_Packed_GameplayOptions) != 0)
                  m_colGameplayOptions.contributeToTreeNode(colGameplayOptionsNode);
        }
    }

    public class GenericQualitiesData
    {
        public uint header;
        public PackableHashTable<STypeInt, int> m_pIntStatsTable = new PackableHashTable<STypeInt, int>();
        public PackableHashTable<STypeBool, int> m_pBoolStatsTable = new PackableHashTable<STypeBool, int>();
        public PackableHashTable<STypeFloat, double> m_pFloatStatsTable = new PackableHashTable<STypeFloat, double>();
        public PackableHashTable<uint, PStringChar> m_pStrStatsTable = new PackableHashTable<uint, PStringChar>();

        public static GenericQualitiesData read(BinaryReader binaryReader)
        {
            GenericQualitiesData newObj = new GenericQualitiesData();
            newObj.header = binaryReader.ReadUInt32();
            if ((newObj.header & 0x01) != 0)
                newObj.m_pIntStatsTable = PackableHashTable<STypeInt, int>.read(binaryReader);
            if ((newObj.header & 0x02) != 0)
                newObj.m_pBoolStatsTable = PackableHashTable<STypeBool, int>.read(binaryReader);
            if ((newObj.header & 0x04) != 0)
                newObj.m_pFloatStatsTable = PackableHashTable<STypeFloat, double>.read(binaryReader);
            if ((newObj.header & 0x08) != 0)
                newObj.m_pStrStatsTable = PackableHashTable<uint, PStringChar>.read(binaryReader);
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
            TreeNode IntStatsNode = node.Nodes.Add("m_pIntStatsTable = ");
            m_pIntStatsTable.contributeToTreeNode(IntStatsNode);
            TreeNode BoolStatsNode = node.Nodes.Add("m_pBoolStatsTable = ");
            m_pBoolStatsTable.contributeToTreeNode(BoolStatsNode);
            TreeNode FloatStatsNode = node.Nodes.Add("m_pFloatStatsTable = ");
            m_pFloatStatsTable.contributeToTreeNode(FloatStatsNode);
            TreeNode StrStatsNode = node.Nodes.Add("m_pStrStatsTable = ");
            m_pStrStatsTable.contributeToTreeNode(StrStatsNode);
        }
    }

    public class PackObjPropertyCollection
    {
        public uint i_iVersion; // AKA g_TurbineCorePackVersion: will always be 0x00000002.
        public List<BaseProperty> PropertyCollection = new List<BaseProperty>();

        public static PackObjPropertyCollection read(BinaryReader binaryReader)
        {
            PackObjPropertyCollection newObj = new PackObjPropertyCollection();
            newObj.i_iVersion = binaryReader.ReadUInt32();

            byte m_numBuckets = binaryReader.ReadByte();
            byte m_numElements = binaryReader.ReadByte();
            for (byte i = 0; i < m_numElements; i++)
            {
                newObj.PropertyCollection.Add(BaseProperty.read(binaryReader));
            }

            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
            node.Nodes.Add("i_iVersion = " + i_iVersion);
            for (int i = 0; i < PropertyCollection.Count; i++) {
                TreeNode propertyNode = node.Nodes.Add($"{PropertyCollection[i].key}");
                PropertyCollection[i].contributeToTreeNode(propertyNode);
            }
            return;
        }
    }

    public enum OptionProperty
    {
        Option_TextType_Property = 0x1000007F,
        Option_ActiveOpacity_Property = 0x10000081,
        Option_Placement_X_Property = 0x10000086,
        Option_Placement_Y_Property = 0x10000087,
        Option_Placement_Width_Property = 0x10000088,
        Option_Placement_Height_Property = 0x10000089,
        Option_DefaultOpacity_Property = 0x10000080,
        Option_Placement_Visibility_Property = 0x1000008a,
        Option_Placement_Property = 0x1000008b,
        Option_PlacementArray_Property = 0x1000008c,
        Option_Placement_Title_Property = 0x1000008d,
    }

    public enum ChatTextFilter : ulong
    {
        ID_ChatOption_TextFilter_Tells = 0x18,
        ID_ChatOption_TextFilter_AreaSpeech = 0x1004,
        ID_ChatOption_TextFilter_Magic = 0x20080,
        ID_ChatOption_TextFilter_Allegience = 0x40C00,
        ID_ChatOption_TextFilter_Fellowship = 0x80000,
        ID_ChatOption_TextFilter_Combat = 0x600040,
        ID_ChatOption_TextFilter_Error = 0x4000000,
        ID_ChatOption_TextFilter_General = 0x8000000,
        ID_ChatOption_TextFilter_Trade = 0x10000000,
        ID_ChatOption_TextFilter_LFG = 0x20000000,
        ID_ChatOption_TextFilter_Roleplay = 0x40000000,
        ID_ChatOption_TextFilter_Gameplay = 0x83912021,
        ID_ChatOption_TextFilter_Society = 0x100000000,
    }

    public class BaseProperty
    {
        public OptionProperty key;
        public OptionProperty m_pcPropertyDesc;
        public float floatPropertyValue;
        public byte bytePropertyValue;
        public uint intPropertyValue;
        public ulong int64PropertyValue;
        public string m_LiteralValue;
        public byte m_Override;
        public uint m_stringID;
        public uint m_tableID;
        public uint bHasStrings;
        public byte m_numBuckets;
        public byte m_numElements;
        public byte boolPropertyValue;
        uint num_properties;
        public List<BaseProperty> PropertyCollectionValue;

        public static BaseProperty read(BinaryReader binaryReader)
        {
            BaseProperty newObj = new BaseProperty();
            newObj.key = (OptionProperty)binaryReader.ReadUInt32();
            switch (newObj.key)
            {
                case OptionProperty.Option_ActiveOpacity_Property:
                    newObj.m_pcPropertyDesc = (OptionProperty)binaryReader.ReadUInt32();
                    newObj.floatPropertyValue = binaryReader.ReadSingle();
                    break;
                case OptionProperty.Option_DefaultOpacity_Property:
                    newObj.m_pcPropertyDesc = (OptionProperty)binaryReader.ReadUInt32();
                    newObj.floatPropertyValue = binaryReader.ReadSingle();
                    break;
                case OptionProperty.Option_Placement_X_Property:
                    newObj.m_pcPropertyDesc = (OptionProperty)binaryReader.ReadUInt32();
                    newObj.intPropertyValue = binaryReader.ReadUInt32();
                    break;
                case OptionProperty.Option_Placement_Y_Property:
                    newObj.m_pcPropertyDesc = (OptionProperty)binaryReader.ReadUInt32();
                    newObj.intPropertyValue = binaryReader.ReadUInt32();
                    break;
                case OptionProperty.Option_Placement_Width_Property:
                    newObj.m_pcPropertyDesc = (OptionProperty)binaryReader.ReadUInt32();
                    newObj.intPropertyValue = binaryReader.ReadUInt32();
                    break;
                case OptionProperty.Option_Placement_Height_Property:
                    newObj.m_pcPropertyDesc = (OptionProperty)binaryReader.ReadUInt32();
                    newObj.intPropertyValue = binaryReader.ReadUInt32();
                    break;
                case OptionProperty.Option_Placement_Visibility_Property:
                    newObj.m_pcPropertyDesc = (OptionProperty)binaryReader.ReadUInt32();
                    newObj.boolPropertyValue = binaryReader.ReadByte();
                    break;
                case OptionProperty.Option_Placement_Title_Property:
                    newObj.m_pcPropertyDesc = (OptionProperty)binaryReader.ReadUInt32();
                    newObj.m_Override = binaryReader.ReadByte();
                    if (newObj.m_Override == 1)
                    {
                        newObj.m_LiteralValue = Util.readUnicodeString(binaryReader);
                    }
                    else
                    {
                        newObj.m_stringID = binaryReader.ReadUInt32();
                        newObj.m_tableID = binaryReader.ReadUInt32();
                    }
                    // This variable is set in the StringInfo::Serialize function.
                    newObj.bHasStrings = binaryReader.ReadUInt32();
                    // These next two are set in the SerializeIntrusiveHashTable function.
                    newObj.m_numBuckets = binaryReader.ReadByte();
                    newObj.m_numElements = binaryReader.ReadByte();
                    break;
                case OptionProperty.Option_TextType_Property:
                    newObj.m_pcPropertyDesc = (OptionProperty)binaryReader.ReadUInt32();
                    newObj.int64PropertyValue = binaryReader.ReadUInt64();
                    break;
                case OptionProperty.Option_PlacementArray_Property:
                    newObj.m_pcPropertyDesc = (OptionProperty)binaryReader.ReadUInt32();
                    newObj.num_properties = binaryReader.ReadUInt32();
                    newObj.PropertyCollectionValue = new List<BaseProperty>();
                    for (uint i = 0; i < newObj.num_properties; i++)
                    {
                        newObj.PropertyCollectionValue.Add(BaseProperty.read(binaryReader));
                    }
                    break;
                case OptionProperty.Option_Placement_Property:
                    byte m_numBuckets = binaryReader.ReadByte();
                    byte m_numElements = binaryReader.ReadByte();
                    newObj.PropertyCollectionValue = new List<BaseProperty>();
                    for (byte i = 0; i < m_numElements; i++)
                    {
                        newObj.PropertyCollectionValue.Add(BaseProperty.read(binaryReader));
                    }
                    break;
            }
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
            switch (key)
            {
                case OptionProperty.Option_ActiveOpacity_Property:
                    node.Nodes.Add("floatPropertyValue = " + floatPropertyValue);
                    break;
                case OptionProperty.Option_DefaultOpacity_Property:
                    node.Nodes.Add("floatPropertyValue = " + floatPropertyValue);
                    break;
                case OptionProperty.Option_Placement_X_Property:
                    node.Nodes.Add("intPropertyValue = " + intPropertyValue);
                    break;
                case OptionProperty.Option_Placement_Y_Property:
                    node.Nodes.Add("intPropertyValue = " + intPropertyValue);
                    break;
                case OptionProperty.Option_Placement_Width_Property:
                    node.Nodes.Add("intPropertyValue = " + intPropertyValue);
                    break;
                case OptionProperty.Option_Placement_Height_Property:
                    node.Nodes.Add("intPropertyValue = " + intPropertyValue);
                    break;
                case OptionProperty.Option_Placement_Visibility_Property:
                    node.Nodes.Add("boolPropertyValue = " + boolPropertyValue);
                    break;
                case OptionProperty.Option_Placement_Title_Property:
                    node.Nodes.Add("m_Override = " + m_Override);
                    if (m_Override == 1)
                    {
                        node.Nodes.Add("m_LiteralValue = " + m_LiteralValue);
                    }
                    else
                    {
                        node.Nodes.Add("m_stringID = " + Utility.FormatHex(m_stringID));
                        node.Nodes.Add("m_tableID = " + Utility.FormatHex(m_tableID));
                    }
                    node.Nodes.Add("bHasStrings = " + bHasStrings);
                    node.Nodes.Add("m_numBuckets = " + m_numBuckets);
                    node.Nodes.Add("m_numElements = " + m_numElements);
                    break;
                case OptionProperty.Option_TextType_Property:
                    TreeNode chatMask = node.Nodes.Add("int64PropertyValue = " + Utility.FormatHex(int64PropertyValue));
                    foreach (ulong e in Enum.GetValues(typeof(ChatTextFilter)))
                    {
                        if ((int64PropertyValue & e) == e)
                        {
                            chatMask.Nodes.Add($"{Enum.GetName(typeof(ChatTextFilter),e)}");
                        }
                    }
                    break;
                case OptionProperty.Option_PlacementArray_Property:
                    node.Nodes.Add("num_properties = " + num_properties);
                    for (int i = 0; i < PropertyCollectionValue.Count; i++)
                    {
                        // Note: num_properties SHOULD equal 17 for the number of client UI elements
                        // but let's check just to be sure.
                        if (num_properties == 17)
                        {
                            TreeNode PropertyNode = node.Nodes.Add($"{PropertyCollectionValue[i].key} (UIElement: {(UIElement)i+1})");
                            PropertyCollectionValue[i].contributeToTreeNode(PropertyNode);
                        }
                        else
                        {
                            TreeNode PropertyNode = node.Nodes.Add($"{PropertyCollectionValue[i].key}");
                            PropertyCollectionValue[i].contributeToTreeNode(PropertyNode);
                        }
                    }
                    break;
                case OptionProperty.Option_Placement_Property:
                    for (int i = 0; i < PropertyCollectionValue.Count; i++)
                    {
                        TreeNode PropertyNode = node.Nodes.Add($"{PropertyCollectionValue[i].key}");
                        PropertyCollectionValue[i].contributeToTreeNode(PropertyNode);
                    }
                    break;
            }
        }
    }

    public class CharacterOptionsEvent : Message {
        PlayerModule i_pMod;

        public static CharacterOptionsEvent read(BinaryReader binaryReader) {
            CharacterOptionsEvent newObj = new CharacterOptionsEvent();
            newObj.i_pMod = PlayerModule.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            TreeNode playerModuleNode = rootNode.Nodes.Add("i_pMod = ");
            i_pMod.contributeToTreeNode(playerModuleNode);
            treeView.Nodes.Add(rootNode);
            playerModuleNode.Expand();
        }
    }

    public class AddShortCut : Message {
        public ShortCutData shortcut;

        public static AddShortCut read(BinaryReader binaryReader) {
            AddShortCut newObj = new AddShortCut();
            newObj.shortcut = ShortCutData.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            TreeNode shortcutNode = rootNode.Nodes.Add("shortcut = ");
            shortcut.contributeToTreeNode(shortcutNode);
            treeView.Nodes.Add(rootNode);
            rootNode.ExpandAll();
        }
    }

    public class AddSpellFavorite : Message {
        public SpellID i_spid;
        public uint i_index;
        public uint i_list;

        public static AddSpellFavorite read(BinaryReader binaryReader) {
            AddSpellFavorite newObj = new AddSpellFavorite();
            newObj.i_spid = (SpellID)binaryReader.ReadUInt32();
            newObj.i_index = binaryReader.ReadUInt32();
            newObj.i_list = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_spid = " + i_spid);
            rootNode.Nodes.Add("i_index = " + i_index);
            rootNode.Nodes.Add("i_list = " + i_list);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class ConfirmationResponse : Message {
        public uint i_confirmType;
        public uint i_context;
        public uint i_bAccepted;

        public static ConfirmationResponse read(BinaryReader binaryReader) {
            ConfirmationResponse newObj = new ConfirmationResponse();
            newObj.i_confirmType = binaryReader.ReadUInt32();
            newObj.i_context = binaryReader.ReadUInt32();
            newObj.i_bAccepted = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_confirmType = " + (ConfirmationType)i_confirmType);
            rootNode.Nodes.Add("i_context = " + i_context);
            rootNode.Nodes.Add("i_bAccepted = " + i_bAccepted);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class QueryAge : Message {
        public uint i_target;

        public static QueryAge read(BinaryReader binaryReader) {
            QueryAge newObj = new QueryAge();
            newObj.i_target = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_target = " + i_target);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class QueryBirth : Message {
        public uint i_target;

        public static QueryBirth read(BinaryReader binaryReader) {
            QueryBirth newObj = new QueryBirth();
            newObj.i_target = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_target = " + i_target);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class RemoveShortCut : Message {
        public uint i_index;

        public static RemoveShortCut read(BinaryReader binaryReader) {
            RemoveShortCut newObj = new RemoveShortCut();
            newObj.i_index = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_index = " + i_index);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class RemoveSpellFavorite : Message {
        public SpellID i_spid;
        public uint i_list;

        public static RemoveSpellFavorite read(BinaryReader binaryReader) {
            RemoveSpellFavorite newObj = new RemoveSpellFavorite();
            newObj.i_spid = (SpellID)binaryReader.ReadUInt32();
            newObj.i_list = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_spid = " + i_spid);
            rootNode.Nodes.Add("i_list = " + i_list);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class SpellbookFilterEvent : Message {
        public uint i_options;

        public static SpellbookFilterEvent read(BinaryReader binaryReader) {
            SpellbookFilterEvent newObj = new SpellbookFilterEvent();
            newObj.i_options = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_options = " + i_options);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class FinishBarber : Message {
        public uint i_base_palette;
        public uint i_head_object;
        public uint i_head_texture;
        public uint i_default_head_texture;
        public uint i_eyes_texture;
        public uint i_default_eyes_texture;
        public uint i_nose_texture;
        public uint i_default_nose_texture;
        public uint i_mouth_texture;
        public uint i_default_mouth_texture;
        public uint i_skin_palette;
        public uint i_hair_palette;
        public uint i_eyes_palette;
        public uint i_setup_id;
        public uint i_option1;
        public uint i_option2;

        public static FinishBarber read(BinaryReader binaryReader) {
            FinishBarber newObj = new FinishBarber();
            newObj.i_base_palette = binaryReader.ReadUInt32();
            newObj.i_head_object = binaryReader.ReadUInt32();
            newObj.i_head_texture = binaryReader.ReadUInt32();
            newObj.i_default_head_texture = binaryReader.ReadUInt32();
            newObj.i_eyes_texture = binaryReader.ReadUInt32();
            newObj.i_default_eyes_texture = binaryReader.ReadUInt32();
            newObj.i_nose_texture = binaryReader.ReadUInt32();
            newObj.i_default_nose_texture = binaryReader.ReadUInt32();
            newObj.i_mouth_texture = binaryReader.ReadUInt32();
            newObj.i_default_mouth_texture = binaryReader.ReadUInt32();
            newObj.i_skin_palette = binaryReader.ReadUInt32();
            newObj.i_hair_palette = binaryReader.ReadUInt32();
            newObj.i_eyes_palette = binaryReader.ReadUInt32();
            newObj.i_setup_id = binaryReader.ReadUInt32();
            newObj.i_option1 = binaryReader.ReadUInt32();
            newObj.i_option2 = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_base_palette = " + Utility.FormatHex(i_base_palette));
            rootNode.Nodes.Add("i_head_object = " + Utility.FormatHex(i_head_object));
            rootNode.Nodes.Add("i_head_texture = " + Utility.FormatHex(i_head_texture));
            rootNode.Nodes.Add("i_default_head_texture = " + Utility.FormatHex(i_default_head_texture));
            rootNode.Nodes.Add("i_eyes_texture = " + Utility.FormatHex(i_eyes_texture));
            rootNode.Nodes.Add("i_default_eyes_texture = " + Utility.FormatHex(i_default_eyes_texture));
            rootNode.Nodes.Add("i_nose_texture = " + Utility.FormatHex(i_nose_texture));
            rootNode.Nodes.Add("i_default_nose_texture = " + Utility.FormatHex(i_default_nose_texture));
            rootNode.Nodes.Add("i_mouth_texture = " + Utility.FormatHex(i_mouth_texture));
            rootNode.Nodes.Add("i_default_mouth_texture = " + Utility.FormatHex(i_default_mouth_texture));
            rootNode.Nodes.Add("i_skin_palette = " + Utility.FormatHex(i_skin_palette));
            rootNode.Nodes.Add("i_hair_palette = " + Utility.FormatHex(i_hair_palette));
            rootNode.Nodes.Add("i_eyes_palette = " + Utility.FormatHex(i_eyes_palette));
            rootNode.Nodes.Add("i_setup_id = " + Utility.FormatHex(i_setup_id));
            rootNode.Nodes.Add("i_option1 = " + i_option1);
            rootNode.Nodes.Add("i_option2 = " + i_option2);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class SetDesiredComponentLevel : Message {
        public uint i_wcid;
        public uint i_amount;

        public static SetDesiredComponentLevel read(BinaryReader binaryReader) {
            SetDesiredComponentLevel newObj = new SetDesiredComponentLevel();
            newObj.i_wcid = binaryReader.ReadUInt32();
            newObj.i_amount = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_wcid = " + i_wcid);
            rootNode.Nodes.Add("i_amount = " + i_amount);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class AbuseLogRequest : Message {
        public PStringChar i_target;
        public int i_status;
        public PStringChar i_complaint;

        public static AbuseLogRequest read(BinaryReader binaryReader) {
            AbuseLogRequest newObj = new AbuseLogRequest();
            newObj.i_target = PStringChar.read(binaryReader);
            newObj.i_status = binaryReader.ReadInt32();
            newObj.i_complaint = PStringChar.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_target = " + i_target);
            rootNode.Nodes.Add("i_status = " + i_status);
            rootNode.Nodes.Add("i_complaint = " + i_complaint);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class AddPlayerPermission : Message {
        public PStringChar i_targetName;

        public static AddPlayerPermission read(BinaryReader binaryReader) {
            AddPlayerPermission newObj = new AddPlayerPermission();
            newObj.i_targetName = PStringChar.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_targetName = " + i_targetName);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class RemoveFromPlayerConsentList : Message {
        public PStringChar i_targetName;

        public static RemoveFromPlayerConsentList read(BinaryReader binaryReader) {
            RemoveFromPlayerConsentList newObj = new RemoveFromPlayerConsentList();
            newObj.i_targetName = PStringChar.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_targetName = " + i_targetName);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class RemovePlayerPermission : Message {
        public PStringChar i_targetName;

        public static RemovePlayerPermission read(BinaryReader binaryReader) {
            RemovePlayerPermission newObj = new RemovePlayerPermission();
            newObj.i_targetName = PStringChar.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_targetName = " + i_targetName);
            treeView.Nodes.Add(rootNode);
        }
    }

    

    public class QueryAgeResponse : Message {
        public PStringChar targetName;
        public PStringChar age;

        public static QueryAgeResponse read(BinaryReader binaryReader) {
            QueryAgeResponse newObj = new QueryAgeResponse();
            newObj.targetName = PStringChar.read(binaryReader);
            newObj.age = PStringChar.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("targetName = " + targetName);
            rootNode.Nodes.Add("age = " + age);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class ConfirmationDone : Message {
        public ConfirmationType confirm;
        public uint context;

        public static ConfirmationDone read(BinaryReader binaryReader) {
        ConfirmationDone newObj = new ConfirmationDone();
            newObj.confirm = (ConfirmationType)binaryReader.ReadUInt32();
            newObj.context = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("confirm = " + confirm);
            rootNode.Nodes.Add("context = " + context);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class StartBarber : Message {
        public uint _base_palette;
        public uint _head_object;
        public uint _head_texture;
        public uint _default_head_texture;
        public uint _eyes_texture;
        public uint _default_eyes_texture;
        public uint _nose_texture;
        public uint _default_nose_texture;
        public uint _mouth_texture;
        public uint _default_mouth_texture;
        public uint _skin_palette;
        public uint _hair_palette;
        public uint _eyes_palette;
        public uint _setup_id;
        public uint option1;
        public uint option2;

        public static StartBarber read(BinaryReader binaryReader) {
            StartBarber newObj = new StartBarber();
            newObj._base_palette = binaryReader.ReadUInt32();
            newObj._head_object = binaryReader.ReadUInt32();
            newObj._head_texture = binaryReader.ReadUInt32();
            newObj._default_head_texture = binaryReader.ReadUInt32();
            newObj._eyes_texture = binaryReader.ReadUInt32();
            newObj._default_eyes_texture = binaryReader.ReadUInt32();
            newObj._nose_texture = binaryReader.ReadUInt32();
            newObj._default_nose_texture = binaryReader.ReadUInt32();
            newObj._mouth_texture = binaryReader.ReadUInt32();
            newObj._default_mouth_texture = binaryReader.ReadUInt32();
            newObj._skin_palette = binaryReader.ReadUInt32();
            newObj._hair_palette = binaryReader.ReadUInt32();
            newObj._eyes_palette = binaryReader.ReadUInt32();
            newObj._setup_id = binaryReader.ReadUInt32();
            newObj.option1 = binaryReader.ReadUInt32();
            newObj.option2 = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("_base_palette = " + Utility.FormatHex(_base_palette));
            rootNode.Nodes.Add("_head_object = " + Utility.FormatHex(_head_object));
            rootNode.Nodes.Add("_head_texture = " + Utility.FormatHex(_head_texture));
            rootNode.Nodes.Add("_default_head_texture = " + Utility.FormatHex(_default_head_texture));
            rootNode.Nodes.Add("_eyes_texture = " + Utility.FormatHex(_eyes_texture));
            rootNode.Nodes.Add("_default_eyes_texture = " + Utility.FormatHex(_default_eyes_texture));
            rootNode.Nodes.Add("_nose_texture = " + Utility.FormatHex(_nose_texture));
            rootNode.Nodes.Add("_default_nose_texture = " + Utility.FormatHex(_default_nose_texture));
            rootNode.Nodes.Add("_mouth_texture = " + Utility.FormatHex(_mouth_texture));
            rootNode.Nodes.Add("_default_mouth_texture = " + Utility.FormatHex(_default_mouth_texture));
            rootNode.Nodes.Add("_skin_palette = " + Utility.FormatHex(_skin_palette));
            rootNode.Nodes.Add("_hair_palette = " + Utility.FormatHex(_hair_palette));
            rootNode.Nodes.Add("_eyes_palette = " + Utility.FormatHex(_eyes_palette));
            rootNode.Nodes.Add("_setup_id = " + Utility.FormatHex(_setup_id));
            rootNode.Nodes.Add("option1 = " + option1);
            rootNode.Nodes.Add("option2 = " + option2);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class ConfirmationRequest : Message {
        public ConfirmationType confirm;
        public uint context;
        public PStringChar userData;

        public static ConfirmationRequest read(BinaryReader binaryReader) {
            ConfirmationRequest newObj = new ConfirmationRequest();
            newObj.confirm = (ConfirmationType)binaryReader.ReadUInt32();
            newObj.context = binaryReader.ReadUInt32();
            newObj.userData = PStringChar.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("confirm = " + confirm);
            rootNode.Nodes.Add("context = " + context);
            rootNode.Nodes.Add("userData = " + userData);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class CharacterErrorEvent : Message
    {
        public uint _error;

        public static CharacterErrorEvent read(BinaryReader binaryReader)
        {
            CharacterErrorEvent newObj = new CharacterErrorEvent();
            newObj._error = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("_error = " + (charError)_error);
            treeView.Nodes.Add(rootNode);
        }
    }
}
