using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class CM_Fellowship : MessageProcessor {

    public override bool acceptMessageData(BinaryReader messageDataReader, TreeView outputTreeView) {
        bool handled = true;

        PacketOpcode opcode = Util.readOpcode(messageDataReader);
        switch (opcode) {
            // TODO: PacketOpcode.Evt_Fellowship__Create_ID = 162,
            // TODO: PacketOpcode.Evt_Fellowship__Quit_ID = 163, // Bidirectional
            // TODO: PacketOpcode.Evt_Fellowship__Dismiss_ID = 164, // Bidirectional
            // TODO: PacketOpcode.Evt_Fellowship__Recruit_ID = 165,,
            // TODO: PacketOpcode.Evt_Fellowship__UpdateRequest_ID = 166,
            // TODO: PacketOpcode.RECV_QUIT_FELLOW_EVENT = 167,
            // TODO: PacketOpcode.RECV_FELLOWSHIP_UPDATE_EVENT = 175,
            // TODO: PacketOpcode.RECV_UPDATE_FELLOW_EVENT = 176,
            // TODO: PacketOpcode.RECV_DISMISS_FELLOW_EVENT = 177,
            // TODO: PacketOpcode.RECV_LOGOFF_FELLOW_EVENT = 178,
            // TODO: PacketOpcode.RECV_DISBAND_FELLOWSHIP_EVENT = 179,
            // TODO: PacketOpcode.Evt_Fellowship__Appraise_ID = 202, 
            // TODO: PacketOpcode.Evt_Fellowship__FellowUpdateDone_ID = 457,
            // TODO: PacketOpcode.Evt_Fellowship__FellowStatsDone_ID = 458,
            // TODO: PacketOpcode.Evt_Fellowship__ChangeFellowOpeness_ID = 657,
            // TODO: PacketOpcode.Evt_Fellowship__FullUpdate_ID = 702,
            // TODO: PacketOpcode.Evt_Fellowship__Disband_ID = 703,
            // TODO: PacketOpcode.Evt_Fellowship__UpdateFellow_ID = 704,
            default: {
                    handled = false;
                    break;
                }
        }

        return handled;
    }


}
