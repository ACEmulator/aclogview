using aclogview;
using System;
using System.IO;
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
            case PacketOpcode.Evt_Game__MovePass_ID:
            case PacketOpcode.Evt_Game__Quit_ID:
                {
                    EmptyMessage message = new EmptyMessage(opcode);
                    message.contributeToTreeView(outputTreeView);
                    ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
                    break;
                }
            case PacketOpcode.Evt_Game__Move_ID:
                {
                    Move message = Move.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            // PacketOpcode.Evt_Game__MoveGrid_ID - Retired message
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
            case PacketOpcode.Evt_Game__Recv_StartGame_ID:
                {
                    Recv_StartGame message = Recv_StartGame.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Game__Recv_MoveResponse_ID:
                {
                    Recv_MoveResponse message = Recv_MoveResponse.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Game__Recv_OpponentTurn_ID:
                {
                    Recv_OpponentTurn message = Recv_OpponentTurn.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Game__Recv_OppenentStalemateState_ID:
                {
                    Recv_OpponentStalemateState message = Recv_OpponentStalemateState.read(messageDataReader);
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

    public enum ChessTeam
    {
        None = -1, 
        White = 0, // Drudges
        Black = 1  // Mosswarts
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
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_idGame = " + Utility.FormatHex(i_idGame));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            rootNode.Nodes.Add("i_iWhichTeam = " + (ChessTeam)i_iWhichTeam);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class Move : Message
    {
        public int i_xFrom;
        public int i_yFrom;
        public int i_xTo;
        public int i_yTo;

        public static Move read(BinaryReader binaryReader)
        {
            Move newObj = new Move();
            newObj.i_xFrom = binaryReader.ReadInt32();
            newObj.i_yFrom = binaryReader.ReadInt32();
            newObj.i_xTo = binaryReader.ReadInt32();
            newObj.i_yTo = binaryReader.ReadInt32();

            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_xFrom = " + i_xFrom);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            rootNode.Nodes.Add("i_yFrom = " + i_yFrom);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            rootNode.Nodes.Add("i_xTo = " + i_xTo);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            rootNode.Nodes.Add("i_yTo = " + i_yTo);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
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
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_fOn = " + i_fOn);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
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
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ServerToClientHeader });
            rootNode.Nodes.Add("i_idGame = " + Utility.FormatHex(i_idGame));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            rootNode.Nodes.Add("i_iWhichTeam = " + (ChessTeam)i_iWhichTeam);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class Recv_StartGame : Message
    {
        public uint i_idGame;
        public int i_iTeam;

        public static Recv_StartGame read(BinaryReader binaryReader)
        {
            Recv_StartGame newObj = new Recv_StartGame();
            newObj.i_idGame = binaryReader.ReadUInt32();
            newObj.i_iTeam = binaryReader.ReadInt32();

            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ServerToClientHeader });
            rootNode.Nodes.Add("i_idGame = " + Utility.FormatHex(i_idGame));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            rootNode.Nodes.Add("i_iTeam = " + (ChessTeam)i_iTeam);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class Recv_MoveResponse : Message
    {
        public uint i_idGame;
        public int i_iMoveResult;

        public static Recv_MoveResponse read(BinaryReader binaryReader)
        {
            Recv_MoveResponse newObj = new Recv_MoveResponse();
            newObj.i_idGame = binaryReader.ReadUInt32();
            newObj.i_iMoveResult = binaryReader.ReadInt32();

            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ServerToClientHeader });
            rootNode.Nodes.Add("i_idGame = " + Utility.FormatHex(i_idGame));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            rootNode.Nodes.Add("i_iMoveResult = " + i_iMoveResult);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            treeView.Nodes.Add(rootNode);
        }
    }


    public class GameMoveData
    {
        public int m_type;
        public uint m_idPlayer;
        public uint m_idPieceToMove;
        public int m_yGrid;
        public int m_xTo;
        public int m_yTo;
        public int Length;

        public static GameMoveData read(BinaryReader binaryReader)
        {
            GameMoveData newObj = new GameMoveData();
            var startPosition = binaryReader.BaseStream.Position;
            newObj.m_type = binaryReader.ReadInt32();
            newObj.m_idPlayer = binaryReader.ReadUInt32();
            newObj.m_idPieceToMove = binaryReader.ReadUInt32();

            switch ((MoveType)newObj.m_type)
            {
                case MoveType.MoveType_Grid:
                    newObj.m_yGrid = binaryReader.ReadInt32();
                    break;
                case MoveType.MoveType_FromTo:
                    newObj.m_yGrid = binaryReader.ReadInt32();
                    newObj.m_xTo = binaryReader.ReadInt32();
                    newObj.m_yTo = binaryReader.ReadInt32();
                    break;
                case MoveType.MoveType_SelectedPiece:
                    break;
            }
            newObj.Length = (int)(binaryReader.BaseStream.Position - startPosition);

            return newObj;
        }

        public void contributeToTreeView(TreeNode node)
        {
            node.Nodes.Add("m_type = " + (MoveType)m_type);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("m_idPlayer = " + Utility.FormatHex(m_idPlayer));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            node.Nodes.Add("m_idPieceToMove = " + m_idPieceToMove);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            switch ((MoveType)m_type)
            {
                case MoveType.MoveType_Grid:
                    node.Nodes.Add("m_yGrid = " + m_yGrid);
                    ContextInfo.AddToList(new ContextInfo { Length = 4 });
                    break;
                case MoveType.MoveType_FromTo:
                    node.Nodes.Add("m_yGrid = " + m_yGrid);
                    ContextInfo.AddToList(new ContextInfo { Length = 4 });
                    node.Nodes.Add("m_xTo = " + m_xTo);
                    ContextInfo.AddToList(new ContextInfo { Length = 4 });
                    node.Nodes.Add("m_yTo = " + m_yTo);
                    ContextInfo.AddToList(new ContextInfo { Length = 4 });
                    break;
                case MoveType.MoveType_SelectedPiece:
                    break;
            }
        }
    }

    public class Recv_OpponentTurn : Message
    {
        public uint i_idGame;
        public int i_iTeam;
        public GameMoveData i_move;

        public static Recv_OpponentTurn read(BinaryReader binaryReader)
        {
            Recv_OpponentTurn newObj = new Recv_OpponentTurn();
            newObj.i_idGame = binaryReader.ReadUInt32();
            newObj.i_iTeam = binaryReader.ReadInt32();
            newObj.i_move = GameMoveData.read(binaryReader);

            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ServerToClientHeader });
            rootNode.Nodes.Add("i_idGame = " + Utility.FormatHex(i_idGame));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            rootNode.Nodes.Add("i_iTeam = " + (ChessTeam)i_iTeam);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            TreeNode moveNode = rootNode.Nodes.Add("i_move = ");
            ContextInfo.AddToList(new ContextInfo { Length = i_move.Length }, updateDataIndex: false);
            i_move.contributeToTreeView(moveNode);
            treeView.Nodes.Add(rootNode);
            rootNode.ExpandAll();
        }
    }

    public class Recv_OpponentStalemateState : Message
    {
        public uint i_idGame;
        public int i_iTeam;
        public int i_fOn;

        public static Recv_OpponentStalemateState read(BinaryReader binaryReader)
        {
            Recv_OpponentStalemateState newObj = new Recv_OpponentStalemateState();
            newObj.i_idGame = binaryReader.ReadUInt32();
            newObj.i_iTeam = binaryReader.ReadInt32();
            newObj.i_fOn = binaryReader.ReadInt32();

            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ServerToClientHeader });
            rootNode.Nodes.Add("i_idGame = " + Utility.FormatHex(i_idGame));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            rootNode.Nodes.Add("i_iTeam = " + (ChessTeam)i_iTeam);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            rootNode.Nodes.Add("i_fOn = " + i_fOn);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
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
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ServerToClientHeader });
            rootNode.Nodes.Add("i_idGame = " + Utility.FormatHex(i_idGame));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            rootNode.Nodes.Add("i_iTeamWinner = " + (ChessTeam)i_iTeamWinner);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            treeView.Nodes.Add(rootNode);
        }
    }
}
