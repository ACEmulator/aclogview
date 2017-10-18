using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using aclogview;

public class CM_Item : MessageProcessor {

    public override bool acceptMessageData(BinaryReader messageDataReader, TreeView outputTreeView) {
        bool handled = true;

        PacketOpcode opcode = Util.readOpcode(messageDataReader);
        switch (opcode) {
            case PacketOpcode.Evt_Item__Appraise_ID: {
                    Appraise message = Appraise.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Item__UseDone_ID: {
                    UseDone message = UseDone.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Item__QueryItemMana_ID: {
                    QueryItemMana message = QueryItemMana.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Item__QueryItemManaResponse_ID: {
                    QueryItemManaResponse message = QueryItemManaResponse.read(messageDataReader);
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

    public class QueryItemMana : Message {
        public uint target;

        public static QueryItemMana read(BinaryReader binaryReader) {
            QueryItemMana newObj = new QueryItemMana();
            newObj.target = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("target = " + Utility.FormatGuid(this.target));                   
            treeView.Nodes.Add(rootNode);
        }
    }

    public class Appraise : Message {
        public uint i_objectID;

        public static Appraise read(BinaryReader binaryReader) {
            Appraise newObj = new Appraise();
            newObj.i_objectID = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_objectID = " + Utility.FormatGuid(i_objectID));
            treeView.Nodes.Add(rootNode);
        }
    }

    public class UseDone : Message {
        public uint etype;

        public static UseDone read(BinaryReader binaryReader) {
            UseDone newObj = new UseDone();
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

    public class QueryItemManaResponse : Message {
        public uint target;
        public float mana;
        public int fSuccess;

        public static QueryItemManaResponse read(BinaryReader binaryReader) {
            QueryItemManaResponse newObj = new QueryItemManaResponse();
            newObj.target = binaryReader.ReadUInt32();
            newObj.mana = binaryReader.ReadSingle();
            newObj.fSuccess = binaryReader.ReadInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("target = " + Utility.FormatGuid(this.target));
            rootNode.Nodes.Add("mana = " + mana);
            rootNode.Nodes.Add("fSuccess = " + fSuccess);
            treeView.Nodes.Add(rootNode);
        }
    }
}
