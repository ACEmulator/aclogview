using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class CM_Train : MessageProcessor {

    public override bool acceptMessageData(BinaryReader messageDataReader, TreeView outputTreeView) {
        bool handled = true;

        PacketOpcode opcode = Util.readOpcode(messageDataReader);
        switch (opcode) {
            case PacketOpcode.Evt_Train__TrainAttribute2nd_ID: {
                    TrainAttribute2nd message = TrainAttribute2nd.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Train__TrainAttribute_ID: {
                    TrainAttribute message = TrainAttribute.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Train__TrainSkill_ID: {
                    TrainSkill message = TrainSkill.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Train__TrainSkillAdvancementClass_ID: {
                    TrainSkillAdvancementClass message = TrainSkillAdvancementClass.read(messageDataReader);
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

    public class TrainAttribute2nd : Message {
        public STypeAttribute2nd i_atype;
        public uint i_xp_spent;

        public static TrainAttribute2nd read(BinaryReader binaryReader) {
            TrainAttribute2nd newObj = new TrainAttribute2nd();
            newObj.i_atype = (STypeAttribute2nd)binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            newObj.i_xp_spent = binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_atype = " + i_atype);
            rootNode.Nodes.Add("i_xp_spent = " + i_xp_spent);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class TrainAttribute : Message {
        public STypeAttribute i_atype;
        public uint i_xp_spent;

        public static TrainAttribute read(BinaryReader binaryReader) {
            TrainAttribute newObj = new TrainAttribute();
            newObj.i_atype = (STypeAttribute)binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            newObj.i_xp_spent = binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_atype = " + i_atype);
            rootNode.Nodes.Add("i_xp_spent = " + i_xp_spent);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class TrainSkill : Message {
        public STypeSkill i_stype;
        public uint i_xp_spent;

        public static TrainSkill read(BinaryReader binaryReader) {
            TrainSkill newObj = new TrainSkill();
            newObj.i_stype = (STypeSkill)binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            newObj.i_xp_spent = binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_stype = " + i_stype);
            rootNode.Nodes.Add("i_xp_spent = " + i_xp_spent);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class TrainSkillAdvancementClass : Message {
        public STypeSkill i_stype; // TODO: Is this the right enum?
        public uint i_xp_spent;

        public static TrainSkillAdvancementClass read(BinaryReader binaryReader) {
            TrainSkillAdvancementClass newObj = new TrainSkillAdvancementClass();
            newObj.i_stype = (STypeSkill)binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            newObj.i_xp_spent = binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_stype = " + i_stype);
            rootNode.Nodes.Add("i_xp_spent = " + i_xp_spent);
            treeView.Nodes.Add(rootNode);
        }
    }
}
