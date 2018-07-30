using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum PowerBarMode {
    PBM_UNDEF,
    PBM_COMBAT,
    PBM_ADVANCED_COMBAT,
    PBM_JUMP,
    PBM_DDD
}

public enum AttackConditions
{
    // Could not find these defined as constants, so came up with appropriate names
    CriticalAugPreventedCritical = (1 << 0), // "Your Critical Protection augmentation allows you to avoid a critical hit!"
    Reckless = (1 << 1), // "Reckless!"
    SneakAttack = (1 << 2), // "Sneak Attack!"
}

