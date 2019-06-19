using System;
using System.IO;
using System.Windows.Forms;

namespace aclogview.ACE_Helpers
{
    class ACETeleloc
    {
        public static void CopyACETelelocToClipboard(PacketRecord record)
        {
            try
            {
                using (BinaryReader fragDataReader = new BinaryReader(new MemoryStream(record.data)))
                {
                    var messageCode = fragDataReader.ReadUInt32();
                    if (messageCode == 0xF7B1) // Game Action
                    {
                        var sequence = fragDataReader.ReadUInt32(); // Sequence
                        messageCode = fragDataReader.ReadUInt32(); // Event
                    }
                    string telelocLine;

                    if (messageCode == (int)PacketOpcode.Evt_Physics__CreateObject_ID)
                    {
                        var parsed = CM_Physics.CreateObject.read(fragDataReader);
                        if ((parsed.physicsdesc.bitfield & (uint)CM_Physics.PhysicsDesc.PhysicsDescInfo.POSITION) != 0)
                            telelocLine = GetTelelocString(parsed.physicsdesc.pos);
                        else
                            throw new Exception("This message does not contain a position.");
                    }
                    else if (messageCode == (int)PacketOpcode.Evt_Movement__UpdatePosition_ID)
                    {
                        var parsed = CM_Movement.UpdatePosition.read(fragDataReader);
                        telelocLine = GetTelelocString(parsed.positionPack.position);
                    }
                    else if (messageCode == (int)PacketOpcode.Evt_Movement__MoveToState_ID)
                    {
                        var parsed = CM_Movement.MoveToState.read(fragDataReader);
                        telelocLine = GetTelelocString(parsed.position);
                    }
                    else
                    {
                        throw new Exception();
                    }
                    Clipboard.SetText(telelocLine);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not copy @teleloc line.\n{ex.Message}", "Error:", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        public static string GetTelelocString(Position position)
        {
            return $"@teleloc {position.objcell_id:X8} {position.frame.m_fOrigin.x} {position.frame.m_fOrigin.y} {position.frame.m_fOrigin.z} {position.frame.qw} {position.frame.qx} {position.frame.qy} {position.frame.qz}";
        }

        public static bool MessageContainsPosition(int Opcode)
        {
            switch (Opcode)
            {
                case (int)PacketOpcode.Evt_Physics__CreateObject_ID:
                case (int)PacketOpcode.Evt_Movement__UpdatePosition_ID:
                case (int)PacketOpcode.Evt_Movement__MoveToState_ID:
                    return true;
                default:
                    return false;
            }
        }
    }
}
