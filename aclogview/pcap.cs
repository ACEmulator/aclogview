using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class PcapHeader {
    public uint magicNumber;
    public ushort versionMajor;
    public ushort versionMinor;
    public uint thisZone;
    public uint sigFigs;
    public uint snapLen;
    public uint network;

    public static PcapHeader read(BinaryReader binaryReader) {
        PcapHeader newObj = new PcapHeader();
        newObj.magicNumber = binaryReader.ReadUInt32();
        newObj.versionMajor = binaryReader.ReadUInt16();
        newObj.versionMinor = binaryReader.ReadUInt16();
        newObj.thisZone = binaryReader.ReadUInt32();
        newObj.sigFigs = binaryReader.ReadUInt32();
        newObj.snapLen = binaryReader.ReadUInt32();
        newObj.network = binaryReader.ReadUInt32();
        return newObj;
    }
}

class PcapRecordHeader {
    public uint tsSec;
    public uint tsUsec;
    public uint inclLen;
    public uint origLen;

    public static PcapRecordHeader read(BinaryReader binaryReader) {
        PcapRecordHeader newObj = new PcapRecordHeader();
        newObj.tsSec = binaryReader.ReadUInt32();
        newObj.tsUsec = binaryReader.ReadUInt32();
        newObj.inclLen = binaryReader.ReadUInt32();
        newObj.origLen = binaryReader.ReadUInt32();
        return newObj;
    }
}

class PcapngBlockHeader {
    public uint blockType;
    public uint blockTotalLength;
    public uint interfaceID;
    public uint tsHigh;
    public uint tsLow;
    public uint capturedLen;
    public uint packetLen;

    public static PcapngBlockHeader read(BinaryReader binaryReader) {
        PcapngBlockHeader newObj = new PcapngBlockHeader();

        newObj.blockType = binaryReader.ReadUInt32();
        newObj.blockTotalLength = binaryReader.ReadUInt32();

        uint tsLow = 0;
        uint capturedLen = 0;
        if (newObj.blockType == 6) {
            newObj.interfaceID = binaryReader.ReadUInt32();
            newObj.tsHigh = binaryReader.ReadUInt32();
            newObj.tsLow = binaryReader.ReadUInt32();
            newObj.capturedLen = binaryReader.ReadUInt32();
            newObj.packetLen = binaryReader.ReadUInt32();
        } else if (newObj.blockType == 3) {
            newObj.packetLen = binaryReader.ReadUInt32();
            newObj.capturedLen = newObj.blockTotalLength - 16;
        }

        return newObj;
    }
}

class EthernetHeader {
    public byte[] dest;
    public byte[] source;
    public ushort proto;

    public static EthernetHeader read(BinaryReader binaryReader) {
        EthernetHeader newObj = new EthernetHeader();
        newObj.dest = binaryReader.ReadBytes(6);
        newObj.source = binaryReader.ReadBytes(6);
        newObj.proto = binaryReader.ReadUInt16();
        return newObj;
    }
}

class IpAddress {
    public byte[] bytes;

    public static IpAddress read(BinaryReader binaryReader) {
        IpAddress newObj = new IpAddress();
        newObj.bytes = binaryReader.ReadBytes(4);
        return newObj;
    }

    public override string ToString()
    {
        if (bytes.Length == 4)
            return bytes[0] + "." + bytes[1] + "." + bytes[2] + "." + bytes[3];

        return base.ToString();
    }
}

class IpHeader {
    public byte verIhl;
    public byte tos;
    public ushort tLen;
    public ushort identification;
    public ushort flagsFo;
    public byte ttl;
    public byte proto;
    public ushort crc;
    public IpAddress sAddr;
    public IpAddress dAddr;

    public static IpHeader read(BinaryReader binaryReader) {
        IpHeader newObj = new IpHeader();
        newObj.verIhl = binaryReader.ReadByte();
        newObj.tos = binaryReader.ReadByte();
        newObj.tLen = binaryReader.ReadUInt16();
        newObj.identification = binaryReader.ReadUInt16();
        newObj.flagsFo = binaryReader.ReadUInt16();
        newObj.ttl = binaryReader.ReadByte();
        newObj.proto = binaryReader.ReadByte();
        newObj.crc = binaryReader.ReadUInt16();
        newObj.sAddr = IpAddress.read(binaryReader);
        newObj.dAddr = IpAddress.read(binaryReader);
        return newObj;
    }
}

class UdpHeader {
    public ushort sPort;
    public ushort dPort;
    public ushort len;
    public ushort crc;

    public static UdpHeader read(BinaryReader binaryReader) {
        UdpHeader newObj = new UdpHeader();
        newObj.sPort = Util.byteSwapped(binaryReader.ReadUInt16());
        newObj.dPort = Util.byteSwapped(binaryReader.ReadUInt16());
        newObj.len = Util.byteSwapped(binaryReader.ReadUInt16());
        newObj.crc = Util.byteSwapped(binaryReader.ReadUInt16());
        return newObj;
    }
}
