using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum EnchantmentVersion {
    Undef_EnchantmentVersion,
    SpellSetID_EnchantmentVersion,
    Newest_EnchantmentVersion = SpellSetID_EnchantmentVersion
}

public enum SpellBanks {
    SPELLCAST_BANK_1,
    SPELLCAST_BANK_2,
    SPELLCAST_BANK_3,
    SPELLCAST_BANK_4,
    SPELLCAST_BANK_5,
    SPELLCAST_BANK_6,
    SPELLCAST_BANK_7,
    SPELLCAST_BANK_8,
    NUM_SPELLCAST_BANKS
}

public enum SpellComponentType {
    Undef_SpellComponentType,
    Power_SpellComponentType,
    Action_SpellComponentType,
    ConceptPrefix_SpellComponentType,
    ConceptSuffix_SpellComponentType,
    Target_SpellComponentType,
    Accent_SpellComponentType,
    Pea_SpellComponentType
}

public enum SpellComponentCategory {
    Scarab_SpellComponentCategory,
    Herb_SpellComponentCategory,
    PowderedGem_SpellComponentCategory,
    AlchemicalSubstance_SpellComponentCategory,
    Talisman_SpellComponentCategory,
    Taper_SpellComponentCategory,
    Pea_SpellComponentCategory,
    Num_SpellComponentCategories,
    Undef_SpellComponentCategory
}

