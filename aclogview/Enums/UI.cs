using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum TeleportAnimState {
    TAS_OFF,
    TAS_WORLD_FADE_OUT,
    TAS_TUNNEL_FADE_IN,
    TAS_TUNNEL,
    TAS_TUNNEL_CONTINUE,
    TAS_TUNNEL_FADE_OUT,
    TAS_WORLD_FADE_IN
}

public enum RadarBlipShape {
    Undef_RadarBlipShape,
    Circle_RadarBlipShape,
    Box_RadarBlipShape,
    X_RadarBlipShape,
    Plus_RadarBlipShape,
    Triangle_RadarBlipShape,
    InvertedTriangle_RadarBlipShape,
    XBox_RadarBlipShape,
    Default_RadarBlipShape,
    AllegianceMember_RadarBlipShape,
    FellowshipLeader_RadarBlipShape,
    Fellowship_RadarBlipShape,
    Threat_RadarBlipShape,
    ThreatAllegiance_RadarBlipShape
}

public enum ConfirmationType {
    UNDEF_CONFIRM,
    ALLEGIANCE_SWEAR_CONFIRM,
    ALTER_SKILL_CONFIRM,
    ALTER_ATTRIBUTE_CONFIRM,
    FELLOWSHIP_RECRUIT_CONFIRM,
    CRAFT_INTERACTION_CONFIRM,
    USE_AUGMENTATION_CONFIRM,
    YESNO_CONFIRM
}

public enum PositionState {
    IN_3D_VIEW,
    WIELDED,
    IN_CONTAINER,
    BEING_REMOVED
}

public enum RadarEnum {
    Undef_RadarEnum,
    ShowNever_RadarEnum,
    ShowMovement_RadarEnum,
    ShowAttacking_RadarEnum,
    ShowAlways_RadarEnum
}

public enum UI_EFFECT_TYPE {
    UI_EFFECT_UNDEF = 0,
    UI_EFFECT_MAGICAL = (1 << 0),
    UI_EFFECT_POISONED = (1 << 1),
    UI_EFFECT_BOOST_HEALTH = (1 << 2),
    UI_EFFECT_BOOST_MANA = (1 << 3),
    UI_EFFECT_BOOST_STAMINA = (1 << 4),
    UI_EFFECT_FIRE = (1 << 5),
    UI_EFFECT_LIGHTNING = (1 << 6),
    UI_EFFECT_FROST = (1 << 7),
    UI_EFFECT_ACID = (1 << 8),
    UI_EFFECT_BLUDGEONING = (1 << 9),
    UI_EFFECT_SLASHING = (1 << 10),
    UI_EFFECT_PIERCING = (1 << 11),
    UI_EFFECT_NETHER = (1 << 12)
}

public enum HousePanelTextColor {
    Normal_HousePanelTextColor,
    RentPaid_HousePanelTextColor,
    RentNotPaid_HousePanelTextColor
}

public enum RadarColor : byte
{
    Default = 0x00,
    Blue = 0x01,
    Gold = 0x02,
    White = 0x03,
    Purple = 0x04,
    Red = 0x05,
    Pink = 0x06,
    Green = 0x07,
    Yellow = 0x08,
    Cyan = 0x09,
    BrightGreen = 0x10,
    Admin = Cyan,
    Advocate = Pink,
    Creature = Gold,
    LifeStone = Blue,
    NPC = Yellow,
    PlayerKiller = Red,
    Portal = Purple,
    Sentinel = Cyan,
    Vendor = Yellow,
    Fellowship = BrightGreen,
    FellowshipLeader = BrightGreen,
    PKLite = Pink
}
