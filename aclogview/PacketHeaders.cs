using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ProtoHeader {
    public uint seqID_;       // Sequence number
    public uint header_;      // Bitmask for which optional headers are present
    public uint checksum_;    // CRC
    public ushort recID_;     // Recipient ID - maps to the receivers_ (aka packet stream? can have different ip per receiver)
    public ushort interval_;  // time since last recieved a packet
    public ushort datalen_;   // Length of data in the packet, excluding this header
    public ushort iteration_; // the table, another bucketing mechanism, logical grouping

    public static ProtoHeader read(BinaryReader binaryReader) {
        ProtoHeader newObj = new ProtoHeader();
        newObj.seqID_ = binaryReader.ReadUInt32();
        newObj.header_ = binaryReader.ReadUInt32();
        newObj.checksum_ = binaryReader.ReadUInt32();
        newObj.recID_ = binaryReader.ReadUInt16();
        newObj.interval_ = binaryReader.ReadUInt16();
        newObj.datalen_ = binaryReader.ReadUInt16();
        newObj.iteration_ = binaryReader.ReadUInt16();
        return newObj;
    }
}

// See RegisterOptionalHeaderType for mask, ::CreateFromData and ::CreateFromStream for flags
public class COptionalHeader {
    public enum OptionalHeaderFlags {
        ohfDisposable = (1 << 0),
        ohfExclusive = (1 << 1),
        ohfNotConn = (1 << 2),
        ohfTimeSensitive = (1 << 3),
        ohfShouldPiggyBack = (1 << 4),
        ohfHighPriority = (1 << 5),
        ohfCountsAsTouch = (1 << 6),
        // NOTE: Skip 22
        ohfEncrypted = (1 << 29),
        ohfSigned = (1 << 30)
    }
    
    //public uint m_dwMask;
    //public uint m_Flags;
}

public class CServerSwitchStructHeader : COptionalHeader {
    public static uint mask = 0x100;
    public static uint flags = (uint)(OptionalHeaderFlags.ohfHighPriority | OptionalHeaderFlags.ohfCountsAsTouch); // 96
}

public class CServerSwitchStruct {
    uint dwSeqNo;
    uint Type;

    public static CServerSwitchStruct read(BinaryReader binaryReader) {
        CServerSwitchStruct newObj = new CServerSwitchStruct();
        newObj.dwSeqNo = binaryReader.ReadUInt32();
        newObj.Type = binaryReader.ReadUInt32();
        return newObj;
    }
}

public class LogonServerAddrHeader : COptionalHeader {
    public static uint mask = 0x200;
    public static uint flags = (uint)(OptionalHeaderFlags.ohfDisposable | OptionalHeaderFlags.ohfExclusive | OptionalHeaderFlags.ohfNotConn); // 7
}

public class sockaddr_in {
    public short sin_family;
    public ushort sin_port;
    public byte[] sin_addr = new byte[4];
    public byte[] sin_zero = new byte[8];

    public static sockaddr_in read(BinaryReader binaryReader) {
        sockaddr_in newObj = new sockaddr_in();
        newObj.sin_family = binaryReader.ReadInt16();
        newObj.sin_port = binaryReader.ReadUInt16();
        newObj.sin_addr = binaryReader.ReadBytes(4);
        newObj.sin_zero = binaryReader.ReadBytes(8);
        return newObj;
    }
}

public class CEmptyHeader1 : COptionalHeader {
    public static uint mask = 0x400u;
    public static uint flags = (uint)(OptionalHeaderFlags.ohfDisposable | OptionalHeaderFlags.ohfExclusive | OptionalHeaderFlags.ohfNotConn); // 7
}

public class CReferralStructHeader : COptionalHeader {
    public static uint mask = 0x800;
    public static uint flags = (uint)(OptionalHeaderFlags.ohfExclusive | OptionalHeaderFlags.ohfHighPriority | OptionalHeaderFlags.ohfCountsAsTouch | OptionalHeaderFlags.ohfSigned); // 1073741922
}

public class CReferralStruct {
    public ulong qwCookie;
    public sockaddr_in Addr;
    public ushort idServer;
    public ushort padding;
    public uint unk; // Missing info, just to make it the proper 32 bytes...

    public static CReferralStruct read(BinaryReader binaryReader) {
        CReferralStruct newObj = new CReferralStruct();
        newObj.qwCookie = binaryReader.ReadUInt64();
        newObj.Addr = sockaddr_in.read(binaryReader);
        newObj.idServer = binaryReader.ReadUInt16();
        newObj.padding = binaryReader.ReadUInt16();
        newObj.unk = binaryReader.ReadUInt32();
        return newObj;
    }
}

public class CSeqIDListHeader : COptionalHeader {
    public uint[] seqIDs;

