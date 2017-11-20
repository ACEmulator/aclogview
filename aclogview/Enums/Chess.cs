using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum MoveType {
    MoveType_Invalid,
    MoveType_Pass,
    MoveType_Resign,
    MoveType_Stalemate,
    MoveType_Grid,
    MoveType_FromTo,
    MoveType_SelectedPiece
}

public enum ChessMoveResult
{
    NoMoveResult = 0,
    OKMoveToEmptySquare = 1,
    OKMoveToOccupiedSquare = 2,
    OKMoveEnPassant = 3,
    OKMoveMask = 1023,
    OKMoveCHECK = 1024,
    OKMoveCHECKMATE = 2048,
    OKMovePromotion = 4096,
    OKMoveToEmptySquareCHECK = 1025,
    OKMoveToOccupiedSquareCHECK = 1026,
    OKMoveEnPassantCHECK = 1027,
    OKMovePromotionCHECK = 5120,
    OKMoveToEmptySquareCHECKMATE = 2049,
    OKMoveToOccupiedSquareCHECKMATE = 2050,
    OKMoveEnPassantCHECKMATE = 2051,
    OKMovePromotionCHECKMATE = 6144,
    BadMoveInvalidCommand = -1,
    BadMoveNotPlaying = -2,
    BadMoveNotYourTurn = -3,
    BadMoveDirection = -100,
    BadMoveDistance = -101,
    BadMoveNoPiece = -102,
    BadMoveNotYours = -103,
    BadMoveDestination = -104,
    BadMoveWouldClobber = -105,
    BadMoveSelfCheck = -106,
    BadMoveWouldCollide = -107,
    BadMoveCantCastleOutOfCheck = -108,
    BadMoveCantCastleThroughCheck = -109,
    BadMoveCantCastleAfterMoving = -110,
    BadMoveInvalidBoardState = -111,
}

public enum ChessPieceType
{
    Empty = 0,
    Pawn = 1,
    Rook = 2,
    Castle = 2,
    Knight = 3,
    Bishop = 4,
    Queen = 5,
    King = 6,
    nPieceTypes = 7
}

public enum GameBoardGrid_Unknown
{
    WHITE_PAWN_INDEX=0,
    WHITE_BISHOP_INDEX=1,
    WHITE_KNIGHT_INDEX=2,
    WHITE_ROOK_INDEX=3,
    WHITE_QUEEN_INDEX=4,
    WHITE_KING_INDEX=5,
    BLACK_PAWN_INDEX=6,
    BLACK_BISHOP_INDEX=7,
    BLACK_KNIGHT_INDEX=8,
    BLACK_ROOK_INDEX=9,
    BLACK_QUEEN_INDEX=10,
    BLACK_KING_INDEX=11
}