using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum COMBAT_USE {
    COMBAT_USE_NONE,
    COMBAT_USE_MELEE,
    COMBAT_USE_MISSILE,
    COMBAT_USE_AMMO,
    COMBAT_USE_SHIELD,
    COMBAT_USE_TWO_HANDED
}

public enum AMMO_TYPE {
    AMMO_NONE,
    AMMO_ARROW,
    AMMO_BOLT,
    AMMO_ATLATL,
    AMMO_ARROW_CRYSTAL,
    AMMO_BOLT_CRYSTAL,
    AMMO_ATLATL_CRYSTAL,
    AMMO_ARROW_CHORIZITE,
    AMMO_BOLT_CHORIZITE,
    AMMO_ATLATL_CHORIZITE
}

public enum ITEM_TYPE {
    TYPE_UNDEF = 0,
    //TYPE_SELF = 0, This is a duplicate of TYPE_UNDEF
    TYPE_MELEE_WEAPON = (1 << 0),
    TYPE_ARMOR = (1 << 1),
    TYPE_CLOTHING = (1 << 2),
    TYPE_VESTEMENTS = 6,
    TYPE_JEWELRY = (1 << 3),
    TYPE_CREATURE = (1 << 4),
    TYPE_FOOD = (1 << 5),
    TYPE_MONEY = (1 << 6),
    TYPE_MISC = (1 << 7),
    TYPE_MISSILE_WEAPON = (1 << 8),
    TYPE_WEAPON = 0x101,
    TYPE_CONTAINER = (1 << 9),
    TYPE_LOCKABLE_MAGIC_TARGET = 0x280,
    TYPE_USELESS = (1 << 10),
    TYPE_GEM = (1 << 11),
    TYPE_SPELL_COMPONENTS = (1 << 12),
    TYPE_WRITABLE = (1 << 13),
    TYPE_KEY = (1 << 14),
    TYPE_CASTER = (1 << 15),
    TYPE_WEAPON_OR_CASTER = 0x8101,
    TYPE_REDIRECTABLE_ITEM_ENCHANTMENT_TARGET = 0x8107,
    TYPE_PORTAL = (1 << 16),
    TYPE_LOCKABLE = (1 << 17),
    TYPE_PROMISSORY_NOTE = (1 << 18),
    TYPE_MANASTONE = (1 << 19),
    TYPE_ITEM_ENCHANTABLE_TARGET = 0x88B8F,
    TYPE_SERVICE = (1 << 20),
    TYPE_MAGIC_WIELDABLE = (1 << 21),
    TYPE_ITEM = 0x2DFBEF,
    TYPE_CRAFT_COOKING_BASE = (1 << 22),
    TYPE_VENDOR_GROCER = 0x446220,
    TYPE_CRAFT_ALCHEMY_BASE = (1 << 23),
    TYPE_CRAFT_FLETCHING_BASE = (1 << 24),
    // Skip 1
    TYPE_CRAFT_ALCHEMY_INTERMEDIATE = (1 << 26),
    TYPE_CRAFT_FLETCHING_INTERMEDIATE = (1 << 27),
    TYPE_LIFESTONE = (1 << 28),
    TYPE_PORTAL_MAGIC_TARGET = 0x10010000,
    TYPE_TINKERING_TOOL = (1 << 29),
    TYPE_TINKERING_MATERIAL = (1 << 30),
    TYPE_VENDOR_SHOPKEEP = 0x480467A7,
    TYPE_GAMEBOARD = (1 << 31),
}

