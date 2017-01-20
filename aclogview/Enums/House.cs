﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum HouseOp {
    Undef_HouseOp,
    Buy_House,
    Rent_House
}

public enum HouseBitmask {
    Undef_HouseBitmask = 0,
    Active_HouseBitmask = (1 << 0),
    RequiresMonarch_HouseBitmask = (1 << 1)
}

public enum HARBitmask {
    Undef_HARBitmask = 0,
    OpenHouse_HARBitmask = (1 << 0),
    AllegianceGuests_HARBitmask = (1 << 1),
    AllegianceStorage_HARBitmask = (1 << 2)
}
