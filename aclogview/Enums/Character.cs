using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum PlayerOption {
    AutoRepeatAttack_PlayerOption,
    IgnoreAllegianceRequests_PlayerOption,
    IgnoreFellowshipRequests_PlayerOption,
    IgnoreTradeRequests_PlayerOption,
    DisableMostWeatherEffects_PlayerOption,
    PersistentAtDay_PlayerOption,
    AllowGive_PlayerOption,
    ViewCombatTarget_PlayerOption,
    ShowTooltips_PlayerOption,
    UseDeception_PlayerOption,
    ToggleRun_PlayerOption,
    StayInChatMode_PlayerOption,
    AdvancedCombatUI_PlayerOption,
    AutoTarget_PlayerOption,
    VividTargetingIndicator_PlayerOption,
    FellowshipShareXP_PlayerOption,
    AcceptLootPermits_PlayerOption,
    FellowshipShareLoot_PlayerOption,
    FellowshipAutoAcceptRequests_PlayerOption,
    SideBySideVitals_PlayerOption,
    CoordinatesOnRadar_PlayerOption,
    SpellDuration_PlayerOption,
    DisableHouseRestrictionEffects_PlayerOption,
    DragItemOnPlayerOpensSecureTrade_PlayerOption,
    DisplayAllegianceLogonNotifications_PlayerOption,
    UseChargeAttack_PlayerOption,
    UseCraftSuccessDialog_PlayerOption,
    HearAllegianceChat_PlayerOption,
    DisplayDateOfBirth_PlayerOption,
    DisplayAge_PlayerOption,
    DisplayChessRank_PlayerOption,
    DisplayFishingSkill_PlayerOption,
    DisplayNumberDeaths_PlayerOption,
    DisplayTimeStamps_PlayerOption,
    SalvageMultiple_PlayerOption,
    HearGeneralChat_PlayerOption,
    HearTradeChat_PlayerOption,
    HearLFGChat_PlayerOption,
    HearRoleplayChat_PlayerOption,
    AppearOffline_PlayerOption,
    DisplayNumberCharacterTitles_PlayerOption,
    MainPackPreferred_PlayerOption,
    LeadMissileTargets_PlayerOption,
    UseFastMissiles_PlayerOption,
    FilterLanguage_PlayerOption,
    ConfirmVolatileRareUse_PlayerOption,
    HearSocietyChat_PlayerOption,
    ShowHelm_PlayerOption,
    DisableDistanceFog_PlayerOption,
    UseMouseTurning_PlayerOption,
    ShowCloak_PlayerOption,
    LockUI_PlayerOption,
    TotalNumberOfPlayerOptions_PlayerOption,
    Invalid_PlayerOption = -1,
}

public enum CharacterOption {
    Undef_CharacterOption = 0,
    // NOTE: Skip 1
    AutoRepeatAttack_CharacterOption = (1 << 1),
    IgnoreAllegianceRequests_CharacterOption = (1 << 2),
    IgnoreFellowshipRequests_CharacterOption = (1 << 3),
    // NOTE: Skip 2
    AllowGive_CharacterOption = (1 << 6),
    ViewCombatTarget_CharacterOption = (1 << 7),
    ShowTooltips_CharacterOption = (1 << 8),
    UseDeception_CharacterOption = (1 << 9),
    ToggleRun_CharacterOption = (1 << 10),
    StayInChatMode_CharacterOption = (1 << 11),
    AdvancedCombatUI_CharacterOption = (1 << 12),
    AutoTarget_CharacterOption = (1 << 13),
    // NOTE: Skip 1
    VividTargetingIndicator_CharacterOption = (1 << 15),
    DisableMostWeatherEffects_CharacterOption = (1 << 16),
    IgnoreTradeRequests_CharacterOption = (1 << 17),
    FellowshipShareXP_CharacterOption = (1 << 18),
    AcceptLootPermits_CharacterOption = (1 << 19),
    FellowshipShareLoot_CharacterOption = (1 << 20),
    SideBySideVitals_CharacterOption = (1 << 21),
    CoordinatesOnRadar_CharacterOption = (1 << 22),
    SpellDuration_CharacterOption = (1 << 23),
    // NOTE: Skip 1
    DisableHouseRestrictionEffects_CharacterOption = (1 << 25),
    DragItemOnPlayerOpensSecureTrade_CharacterOption = (1 << 26),
    DisplayAllegianceLogonNotifications_CharacterOption = (1 << 27),
    UseChargeAttack_CharacterOption = (1 << 29),
    AutoAcceptFellowRequest_CharacterOption = (1 << 29),
    HearAllegianceChat_CharacterOption = (1 << 30),
    UseCraftSuccessDialog_CharacterOption = (1 << 31),
    Default_CharacterOption = AutoRepeatAttack_CharacterOption | IgnoreFellowshipRequests_CharacterOption | AllowGive_CharacterOption | ShowTooltips_CharacterOption | ToggleRun_CharacterOption | AutoTarget_CharacterOption | VividTargetingIndicator_CharacterOption | ToggleRun_CharacterOption | ToggleRun_CharacterOption | FellowshipShareXP_CharacterOption | CoordinatesOnRadar_CharacterOption | SpellDuration_CharacterOption | UseChargeAttack_CharacterOption | HearAllegianceChat_CharacterOption // 1355064650
}

