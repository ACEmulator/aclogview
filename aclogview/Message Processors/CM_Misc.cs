using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class CM_Misc : MessageProcessor {

    public override bool acceptMessageData(BinaryReader messageDataReader, TreeView outputTreeView) {
        bool handled = true;

        PacketOpcode opcode = Util.readOpcode(messageDataReader);
        switch (opcode) {
            case PacketOpcode.Evt_Misc__PortalStorm_ID:
            case PacketOpcode.Evt_Misc__PortalStormSubsided_ID: {
                    EmptyMessage message = new EmptyMessage(opcode);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Misc__PortalStormBrewing_ID: {
                    PortalStormBrewing message = PortalStormBrewing.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Misc__PortalStormImminent_ID: {
                    PortalStormImminent message = PortalStormImminent.read(messageDataReader);
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

    public class PortalStormBrewing : Message {
        public float extent;

        public static PortalStormBrewing read(BinaryReader binaryReader) {
            PortalStormBrewing newObj = new PortalStormBrewing();
            newObj.extent = binaryReader.ReadSingle();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("extent = " + extent);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class PortalStormImminent : Message {
        public float extent;

        public static PortalStormImminent read(BinaryReader binaryReader) {
            PortalStormImminent newObj = new PortalStormImminent();
            newObj.extent = binaryReader.ReadSingle();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("extent = " + extent);
            treeView.Nodes.Add(rootNode);
        }
    }
}
