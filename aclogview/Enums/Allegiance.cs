using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum AllegianceVersion {
    Undef_AllegianceVersion,
    SpokespersonAdded_AllegianceVersion,
    PoolsAdded_AllegianceVersion,
    MotdAdded_AllegianceVersion,
    ChatRoomIDAdded_AllegianceVersion,
    BannedCharactersAdded_AllegianceVersion,
    MultipleAllegianceOfficersAdded_AllegianceVersion,
    Bindstones_AllegianceVersion,
    AllegianceName_AllegianceVersion,
    OfficersTitlesAdded_AllegianceVersion,
    LockedState_AllegianceVersion,
    ApprovedVassal_AllegianceVersion,
    Newest_AllegianceVersion = ApprovedVassal_AllegianceVersion
}

public enum AllegianceIndex {
    Undef_AllegianceIndex = 0x0,
    LoggedIn_AllegianceIndex = 0x1,
    Update_AllegianceIndex = 0x2,
    HasAllegianceAge_AllegianceIndex = 0x4,
    HasPackedLevel_AllegianceIndex = 0x8,
    MayPassupExperience_AllegianceIndex = 0x10
}

public enum eAllegianceHouseAction {
    Undef_AllegianceHouseAction,
    CheckStatus_AllegianceHouseAction,
    GuestOpen_AllegianceHouseAction,
    GuestClose_AllegianceHouseAction,
    StorageOpen_AllegianceHouseAction,
    StorageClose_AllegianceHouseAction,
    NumberOfActions_AllegianceHouseAction = StorageClose_AllegianceHouseAction
}

public enum eAllegianceOfficerLevel {
    Undef_AllegianceOfficerLevel,
    Speaker_AllegianceOfficerLevel,
    Seneschal_AllegianceOfficerLevel,
    Castellan_AllegianceOfficerLevel,
    NumberOfOfficerTitles_AllegianceOfficerLevel = Castellan_AllegianceOfficerLevel
}

public enum eAllegianceLockAction {
    Undef_AllegianceLockAction,
    OffLock_AllegianceLockAction,
    OnLock_AllegianceLockAction,
    ToggleLock_AllegianceLockAction,
    CheckLock_AllegianceLockAction,
    CheckApproved_AllegianceLockAction,
    ClearApproved_AllegianceLockAction,
    NumberOfActions_AllegianceLockAction = ClearApproved_AllegianceLockAction
}