public enum CharacterOptions2 {
    Undef_CharacterOptions2 = 0,
    PersistentAtDay_CharacterOptions2 = (1 << 0),
    DisplayDateOfBirth_CharacterOptions2 = (1 << 1),
    DisplayChessRank_CharacterOptions2 = (1 << 2),
    DisplayFishingSkill_CharacterOptions2 = (1 << 3),
    DisplayNumberDeaths_CharacterOptions2 = (1 << 4),
    DisplayAge_CharacterOptions2 = (1 << 5),
    TimeStamp_CharacterOptions2 = (1 << 6),
    SalvageMultiple_CharacterOptions2 = (1 << 7),
    HearGeneralChat_CharacterOptions2 = (1 << 8),
    HearTradeChat_CharacterOptions2 = (1 << 9),
    HearLFGChat_CharacterOptions2 = (1 << 10),
    HearRoleplayChat_CharacterOptions2 = (1 << 11),
    AppearOffline_CharacterOptions2 = (1 << 12),
    DisplayNumberCharacterTitles_CharacterOptions2 = (1 << 13),
    MainPackPreferred_CharacterOptions2 = (1 << 14),
    LeadMissileTargets_CharacterOptions2 = (1 << 15),
    UseFastMissiles_CharacterOptions2 = (1 << 16),
    FilterLanguage_CharacterOptions2 = (1 << 17),
    ConfirmVolatileRareUse_CharacterOptions2 = (1 << 18),
    HearSocietyChat_CharacterOptions2 = (1 << 19),
    ShowHelm_CharacterOptions2 = (1 << 20),
    DisableDistanceFog_CharacterOptions2 = (1 << 21),
    UseMouseTurning_CharacterOptions2 = (1 << 22),
    ShowCloak_CharacterOptions2 = (1 << 23),
    LockUI_CharacterOptions2 = (1 << 24),
    Default_CharacterOptions2 = HearGeneralChat_CharacterOptions2 | HearTradeChat_CharacterOptions2 | HearLFGChat_CharacterOptions2 | LeadMissileTargets_CharacterOptions2 | ConfirmVolatileRareUse_CharacterOptions2 | ShowHelm_CharacterOptions2 | ShowCloak_CharacterOptions2 // 9733888
}

public enum SpellbookFilter {
    Undef_SpellbookFilter = 0,
    Creature_SpellbookFilter = (1 << 1),
    Item_SpellbookFilter = (1 << 2),
    Life_SpellbookFilter = (1 << 3),
    War_SpellbookFilter = (1 << 4),
    Level_1_SpellbookFilter = (1 << 5),
    Level_2_SpellbookFilter = (1 << 6),
    Level_3_SpellbookFilter = (1 << 7),
    Level_4_SpellbookFilter = (1 << 8),
    Level_5_SpellbookFilter = (1 << 9),
    Level_6_SpellbookFilter = (1 << 10),
    Level_7_SpellbookFilter = (1 << 11),
    Level_8_SpellbookFilter = (1 << 12),
    Level_9_SpellbookFilter = (1 << 13),
    Void_SpellbookFilter = (1 << 14),
    Default_SpellbookFilter = (1 << 15)
}

namespace ACCharGenResult {
    public enum PackVersion__guessedname {
        CurrentPackVersion = 1,
        OldestValidPackVersion = 1
    }

    public enum VersionInfo__guessedname {
        InvalidVersion = 0,
        SizeOfVersion = 4,
        SizeOfChecksum = 4
    }
}

public enum CG_VERIFICATION_RESPONSE {
    UNDEF_CG_VERIFICATION_RESPONSE,
    CG_VERIFICATION_RESPONSE_OK,
    CG_VERIFICATION_RESPONSE_PENDING,
    CG_VERIFICATION_RESPONSE_NAME_IN_USE,
    CG_VERIFICATION_RESPONSE_NAME_BANNED,
    CG_VERIFICATION_RESPONSE_CORRUPT,
    CG_VERIFICATION_RESPONSE_DATABASE_DOWN,
    CG_VERIFICATION_RESPONSE_ADMIN_PRIVILEGE_DENIED,
    NUM_CG_VERIFICATION_RESPONSES
}

public enum HeritageGroup {
    Invalid_HeritageGroup,
    Aluvian_HeritageGroup,
    Gharundim_HeritageGroup,
    Sho_HeritageGroup,
    Viamontian_HeritageGroup,
    Shadowbound_HeritageGroup,
    Gearknight_HeritageGroup,
    Tumerok_HeritageGroup,
    Lugian_HeritageGroup,
    Empyrean_HeritageGroup,
    Penumbraen_HeritageGroup,
    Undead_HeritageGroup,
    Olthoi_HeritageGroup,
    OlthoiAcid_HeritageGroup
}

public enum Gender
{
    Male,
    Female
}
