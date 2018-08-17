using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

public enum UIElement
{
    Undef,
    Main,
    Floaty1,
    Floaty2,
    Floaty3,
    Floaty4,
    FloatyEnvPanel,
    FloatyExamination,
    FloatyMainChat,
    FloatyPanel,
    FloatyToolbar,
    FloatyVitals,
    FloatyIndicators,
    FloatyPowerBar,
    FloatyRadar,
    FloatyCombatPanel,
    SmartBox,
    FloatySideVitals
}

// Note: This enum was modified to start at -1 so that it
// lines up properly with the values sent from the client.
// gmCGTownPage::ETown
public enum CG_Town
{
    ECG_TOWN_INVALID = -1,
    ECG_TOWN_HOLTBURG,
    ECG_TOWN_SHOUSHI,
    ECG_TOWN_YARAQ,
    ECG_TOWN_SANAMAR,
    ECG_TOWN_OLTHOILAIR, // Guessed name
};

// gmCGProfessionPage::EProfession
public enum CG_Profession
{
    ECG_CUSTOM = 0,
    ECG_BOWHUNTER = 1,
    ECG_SWASHBUCKLER = 2,
    ECG_LIFECASTER = 3,
    ECG_WARMAGE = 4,
    ECG_WAYFARER = 5,
    ECG_SOLDIER = 6
};
