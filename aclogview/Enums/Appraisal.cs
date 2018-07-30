using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum ObjectSelectStatus {
    Invalid_OSS,
    ObjectOnscreen_OSS,
    ObjectOffscreen_OSS,
    ObjectNotFound_OSS
}

public enum ObjectInfoEnum {
    DEFAULT_OI = 0,
    CONTACT_OI = (1 << 0),
    ON_WALKABLE_OI = (1 << 1),
    IS_VIEWER_OI = (1 << 2),
    PATH_CLIPPED_OI = (1 << 3),
    FREE_ROTATE_OI = (1 << 4),
    // NOTE: Skip 1
    PERFECT_CLIP_OI = (1 << 6),
    IS_IMPENETRABLE = (1 << 7),
    IS_PLAYER = (1 << 8),
    EDGE_SLIDE = (1 << 9),
    IGNORE_CREATURES = (1 << 10),
    IS_PK = (1 << 11),
    IS_PKLITE = (1 << 12)
}

public enum AppraisalLongDescDecorations {
    LDDecoration_PrependWorkmanship = (1 << 0),
    LDDecoration_PrependMaterial = (1 << 1),
    LDDecoration_AppendGemInfo = (1 << 2)
}

// Gleaned from client code. See CharExamineUI::SetAppraiseInfo()
public enum FactionBits
{
    No_Faction,
    Celestial_Hand,
    Eldrytch_Web,
    // skip 1
    Radiant_Blood = 4
}
