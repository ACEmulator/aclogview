using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using aclogview;

public class CM_Death : MessageProcessor
{

    public override bool acceptMessageData(BinaryReader messageDataReader, TreeView outputTreeView)
    {
        bool handled = true;

        PacketOpcode opcode = Util.readOpcode(messageDataReader);
        switch (opcode)
        {
            case PacketOpcode.PLAYER_DEATH_EVENT:
                {
                    PlayerDeathEvent message = PlayerDeathEvent.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.VICTIM_NOTIFICATION_EVENT:
                {
                    VictimDeathNotice message = VictimDeathNotice.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.KILLER_NOTIFICATION_EVENT:
                {
                    KillerDeathNotice message = KillerDeathNotice.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            default:
                {
                    handled = false;
                    break;
                }
        }

        return handled;
    }

    public class PlayerDeathEvent : Message
    {
        public PStringChar DeathMessageText;
        public uint VictimId;
        public uint KillerId;

        public static PlayerDeathEvent read(BinaryReader binaryReader)
        {
            PlayerDeathEvent newObj = new PlayerDeathEvent();
            newObj.DeathMessageText = PStringChar.read(binaryReader);
            newObj.VictimId = binaryReader.ReadUInt32();
            newObj.KillerId = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("death_message = " + DeathMessageText);
            rootNode.Nodes.Add("victim_id = " + Utility.FormatGuid(VictimId));
            rootNode.Nodes.Add("killer_id = " + Utility.FormatGuid(KillerId));
            treeView.Nodes.Add(rootNode);
        }
    }

    public class VictimDeathNotice : Message
    {
        public PStringChar DeathMessageText;
        public static VictimDeathNotice read(BinaryReader binaryReader)
        {
            VictimDeathNotice newObj = new VictimDeathNotice();
            newObj.DeathMessageText = PStringChar.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("death_message = " + DeathMessageText);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class KillerDeathNotice : Message
    {
        public PStringChar DeathMessageText;
        public static KillerDeathNotice read(BinaryReader binaryReader)
        {
            KillerDeathNotice newObj = new KillerDeathNotice();
            newObj.DeathMessageText = PStringChar.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("death_message = " + DeathMessageText);
            treeView.Nodes.Add(rootNode);
        }
    }
}