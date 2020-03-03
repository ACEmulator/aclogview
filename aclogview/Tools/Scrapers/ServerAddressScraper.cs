using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace aclogview.Tools.Scrapers
{
    class ServerAddressScraper :Scraper
    {
        public override string Description => "Server Address Scraper";

        private readonly Dictionary<string, HashSet<IPAddress>> listByName = new Dictionary<string, HashSet<IPAddress>>();
        private readonly Dictionary<IPAddress, HashSet<string>> listByAddress = new Dictionary<IPAddress, HashSet<string>>();

        public override void Reset()
        {
            listByName.Clear();
            listByAddress.Clear();

            base.Reset();
        }

        public override (int hits, int messageExceptions) ProcessFileRecords(string fileName, List<PacketRecord> records, ref bool searchAborted)
        {
            int hits = 0;
            int messageExceptions = 0;

            string serverName = null;

            foreach (PacketRecord record in records)
            {
                if (searchAborted)
                    return (hits, messageExceptions);

                try
                {
                    if (record.data.Length <= 4)
                        continue;

                    using (var memoryStream = new MemoryStream(record.data))
                    using (var binaryReader = new BinaryReader(memoryStream))
                    {
                        var messageCode = binaryReader.ReadUInt32();

                        if (messageCode == (uint)PacketOpcode.Evt_Login__WorldInfo_ID) // 0xF7E1
                        {
                            var message = CM_Login.WorldInfo.read(binaryReader);
                            serverName = message.strWorldName.m_buffer;
                            continue;
                        }

                        if (serverName == null)
                            continue;

                        if (!record.isSend)
                        {
                            var sAddr = new IPAddress(record.ipHeader.sAddr.bytes);

                            lock (listByName)
                            {
                                if (listByName.TryGetValue(serverName, out var value))
                                {
                                    if (value.Add(sAddr))
                                        hits++;
                                }
                                else
                                {
                                    hits++;
                                    listByName[serverName] = new HashSet<IPAddress> {sAddr};
                                }
                            }

                            lock (listByAddress)
                            {
                                if (listByAddress.TryGetValue(sAddr, out var value))
                                {
                                    if (value.Add(serverName))
                                        hits++;
                                }
                                else
                                {
                                    hits++;
                                    listByAddress[sAddr] = new HashSet<string> {serverName};
                                }
                            }
                        }
                        else
                        {
                            var dAddr = new IPAddress(record.ipHeader.dAddr.bytes);

                            lock (listByName)
                            {
                                if (listByName.TryGetValue(serverName, out var value))
                                {
                                    if (value.Add(dAddr))
                                        hits++;
                                }
                                else
                                {
                                    hits++;
                                    listByName[serverName] = new HashSet<IPAddress> {dAddr};
                                }
                            }

                            lock (listByAddress)
                            {
                                if (listByAddress.TryGetValue(dAddr, out var value))
                                {
                                    if (value.Add(serverName))
                                        hits++;
                                }
                                else
                                {
                                    hits++;
                                    listByAddress[dAddr] = new HashSet<string> {serverName};
                                }
                            }
                        }
                    }
                }
                catch (InvalidDataException)
                {
                    // This is a pcap parse error
                }
                catch (Exception ex)
                {
                    messageExceptions++;
                    // Do something with the exception maybe
                }
            }

            return (hits, messageExceptions);
        }

        public override void WriteOutput(string destinationRoot, ref bool writeOutputAborted)
        {
            var sb = new StringBuilder();

            sb.AppendLine("List by Name:");
            sb.AppendLine();

            foreach (var kvp in listByName)
            {
                sb.AppendLine(kvp.Key);

                foreach (var value in kvp.Value)
                    sb.AppendLine(value.ToString());

                sb.AppendLine();
            }


            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("List by Address:");
            sb.AppendLine();

            foreach (var kvp in listByAddress)
            {
                sb.AppendLine(kvp.Key.ToString());

                foreach (var value in kvp.Value)
                    sb.AppendLine(value);

                sb.AppendLine();
            }

            var fileName = GetFileName(destinationRoot);
            File.WriteAllText(fileName, sb.ToString());
        }
    }
}
