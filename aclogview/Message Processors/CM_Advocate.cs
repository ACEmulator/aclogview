using aclogview;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class CM_Advocate : MessageProcessor {

    public override bool acceptMessageData(BinaryReader messageDataReader, TreeView outputTreeView) {
        bool handled = true;

        PacketOpcode opcode = Util.readOpcode(messageDataReader);
        switch (opcode) {
            case PacketOpcode.Evt_Advocate__Bestow_ID:
            case PacketOpcode.Evt_Advocate__SetState_ID:
            case PacketOpcode.Evt_Advocate__SetAttackable_ID: {
                    handled = false;
                    break;
                }
            case PacketOpcode.Evt_Advocate__Teleport_ID: {
                    Teleport message = Teleport.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Advocate__TeleportTo_ID: {
                    handled = false;
                    break;
                }
            default: {
                    handled = false;
                    break;
                }
        }

        return handled;
    }
    // Note: Context info has not been tested as there are no pcaps of this message.
    public class Teleport : Message {
        public PStringChar i_target;
        public Position i_dest;

        public static Teleport read(BinaryReader binaryReader) {
            Teleport newObj = new Teleport();
            newObj.i_target = PStringChar.read(binaryReader);
            newObj.i_dest = Position.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_target = " + i_target);
            ContextInfo.AddToList(new ContextInfo { Length = i_target.Length, DataType = DataType.Serialized_AsciiString });
            TreeNode destNode = rootNode.Nodes.Add("i_dest = ");
            ContextInfo.AddToList(new ContextInfo { Length = i_dest.Length }, updateDataIndex: false);
            i_dest.contributeToTreeNode(destNode);
            ContextInfo.AddToList(new ContextInfo { Length = i_dest.Length });
            treeView.Nodes.Add(rootNode);
        }
    }
}
