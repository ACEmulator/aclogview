using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace aclogview
{
    static class PCapReader
    {
        public static List<PacketRecord> LoadPcap(string fileName, bool asMessages, ref bool abort)
        {
            using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (BinaryReader binaryReader = new BinaryReader(fileStream))
                {
                    uint magicNumber = binaryReader.ReadUInt32();

                    binaryReader.BaseStream.Position = 0;

                    if (magicNumber == 0xA1B2C3D4 || magicNumber == 0xD4C3B2A1)
                        return loadPcapPacketRecords(binaryReader, asMessages, ref abort);

                    return loadPcapngPacketRecords(binaryReader, asMessages, ref abort);
                }
            }
        }

        private class FragNumComparer : IComparer<BlobFrag>
        {
            int IComparer<BlobFrag>.Compare(BlobFrag a, BlobFrag b)
            {
                if (a.memberHeader_.blobNum > b.memberHeader_.blobNum)
                    return 1;
                if (a.memberHeader_.blobNum < b.memberHeader_.blobNum)
                    return -1;
                else
                    return 0;
            }
        }

        private static bool addPacketIfFinished(List<PacketRecord> finishedRecords, PacketRecord record)
        {
            record.frags.Sort(new FragNumComparer());

            // Make sure all fragments are present
            if (record.frags.Count < record.frags[0].memberHeader_.numFrags
                || record.frags[0].memberHeader_.blobNum != 0
                || record.frags[record.frags.Count - 1].memberHeader_.blobNum != record.frags[0].memberHeader_.numFrags - 1)
            {
                return false;
            }

            record.index = finishedRecords.Count;

            // Remove duplicate fragments
            int index = 0;
            while (index < record.frags.Count - 1)
            {
                if (record.frags[index].memberHeader_.blobNum == record.frags[index + 1].memberHeader_.blobNum)
                    record.frags.RemoveAt(index);
                else
                    index++;
            }

            int totalMessageSize = 0;
            foreach (BlobFrag frag in record.frags)
            {
                totalMessageSize += frag.dat_.Length;
            }

            record.data = new byte[totalMessageSize];
            int offset = 0;
            foreach (BlobFrag frag in record.frags)
            {
                Buffer.BlockCopy(frag.dat_, 0, record.data, offset, frag.dat_.Length);
                offset += frag.dat_.Length;
            }

            finishedRecords.Add(record);

            return true;
        }

        private static PcapRecordHeader readPcapRecordHeader(BinaryReader binaryReader, int curPacket)
        {
            if (binaryReader.BaseStream.Length - binaryReader.BaseStream.Position < 16)
            {
                throw new InvalidDataException("Stream cut short (packet " + curPacket + "), stopping read: " + (binaryReader.BaseStream.Length - binaryReader.BaseStream.Position));
            }

            PcapRecordHeader recordHeader = PcapRecordHeader.read(binaryReader);

            if (recordHeader.inclLen > 50000)
            {
                throw new InvalidDataException("Enormous packet (packet " + curPacket + "), stopping read: " + recordHeader.inclLen);
            }

            // Make sure there's enough room for an ethernet header
            if (recordHeader.inclLen < 14)
            {
                binaryReader.BaseStream.Position += recordHeader.inclLen;
                return null;
            }

            return recordHeader;
        }

        private static List<PacketRecord> loadPcapPacketRecords(BinaryReader binaryReader, bool asMessages, ref bool abort)
        {
            List<PacketRecord> results = new List<PacketRecord>();

            /*PcapHeader pcapHeader = */
            PcapHeader.read(binaryReader);

            int curPacket = 0;

            Dictionary<ulong, PacketRecord> incompletePacketMap = new Dictionary<ulong, PacketRecord>();

            while (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length)
            {
                if (abort)
                    break;

                PcapRecordHeader recordHeader;
                try
                {
                    recordHeader = readPcapRecordHeader(binaryReader, curPacket);

                    if (recordHeader == null)
                    {
                        continue;
                    }
                }
                catch (InvalidDataException e)
                {
                    break;
                }

                long packetStartPos = binaryReader.BaseStream.Position;

                try
                {
                    if (asMessages)
                    {
                        if (!readMessageData(binaryReader, recordHeader.inclLen, recordHeader.tsSec, results, incompletePacketMap))
                            break;
                    }
                    else
                    {
                        var packetRecord = readPacketData(binaryReader, recordHeader.inclLen, recordHeader.tsSec, curPacket);

                        if (packetRecord == null)
                            break;

                        results.Add(packetRecord);
                    }

                    curPacket++;
                }
                catch (Exception e)
                {
                    binaryReader.BaseStream.Position += recordHeader.inclLen - (binaryReader.BaseStream.Position - packetStartPos);
                }
            }

            return results;
        }

        private static PcapngBlockHeader readPcapngBlockHeader(BinaryReader binaryReader, int curPacket)
        {
            if (binaryReader.BaseStream.Length - binaryReader.BaseStream.Position < 8)
            {
                throw new InvalidDataException("Stream cut short (packet " + curPacket + "), stopping read: " + (binaryReader.BaseStream.Length - binaryReader.BaseStream.Position));
            }

            long blockStartPos = binaryReader.BaseStream.Position;

            PcapngBlockHeader blockHeader = PcapngBlockHeader.read(binaryReader);

            if (blockHeader.capturedLen > 50000)
            {
                throw new InvalidDataException("Enormous packet (packet " + curPacket + "), stopping read: " + blockHeader.capturedLen);
            }

            // Make sure there's enough room for an ethernet header
            if (blockHeader.capturedLen < 14)
            {
                binaryReader.BaseStream.Position += blockHeader.blockTotalLength - (binaryReader.BaseStream.Position - blockStartPos);
                return null;
            }

            return blockHeader;
        }

        private static List<PacketRecord> loadPcapngPacketRecords(BinaryReader binaryReader, bool asMessages, ref bool abort)
        {
            List<PacketRecord> results = new List<PacketRecord>();

            int curPacket = 0;

            Dictionary<ulong, PacketRecord> incompletePacketMap = new Dictionary<ulong, PacketRecord>();

            while (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length)
            {
                if (abort)
                    break;

                long blockStartPos = binaryReader.BaseStream.Position;

                PcapngBlockHeader blockHeader;
                try
                {
                    blockHeader = readPcapngBlockHeader(binaryReader, curPacket);

                    if (blockHeader == null)
                    {
                        continue;
                    }
                }
                catch (InvalidDataException e)
                {
                    break;
                }

                long packetStartPos = binaryReader.BaseStream.Position;

                try
                {
                    if (asMessages)
                    {
                        if (!readMessageData(binaryReader, blockHeader.capturedLen, blockHeader.tsLow, results, incompletePacketMap))
                            break;
                    }
                    else
                    {
                        var packetRecord = readPacketData(binaryReader, blockHeader.capturedLen, blockHeader.tsLow, curPacket);

                        if (packetRecord == null)
                            break;

                        results.Add(packetRecord);
                    }

                    curPacket++;
                }
                catch (Exception e)
                {
                    binaryReader.BaseStream.Position += blockHeader.capturedLen - (binaryReader.BaseStream.Position - packetStartPos);
                }

                binaryReader.BaseStream.Position += blockHeader.blockTotalLength - (binaryReader.BaseStream.Position - blockStartPos);
            }

            return results;
        }

        private static bool readNetworkHeaders(BinaryReader binaryReader)
        {
            EthernetHeader ethernetHeader = EthernetHeader.read(binaryReader);

            // Skip non-IP packets
            if (ethernetHeader.proto != 8)
            {
                throw new InvalidDataException();
            }

            IpHeader ipHeader = IpHeader.read(binaryReader);

            // Skip non-UDP packets
            if (ipHeader.proto != 17)
            {
                throw new InvalidDataException();
            }

            UdpHeader udpHeader = UdpHeader.read(binaryReader);

            bool isSend = (udpHeader.dPort >= 9000 && udpHeader.dPort <= 10013);
            bool isRecv = (udpHeader.sPort >= 9000 && udpHeader.sPort <= 10013);

            // Skip non-AC-port packets
            if (!isSend && !isRecv)
            {
                throw new InvalidDataException();
            }

            return isSend;
        }

        private static PacketRecord readPacketData(BinaryReader binaryReader, long len, uint tsSec, int curPacket)
        {
            // Begin reading headers
            long packetStartPos = binaryReader.BaseStream.Position;

            bool isSend = readNetworkHeaders(binaryReader);

            long headersSize = binaryReader.BaseStream.Position - packetStartPos;

            // Begin reading non-header packet content
            StringBuilder packetHeadersStr = new StringBuilder();
            StringBuilder packetTypeStr = new StringBuilder();

            PacketRecord packet = new PacketRecord();
            packet.index = curPacket;
            packet.isSend = isSend;
            packet.tsSec = tsSec;
            packet.extraInfo = "";
            packet.data = binaryReader.ReadBytes((int)(len - headersSize));
			using (BinaryReader packetReader = new BinaryReader(new MemoryStream(packet.data)))
			{
				try
				{
					ProtoHeader pHeader = ProtoHeader.read(packetReader);

					packet.optionalHeadersLen = readOptionalHeaders(pHeader.header_, packetHeadersStr, packetReader);

					if (packetReader.BaseStream.Position == packetReader.BaseStream.Length)
						packetTypeStr.Append("<Header Only>");

					uint HAS_FRAGS_MASK = 0x4; // See SharedNet::SplitPacketData

					if ((pHeader.header_ & HAS_FRAGS_MASK) != 0)
					{
						while (packetReader.BaseStream.Position != packetReader.BaseStream.Length)
						{
							if (packetTypeStr.Length != 0)
								packetTypeStr.Append(" + ");

							BlobFrag newFrag = readFragment(packetReader);
							packet.frags.Add(newFrag);

							if (newFrag.memberHeader_.blobNum != 0)
							{
								packetTypeStr.Append("FragData[");
								packetTypeStr.Append(newFrag.memberHeader_.blobNum);
								packetTypeStr.Append("]");
							}
							else
							{
								using (BinaryReader fragDataReader = new BinaryReader(new MemoryStream(newFrag.dat_)))
								{
									PacketOpcode opcode = Util.readOpcode(fragDataReader);
									packet.opcodes.Add(opcode);
									packetTypeStr.Append(opcode);
								}
							}
						}
					}

					if (packetReader.BaseStream.Position != packetReader.BaseStream.Length)
						packet.extraInfo = "Didnt read entire packet! " + packet.extraInfo;
				}
				catch (OutOfMemoryException e)
				{
					//MessageBox.Show("Out of memory (packet " + curPacket + "), stopping read: " + e);
					return null;
				}
				catch (Exception e)
				{
					packet.extraInfo += "EXCEPTION: " + e.Message + " " + e.StackTrace;
				}
			}
            packet.packetHeadersStr = packetHeadersStr.ToString();
            packet.packetTypeStr = packetTypeStr.ToString();

            return packet;
        }

        private static bool readMessageData(BinaryReader binaryReader, long len, uint tsSec, List<PacketRecord> results, Dictionary<ulong, PacketRecord> incompletePacketMap)
        {
            // Begin reading headers
            long packetStartPos = binaryReader.BaseStream.Position;

            bool isSend = readNetworkHeaders(binaryReader);

            long headersSize = binaryReader.BaseStream.Position - packetStartPos;

            // Begin reading non-header packet content
            StringBuilder packetHeadersStr = new StringBuilder();
            StringBuilder packetTypeStr = new StringBuilder();

            PacketRecord packet = null;
            byte[] packetData = binaryReader.ReadBytes((int)(len - headersSize));
			using (BinaryReader packetReader = new BinaryReader(new MemoryStream(packetData)))
			{
				try
				{
					ProtoHeader pHeader = ProtoHeader.read(packetReader);

					uint HAS_FRAGS_MASK = 0x4; // See SharedNet::SplitPacketData

					if ((pHeader.header_ & HAS_FRAGS_MASK) != 0)
					{
						readOptionalHeaders(pHeader.header_, packetHeadersStr, packetReader);

						while (packetReader.BaseStream.Position != packetReader.BaseStream.Length)
						{
							BlobFrag newFrag = readFragment(packetReader);

							ulong blobID = newFrag.memberHeader_.blobID;
							if (incompletePacketMap.ContainsKey(blobID))
							{
								packet = incompletePacketMap[newFrag.memberHeader_.blobID];
							}
							else
							{
								packet = new PacketRecord();
								incompletePacketMap.Add(blobID, packet);
							}

							if (newFrag.memberHeader_.blobNum == 0)
							{
								packet.isSend = isSend;
								packet.tsSec = tsSec;
								packet.extraInfo = "";

								using (BinaryReader fragDataReader = new BinaryReader(new MemoryStream(newFrag.dat_)))
								{
									PacketOpcode opcode = Util.readOpcode(fragDataReader);
									packet.opcodes.Add(opcode);
									packet.packetTypeStr = opcode.ToString();
								}
							}

							packet.packetHeadersStr += packetHeadersStr.ToString();

							packet.frags.Add(newFrag);

							if (addPacketIfFinished(results, packet))
							{
								incompletePacketMap.Remove(blobID);
							}
						}

						if (packetReader.BaseStream.Position != packetReader.BaseStream.Length)
							packet.extraInfo = "Didnt read entire packet! " + packet.extraInfo;
					}
				}
				catch (OutOfMemoryException e)
				{
					//MessageBox.Show("Out of memory (packet " + curPacket + "), stopping read: " + e);
					return false;
				}
				catch (Exception e)
				{
					packet.extraInfo += "EXCEPTION: " + e.Message + " " + e.StackTrace;
				}
			}
            return true;
        }

        private static BlobFrag readFragment(BinaryReader packetReader)
        {
            BlobFrag newFrag = new BlobFrag();
            newFrag.memberHeader_ = BlobFragHeader_t.read(packetReader);
            newFrag.dat_ = packetReader.ReadBytes(newFrag.memberHeader_.blobFragSize - 16); // 16 == size of frag header

            return newFrag;
        }

        private static int readOptionalHeaders(uint header_, StringBuilder packetHeadersStr, BinaryReader packetReader)
        {
            long readStartPos = packetReader.BaseStream.Position;

            if ((header_ & CServerSwitchStructHeader.mask) != 0)
            {
                /*CServerSwitchStruct serverSwitchStruct = */CServerSwitchStruct.read(packetReader);
                if (packetHeadersStr.Length != 0)
                    packetHeadersStr.Append(" | ");
                packetHeadersStr.Append("Server Switch");
            }

            if ((header_ & LogonServerAddrHeader.mask) != 0)
            {
                /*sockaddr_in serverAddr = */sockaddr_in.read(packetReader);
                if (packetHeadersStr.Length != 0)
                    packetHeadersStr.Append(" | ");
                packetHeadersStr.Append("Logon Server Addr");
            }

            if ((header_ & CEmptyHeader1.mask) != 0)
            {
                if (packetHeadersStr.Length != 0)
                    packetHeadersStr.Append(" | ");
                packetHeadersStr.Append("Empty Header 1");
            }

            if ((header_ & CReferralStructHeader.mask) != 0)
            {
                /*CReferralStruct referralStruct = */CReferralStruct.read(packetReader);
                if (packetHeadersStr.Length != 0)
                    packetHeadersStr.Append(" | ");
                packetHeadersStr.Append("Referral");
            }

            if ((header_ & NakHeader.mask) != 0)
            {
                /*CSeqIDListHeader nakSeqIDs = */NakHeader.read(packetReader);
                if (packetHeadersStr.Length != 0)
                    packetHeadersStr.Append(" | ");
                packetHeadersStr.Append("Nak");
            }

            if ((header_ & EmptyAckHeader.mask) != 0)
            {
                /*CSeqIDListHeader ackSeqIDs = */EmptyAckHeader.read(packetReader);
                if (packetHeadersStr.Length != 0)
                    packetHeadersStr.Append(" | ");
                packetHeadersStr.Append("Empty Ack");
            }

            if ((header_ & PakHeader.mask) != 0)
            {
                /*PakHeader pakHeader = */PakHeader.read(packetReader);
                if (packetHeadersStr.Length != 0)
                    packetHeadersStr.Append(" | ");
                packetHeadersStr.Append("Pak");
            }

            if ((header_ & CEmptyHeader2.mask) != 0)
            {
                if (packetHeadersStr.Length != 0)
                    packetHeadersStr.Append(" | ");
                packetHeadersStr.Append("Empty Header 2");
            }

            if ((header_ & CLogonHeader.mask) != 0)
            {
                CLogonHeader.HandshakeWireData handshakeData = CLogonHeader.HandshakeWireData.read(packetReader);
                /*byte[] authData = */packetReader.ReadBytes((int)handshakeData.cbAuthData);
                if (packetHeadersStr.Length != 0)
                    packetHeadersStr.Append(" | ");
                packetHeadersStr.Append("Logon");
            }

            if ((header_ & ULongHeader.mask) != 0)
            {
                /*ULongHeader ulongHeader = */ULongHeader.read(packetReader);
                if (packetHeadersStr.Length != 0)
                    packetHeadersStr.Append(" | ");
                packetHeadersStr.Append("ULong 1");
            }

            if ((header_ & CConnectHeader.mask) != 0)
            {
                /*CConnectHeader.HandshakeWireData handshakeData = */CConnectHeader.HandshakeWireData.read(packetReader);
                if (packetHeadersStr.Length != 0)
                    packetHeadersStr.Append(" | ");
                packetHeadersStr.Append("Connect");
            }

            if ((header_ & ULongHeader2.mask) != 0)
            {
                /*ULongHeader2 ulongHeader = */ULongHeader2.read(packetReader);
                if (packetHeadersStr.Length != 0)
                    packetHeadersStr.Append(" | ");
                packetHeadersStr.Append("ULong 2");
            }

            if ((header_ & NetErrorHeader.mask) != 0)
            {
                /*NetError netError = */NetError.read(packetReader);
                if (packetHeadersStr.Length != 0)
                    packetHeadersStr.Append(" | ");
                packetHeadersStr.Append("Net Error");
            }

            if ((header_ & NetErrorHeader_cs_DisconnectReceived.mask) != 0)
            {
                /*NetError netError = */NetError.read(packetReader);
                if (packetHeadersStr.Length != 0)
                    packetHeadersStr.Append(" | ");
                packetHeadersStr.Append("Net Error Disconnect");
            }

            if ((header_ & CICMDCommandStructHeader.mask) != 0)
            {
                /*CICMDCommandStruct icmdStruct = */CICMDCommandStruct.read(packetReader);
                if (packetHeadersStr.Length != 0)
                    packetHeadersStr.Append(" | ");
                packetHeadersStr.Append("ICmd");
            }

            if ((header_ & CTimeSyncHeader.mask) != 0)
            {
                /*CTimeSyncHeader timeSyncHeader = */CTimeSyncHeader.read(packetReader);
                if (packetHeadersStr.Length != 0)
                    packetHeadersStr.Append(" | ");
                packetHeadersStr.Append("Time Sync");
            }

            if ((header_ & CEchoRequestHeader.mask) != 0)
            {
                /*CEchoRequestHeader echoRequestHeader = */CEchoRequestHeader.read(packetReader);
                if (packetHeadersStr.Length != 0)
                    packetHeadersStr.Append(" | ");
                packetHeadersStr.Append("Echo Request");
            }

            if ((header_ & CEchoResponseHeader.mask) != 0)
            {
                /*CEchoResponseHeader.CEchoResponseHeaderWireData echoResponseData = */CEchoResponseHeader.CEchoResponseHeaderWireData.read(packetReader);
                if (packetHeadersStr.Length != 0)
                    packetHeadersStr.Append(" | ");
                packetHeadersStr.Append("Echo Response");
            }

            if ((header_ & CFlowStructHeader.mask) != 0)
            {
                /*CFlowStruct flowStruct = */CFlowStruct.read(packetReader);
                if (packetHeadersStr.Length != 0)
                    packetHeadersStr.Append(" | ");
                packetHeadersStr.Append("Flow");
            }

            return (int)(packetReader.BaseStream.Position - readStartPos);
        }
    }
}
