using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum InventoryRequest {
    IR_NONE,
    IR_MERGE,
    IR_SPLIT,
    IR_MOVE,
    IR_PICK_UP,
    IR_PUT_IN_CONTAINER,
    IR_DROP,
    IR_WIELD,
    IR_VIEW_AS_GROUND_CONTAINER,
    IR_GIVE,
    IR_SHOP_EVENT
}

public enum DropItemFlags {
    DROPITEM_FLAGS_NONE = 0,
    DROPITEM_IS_CONTAINER = (1 << 0),
    DROPITEM_IS_VENDOR = (1 << 1),
    DROPITEM_IS_SHORTCUT = (1 << 2),
    DROPITEM_IS_SALVAGE = (1 << 3),
    DROPITEM_IS_ALIAS = DROPITEM_IS_VENDOR | DROPITEM_IS_SHORTCUT | DROPITEM_IS_SALVAGE // 14
}

public enum DestinationType {
    Undef_DestinationType = 0,
    Contain_DestinationType = (1 << 0),
    Wield_DestinationType = (1 << 1),
    Shop_DestinationType = (1 << 2),
    Treasure_DestinationType = (1 << 3),
    HouseBuy_DestinationType = (1 << 4),
    HouseRent_DestinationType = (1 << 5),
    Checkpoint_DestinationType = Contain_DestinationType | Wield_DestinationType | Shop_DestinationType, // 7
    ContainTreasure_DestinationType = Contain_DestinationType | Treasure_DestinationType, // 9
    WieldTreasure_DestinationType = Wield_DestinationType | Treasure_DestinationType, // 10
    ShopTreasure_DestinationType = Shop_DestinationType | Treasure_DestinationType // 12
}

public enum RegenerationType {
    Undef_RegenerationType = 0,
    Destruction_RegenerationType = (1 << 0),
    PickUp_RegenerationType = (1 << 1),
    Death_RegenerationType = (1 << 2)
}

public enum RegenLocationType {
    Undef_RegenLocationType = 0,
    OnTop_RegenLocationType = (1 << 0),
    Scatter_RegenLocationType = (1 << 1),
    Specific_RegenLocationType = (1 << 2),
    Contain_RegenLocationType = (1 << 3),
    Wield_RegenLocationType = (1 << 4),
    Shop_RegenLocationType = (1 << 5),
    Treasure_RegenLocationType = (1 << 6),
    Checkpoint_RegenLocationType = Contain_RegenLocationType | Wield_RegenLocationType | Shop_RegenLocationType, // 56
    OnTopTreasure_RegenLocationType = OnTop_RegenLocationType | Treasure_RegenLocationType, // 65
    ScatterTreasure_RegenLocationType = Scatter_RegenLocationType | Treasure_RegenLocationType, // 66
    SpecificTreasure_RegenLocationType = Specific_RegenLocationType | Treasure_RegenLocationType, // 68
    ContainTreasure_RegenLocationType = Contain_RegenLocationType | Treasure_RegenLocationType, // 72
    WieldTreasure_RegenLocationType = Wield_RegenLocationType | Treasure_RegenLocationType, // 80
    ShopTreasure_RegenLocationType = Shop_RegenLocationType | Treasure_RegenLocationType // 96
}
