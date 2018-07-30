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