public enum ITEM_USEABLE {
    USEABLE_UNDEF = 0,
    USEABLE_NO = (1 << 0),
    USEABLE_SELF = (1 << 1),
    USEABLE_WIELDED = (1 << 2),
    USEABLE_CONTAINED = (1 << 3),
    USEABLE_VIEWED = (1 << 4),
    USEABLE_REMOTE = (1 << 5),
    USEABLE_NEVER_WALK = (1 << 6),
    USEABLE_OBJSELF = (1 << 7),
    USEABLE_CONTAINED_VIEWED = 24,
    USEABLE_CONTAINED_VIEWED_REMOTE = 56,
    USEABLE_CONTAINED_VIEWED_REMOTE_NEVER_WALK = 120,
    USEABLE_VIEWED_REMOTE = 48,
    USEABLE_VIEWED_REMOTE_NEVER_WALK = 112,
    USEABLE_REMOTE_NEVER_WALK = 96,
    USEABLE_SOURCE_WIELDED_TARGET_WIELDED = 262148,
    USEABLE_SOURCE_WIELDED_TARGET_CONTAINED = 524292,
    USEABLE_SOURCE_WIELDED_TARGET_VIEWED = 1048580,
    USEABLE_SOURCE_WIELDED_TARGET_REMOTE = 2097156,
    USEABLE_SOURCE_WIELDED_TARGET_REMOTE_NEVER_WALK = 6291460,
    USEABLE_SOURCE_CONTAINED_TARGET_WIELDED = 262152,
    USEABLE_SOURCE_CONTAINED_TARGET_CONTAINED = 524296,
    USEABLE_SOURCE_CONTAINED_TARGET_OBJSELF_OR_CONTAINED = 8912904,
    USEABLE_SOURCE_CONTAINED_TARGET_SELF_OR_CONTAINED = 655368,
    USEABLE_SOURCE_CONTAINED_TARGET_VIEWED = 1048584,
    USEABLE_SOURCE_CONTAINED_TARGET_REMOTE = 2097160,
    USEABLE_SOURCE_CONTAINED_TARGET_REMOTE_NEVER_WALK = 6291464,
    USEABLE_SOURCE_CONTAINED_TARGET_REMOTE_OR_SELF = 2228232,
    USEABLE_SOURCE_VIEWED_TARGET_WIELDED = 262160,
    USEABLE_SOURCE_VIEWED_TARGET_CONTAINED = 524304,
    USEABLE_SOURCE_VIEWED_TARGET_VIEWED = 1048592,
    USEABLE_SOURCE_VIEWED_TARGET_REMOTE = 2097168,
    USEABLE_SOURCE_REMOTE_TARGET_WIELDED = 262176,
    USEABLE_SOURCE_REMOTE_TARGET_CONTAINED = 524320,
    USEABLE_SOURCE_REMOTE_TARGET_VIEWED = 1048608,
    USEABLE_SOURCE_REMOTE_TARGET_REMOTE = 2097184,
    USEABLE_SOURCE_REMOTE_TARGET_REMOTE_NEVER_WALK = 6291488,
    USEABLE_SOURCE_MASK = 65535,
    USEABLE_TARGET_MASK = -65536,
}

public enum ImbuedEffectType {
    Undef_ImbuedEffectType = 0,
    CriticalStrike_ImbuedEffectType = (1 << 0),
    CripplingBlow_ImbuedEffectType = (1 << 1),
    ArmorRending_ImbuedEffectType = (1 << 2),
    SlashRending_ImbuedEffectType = (1 << 3),
    PierceRending_ImbuedEffectType = (1 << 4),
    BludgeonRending_ImbuedEffectType = (1 << 5),
    AcidRending_ImbuedEffectType = (1 << 6),
    ColdRending_ImbuedEffectType = (1 << 7),
    ElectricRending_ImbuedEffectType = (1 << 8),
    FireRending_ImbuedEffectType = (1 << 9),
    MeleeDefense_ImbuedEffectType = (1 << 10),
    MissileDefense_ImbuedEffectType = (1 << 11),
    MagicDefense_ImbuedEffectType = (1 << 12),
    Spellbook_ImbuedEffectType = (1 << 13),
    NetherRending_ImbuedEffectType = (1 << 14),
    IgnoreSomeMagicProjectileDamage_ImbuedEffectType = (1 << 29),
    AlwaysCritical_ImbuedEffectType = (1 << 30),
    IgnoreAllArmor_ImbuedEffectType = (1 << 31)
}

public enum WeaponType {
    Undef_WeaponType,
    Unarmed_WeaponType,
    Sword_WeaponType,
    Axe_WeaponType,
    Mace_WeaponType,
    Spear_WeaponType,
    Dagger_WeaponType,
    Staff_WeaponType,
    Bow_WeaponType,
    Crossbow_WeaponType,
    Thrown_WeaponType,
    TwoHanded_WeaponType,
    Magic_WeaponType
}

public enum Enchantment_BFIndex {
    BF_INSCRIBABLE = (1 << 0),
    BF_HEALER = (1 << 1),
    BF_FOOD = (1 << 2),
    BF_LOCKPICK = (1 << 2)
}

public enum AttunedStatusEnum {
    Normal_AttunedStatus,
    Attuned_AttunedStatus,
    Sticky_AttunedStatus
}

public enum BondedStatusEnum {
    Destroy_BondedStatus = -2,
    Slippery_BondedStatus = -1,
    Normal_BondedStatus = 0,
    Bonded_BondedStatus = 1,
    Sticky_BondedStatus = 2
}

