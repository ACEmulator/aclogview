using System.Collections.Generic;

namespace aclogview
{
    class PacketRecord
    {
        public int index;

        public IpHeader ipHeader;
        public uint ServerPort;

        public bool isSend;

        // !isPcapng
        public uint tsSec;
        public uint tsUsec;

        // isPcapng
        public uint tsHigh;
        public uint tsLow;


        public string packetHeadersStr;
        public string packetTypeStr;
        public int optionalHeadersLen;
        public List<PacketOpcode> opcodes = new List<PacketOpcode>();
        public string extraInfo;

        public byte[] data;
        public List<BlobFrag> frags = new List<BlobFrag>();

        public uint Seq;
        public uint Queue;
        public uint Iteration;
    }
}
