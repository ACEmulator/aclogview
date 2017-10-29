using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            rootNode.Nodes.Add("etype = " + etype);
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
            rootNode.Nodes.Add("i_target = " + Utility.FormatGuid(i_target));
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
            rootNode.Nodes.Add("i_target = " + Utility.FormatGuid(i_target));
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
            rootNode.Nodes.Add("i_on = " + i_on);
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

        public static AllegianceData read(BinaryReader binaryReader) {
            AllegianceData newObj = new AllegianceData();
            newObj._id = binaryReader.ReadUInt32();
            newObj._cp_cached = binaryReader.ReadUInt32();
            newObj._cp_tithed = binaryReader.ReadUInt32();
            newObj._bitfield = binaryReader.ReadUInt32();
            newObj._gender = binaryReader.ReadByte();
            newObj._hg = binaryReader.ReadByte();
            newObj._rank = binaryReader.ReadUInt16();
            if ((newObj._bitfield & 0x8) != 0) {
                newObj._level = binaryReader.ReadUInt32();
            }
            newObj._loyalty = binaryReader.ReadUInt16();
            newObj._leadership = binaryReader.ReadUInt16();
            if ((newObj._bitfield & 0x4) != 0) {
                newObj._time_online__small = binaryReader.ReadUInt32();
                newObj._allegiance_age = binaryReader.ReadUInt32();
            } else {
                newObj._time_online__large = binaryReader.ReadUInt64();
            }
            newObj._name = PStringChar.read(binaryReader);
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node) {
            node.Nodes.Add("_id = " + Utility.FormatGuid(_id));
            node.Nodes.Add("_cp_cached = " + _cp_cached);
            node.Nodes.Add("_cp_tithed = " + _cp_tithed);
            node.Nodes.Add("_bitfield = " + _bitfield);
            node.Nodes.Add("_gender = " + (Gender)_gender);
            node.Nodes.Add("_hg = " + (HeritageGroup)_hg);
            node.Nodes.Add("_rank = " + _rank);
            node.Nodes.Add("_level = " + _level);
            node.Nodes.Add("_loyalty = " + _loyalty);
            node.Nodes.Add("_leadership = " + _leadership);
            if ((_bitfield & 0x4) != 0) {
                node.Nodes.Add("_time_online__small = " + _time_online__small);
                node.Nodes.Add("_allegiance_age = " + _allegiance_age);
            } else {
                node.Nodes.Add("_time_online__large = " + _time_online__large);
            }
            node.Nodes.Add("_name = " + _name);
        }
    }

    public class AllegianceNode
    {
        public uint _parent_id;
        public AllegianceData _monarch;
        public AllegianceNode _patron;
        public AllegianceNode _peer;
        public List<AllegianceNode> vassalList;
        public AllegianceData _data;

        // TODO: May need to pass the character Guid down to this procedure to check where our character is in the hierarchy.
        // There is a chance that a node might get put in the wrong place in the hierarchy. We should be able to check
        // each nodes parent ID and group them accordingly once the correct hierarchy is known.
        // For example: if we are directly beneath the monarch does that make us a peer or a patron?
        // If we are the monarch does that make us a monarch node or a peer node? Can vassals only be listed under a patron node?
        public static AllegianceNode read(BinaryReader binaryReader, ushort packed_nodes) {
            AllegianceNode newObj = new AllegianceNode();
            newObj.vassalList = new List<AllegianceNode>();
            for (int i = 0; i < packed_nodes; i++) {
                if (i == 0) {
                    newObj._monarch = AllegianceData.read(binaryReader);
                }
                else if (i == 1) {
                    newObj._patron = new AllegianceNode();
                    newObj._patron._parent_id = binaryReader.ReadUInt32();
                    newObj._patron._data = AllegianceData.read(binaryReader);
                }
                else if (i == 2) {
                    newObj._peer = new AllegianceNode();
                    newObj._peer._parent_id = binaryReader.ReadUInt32();
                    newObj._peer._data = AllegianceData.read(binaryReader);
                }
                else {
                    var _vassal = new AllegianceNode();
                    _vassal._parent_id = binaryReader.ReadUInt32();
                    _vassal._data = AllegianceData.read(binaryReader);
                    newObj.vassalList.Add(_vassal);
                }
            }
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node) {
            TreeNode monarchNode = node.Nodes.Add("_monarch = ");
            _monarch.contributeToTreeNode(monarchNode);
            TreeNode patronNode = monarchNode.Nodes.Add("_patron = ");
            patronNode.Nodes.Add("_parent_id = " + Utility.FormatGuid(_patron._parent_id));
            _patron._data.contributeToTreeNode(patronNode);
            if (_peer != null) {
                TreeNode peerNode = patronNode.Nodes.Add("_peer = ");
                peerNode.Nodes.Add("_parent_id = " + Utility.FormatGuid(_peer._parent_id));
                _peer._data.contributeToTreeNode(peerNode);
                for (int i = 0; i < vassalList.Count; i++)
                {
                    TreeNode vassalNode = peerNode.Nodes.Add($"_vassal {i + 1} = ");
                    vassalNode.Nodes.Add("_parent_id = " + Utility.FormatGuid(vassalList[i]._parent_id));
                    vassalList[i]._data.contributeToTreeNode(vassalNode);
                }
            }
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
        public AllegianceNode m_pMonarch;

        public static AllegianceHierarchy read(BinaryReader binaryReader)
        {
            AllegianceHierarchy newObj = new AllegianceHierarchy();
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
                newObj.m_pMonarch = AllegianceNode.read(binaryReader, newObj.packed_nodes);
            }
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
            node.Nodes.Add("packed_nodes = " + packed_nodes);
            node.Nodes.Add("m_oldVersion = " + m_oldVersion);
            TreeNode officersNode = node.Nodes.Add("m_AllegianceOfficers = ");
            m_AllegianceOfficers.contributeToTreeNode(officersNode);
            TreeNode officerTitlesNode = node.Nodes.Add("m_OfficerTitles = ");
            m_OfficerTitles.contributeToTreeNode(officerTitlesNode);
            node.Nodes.Add("m_monarchBroadcastTime = " + m_monarchBroadcastTime);
            node.Nodes.Add("m_monarchBroadcastsToday = " + m_monarchBroadcastsToday);
            node.Nodes.Add("m_spokesBroadcastTime = " + m_spokesBroadcastTime);
            node.Nodes.Add("m_spokesBroadcastsToday = " + m_spokesBroadcastsToday);
            node.Nodes.Add("m_motd = " + m_motd);
            node.Nodes.Add("m_motdSetBy = " + m_motdSetBy);
            node.Nodes.Add("m_chatRoomID = " + m_chatRoomID);
            TreeNode bindNode = node.Nodes.Add("m_BindPoint = ");
            m_BindPoint.contributeToTreeNode(bindNode);
            node.Nodes.Add("m_AllegianceName = " + m_AllegianceName);
            node.Nodes.Add("m_NameLastSetTime = " + m_NameLastSetTime);
            node.Nodes.Add("m_isLocked = " + m_isLocked);
            node.Nodes.Add("m_ApprovedVassal = " + Utility.FormatGuid(m_ApprovedVassal));
            if (packed_nodes > 0) {
                m_pMonarch.contributeToTreeNode(node);
            }
        }
    }

    public class AllegianceProfile
    {
        public uint _total_members;
        public uint _total_vassals;
        public AllegianceHierarchy _allegiance;

        public static AllegianceProfile read(BinaryReader binaryReader)
        {
            AllegianceProfile newObj = new AllegianceProfile();
            newObj._total_members = binaryReader.ReadUInt32();
            newObj._total_vassals = binaryReader.ReadUInt32();
            newObj._allegiance = AllegianceHierarchy.read(binaryReader);
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
            node.Nodes.Add("_total_members = " + _total_members);
            node.Nodes.Add("_total_vassals = " + _total_vassals);
            TreeNode hierarchyNode = node.Nodes.Add("allegianceHierarchy = ");
            _allegiance.contributeToTreeNode(hierarchyNode);
        }
    }

    public class AllegianceUpdate : Message {
        public uint _rank;
        public AllegianceProfile allegianceProfile;

        public static AllegianceUpdate read(BinaryReader binaryReader) {
            AllegianceUpdate newObj = new AllegianceUpdate();
            newObj._rank = binaryReader.ReadUInt32();
            newObj.allegianceProfile = AllegianceProfile.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Nodes.Add("_rank = " + _rank);
            TreeNode profileNode = rootNode.Nodes.Add("allegianceProfile = ");
            allegianceProfile.contributeToTreeNode(profileNode);
            treeView.Nodes.Add(rootNode);
            rootNode.ExpandAll();
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
            rootNode.Nodes.Add("i_msg = " + i_msg);
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
            rootNode.Nodes.Add("i_char_name = " + i_char_name);
            rootNode.Nodes.Add("i_level = " + i_level);
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
            rootNode.Nodes.Add("i_level = " + i_level);
            rootNode.Nodes.Add("i_title = " + i_title);
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
            rootNode.Nodes.Add("i_iAction = " + i_iAction);
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
            rootNode.Nodes.Add("i_char_name = " + i_char_name);
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
            rootNode.Nodes.Add("i_char_name = " + i_char_name);
            rootNode.Nodes.Add("i_bOn = " + i_bOn);
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
            rootNode.Nodes.Add("i_iAction = " + i_iAction);
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
            rootNode.Nodes.Add("e = " + e);
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
            rootNode.Nodes.Add("i_msg = " + i_msg);
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
            rootNode.Nodes.Add("i_bootee_name = " + i_bootee_name);
            rootNode.Nodes.Add("i_account_boot = " + i_account_boot);
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
            rootNode.Nodes.Add("member = " + Utility.FormatGuid(member));
            rootNode.Nodes.Add("bNowLoggedIn = " + bNowLoggedIn);
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
            rootNode.Nodes.Add("i_target_name = " + i_target_name);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class AllegianceInfoResponseEvent : Message {
        public uint target;
        public AllegianceProfile prof;

        public static AllegianceInfoResponseEvent read(BinaryReader binaryReader) {
            AllegianceInfoResponseEvent newObj = new AllegianceInfoResponseEvent();
            newObj.target = binaryReader.ReadUInt32();
            newObj.prof = AllegianceProfile.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("target = " + Utility.FormatGuid(target));
            TreeNode profileNode = rootNode.Nodes.Add("prof = ");
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
            rootNode.Nodes.Add("i_char_name = " + i_char_name);
            rootNode.Nodes.Add("i_reason = " + i_reason);
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
            rootNode.Nodes.Add("i_char_name = " + i_char_name);
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
            rootNode.Nodes.Add("i_char_name = " + i_char_name);
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
            rootNode.Nodes.Add("i_char_name = " + i_char_name);
            treeView.Nodes.Add(rootNode);
        }
    }
}