    public static CSeqIDListHeader read(BinaryReader binaryReader, ref string extraInfo) {
        CSeqIDListHeader newObj = new CSeqIDListHeader();
        uint num = binaryReader.ReadUInt32();
        var arrayByteRealLen = binaryReader.BaseStream.Length - binaryReader.BaseStream.Position;
        var diff = arrayByteRealLen - (num * 4);
        if (diff < 0)
        {
            uint seqShort = (uint)Math.Ceiling(Math.Abs((double)((double)diff / (double)4))); //number of sequence IDs short
            long extrBytes = arrayByteRealLen - ((num - seqShort) * 4);
            extraInfo += $"NAK SeqID list truncated from {num} to {num - seqShort} IDs with {extrBytes} of {arrayByteRealLen} bytes remaining, ( {num * 4} bytes expected ); ";
            num = num - seqShort;
        }
        newObj.seqIDs = new uint[num];
        for (uint i = 0; i < num; ++i)
        {
            newObj.seqIDs[i] = binaryReader.ReadUInt32();
        }
        return newObj;
    }
}

// HandleNak
public class NakHeader : CSeqIDListHeader {
    public static uint mask = 0x1000;
    public static uint flags = (uint)(OptionalHeaderFlags.ohfDisposable | OptionalHeaderFlags.ohfHighPriority); // 33
}

// HandleEmptyAck
public class EmptyAckHeader : CSeqIDListHeader {
    public static uint mask = 0x2000;
    public static uint flags = (uint)(OptionalHeaderFlags.ohfDisposable | OptionalHeaderFlags.ohfHighPriority); // 33
}

// HandlePak
public class PakHeader : COptionalHeader {
    public static uint mask = 0x4000;
    public static uint flags = (uint)(OptionalHeaderFlags.ohfDisposable); // 1

    uint num;

    public static PakHeader read(BinaryReader binaryReader) {
        PakHeader newObj = new PakHeader();
        newObj.num = binaryReader.ReadUInt32();
        return newObj;
    }
}

public class CEmptyHeader2 : COptionalHeader {
    public static uint mask = 0x8000;
    public static uint flags = (uint)(OptionalHeaderFlags.ohfDisposable | OptionalHeaderFlags.ohfExclusive); // 3
}

public class CLogonHeader : COptionalHeader {
    public static uint mask = 0x10000;
    public static uint flags = (uint)(OptionalHeaderFlags.ohfDisposable | OptionalHeaderFlags.ohfExclusive | OptionalHeaderFlags.ohfNotConn); // 7

    public class HandshakeWireData {
        public PStringChar ClientVersion;
        public uint cbAuthData;

        public static HandshakeWireData read(BinaryReader binaryReader) {
            HandshakeWireData newObj = new HandshakeWireData();
            newObj.ClientVersion = PStringChar.read(binaryReader);
            newObj.cbAuthData = binaryReader.ReadUInt32();
            return newObj;
        }
    }
}

public class ULongHeader : COptionalHeader {
    public static uint mask = 0x20000;
    public static uint flags = (uint)(OptionalHeaderFlags.ohfDisposable | OptionalHeaderFlags.ohfExclusive | OptionalHeaderFlags.ohfNotConn); // 7

    ulong m_Prim;

    public static ULongHeader read(BinaryReader binaryReader) {
        ULongHeader newObj = new ULongHeader();
        newObj.m_Prim = binaryReader.ReadUInt64();
        return newObj;
    }
}

public class CConnectHeader : COptionalHeader {
    public static uint mask = 0x40000;
    public static uint flags = (uint)(OptionalHeaderFlags.ohfDisposable | OptionalHeaderFlags.ohfExclusive | OptionalHeaderFlags.ohfNotConn | OptionalHeaderFlags.ohfEncrypted); // 536870919

    public class HandshakeWireData {
        public double ServerTime;
        public ulong qwCookie;
        public uint NetID;
        public uint OutgoingSeed;
        public uint IncomingSeed;
        public uint unk; // Missing info, just to make it the proper 32 bytes...

        public static HandshakeWireData read(BinaryReader binaryReader) {
            HandshakeWireData newObj = new HandshakeWireData();
            newObj.ServerTime = binaryReader.ReadDouble();
            newObj.qwCookie = binaryReader.ReadUInt64();
            newObj.NetID = binaryReader.ReadUInt32();
            newObj.OutgoingSeed = binaryReader.ReadUInt32();
            newObj.IncomingSeed = binaryReader.ReadUInt32();
            newObj.unk = binaryReader.ReadUInt32();
            return newObj;
        }
    }
}

public class ULongHeader2 : COptionalHeader {
    public static uint mask = 0x80000;
    public static uint flags = (uint)(OptionalHeaderFlags.ohfDisposable | OptionalHeaderFlags.ohfExclusive | OptionalHeaderFlags.ohfNotConn | OptionalHeaderFlags.ohfEncrypted); // 536870919

    ulong m_Prim;

    public static ULongHeader2 read(BinaryReader binaryReader) {
        ULongHeader2 newObj = new ULongHeader2();
        newObj.m_Prim = binaryReader.ReadUInt64();
        return newObj;
    }
}

