using System.IO;
using System.Windows.Forms;
using aclogview;
using static CM_Inventory;

public class CM_Trade : MessageProcessor {

    public override bool acceptMessageData(BinaryReader messageDataReader, TreeView outputTreeView) {
        bool handled = true;

        PacketOpcode opcode = Util.readOpcode(messageDataReader);
        switch (opcode) {
            case PacketOpcode.Evt_Trade__CloseTradeNegotiations_ID:
            case PacketOpcode.Evt_Trade__DeclineTrade_ID:
            case PacketOpcode.Evt_Trade__ResetTrade_ID: {
                    EmptyMessage message = new EmptyMessage(opcode);
                    message.contributeToTreeView(outputTreeView);
                    ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
                    break;
                }

            case PacketOpcode.Evt_Trade__Recv_ClearTradeAcceptance_ID: {
                    EmptyMessage message = new EmptyMessage(opcode);
                    message.contributeToTreeView(outputTreeView);
                    ContextInfo.AddToList(new ContextInfo { DataType = DataType.ServerToClientHeader });
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
            // TODO: PacketOpcode.Evt_Trade__RemoveFromTrade_ID (retired)
            case PacketOpcode.Evt_Trade__AcceptTrade_ID: {
                    AcceptTrade message = AcceptTrade.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            // TODO: PacketOpcode.Evt_Trade__DumpTrade_ID (retired)
            case PacketOpcode.Evt_Trade__Recv_RegisterTrade_ID: {
                    Recv_RegisterTrade message = Recv_RegisterTrade.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Trade__Recv_OpenTrade_ID: { // (retired)
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
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_other = " + Utility.FormatHex(i_other));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
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
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_item = " + Utility.FormatHex(i_item));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            rootNode.Nodes.Add("i_loc = " + i_loc);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            treeView.Nodes.Add(rootNode);
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
        public int Length;

        public static Trade read(BinaryReader binaryReader) {
            Trade newObj = new Trade();
            var startPosition = binaryReader.BaseStream.Position;
            newObj._partner = binaryReader.ReadUInt32();
            newObj._stamp = binaryReader.ReadDouble();
            newObj._status = binaryReader.ReadUInt32();
            newObj._initiator = binaryReader.ReadUInt32();
            newObj._accepted = binaryReader.ReadUInt32();
            newObj._p_accepted = binaryReader.ReadUInt32();
            newObj._self_list = PList<ContentProfile>.read(binaryReader);
            newObj._partner_list = PList<ContentProfile>.read(binaryReader);
            newObj.Length = (int)(binaryReader.BaseStream.Position - startPosition);
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node) {
            node.Nodes.Add("_partner = " + Utility.FormatHex(_partner));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            node.Nodes.Add("_stamp = " + _stamp);
            ContextInfo.AddToList(new ContextInfo { Length = 8 });
            node.Nodes.Add("_status = " + (TradeStatusEnum)_status);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_initiator = " + _initiator);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_accepted = " + _accepted);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_p_accepted = " + _p_accepted);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            TreeNode selfListNode = node.Nodes.Add("_self_list = ");
            ContextInfo.AddToList(new ContextInfo { Length = _self_list.Length }, updateDataIndex: false);
            ContextInfo.DataIndex += 4;
            for (int i = 0; i < _self_list.list.Count; i++)
            {
                TreeNode itemNode = selfListNode.Nodes.Add($"item {i+1} = ");
                ContextInfo.AddToList(new ContextInfo { Length = 8 }, updateDataIndex: false);
                _self_list.list[i].contributeToTreeNode(itemNode);
            }
            TreeNode partnerListNode = node.Nodes.Add("_partner_list = ");
            ContextInfo.AddToList(new ContextInfo { Length = _partner_list.Length }, updateDataIndex: false);
            ContextInfo.DataIndex += 4;
            for (int i = 0; i < _partner_list.list.Count; i++)
            {
                TreeNode itemNode = partnerListNode.Nodes.Add($"item {i+1} = ");
                ContextInfo.AddToList(new ContextInfo { Length = 8 }, updateDataIndex: false);
                _partner_list.list[i].contributeToTreeNode(itemNode);
            }
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
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            TreeNode stuffNode = rootNode.Nodes.Add("i_stuff = ");
            ContextInfo.AddToList(new ContextInfo { Length = i_stuff.Length }, updateDataIndex: false);
            i_stuff.contributeToTreeNode(stuffNode);
            stuffNode.Expand();
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
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ServerToClientHeader });
            rootNode.Nodes.Add("initiator = " + Utility.FormatHex(initiator));
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            rootNode.Nodes.Add("partner = " + Utility.FormatHex(partner));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            rootNode.Nodes.Add("stamp = " + stamp);
            ContextInfo.AddToList(new ContextInfo { Length = 8 });
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
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ServerToClientHeader });
            rootNode.Nodes.Add("source = " + source);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
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
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ServerToClientHeader });
            rootNode.Nodes.Add("etype = " + (WERROR)etype);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
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
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ServerToClientHeader });
            rootNode.Nodes.Add("item = " + Utility.FormatHex(item));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            rootNode.Nodes.Add("id = " + (TradeListIDEnum)id);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            rootNode.Nodes.Add("loc = " + loc);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
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
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ServerToClientHeader });
            rootNode.Nodes.Add("i_iidItem = " + Utility.FormatHex(i_iidItem));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            rootNode.Nodes.Add("id = " + id);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
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
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ServerToClientHeader });
            rootNode.Nodes.Add("source = " + Utility.FormatHex(source));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
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
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ServerToClientHeader });
            rootNode.Nodes.Add("source = " + Utility.FormatHex(source));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
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
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ServerToClientHeader });
            rootNode.Nodes.Add("source = " + Utility.FormatHex(source));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
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
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ServerToClientHeader });
            rootNode.Nodes.Add("i_iidItem = " + Utility.FormatHex(i_iidItem));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            rootNode.Nodes.Add("etype = " + (WERROR)etype);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            treeView.Nodes.Add(rootNode);
        }
    }
}
