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
            // TODO: PacketOpcode.Evt_Advocate__Bestow_ID
            // TODO: PacketOpcode.Evt_Advocate__SetState_ID
            // TODO: PacketOpcode.Evt_Advocate__SetAttackable_ID
            case PacketOpcode.Evt_Advocate__Teleport_ID: {
                    Teleport message = Teleport.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            // TODO: PacketOpcode.Evt_Advocate__TeleportTo_ID
            default: {
                    handled = false;
                    break;
                }
        }

        return handled;
    }

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
            rootNode.Nodes.Add("i_target = " + i_target);
            TreeNode destNode = rootNode.Nodes.Add("i_dest = ");
            i_dest.contributeToTreeNode(destNode);
            treeView.Nodes.Add(rootNode);
        }
    }
}