public class NetErrorHeader : COptionalHeader {
    public static uint mask = 0x100000;
    public static uint flags = (uint)(OptionalHeaderFlags.ohfDisposable | OptionalHeaderFlags.ohfExclusive | OptionalHeaderFlags.ohfNotConn); // 7
}

public class NetError {
    public uint m_stringID;
    public uint m_tableID; // NOTE: Presence of this field depends on the NetError subclass??

    public static NetError read(BinaryReader binaryReader) {
        NetError newObj = new NetError();
        newObj.m_stringID = binaryReader.ReadUInt32();
        newObj.m_tableID = binaryReader.ReadUInt32();
        return newObj;
    }
}

public class NetErrorHeader_cs_DisconnectReceived : COptionalHeader {
    public static uint mask = 0x200000;
    public static uint flags = (uint)(OptionalHeaderFlags.ohfExclusive); // 2
}

public class CICMDCommandStructHeader : COptionalHeader {
    public static uint mask = 0x400000;
    public static uint flags = (uint)(OptionalHeaderFlags.ohfDisposable | OptionalHeaderFlags.ohfExclusive | OptionalHeaderFlags.ohfNotConn); // 7
}

public class CICMDCommandStruct {
    public uint Cmd;
    public uint Param;

    public static CICMDCommandStruct read(BinaryReader binaryReader) {
        CICMDCommandStruct newObj = new CICMDCommandStruct();
        newObj.Cmd = binaryReader.ReadUInt32();
        newObj.Param = binaryReader.ReadUInt32();
        return newObj;
    }
}

public class CTimeSyncHeader : COptionalHeader {
    public static uint mask = 0x1000000;
    public static uint flags = (uint)(OptionalHeaderFlags.ohfTimeSensitive | OptionalHeaderFlags.ohfShouldPiggyBack); // 24

    public ulong m_time;

    public static CTimeSyncHeader read(BinaryReader binaryReader) {
        CTimeSyncHeader newObj = new CTimeSyncHeader();
        newObj.m_time = binaryReader.ReadUInt64();
        return newObj;
    }
}

public class CEchoRequestHeader : COptionalHeader {
    public static uint mask = 0x2000000;
    public static uint flags = (uint)(OptionalHeaderFlags.ohfTimeSensitive | OptionalHeaderFlags.ohfShouldPiggyBack); // 24

    public float m_LocalTime;

    public static CEchoRequestHeader read(BinaryReader binaryReader) {
        CEchoRequestHeader newObj = new CEchoRequestHeader();
        newObj.m_LocalTime = binaryReader.ReadSingle();
        return newObj;
    }
}

public class CEchoResponseHeader : COptionalHeader {
    public static uint mask = 0x4000000;
    public static uint flags = (uint)(OptionalHeaderFlags.ohfTimeSensitive | OptionalHeaderFlags.ohfShouldPiggyBack); // 24

    public class CEchoResponseHeaderWireData {
        public float LocalTime;
        public float HoldingTime;

        public static CEchoResponseHeaderWireData read(BinaryReader binaryReader) {
            CEchoResponseHeaderWireData newObj = new CEchoResponseHeaderWireData();
            newObj.LocalTime = binaryReader.ReadSingle();
            newObj.HoldingTime = binaryReader.ReadSingle();
            return newObj;
        }
    }
}

public class CFlowStructHeader : COptionalHeader {
    public static uint mask = 0x8000000;
    public static uint flags = (uint)(OptionalHeaderFlags.ohfShouldPiggyBack); // 16
}

public class CFlowStruct {
    public uint cbDataRecvd;
    public ushort interval;

    public static CFlowStruct read(BinaryReader binaryReader) {
        CFlowStruct newObj = new CFlowStruct();
        newObj.cbDataRecvd = binaryReader.ReadUInt32();
        newObj.interval = binaryReader.ReadUInt16();
        return newObj;
    }
}

public class BlobFragHeader_t {
    public ulong blobID;
    public ushort numFrags;
    public ushort blobFragSize;
    public ushort blobNum;
    public ushort queueID;

    public static BlobFragHeader_t read(BinaryReader binaryReader) {
        BlobFragHeader_t newObj = new BlobFragHeader_t();
        newObj.blobID = binaryReader.ReadUInt64();
        newObj.numFrags = binaryReader.ReadUInt16();
        newObj.blobFragSize = binaryReader.ReadUInt16();
        newObj.blobNum = binaryReader.ReadUInt16();
        newObj.queueID = binaryReader.ReadUInt16();
        return newObj;
    }
}

class OrderHdr {
    public uint stamp; // Ordering number

    public static OrderHdr read(BinaryReader binaryReader) {
        OrderHdr newObj = new OrderHdr();
        newObj.stamp = binaryReader.ReadUInt32();
        return newObj;
    }
}

class WOrderHdr {
    public uint id;    // GUID of object to apply to
    public uint stamp; // Ordering number

    public static WOrderHdr read(BinaryReader binaryReader) {
        WOrderHdr newObj = new WOrderHdr();
        newObj.id = binaryReader.ReadUInt32();
        newObj.stamp = binaryReader.ReadUInt32();
        return newObj;
    }
}