public enum SpellSetID {
    Invalid_SpellSetID,
    Test_SpellSetID,
    Test_EquipmentSet_SpellSetID,
    UNKNOWN__GUESSEDNAME, // NOTE: Missing 1
    CarraidasBenediction_EquipmentSet_SpellSetID,
    NobleRelic_EquipmentSet_SpellSetID,
    AncientRelic_EquipmentSet_SpellSetID,
    AlduressaRelic_EquipmentSet_SpellSetID,
    Ninja_EquipmentSet_SpellSetID,
    EmpyreanRings_EquipmentSet_SpellSetID,
    ArmMindHeart_EquipmentSet_SpellSetID,
    ArmorPerfectLight_EquipmentSet_SpellSetID,
    ArmorPerfectLight2_EquipmentSet_SpellSetID,
    Soldiers_EquipmentSet_SpellSetID,
    Adepts_EquipmentSet_SpellSetID,
    Archers_EquipmentSet_SpellSetID,
    Defenders_EquipmentSet_SpellSetID,
    Tinkers_EquipmentSet_SpellSetID,
    Crafters_EquipmentSet_SpellSetID,
    Hearty_EquipmentSet_SpellSetID,
    Dexterous_EquipmentSet_SpellSetID,
    Wise_EquipmentSet_SpellSetID,
    Swift_EquipmentSet_SpellSetID,
    Hardened_EquipmentSet_SpellSetID,
    Reinforced_EquipmentSet_SpellSetID,
    Interlocking_EquipmentSet_SpellSetID,
    Flameproof_EquipmentSet_SpellSetID,
    Acidproof_EquipmentSet_SpellSetID,
    Coldproof_EquipmentSet_SpellSetID,
    Lightningproof_EquipmentSet_SpellSetID,
    SocietyArmor_EquipmentSet_SpellSetID,
    ColosseumClothing_EquipmentSet_SpellSetID,
    GraveyardClothing_EquipmentSet_SpellSetID,
    OlthoiClothing_EquipmentSet_SpellSetID,
    NoobieArmor_EquipmentSet_SpellSetID,
    AetheriaDefense_EquipmentSet_SpellSetID,
    AetheriaDestruction_EquipmentSet_SpellSetID,
    AetheriaFury_EquipmentSet_SpellSetID,
    AetheriaGrowth_EquipmentSet_SpellSetID,
    AetheriaVigor_EquipmentSet_SpellSetID,
    RareDamageResistance_EquipmentSet_SpellSetID,
    RareDamageBoost_EquipmentSet_SpellSetID,
    OlthoiArmorDRed_Set_SpellSetID,
    OlthoiArmorCRat_Set_SpellSetID,
    OlthoiArmorCRed_Set_SpellSetID,
    OlthoiArmorDRat_Set_SpellSetID,
    AlduressaRelicUpgrade_EquipmentSet_SpellSetID,
    AncientRelicUpgrade_EquipmentSet_SpellSetID,
    NobleRelicUpgrade_EquipmentSet_SpellSetID,
    CloakAlchemy_EquipmentSet_SpellSetID,
    CloakArcaneLore_EquipmentSet_SpellSetID,
    CloakArmorTinkering_EquipmentSet_SpellSetID,
    CloakAssessPerson_EquipmentSet_SpellSetID,
    CloakAxe_EquipmentSet_SpellSetID,
    CloakBow_EquipmentSet_SpellSetID,
    CloakCooking_EquipmentSet_SpellSetID,
    CloakCreatureEnchantment_EquipmentSet_SpellSetID,
    CloakCrossbow_EquipmentSet_SpellSetID,
    CloakDagger_EquipmentSet_SpellSetID,
    CloakDeception_EquipmentSet_SpellSetID,
    CloakFletching_EquipmentSet_SpellSetID,
    CloakHealing_EquipmentSet_SpellSetID,
    CloakItemEnchantment_EquipmentSet_SpellSetID,
    CloakItemTinkering_EquipmentSet_SpellSetID,
    CloakLeadership_EquipmentSet_SpellSetID,
    CloakLifeMagic_EquipmentSet_SpellSetID,
    CloakLoyalty_EquipmentSet_SpellSetID,
    CloakMace_EquipmentSet_SpellSetID,
    CloakMagicDefense_EquipmentSet_SpellSetID,
    CloakMagicItemTinkering_EquipmentSet_SpellSetID,
    CloakManaConversion_EquipmentSet_SpellSetID,
    CloakMeleeDefense_EquipmentSet_SpellSetID,
    CloakMissileDefense_EquipmentSet_SpellSetID,
    CloakSalvaging_EquipmentSet_SpellSetID,
    CloakSpear_EquipmentSet_SpellSetID,
    CloakStaff_EquipmentSet_SpellSetID,
    CloakSword_EquipmentSet_SpellSetID,
    CloakThrownWeapon_EquipmentSet_SpellSetID,
    CloakTwoHandedCombat_EquipmentSet_SpellSetID,
    CloakUnarmedCombat_EquipmentSet_SpellSetID,
    CloakVoidMagic_EquipmentSet_SpellSetID,
    CloakWarMagic_EquipmentSet_SpellSetID,
    CloakWeaponTinkering_EquipmentSet_SpellSetID,
    CloakAssessCreature_EquipmentSet_SpellSetID,
    CloakDirtyFighting_EquipmentSet_SpellSetID,
    CloakDualWield_EquipmentSet_SpellSetID,
    CloakRecklessness_EquipmentSet_SpellSetID,
    CloakShield_EquipmentSet_SpellSetID,
    CloakSneakAttack_EquipmentSet_SpellSetID,
    Ninja_New_EquipmentSet_SpellSetID,
    CloakSummoning_EquipmentSet_SpellSetID,
    // Gleaned SpellSet IDs
    ShroudedSoul_EquipmentSet_SpellSetID, // Shrouded Soul Shadow Armor
    DarkenedMind_EquipmentSet_SpellSetID, // Darkened Mind Shadow Armor
    CloudedSpirit_EquipmentSet_SpellSetID, // Clouded Spirit Shadow Armor
    // Missing 94-119
    EnhancedShroudedSoul_EquipmentSet_SpellSetID = 120, // Enhanced Shrouded Soul Shadow Armor
    // Missing 121-125
    EnhancedCloudedSpirit_EquipmentSet_SpellSetID = 126, // Enhanced Clouded Spirit Shadow Armor
    // Missing 127-129
    ShimmeringShadow_EquipmentSet_SpellSetID = 130, // Shimmering Shadow Prismatic Shadow Armor
    BrownSocietyLocket_EquipmentSet_SpellSetID = 131, // Brown Society Locket
    YellowSocietyLocket_EquipmentSet_SpellSetID = 132, // Yellow Society Locket
    RedSocietyBand_EquipmentSet_SpellSetID = 133, // Red Society Band
    GreenSocietyBand_EquipmentSet_SpellSetID = 134, // Green Society Band
    PurpleSocietyBand_EquipmentSet_SpellSetID = 135, // Purple Society Band
    BlueSocietyBand_EquipmentSet_SpellSetID = 136, // Blue Society Band
    GauntletGarb_EquipmentSet_SpellSetID = 137, // Gauntlet Garb Set
    UNKNOWN_138_EquipmentSet_SpellSetID = 138, // Unknown examples: Dark Frost Compound Crossbow, Dark Acid Bow, Dark Acid Crossbow, Spell Bound Crossbow
    UNKNOWN_139_EquipmentSet_SpellSetID = 139, // Unknown examples: Gharu'ndim Wand, Soul Bound Staff
    UNKNOWN_140_EquipmentSet_SpellSetID = 140 // Unknown examples: Nodachi, Purified Mouryou Katana, Frost Shashqa (Possibly cleaving two handed weapons?)
}
