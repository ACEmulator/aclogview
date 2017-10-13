using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum eChatTypes {
    eTextTypeDefault,
    eTextTypeAllChannels,
    eTextTypeSpeech,
    eTextTypeSpeechDirect,
    eTextTypeSpeechDirectSend,
    eTextTypeSystemSvent,
    eTextTypeCombat,
    eTextTypeMagic,
    eTextTypeChannel,
    eTextTypeChannelCend,
    eTextTypeSocialChannel,
    eTextTypeSocialChannelSend,
    eTextTypeEmote,
    eTextTypeAdvancement,
    eTextTypeAbuseChannel,
    eTextTypeHelpChannel,
    eTextTypeAppraisalChannel,
    eTextTypeMagicCastingChannel,
    eTextTypeAllegienceChannel,
    eTextTypeFellowshipChannel,
    eTextTypeWorld_broadcast,
    eTextTypeCombatEnemy,
    eTextTypeCombatSelf,
    eTextTypeRecall,
    eTextTypeCraft,
    eTextTypeTotalNumChannels
}

public enum SquelchTypes {
    AllChannels         = 1,
    Speech              = 2,
    SpeechDirect        = 3, // @tell
    Combat              = 6,
    Magic               = 7,
    Emote               = 12,
    AppraisalChannel    = 16,
    MagicCastingChannel = 17,
    AllegienceChannel   = 18,
    FellowshipChannel   = 19,
    CombatEnemy         = 21,
    CombatSelf          = 22,
    Recall              = 23,
    Craft               = 24,
    Salvaging           = 25
}

public enum SquelchMasks {
    Speech              = 0x00000004,
    SpeechDirect        = 0x00000008, // @tell
    Combat              = 0x00000040,
    Magic               = 0x00000080,
    Emote               = 0x00001000,
    AppraisalChannel    = 0x00010000,
    MagicCastingChannel = 0x00020000,
    AllegienceChannel   = 0x00040000,
    FellowshipChannel   = 0x00080000,
    CombatEnemy         = 0x00200000,
    CombatSelf          = 0x00400000,
    Recall              = 0x00800000,
    Craft               = 0x01000000,
    Salvaging           = 0x02000000,
    AllChannels         = unchecked((int)0xFFFFFFFF)
}

public enum ChatTypeEnum {
    Undef_ChatTypeEnum,
    Allegiance_ChatTypeEnum,
    General_ChatTypeEnum,
    Trade_ChatTypeEnum,
    LFG_ChatTypeEnum,
    Roleplay_ChatTypeEnum,
    Society_ChatTypeEnum,
    SocietyCelHan_ChatTypeEnum,
    SocietyEldWeb_ChatTypeEnum,
    SocietyRadBlo_ChatTypeEnum,
    Olthoi_ChatTypeEnum
}

public enum GroupChatType
{
    /// <summary>
    /// Unknown
    /// </summary>
    /// Included for completeness since it is listed in the client
    Unknown = 0x00000000,

    /// <summary>
    /// @abuse - Abuse Channel
    /// </summary>
    TellAbuse = 0x00000001,

    /// <summary>
    /// @admin - Admin Channel (@ad)
    /// </summary>
    TellAdmin = 0x00000002,

    /// <summary>
    /// @audit - Audit Channel (@au)
    /// This channel was used to echo copies of enforcement commands (such as: ban, gag, boot) to all other online admins
    /// </summary>
    TellAudit = 0x00000004,

    /// <summary>
    /// @av1 - Advocate Channel (@advocate) (@advocate1)
    /// </summary>
    TellAdvocate = 0x00000008,

    /// <summary>
    /// @av2 - Advocate2 Channel (@advocate2)
    /// </summary>
    TellAdvocate2 = 0x00000010,

    /// <summary>
    /// @av3 - Advocate3 Channel (@advocate3)
    /// </summary>
    TellAdvocate3 = 0x000000020,

    /// <summary>
    /// @sent - Sentinel Channel (@sentinel)
    /// </summary>
    TellSentinel = 0x000000200,

    /// <summary>
    /// @[command name tbd] - Help Channel
    /// </summary>
    TellHelp = 0x000000400,

    /// <summary>
    /// @f - Tell Fellowship
    /// </summary>
    TellFellowship = 0x00000800,

    /// <summary>
    /// @v - Tell Vassals
    /// </summary>
    TellVassals = 0x00001000,

    /// <summary>
    /// @p - Tell Patron
    /// </summary>
    TellPatron = 0x00002000,

    /// <summary>
    /// @m - Tell Monarch
    /// </summary>
    TellMonarch = 0x00004000,

    /// <summary>
    /// @c - Tell Co-Vassals
    /// </summary>
    TellCoVassals = 0x01000000,

    /// <summary>
    /// @allegiance broadcast - Tell All Allegiance Members
    /// </summary>
    AllegianceBroadcast = 0x02000000,

    /// <summary>
    /// Player is now the leader of this fellowship.
    /// </summary>
    FellowshipBroadcast = 0x04000000,

    /// <summary>
    /// Celestial Hand Society
    /// </summary>
    CelestialHandBroadcast = 0x08000000,

    /// <summary>
    /// Eldrytch Web Society
    /// </summary>
    EldrytchWebBroadcast = 0x10000000,

    /// <summary>
    /// Radiant Blood Society
    /// </summary>
    RadiantBloodBroadcast = 0x20000000,

    /// <summary>
    /// Olthoi
    /// </summary>
    OlthoiBroadcast = 0x40000000
}