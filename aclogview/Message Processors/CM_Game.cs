using aclogview;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class CM_Game : MessageProcessor {

    public override bool acceptMessageData(BinaryReader messageDataReader, TreeView outputTreeView) {
        bool handled = true;

        PacketOpcode opcode = Util.readOpcode(messageDataReader);
        switch (opcode) {
            case PacketOpcode.Evt_Game__Join_ID:
                {
                    Join message = Join.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Game__Quit_ID:
                {
                    EmptyMessage message = new EmptyMessage(opcode);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Game__Stalemate_ID:
                {
                    Stalemate message = Stalemate.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Game__Recv_JoinGameResponse_ID:
                {
                    Recv_JoinGameResponse message = Recv_JoinGameResponse.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Game__Recv_GameOver_ID:
                {
                    Recv_GameOver message = Recv_GameOver.read(messageDataReader);
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


    public class Join : Message
    {
        public uint i_idGame; // Gameboard ID
        public int i_iWhichTeam;

        public static Join read(BinaryReader binaryReader)
        {
            Join newObj = new Join();
            newObj.i_idGame = binaryReader.ReadUInt32();
            newObj.i_iWhichTeam = binaryReader.ReadInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_idGame = " + Utility.FormatHex(i_idGame)); 
            rootNode.Nodes.Add("i_iWhichTeam = " + i_iWhichTeam);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class Stalemate : Message
    {
        public int i_fOn;

        public static Stalemate read(BinaryReader binaryReader)
        {
            Stalemate newObj = new Stalemate();
            newObj.i_fOn = binaryReader.ReadInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_fOn = " + i_fOn);
            treeView.Nodes.Add(rootNode);
        }
    }


    public class Recv_JoinGameResponse : Message
    {
        public uint i_idGame;
        public int i_iWhichTeam;

        public static Recv_JoinGameResponse read(BinaryReader binaryReader)
        {
            Recv_JoinGameResponse newObj = new Recv_JoinGameResponse();
            newObj.i_idGame = binaryReader.ReadUInt32();
            newObj.i_iWhichTeam = binaryReader.ReadInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_idGame = " + Utility.FormatHex(i_idGame));
            rootNode.Nodes.Add("i_iWhichTeam = " + i_iWhichTeam);  // TODO: White = 0 (Drudges), Black = 1 (Mosswarts)
            treeView.Nodes.Add(rootNode);
        }
    }

    public class Recv_GameOver : Message
    {
        public uint i_idGame;
        public int i_iTeamWinner;

        public static Recv_GameOver read(BinaryReader binaryReader)
        {
            Recv_GameOver newObj = new Recv_GameOver();
            newObj.i_idGame = binaryReader.ReadUInt32();
            newObj.i_iTeamWinner = binaryReader.ReadInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_idGame = " + Utility.FormatHex(i_idGame));
            rootNode.Nodes.Add("i_iTeamWinner = " + i_iTeamWinner);
            treeView.Nodes.Add(rootNode);
        }
    }
}
