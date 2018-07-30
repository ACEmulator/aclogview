using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsObjHook {
    public enum PhysicsHookType {
        SCALING,
        TRANSLUCENCY,
        PART_TRANSLUCENCY,
        LUMINOSITY,
        DIFFUSION,
        PART_LUMINOSITY,
        PART_DIFFUSION,
        CALL_PES
    }
}

public enum HookTypeEnum {
    Undef_HookTypeEnum = 0,
    Floor_HookTypeEnum = (1 << 0),
    Wall_HookTypeEnum = (1 << 1),
    Ceiling_HookTypeEnum = (1 << 2),
    Yard_HookTypeEnum = (1 << 3),
    Roof_HookTypeEnum = (1 << 4)
}

public enum PhysicsTimeStamp {
    POSITION_TS,
    MOVEMENT_TS,
    STATE_TS,
    VECTOR_TS,
    TELEPORT_TS,
    SERVER_CONTROLLED_MOVE_TS,
    FORCE_POSITION_TS,
    OBJDESC_TS,
    INSTANCE_TS,
    NUM_PHYSICS_TS
}

public enum SetPositionError {
    OK_SPE,
    GENERAL_FAILURE_SPE,
    NO_VALID_POSITION_SPE,
    NO_CELL_SPE,
    COLLIDED_SPE,
    INVALID_ARGUMENTS = 256
}

public enum SetPositionFlag {
    PLACEMENT_SPF = (1 << 0),
    TELEPORT_SPF = (1 << 1),
    RESTORE_SPF = (1 << 2),
    // NOTE: Skip 1
    SLIDE_SPF = (1 << 4),
    DONOTCREATECELLS_SPF = (1 << 5),
    // NOTE: Skip 2
    SCATTER_SPF = (1 << 8),
    RANDOMSCATTER_SPF = (1 << 9),
    LINE_SPF = (1 << 10),
    // NOTE: Skip 1
    SEND_POSITION_EVENT_SPF = (1 << 12)
}

public enum ObjCollisionProfile_Bitfield {
    Undef_ECPB = 0,
    Creature_OCPB = (1 << 0),
    Player_OCPB = (1 << 1),
    Attackable_OCPB = (1 << 2),
    Missile_OCPB = (1 << 3),
    Contact_OCPB = (1 << 4),
    MyContact_OCPB = (1 << 5),
    Door_OCPB = (1 << 6),
    Cloaked_OCPB = (1 << 7)
}

public enum EnvCollisionProfile_Bitfield {
    Undef_ECPB = 0,
    MyContact_ECPB = (1 << 0)
}

public enum TransientState {
    CONTACT_TS = (1 << 0),
    ON_WALKABLE_TS = (1 << 1),
    SLIDING_TS = (1 << 2),
    WATER_CONTACT_TS = (1 << 3),
    STATIONARY_FALL_TS = (1 << 4),
    STATIONARY_STOP_TS = (1 << 5),
    STATIONARY_STUCK_TS = (1 << 6),
    ACTIVE_TS = (1 << 7),
    CHECK_ETHEREAL_TS = (1 << 8)
}

public enum TransitionState {
    INVALID_TS,
    OK_TS,
    COLLIDED_TS,
    ADJUSTED_TS,
    SLID_TS
}
