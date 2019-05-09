using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using aclogview;

public class CM_Allegiance : MessageProcessor {

    public override bool acceptMessageData(BinaryReader messageDataReader, TreeView outputTreeView) {
        bool handled = true;

        PacketOpcode opcode = Util.readOpcode(messageDataReader);
        switch (opcode) {
            case PacketOpcode.Evt_Allegiance__QueryAllegianceName_ID:
            case PacketOpcode.Evt_Allegiance__ClearAllegianceName_ID:
            case PacketOpcode.Evt_Allegiance__ListAllegianceOfficerTitles_ID:
            case PacketOpcode.Evt_Allegiance__ClearAllegianceOfficerTitles_ID:
            case PacketOpcode.Evt_Allegiance__QueryMotd_ID:
            case PacketOpcode.Evt_Allegiance__ClearMotd_ID:
            case PacketOpcode.Evt_Allegiance__ListAllegianceBans_ID:
            case PacketOpcode.Evt_Allegiance__ListAllegianceOfficers_ID:
            case PacketOpcode.Evt_Allegiance__ClearAllegianceOfficers_ID:
            case PacketOpcode.Evt_Allegiance__RecallAllegianceHometown_ID: {
                    EmptyMessage message = new EmptyMessage(opcode);
                    message.contributeToTreeView(outputTreeView);
                    ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
                    break;
                }
            case PacketOpcode.Evt_Allegiance__AllegianceUpdateAborted_ID: {
                    AllegianceUpdateAborted message = AllegianceUpdateAborted.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Allegiance__SwearAllegiance_ID: {
                    SwearAllegiance message = SwearAllegiance.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Allegiance__BreakAllegiance_ID: {
                    BreakAllegiance message = BreakAllegiance.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Allegiance__UpdateRequest_ID: {
                    UpdateRequest message = UpdateRequest.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.ALLEGIANCE_UPDATE_EVENT: {
                    AllegianceUpdate message = AllegianceUpdate.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Allegiance__SetAllegianceName_ID: {
                    SetAllegianceName message = SetAllegianceName.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Allegiance__SetAllegianceOfficer_ID: {
                    SetAllegianceOfficer message = SetAllegianceOfficer.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Allegiance__SetAllegianceOfficerTitle_ID: {
                    SetAllegianceOfficerTitle message = SetAllegianceOfficerTitle.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Allegiance__DoAllegianceLockAction_ID: {
                    DoAllegianceLockAction message = DoAllegianceLockAction.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Allegiance__SetAllegianceApprovedVassal_ID: {
                    SetAllegianceApprovedVassal message = SetAllegianceApprovedVassal.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Allegiance__AllegianceChatGag_ID: {
                    AllegianceChatGag message = AllegianceChatGag.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Allegiance__DoAllegianceHouseAction_ID: {
                    DoAllegianceHouseAction message = DoAllegianceHouseAction.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Allegiance__AllegianceUpdateDone_ID:
                {
                    AllegianceUpdateDone message = AllegianceUpdateDone.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Allegiance__SetMotd_ID: {
                    SetMotd message = SetMotd.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Allegiance__BreakAllegianceBoot_ID: {
                    BreakAllegianceBoot message = BreakAllegianceBoot.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Allegiance__AllegianceLoginNotificationEvent_ID: {
                    AllegianceLoginNotificationEvent message = AllegianceLoginNotificationEvent.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Allegiance__AllegianceInfoRequest_ID: {
                    AllegianceInfoRequest message = AllegianceInfoRequest.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Allegiance__AllegianceInfoResponseEvent_ID: {
                    AllegianceInfoResponseEvent message = AllegianceInfoResponseEvent.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Allegiance__AllegianceChatBoot_ID: {
                    AllegianceChatBoot message = AllegianceChatBoot.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Allegiance__AddAllegianceBan_ID: {
                    AddAllegianceBan message = AddAllegianceBan.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Allegiance__RemoveAllegianceBan_ID: {
                    RemoveAllegianceBan message = RemoveAllegianceBan.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            // TODO: PacketOpcode.Evt_Allegiance__AddAllegianceOfficer_ID
            case PacketOpcode.Evt_Allegiance__RemoveAllegianceOfficer_ID: {
                    RemoveAllegianceOfficer message = RemoveAllegianceOfficer.read(messageDataReader);
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

    // Could not find any retail pcaps of this message
    public class AllegianceUpdateAborted : Message {
        public uint etype;

        public static AllegianceUpdateAborted read(BinaryReader binaryReader) {
            AllegianceUpdateAborted newObj = new AllegianceUpdateAborted();
            newObj.etype = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ServerToClientHeader });
            rootNode.Nodes.Add("etype = " + etype);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class SwearAllegiance : Message {
        public uint i_target;

        public static SwearAllegiance read(BinaryReader binaryReader) {
            SwearAllegiance newObj = new SwearAllegiance();
            newObj.i_target = binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_target = " + Utility.FormatHex(i_target));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class BreakAllegiance : Message {
        public uint i_target;

        public static BreakAllegiance read(BinaryReader binaryReader) {
            BreakAllegiance newObj = new BreakAllegiance();
            newObj.i_target = binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_target = " + Utility.FormatHex(i_target));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class UpdateRequest : Message {
        public uint i_on;

        public static UpdateRequest read(BinaryReader binaryReader) {
            UpdateRequest newObj = new UpdateRequest();
            newObj.i_on = binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_on = " + i_on);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class AllegianceData {
        public uint _id;
        public uint _cp_cached;
        public uint _cp_tithed;
        public uint _bitfield;
        public byte _gender;
        public byte _hg;
        public ushort _rank;
        public uint _level;
        public ushort _loyalty;
        public ushort _leadership;
        public ulong _time_online__large;
        public uint _time_online__small;
        public uint _allegiance_age;
        public PStringChar _name;
        public int Length;

        public static AllegianceData read(BinaryReader binaryReader) {
            AllegianceData newObj = new AllegianceData();
            var startPosition = binaryReader.BaseStream.Position;
            newObj._id = binaryReader.ReadUInt32();
            newObj._cp_cached = binaryReader.ReadUInt32();
            newObj._cp_tithed = binaryReader.ReadUInt32();
            newObj._bitfield = binaryReader.ReadUInt32();
            newObj._gender = binaryReader.ReadByte();
            newObj._hg = binaryReader.ReadByte();
            newObj._rank = binaryReader.ReadUInt16();
            if ((newObj._bitfield & (uint)AllegianceIndex.HasPackedLevel_AllegianceIndex) != 0) {
                newObj._level = binaryReader.ReadUInt32();
            }
            newObj._loyalty = binaryReader.ReadUInt16();
            newObj._leadership = binaryReader.ReadUInt16();
            if ((newObj._bitfield & (uint)AllegianceIndex.HasAllegianceAge_AllegianceIndex) != 0) {
                newObj._time_online__small = binaryReader.ReadUInt32();
                newObj._allegiance_age = binaryReader.ReadUInt32();
            } else {
                newObj._time_online__large = binaryReader.ReadUInt64();
            }
            newObj._name = PStringChar.read(binaryReader);
            newObj.Length = (int)(binaryReader.BaseStream.Position - startPosition);

            return newObj;
        }

        public void contributeToTreeNode(TreeNode node) {
            node.Nodes.Add("_id = " + Utility.FormatHex(_id));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            node.Nodes.Add("_cp_cached = " + _cp_cached);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_cp_tithed = " + _cp_tithed);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            TreeNode bitfieldNode = node.Nodes.Add("_bitfield = " + Utility.FormatHex(_bitfield));
            ContextInfo.AddToList(new ContextInfo { Length = 4 }, updateDataIndex: false);
            foreach (AllegianceIndex element in Enum.GetValues(typeof(AllegianceIndex)))
            {
                if ((_bitfield & (uint)element) != 0)
                {
                    bitfieldNode.Nodes.Add(Enum.GetName(typeof(AllegianceIndex), element));
                    ContextInfo.AddToList(new ContextInfo { Length = 4 }, updateDataIndex: false);
                }
            }
            // Now update index
            ContextInfo.DataIndex += 4;
            node.Nodes.Add("_gender = " + (Gender)_gender);
            ContextInfo.AddToList(new ContextInfo { Length = 1 });
            node.Nodes.Add("_hg = " + (HeritageGroup)_hg);
            ContextInfo.AddToList(new ContextInfo { Length = 1 });
            node.Nodes.Add("_rank = " + _rank);
            ContextInfo.AddToList(new ContextInfo { Length = 2 });
            if ((_bitfield & (uint) AllegianceIndex.HasPackedLevel_AllegianceIndex) != 0)
            {
                node.Nodes.Add("_level = " + _level);
                ContextInfo.AddToList(new ContextInfo { Length = 4 });

            }
            node.Nodes.Add("_loyalty = " + _loyalty);
            ContextInfo.AddToList(new ContextInfo { Length = 2 });
            node.Nodes.Add("_leadership = " + _leadership);
            ContextInfo.AddToList(new ContextInfo { Length = 2 });
            if ((_bitfield & (uint)AllegianceIndex.HasAllegianceAge_AllegianceIndex) != 0) {
                node.Nodes.Add("_time_online__small = " + _time_online__small);
                ContextInfo.AddToList(new ContextInfo { Length = 4 });
                node.Nodes.Add("_allegiance_age = " + _allegiance_age);
                ContextInfo.AddToList(new ContextInfo { Length = 4 });
            }
            else {
                node.Nodes.Add("_time_online__large = " + _time_online__large);
                ContextInfo.AddToList(new ContextInfo { Length = 8 });
            }
            node.Nodes.Add("_name = " + _name);
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Serialized_AsciiString, Length = _name.Length });
        }
    }

    public enum AllegianceNodeIndex : byte
    {
        Monarch,
        SwornToMonarch,
        SwornToPatron
    }

    public class AllegianceNodes
    {
        public AllegianceData _monarch;
        public AllegianceNode _patron;
        public AllegianceNode _peer;
        public List<AllegianceNode> vassalList;
        public AllegianceNodeIndex characterIndex;
        public int Length;

        public static AllegianceNodes read(BinaryReader binaryReader, ushort packed_nodes, uint characterId) {
            AllegianceNodes newObj = new AllegianceNodes();
            newObj.vassalList = new List<AllegianceNode>();

            var startPosition = binaryReader.BaseStream.Position;
            var tempList = new List<AllegianceNode>();
            for (int i = 0; i < packed_nodes; i++) {
                if (i == 0)
                {
                    newObj._monarch = AllegianceData.read(binaryReader);
                    continue;
                }

                tempList.Add(AllegianceNode.read(binaryReader));
            }

            if (newObj._monarch?._id == characterId)
            {
                newObj.characterIndex = AllegianceNodeIndex.Monarch;
                newObj.vassalList = tempList;
            }
            if (tempList.Count > 0 && tempList[0]._data._id == characterId)
            {
                newObj._peer = tempList[0];
                newObj.characterIndex = AllegianceNodeIndex.SwornToMonarch;
                newObj.vassalList = tempList.GetRange(1, tempList.Count-1);
            }
            if (tempList.Count > 1 && tempList[1]._data._id == characterId)
            {
                newObj._patron = tempList[0];
                newObj._peer = tempList[1];
                newObj.characterIndex = AllegianceNodeIndex.SwornToPatron;
                newObj.vassalList = tempList.GetRange(2, tempList.Count-2);
            }
            newObj.Length = (int)(binaryReader.BaseStream.Position - startPosition);

            return newObj;
        }

        public void contributeToTreeNode(TreeNode node) {
            TreeNode monarchNode = node.Nodes.Add($"_monarch {(characterIndex == AllegianceNodeIndex.Monarch ? "(self)" : "")} = ");
            ContextInfo.AddToList(new ContextInfo { Length = _monarch.Length }, updateDataIndex: false);
            _monarch.contributeToTreeNode(monarchNode);
            TreeNode patronNode = null;
            if (_patron != null)
            {
                patronNode = monarchNode.Nodes.Add($"_patron = ");
                ContextInfo.AddToList(new ContextInfo { Length = _patron.Length }, updateDataIndex: false);
                _patron.contributeToTreeNode(patronNode);
            }
            TreeNode peerNode = null;
            if (_peer != null) {
                if (characterIndex == AllegianceNodeIndex.SwornToMonarch)
                    peerNode = monarchNode.Nodes.Add($"_peer (self) = ");
                else if (characterIndex == AllegianceNodeIndex.SwornToPatron)
                    peerNode = patronNode.Nodes.Add($"_peer (self) = ");
                ContextInfo.AddToList(new ContextInfo { Length = _peer.Length }, updateDataIndex: false);
                _peer.contributeToTreeNode(peerNode);
            }

            TreeNode nodeToAppendVassals;
            if (characterIndex == AllegianceNodeIndex.Monarch)
                nodeToAppendVassals = monarchNode;
            else
                nodeToAppendVassals = peerNode;

            for (int i = 0; i < vassalList.Count; i++)
            {
                TreeNode vassalNode = nodeToAppendVassals.Nodes.Add($"_vassal {i + 1} = ");
                ContextInfo.AddToList(new ContextInfo { Length = vassalList[i].Length }, updateDataIndex: false);
                vassalList[i].contributeToTreeNode(vassalNode);
            }
        }
    }

    public class AllegianceNode
    {
        public uint _parent_id;
        public AllegianceData _data;
        public int Length;

        public static AllegianceNode read(BinaryReader binaryReader)
        {
            var newObj = new AllegianceNode();
            var startPosition = binaryReader.BaseStream.Position;
            newObj._parent_id = binaryReader.ReadUInt32();
            newObj._data = AllegianceData.read(binaryReader);
            newObj.Length = (int)(binaryReader.BaseStream.Position - startPosition);

            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
            node.Nodes.Add("_parent_id = " + Utility.FormatHex(_parent_id));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            _data.contributeToTreeNode(node);
        }
    }

    public class AllegianceHierarchy
    {
        public ushort packed_nodes;
        public AllegianceVersion m_oldVersion;
        public PackableHashTable<uint, uint> m_AllegianceOfficers;
        public PList<PStringChar> m_OfficerTitles;
        public int m_monarchBroadcastTime;
        public uint m_monarchBroadcastsToday;
        public int m_spokesBroadcastTime;
        public uint m_spokesBroadcastsToday;
        public PStringChar m_motd;
        public PStringChar m_motdSetBy;
        public uint m_chatRoomID;
        public Position m_BindPoint;
        public PStringChar m_AllegianceName;
        public int m_NameLastSetTime;
        public uint m_isLocked;
        public uint m_ApprovedVassal;
        public AllegianceNodes m_pMonarch;
        public int Length;

        public static AllegianceHierarchy read(BinaryReader binaryReader, uint characterId)
        {
            AllegianceHierarchy newObj = new AllegianceHierarchy();
            var startPosition = binaryReader.BaseStream.Position;
            newObj.packed_nodes = binaryReader.ReadUInt16();
            newObj.m_oldVersion = (AllegianceVersion)binaryReader.ReadUInt16();
            newObj.m_AllegianceOfficers = PackableHashTable<uint,uint>.read(binaryReader);
            newObj.m_OfficerTitles = PList<PStringChar>.read(binaryReader);
            newObj.m_monarchBroadcastTime = binaryReader.ReadInt32();
            newObj.m_monarchBroadcastsToday = binaryReader.ReadUInt32();
            newObj.m_spokesBroadcastTime = binaryReader.ReadInt32();
            newObj.m_spokesBroadcastsToday = binaryReader.ReadUInt32();
            newObj.m_motd = PStringChar.read(binaryReader);
            newObj.m_motdSetBy = PStringChar.read(binaryReader);
            newObj.m_chatRoomID = binaryReader.ReadUInt32();
            newObj.m_BindPoint = Position.read(binaryReader);
            newObj.m_AllegianceName = PStringChar.read(binaryReader);
            newObj.m_NameLastSetTime = binaryReader.ReadInt32();
            newObj.m_isLocked = binaryReader.ReadUInt32();
            newObj.m_ApprovedVassal = binaryReader.ReadUInt32();
            if (newObj.packed_nodes > 0) {
                newObj.m_pMonarch = AllegianceNodes.read(binaryReader, newObj.packed_nodes, characterId);
            }
            newObj.Length = (int)(binaryReader.BaseStream.Position - startPosition);
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
            node.Nodes.Add("packed_nodes = " + packed_nodes);
            ContextInfo.AddToList(new ContextInfo { Length = 2 });
            node.Nodes.Add("m_oldVersion = " + m_oldVersion);
            ContextInfo.AddToList(new ContextInfo { Length = 2 });
            TreeNode officersNode = node.Nodes.Add("m_AllegianceOfficers = ");
            ContextInfo.AddToList(new ContextInfo { Length = m_AllegianceOfficers.Length });
            m_AllegianceOfficers.contributeToTreeNode(officersNode);
            TreeNode officerTitlesNode = node.Nodes.Add("m_OfficerTitles = ");
            ContextInfo.AddToList(new ContextInfo { Length = m_OfficerTitles.Length });
            m_OfficerTitles.contributeToTreeNode(officerTitlesNode);
            node.Nodes.Add("m_monarchBroadcastTime = " + m_monarchBroadcastTime);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("m_monarchBroadcastsToday = " + m_monarchBroadcastsToday);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("m_spokesBroadcastTime = " + m_spokesBroadcastTime);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("m_spokesBroadcastsToday = " + m_spokesBroadcastsToday);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("m_motd = " + m_motd);
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Serialized_AsciiString, Length = m_motd.Length });
            node.Nodes.Add("m_motdSetBy = " + m_motdSetBy);
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Serialized_AsciiString, Length = m_motdSetBy.Length });
            node.Nodes.Add("m_chatRoomID = " + m_chatRoomID);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            TreeNode bindNode = node.Nodes.Add("m_BindPoint = ");
            ContextInfo.AddToList(new ContextInfo { Length = m_BindPoint.Length }, updateDataIndex: false);
            m_BindPoint.contributeToTreeNode(bindNode);
            node.Nodes.Add("m_AllegianceName = " + m_AllegianceName);
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Serialized_AsciiString, Length = m_AllegianceName.Length });
            node.Nodes.Add("m_NameLastSetTime = " + m_NameLastSetTime);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("m_isLocked = " + m_isLocked);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("m_ApprovedVassal = " + Utility.FormatHex(m_ApprovedVassal));
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            if (packed_nodes > 0) {
                m_pMonarch.contributeToTreeNode(node);
                ContextInfo.AddToList(new ContextInfo { Length = m_pMonarch.Length }, updateDataIndex: false);
            }
        }
    }

    public class AllegianceProfile
    {
        public uint _total_members;
        public uint _total_vassals;
        public AllegianceHierarchy _allegiance;
        public int Length;

        public static AllegianceProfile read(BinaryReader binaryReader, uint characterId)
        {
            AllegianceProfile newObj = new AllegianceProfile();
            var startPosition = binaryReader.BaseStream.Position;
            newObj._total_members = binaryReader.ReadUInt32();
            newObj._total_vassals = binaryReader.ReadUInt32();
            newObj._allegiance = AllegianceHierarchy.read(binaryReader, characterId);
            newObj.Length = (int)(binaryReader.BaseStream.Position - startPosition);
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
            node.Nodes.Add("_total_members = " + _total_members);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_total_vassals = " + _total_vassals);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            TreeNode hierarchyNode = node.Nodes.Add("allegianceHierarchy = ");
            ContextInfo.AddToList(new ContextInfo { Length = _allegiance.Length }, updateDataIndex: false);
            _allegiance.contributeToTreeNode(hierarchyNode);
        }
    }

    public class AllegianceUpdate : Message {
        public uint _rank;
        public AllegianceProfile allegianceProfile;

        public static AllegianceUpdate read(BinaryReader binaryReader) {
            AllegianceUpdate newObj = new AllegianceUpdate();
            // Get character object ID so we can display correct order of patron/self/vassals
            binaryReader.BaseStream.Position -= 12;
            var characterId = binaryReader.ReadUInt32();
            binaryReader.BaseStream.Position += 8;
            newObj._rank = binaryReader.ReadUInt32();
            newObj.allegianceProfile = AllegianceProfile.read(binaryReader, characterId);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ServerToClientHeader });
            rootNode.Nodes.Add("_rank = " + _rank);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            TreeNode profileNode = rootNode.Nodes.Add("allegianceProfile = ");
            ContextInfo.AddToList(new ContextInfo { Length = allegianceProfile.Length }, updateDataIndex: false);
            allegianceProfile.contributeToTreeNode(profileNode);
            rootNode.ExpandAll();
            treeView.Nodes.Add(rootNode);
        }
    }

    public class SetAllegianceName : Message {
        public PStringChar i_msg;

        public static SetAllegianceName read(BinaryReader binaryReader) {
            SetAllegianceName newObj = new SetAllegianceName();
            newObj.i_msg = PStringChar.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_msg = " + i_msg);
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Serialized_AsciiString, Length = i_msg.Length });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class SetAllegianceOfficer : Message {
        public PStringChar i_char_name;
        public eAllegianceOfficerLevel i_level;

        public static SetAllegianceOfficer read(BinaryReader binaryReader) {
            SetAllegianceOfficer newObj = new SetAllegianceOfficer();
            newObj.i_char_name = PStringChar.read(binaryReader);
            newObj.i_level = (eAllegianceOfficerLevel)binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_char_name = " + i_char_name);
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Serialized_AsciiString, Length = i_char_name.Length });
            rootNode.Nodes.Add("i_level = " + i_level);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class SetAllegianceOfficerTitle : Message {
        public eAllegianceOfficerLevel i_level;
        public PStringChar i_title;

        public static SetAllegianceOfficerTitle read(BinaryReader binaryReader) {
            SetAllegianceOfficerTitle newObj = new SetAllegianceOfficerTitle();
            newObj.i_level = (eAllegianceOfficerLevel)binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            newObj.i_title = PStringChar.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_level = " + i_level);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            rootNode.Nodes.Add("i_title = " + i_title);
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Serialized_AsciiString, Length = i_title.Length });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class DoAllegianceLockAction : Message {
        public eAllegianceLockAction i_iAction;

        public static DoAllegianceLockAction read(BinaryReader binaryReader) {
            DoAllegianceLockAction newObj = new DoAllegianceLockAction();
            newObj.i_iAction = (eAllegianceLockAction)binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_iAction = " + i_iAction);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class SetAllegianceApprovedVassal : Message {
        public PStringChar i_char_name;

        public static SetAllegianceApprovedVassal read(BinaryReader binaryReader) {
            SetAllegianceApprovedVassal newObj = new SetAllegianceApprovedVassal();
            newObj.i_char_name = PStringChar.read(binaryReader);
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_char_name = " + i_char_name);
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Serialized_AsciiString, Length = i_char_name.Length });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class AllegianceChatGag : Message {
        public PStringChar i_char_name;
        public int i_bOn;

        public static AllegianceChatGag read(BinaryReader binaryReader) {
            AllegianceChatGag newObj = new AllegianceChatGag();
            newObj.i_char_name = PStringChar.read(binaryReader);
            newObj.i_bOn = binaryReader.ReadInt32();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_char_name = " + i_char_name);
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Serialized_AsciiString, Length = i_char_name.Length });
            rootNode.Nodes.Add("i_bOn = " + i_bOn);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class DoAllegianceHouseAction : Message {
        public eAllegianceHouseAction i_iAction;

        public static DoAllegianceHouseAction read(BinaryReader binaryReader) {
            DoAllegianceHouseAction newObj = new DoAllegianceHouseAction();
            newObj.i_iAction = (eAllegianceHouseAction)binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_iAction = " + i_iAction);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class AllegianceUpdateDone : Message
    {
        public uint e;
        // NOTE: The client doesn't appear to use any information from this message.
        public static AllegianceUpdateDone read(BinaryReader binaryReader)
        {
            AllegianceUpdateDone newObj = new AllegianceUpdateDone();
            newObj.e = binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ServerToClientHeader });
            rootNode.Nodes.Add("e = " + e);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class SetMotd : Message {
        public PStringChar i_msg;

        public static SetMotd read(BinaryReader binaryReader) {
            SetMotd newObj = new SetMotd();
            newObj.i_msg = PStringChar.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_msg = " + i_msg);
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Serialized_AsciiString, Length = i_msg.Length });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class BreakAllegianceBoot : Message {
        public PStringChar i_bootee_name;
        public int i_account_boot;

        public static BreakAllegianceBoot read(BinaryReader binaryReader) {
            BreakAllegianceBoot newObj = new BreakAllegianceBoot();
            newObj.i_bootee_name = PStringChar.read(binaryReader);
            newObj.i_account_boot = binaryReader.ReadInt32();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_bootee_name = " + i_bootee_name);
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Serialized_AsciiString, Length = i_bootee_name.Length });
            rootNode.Nodes.Add("i_account_boot = " + i_account_boot);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class AllegianceLoginNotificationEvent : Message {
        public uint member;
        public int bNowLoggedIn;

        public static AllegianceLoginNotificationEvent read(BinaryReader binaryReader) {
            AllegianceLoginNotificationEvent newObj = new AllegianceLoginNotificationEvent();
            newObj.member = binaryReader.ReadUInt32();
            newObj.bNowLoggedIn = binaryReader.ReadInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ServerToClientHeader });
            rootNode.Nodes.Add("member = " + Utility.FormatHex(member));
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            rootNode.Nodes.Add("bNowLoggedIn = " + bNowLoggedIn);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class AllegianceInfoRequest : Message {
        public PStringChar i_target_name;
        public int i_account_boot;

        public static AllegianceInfoRequest read(BinaryReader binaryReader) {
            AllegianceInfoRequest newObj = new AllegianceInfoRequest();
            newObj.i_target_name = PStringChar.read(binaryReader);
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_target_name = " + i_target_name);
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Serialized_AsciiString, Length = i_target_name.Length });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class AllegianceInfoResponseEvent : Message {
        public uint target;
        public AllegianceProfile prof;

        public static AllegianceInfoResponseEvent read(BinaryReader binaryReader) {
            AllegianceInfoResponseEvent newObj = new AllegianceInfoResponseEvent();
            // Get character object ID so we can display correct order of patron/self/vassals
            binaryReader.BaseStream.Position -= 12;
            var characterId = binaryReader.ReadUInt32();
            binaryReader.BaseStream.Position += 8;
            newObj.target = binaryReader.ReadUInt32();
            newObj.prof = AllegianceProfile.read(binaryReader, characterId);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ServerToClientHeader });
            rootNode.Nodes.Add("target = " + Utility.FormatHex(target));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            TreeNode profileNode = rootNode.Nodes.Add("prof = ");
            ContextInfo.AddToList(new ContextInfo { Length = prof.Length }, updateDataIndex: false);
            prof.contributeToTreeNode(profileNode);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class AllegianceChatBoot : Message {
        public PStringChar i_char_name;
        public PStringChar i_reason;

        public static AllegianceChatBoot read(BinaryReader binaryReader) {
            AllegianceChatBoot newObj = new AllegianceChatBoot();
            newObj.i_char_name = PStringChar.read(binaryReader);
            newObj.i_reason = PStringChar.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_char_name = " + i_char_name);
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Serialized_AsciiString, Length = i_char_name.Length });
            rootNode.Nodes.Add("i_reason = " + i_reason);
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Serialized_AsciiString, Length = i_reason.Length });

            treeView.Nodes.Add(rootNode);
        }
    }

    public class AddAllegianceBan : Message {
        public PStringChar i_char_name;

        public static AddAllegianceBan read(BinaryReader binaryReader) {
            AddAllegianceBan newObj = new AddAllegianceBan();
            newObj.i_char_name = PStringChar.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_char_name = " + i_char_name);
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Serialized_AsciiString, Length = i_char_name.Length });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class RemoveAllegianceBan : Message {
        public PStringChar i_char_name;

        public static RemoveAllegianceBan read(BinaryReader binaryReader) {
            RemoveAllegianceBan newObj = new RemoveAllegianceBan();
            newObj.i_char_name = PStringChar.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_char_name = " + i_char_name);
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Serialized_AsciiString, Length = i_char_name.Length });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class RemoveAllegianceOfficer : Message {
        public PStringChar i_char_name;

        public static RemoveAllegianceOfficer read(BinaryReader binaryReader) {
            RemoveAllegianceOfficer newObj = new RemoveAllegianceOfficer();
            newObj.i_char_name = PStringChar.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_char_name = " + i_char_name);
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Serialized_AsciiString, Length = i_char_name.Length });
            treeView.Nodes.Add(rootNode);
        }
    }
}