public enum EnchantmentTypeEnum {
    Undef_EnchantmentType                  = 0,
    Attribute_EnchantmentType              = (1 << 0),
    SecondAtt_EnchantmentType              = (1 << 1),
    Int_EnchantmentType                    = (1 << 2),
    Float_EnchantmentType                  = (1 << 3),
    Skill_EnchantmentType                  = (1 << 4),
    BodyDamageValue_EnchantmentType        = (1 << 5),
    BodyDamageVariance_EnchantmentType     = (1 << 6),
    BodyArmorValue_EnchantmentType         = (1 << 7), // Natural Armor
    // NOTE: Skip 4
    SingleStat_EnchantmentType             = (1 << 12),
    MultipleStat_EnchantmentType           = (1 << 13),
    Multiplicative_EnchantmentType         = (1 << 14),
    Additive_EnchantmentType               = (1 << 15),
    AttackSkills_EnchantmentType           = (1 << 16),
    DefenseSkills_EnchantmentType          = (1 << 17),
    // NOTE: Skip 3
    Multiplicative_Degrade_EnchantmentType = (1 << 20),
    Additive_Degrade_EnchantmentType       = (1 << 21),
    // NOTE: Skip 1
    Vitae_EnchantmentType                  = (1 << 23),
    Cooldown_EnchantmentType               = (1 << 24),
    Beneficial_EnchantmentType             = (1 << 25),
    StatTypes_EnchantmentType              = 255
}

public enum EquipmentSet {
    ShadowMelee_EquipmentSet = 91,
    ShadowMagic_EquipmentSet,
    ShadowMissile_EquipmentSet,
    ShadowMeleeAcidMinor_EquipmentSet,
    ShadowMeleeElectricMinor_EquipmentSet,
    ShadowMeleeFireMinor_EquipmentSet,
    ShadowMeleeFrostMinor_EquipmentSet,
    ShadowMagicAcidMinor_EquipmentSet,
    ShadowMagicElectricMinor_EquipmentSet,
    ShadowMagicFireMinor_EquipmentSet,
    ShadowMagicFrostMinor_EquipmentSet,
    ShadowMissileAcidMinor_EquipmentSet,
    ShadowMissileElectricMinor_EquipmentSet,
    ShadowMissileFireMinor_EquipmentSet,
    ShadowMissileFrostMinor_EquipmentSet,
    ShadowMeleeAcidMajor_EquipmentSet,
    ShadowMeleeElectricMajor_EquipmentSet,
    ShadowMeleeFireMajor_EquipmentSet,
    ShadowMeleeFrostMajor_EquipmentSet,
    ShadowMagicAcidMajor_EquipmentSet,
    ShadowMagicElectricMajor_EquipmentSet,
    ShadowMagicFireMajor_EquipmentSet,
    ShadowMagicFrostMajor_EquipmentSet,
    ShadowMissileAcidMajor_EquipmentSet,
    ShadowMissileElectricMajor_EquipmentSet,
    ShadowMissileFireMajor_EquipmentSet,
    ShadowMissileFrostMajor_EquipmentSet,
    ShadowMeleeAcidBlackfire_EquipmentSet,
    ShadowMeleeElectricBlackfire_EquipmentSet,
    ShadowMeleeFireBlackfire_EquipmentSet,
    ShadowMeleeFrostBlackfire_EquipmentSet,
    ShadowMagicAcidBlackfire_EquipmentSet,
    ShadowMagicElectricBlackfire_EquipmentSet,
    ShadowMagicFireBlackfire_EquipmentSet,
    ShadowMagicFrostBlackfire_EquipmentSet,
    ShadowMissileAcidBlackfire_EquipmentSet,
    ShadowMissileElectricBlackfire_EquipmentSet,
    ShadowMissileFireBlackfire_EquipmentSet,
    ShadowMissileFrostBlackfire_EquipmentSet,
    ShadowPrismatic_EquipmentSet
}

public enum CoverageMask : uint
{
    UnderwearUpperLegs = 0x00000002,
    UnderwearLowerLegs = 0x00000004,
    UnderwearChest     = 0x00000008,
    UnderwearAbdomen   = 0x00000010,
    UnderwearUpperArms = 0x00000020,
    UnderwearLowerArms = 0x00000040,
    OuterwearUpperLegs = 0x00000100,
    OuterwearLowerLegs = 0x00000200,
    OuterwearChest     = 0x00000400,
    OuterwearAbdomen   = 0x00000800,
    OuterwearUpperArms = 0x00001000,
    OuterwearLowerArms = 0x00002000,
    Head               = 0x00004000,
    Hands              = 0x00008000,
    Feet               = 0x00010000,
}

// This enum is not found in the client.
public enum ContainerProperties
{
    None,
    Container,
    Foci
}
