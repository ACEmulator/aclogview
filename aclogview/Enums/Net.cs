using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum charError {
    CHAR_ERROR_UNDEF,
    CHAR_ERROR_LOGON,                                // "Cannot have two accounts logged on at the same time."
    CHAR_ERROR_LOGGED_ON,
    CHAR_ERROR_ACCOUNT_LOGON,                        // "Server could not access your account information. Please try again in a few minutes."
    CHAR_ERROR_SERVER_CRASH,                         // "The server has disconnected. Please try again in a few minutes."
    CHAR_ERROR_LOGOFF,                               // "Server could not log off your character"
    CHAR_ERROR_DELETE,                               // "Server could not delete your character."
    CHAR_ERROR_NO_PREMADE, 
    CHAR_ERROR_ACCOUNT_IN_USE,                       // "The server has disconnected. Please try again in a few minutes." (dupe of 4)
    CHAR_ERROR_ACCOUNT_INVALID,                      // "The account name you specified was not valid."
    CHAR_ERROR_ACCOUNT_DOESNT_EXIST,                 // "The account you specified doesn't exist."
    CHAR_ERROR_ENTER_GAME_GENERIC,                   // "ID_CHAR_ERROR_ENTER_GAME_OLD_CHARACTER"
    CHAR_ERROR_ENTER_GAME_STRESS_ACCOUNT,            // "You cannot enter the game with a stress creating character."
    CHAR_ERROR_ENTER_GAME_CHARACTER_IN_WORLD,        // "One of your characters is still in the world. Please try again in a few minutes."
    CHAR_ERROR_ENTER_GAME_PLAYER_ACCOUNT_MISSING,    // "Server unable to find player account. Please try again later."
    CHAR_ERROR_ENTER_GAME_CHARACTER_NOT_OWNED,       // "You do not own this character."
    CHAR_ERROR_ENTER_GAME_CHARACTER_IN_WORLD_SERVER, // "One of your characters is currently in the world. Please try again later. This is likely an internal server error."
    CHAR_ERROR_ENTER_GAME_OLD_CHARACTER,             // (No text) (17 == 0x11 == ID_CHAR_ERROR_ENTER_GAME_GENERIC??)
    CHAR_ERROR_ENTER_GAME_CORRUPT_CHARACTER,         // "This character's data has been corrupted. Please delete it and create a new character."
    CHAR_ERROR_ENTER_GAME_START_SERVER_DOWN,         // "This character's starting server is experiencing difficulties. Please try again in a few minutes."
    CHAR_ERROR_ENTER_GAME_COULDNT_PLACE_CHARACTER,   // "This character couldn't be placed in the world right now. Please try again in a few minutes."
    CHAR_ERROR_LOGON_SERVER_FULL,                    // "Sorry, but the Asheron's Call server is full currently. Please try again later."
    CHAR_ERROR_CHARACTER_IS_BOOTED,
    CHAR_ERROR_ENTER_GAME_CHARACTER_LOCKED,          // "A save of this character is still in progress, please try again later."
    CHAR_ERROR_SUBSCRIPTION_EXPIRED,                 // "Your subscription to this game has expired."
    CHAR_ERROR_NUM_ERRORS
}

public enum ServerSwitchType {
    ssWorldSwitch,
    ssLogonSwitch
}

public enum ReceiverState {
    UNDEF_STATE,
    NAK_STATE,
    NO_NAK_STATE,
    NO_STATE
}

public enum ConnectionState {
    cs_Disconnected,
    cs_AwaitingWorldAuth,
    cs_AuthSent,
    cs_ConnectionRequestSent,
    cs_ConnectionRequestAcked,
    cs_Connected,
    cs_DisconnectReceived,
    cs_DisconnectSent
}

public enum ICMDCommandEnum {
    cmdNOP = 1,
    cmdEchoRequest = 1902465605,
    cmdEchoReply = 1819300421
}

public enum SEND_CODE {
    UNDEF_SEND,
    OK_SEND,
    NET_FAIL_SEND,
    FLOW_FAIL_SEND
}

public enum DEFAULT_AUTHFLAGS {
    AUTHFLAGS_ENABLECRYPTO = (1 << 0),
    AUTHFLAGS_ADMINACCTOVERRIDE = (1 << 1),
    AUTHFLAGS_EXTRADATA = (1 << 2),
    AUTHFLAGS_LASTDEFAULT = AUTHFLAGS_EXTRADATA
}

namespace ClientNet {
    public enum WAIT_RESULT {
        UNDEF_WAIT_RESULT,
        DONE_WAIT_RESULT,
        FAILED_WAIT_RESULT,
        ROUTED_WAIT_RESULT,
        NO_LOGON_SERVER_WAIT_RESULT,
        EXPIRED_ZONE_TICKET_RESULT
    }
}

namespace PacketInfo {
    public enum Protocol {
        fe_tcp,
        be_tcp,
        fe_udp
    }
}

public enum NetStatus {
    Net_Initializing,
    Net_LoginAuthenticating,
    Net_LoginConnecting,
    Net_LoginConnected,
    Net_LoginConnectionError,
    Net_WorldConnectionError
}

public enum NetBlobProcessedStatus {
    NETBLOB_UNDEF_STATUS,
    NETBLOB_PROCESSED_OK,
    NETBLOB_OLD_INSTANCE,
    NETBLOB_ERROR,
    NETBLOB_QUEUED
}
