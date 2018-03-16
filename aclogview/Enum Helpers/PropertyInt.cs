using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aclogview.Enum_Helpers
{
    class PropertyInt
    {
        public static Dictionary<STypeInt, Type> EnumMapper = new Dictionary<STypeInt, Type>()
        {
            {STypeInt.ATTACK_TYPE_INT, typeof(AttackType)},
            {STypeInt.CURRENT_WIELDED_LOCATION_INT, typeof(INVENTORY_LOC)},
            {STypeInt.PLAYER_KILLER_STATUS_INT, typeof(PKStatusEnum)},
            {STypeInt.EQUIPMENT_SET_ID_INT, typeof(EquipmentSet)},
            {STypeInt.PALETTE_TEMPLATE_INT, typeof(PALETTE_TEMPLATE)},
            {STypeInt.CLOTHING_PRIORITY_INT, typeof(CoverageMask)},
            {STypeInt.LOCATIONS_INT, typeof(INVENTORY_LOC)},
            {STypeInt.UI_EFFECTS_INT, typeof(UI_EFFECT_TYPE)},
            {STypeInt.ATTUNED_INT, typeof(AttunedStatusEnum)},
            {STypeInt.HOOK_TYPE_INT, typeof(HookTypeEnum)},
            {STypeInt.SLAYER_CREATURE_TYPE_INT, typeof(CreatureType)},
            {STypeInt.HERITAGE_SPECIFIC_ARMOR_INT, typeof(HeritageGroup)},
            {STypeInt.WIELD_REQUIREMENTS_INT, typeof(WieldRequirement)},
            {STypeInt.WIELD_REQUIREMENTS_2_INT, typeof(WieldRequirement)},
            {STypeInt.WIELD_REQUIREMENTS_3_INT, typeof(WieldRequirement)},
            {STypeInt.WIELD_REQUIREMENTS_4_INT, typeof(WieldRequirement)},
            {STypeInt.ITEM_XP_STYLE_INT, typeof(ItemXpStyle)},
            {STypeInt.COMBAT_MODE_INT, typeof(COMBAT_MODE)},
            {STypeInt.GENDER_INT, typeof(Gender)},
            {STypeInt.HERITAGE_GROUP_INT, typeof(HeritageGroup)},
            {STypeInt.FACTION1_BITS_INT, typeof(FactionBits)},
            {STypeInt.AETHERIA_BITFIELD_INT, typeof(AetheriaBitfield)},
            {STypeInt.MELEE_MASTERY_INT, typeof(WeaponType)},
            {STypeInt.RANGED_MASTERY_INT, typeof(WeaponType)},
            {STypeInt.SUMMONING_MASTERY_INT, typeof(SummoningMastery)}
        };

        public static void contributeToTreeNode(TreeNode node, CM_Qualities.QualityValues qv)
        {
            if (EnumMapper.ContainsKey((STypeInt)qv.stype))
            {
                var result = EnumMapper.TryGetValue((STypeInt)qv.stype, out var mappedType);
                if (result)
                {
                    contributeType(node, mappedType, qv.val);
                    return;
                }
            }
            node.Nodes.Add("val = " + (int)qv.val);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
        }

        public static void contributeType(TreeNode node, Type type, object value)
        {
            switch (type.Name)
            {
                case ("INVENTORY_LOC"):
                    var mask = Convert.ToUInt32(value);
                    var equipMaskNode = node.Nodes.Add("i_equipMask = " + Utility.FormatHex(mask));
                    ContextInfo.AddToList(new ContextInfo { Length = 4 }, updateDataIndex: false);
                    CM_Inventory.InventoryLocation.contributeToTreeNode(equipMaskNode, mask);
                    break;
                case ("CoverageMask"):
                    CM_Physics.ClothingPriority.contributeToTreeNode(node, Convert.ToUInt32(value));
                    break;
                case ("HookTypeEnum"):
                    TreeNode hookTypeNode = node.Nodes.Add("_hook_type = " + Utility.FormatHex(Convert.ToUInt32(value)));
                    ContextInfo.AddToList(new ContextInfo { Length = 4 }, updateDataIndex: false);
                    CM_Physics.HookType.contributeToTreeNode(hookTypeNode, Convert.ToUInt32(value));
                    break;
                default:
                    // Simple non-bitfield enums
                    var convertedValue = Enum.Format(type, value, "g");
                    node.Nodes.Add("val = " + convertedValue);
                    ContextInfo.AddToList(new ContextInfo { Length = 4 });
                    break;
            }
        }
    }
}
