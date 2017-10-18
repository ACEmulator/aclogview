using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using aclogview;

public class CM_Trade : MessageProcessor {

    public override bool acceptMessageData(BinaryReader messageDataReader, TreeView outputTreeView) {
        bool handled = true;

        PacketOpcode opcode = Util.readOpcode(messageDataReader);
        switch (opcode) {
            case PacketOpcode.Evt_Trade__CloseTradeNegotiations_ID:
            case PacketOpcode.Evt_Trade__DeclineTrade_ID:
            case PacketOpcode.Evt_Trade__ResetTrade_ID:
            case PacketOpcode.Evt_Trade__Recv_ClearTradeAcceptance_ID: {
                    EmptyMessage message = new EmptyMessage(opcode);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Trade__OpenTradeNegotiations_ID: {
                    OpenTradeNegotiations message = OpenTradeNegotiations.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Trade__AddToTrade_ID: {
                    AddToTrade message = AddToTrade.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            // TODO: PacketOpcode.Evt_Trade__RemoveFromTrade_ID
            case PacketOpcode.Evt_Trade__AcceptTrade_ID: {
                    AcceptTrade message = AcceptTrade.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            // TODO: PacketOpcode.Evt_Trade__DumpTrade_ID
            case PacketOpcode.Evt_Trade__Recv_RegisterTrade_ID: {
                    Recv_RegisterTrade message = Recv_RegisterTrade.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Trade__Recv_OpenTrade_ID: {
                    Recv_OpenTrade message = Recv_OpenTrade.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Trade__Recv_CloseTrade_ID: {
                    Recv_CloseTrade message = Recv_CloseTrade.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Trade__Recv_AddToTrade_ID: {
                    Recv_AddToTrade message = Recv_AddToTrade.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Trade__Recv_RemoveFromTrade_ID: {
                    Recv_RemoveFromTrade message = Recv_RemoveFromTrade.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Trade__Recv_AcceptTrade_ID: {
                    Recv_AcceptTrade message = Recv_AcceptTrade.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Trade__Recv_DeclineTrade_ID: {
                    Recv_DeclineTrade message = Recv_DeclineTrade.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Trade__Recv_ResetTrade_ID: {
                    Recv_ResetTrade message = Recv_ResetTrade.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Trade__Recv_TradeFailure_ID: {
                    Recv_TradeFailure message = Recv_TradeFailure.read(messageDataReader);
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

    public class OpenTradeNegotiations : Message {
        public uint i_other;

        public static OpenTradeNegotiations read(BinaryReader binaryReader) {
            OpenTradeNegotiations newObj = new OpenTradeNegotiations();
            newObj.i_other = binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_other = " + Utility.FormatGuid(i_other));
            treeView.Nodes.Add(rootNode);
        }
    }

    public class AddToTrade : Message {
        public uint i_item;
        public uint i_loc;

        public static AddToTrade read(BinaryReader binaryReader) {
            AddToTrade newObj = new AddToTrade();
            newObj.i_item = binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            newObj.i_loc = binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_item = " + Utility.FormatGuid(i_item));
            rootNode.Nodes.Add("i_loc = " + i_loc);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class ContentProfile {
        public uint m_iid;
        public uint m_uContainerProperties;

        public static ContentProfile read(BinaryReader binaryReader) {
            ContentProfile newObj = new ContentProfile();
            newObj.m_iid = binaryReader.ReadUInt32();
            newObj.m_uContainerProperties = binaryReader.ReadUInt32();
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node) {
            node.Nodes.Add("m_iid = " + m_iid);
            node.Nodes.Add("m_uContainerProperties = " + m_uContainerProperties);
        }
    }

    public class Trade {
        public uint _partner;
        public double _stamp;
        public uint _status;
        public uint _initiator;
        public uint _accepted;
        public uint _p_accepted;
        public PList<ContentProfile> _self_list;
        public PList<ContentProfile> _partner_list;

        public static Trade read(BinaryReader binaryReader) {
            Trade newObj = new Trade();
            newObj._partner = binaryReader.ReadUInt32();
            newObj._stamp = binaryReader.ReadDouble();
            newObj._status = binaryReader.ReadUInt32();
            newObj._initiator = binaryReader.ReadUInt32();
            newObj._accepted = binaryReader.ReadUInt32();
            newObj._p_accepted = binaryReader.ReadUInt32();
            newObj._self_list = PList<ContentProfile>.read(binaryReader);
            newObj._partner_list = PList<ContentProfile>.read(binaryReader);
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node) {
            node.Nodes.Add("_partner = " + _partner);
            node.Nodes.Add("_stamp = " + _stamp);
            node.Nodes.Add("_status = " + _status);
            node.Nodes.Add("_initiator = " + _initiator);
            node.Nodes.Add("_accepted = " + _accepted);
            node.Nodes.Add("_p_accepted = " + _p_accepted);
            TreeNode selfListNode = node.Nodes.Add("_self_list = ");
            _self_list.contributeToTreeNode(selfListNode);
            TreeNode partnerListNode = node.Nodes.Add("_partner_list = ");
            _partner_list.contributeToTreeNode(partnerListNode);
        }
    }

    public class AcceptTrade : Message {
        public Trade i_stuff;

        public static AcceptTrade read(BinaryReader binaryReader) {
            AcceptTrade newObj = new AcceptTrade();
            newObj.i_stuff = Trade.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            TreeNode stuffNode = rootNode.Nodes.Add("i_stuff = ");
            i_stuff.contributeToTreeNode(stuffNode);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class Recv_RegisterTrade : Message {
        public uint initiator;
        public uint partner;
        public double stamp; // TODO: Perhaps actually long double??

        public static Recv_RegisterTrade read(BinaryReader binaryReader) {
            Recv_RegisterTrade newObj = new Recv_RegisterTrade();
            newObj.initiator = binaryReader.ReadUInt32();
            newObj.partner = binaryReader.ReadUInt32();
            newObj.stamp = binaryReader.ReadDouble();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("initiator = " + Utility.FormatGuid(initiator));
            rootNode.Nodes.Add("partner = " + Utility.FormatGuid(partner));
            rootNode.Nodes.Add("stamp = " + stamp);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class Recv_OpenTrade : Message {
        public uint source;

        public static Recv_OpenTrade read(BinaryReader binaryReader) {
            Recv_OpenTrade newObj = new Recv_OpenTrade();
            newObj.source = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("source = " + source);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class Recv_CloseTrade : Message {
        public uint etype;

        public static Recv_CloseTrade read(BinaryReader binaryReader) {
            Recv_CloseTrade newObj = new Recv_CloseTrade();
            newObj.etype = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("etype = " + (WERROR)etype);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class Recv_AddToTrade : Message {
        public uint item;
        public uint id;
        public uint loc;

        public static Recv_AddToTrade read(BinaryReader binaryReader) {
            Recv_AddToTrade newObj = new Recv_AddToTrade();
            newObj.item = binaryReader.ReadUInt32();
            newObj.id = binaryReader.ReadUInt32();
            newObj.loc = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("item = " + Utility.FormatGuid(item));
            rootNode.Nodes.Add("id = " + (TradeListIDEnum)id);
            rootNode.Nodes.Add("loc = " + loc);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class Recv_RemoveFromTrade : Message {
        public uint i_iidItem;
        public uint id;

        public static Recv_RemoveFromTrade read(BinaryReader binaryReader) {
            Recv_RemoveFromTrade newObj = new Recv_RemoveFromTrade();
            newObj.i_iidItem = binaryReader.ReadUInt32();
            newObj.id = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_iidItem = " + i_iidItem);
            rootNode.Nodes.Add("id = " + id);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class Recv_AcceptTrade : Message {
        public uint source;

        public static Recv_AcceptTrade read(BinaryReader binaryReader) {
            Recv_AcceptTrade newObj = new Recv_AcceptTrade();
            newObj.source = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("source = " + Utility.FormatGuid(source));
            treeView.Nodes.Add(rootNode);
        }
    }

    public class Recv_DeclineTrade : Message {
        public uint source;

        public static Recv_DeclineTrade read(BinaryReader binaryReader) {
            Recv_DeclineTrade newObj = new Recv_DeclineTrade();
            newObj.source = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("source = " + Utility.FormatGuid(source));
            treeView.Nodes.Add(rootNode);
        }
    }

    public class Recv_ResetTrade : Message {
        public uint source;

        public static Recv_ResetTrade read(BinaryReader binaryReader) {
            Recv_ResetTrade newObj = new Recv_ResetTrade();
            newObj.source = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("source = " + Utility.FormatGuid(source));
            treeView.Nodes.Add(rootNode);
        }
    }

    public class Recv_TradeFailure : Message {
        public uint i_iidItem;
        public uint etype;

        public static Recv_TradeFailure read(BinaryReader binaryReader) {
            Recv_TradeFailure newObj = new Recv_TradeFailure();
            newObj.i_iidItem = binaryReader.ReadUInt32();
            newObj.etype = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_iidItem = " + Utility.FormatGuid(i_iidItem));
            rootNode.Nodes.Add("etype = " + (WERROR)etype);
            treeView.Nodes.Add(rootNode);
        }
    }
}
