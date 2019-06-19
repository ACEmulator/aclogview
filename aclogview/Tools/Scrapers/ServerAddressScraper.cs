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

        private readonly Dictionary<string, HashSet<IPAddress>> servers = new Dictionary<string, HashSet<IPAddress>>();

        public override void ProcessFileRecords(string fileName, List<PacketRecord> records, ref bool searchAborted)
        {
            string serverName = null;

            foreach (PacketRecord record in records)
            {
                if (searchAborted)
                    return;

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

                            lock (servers)
                            {
                                if (servers.TryGetValue(serverName, out var value))
                                    value.Add(sAddr);
                                else
                                    servers[serverName] = new HashSet<IPAddress> { sAddr };
                            }
                        }
                        else
                        {
                            var dAddr = new IPAddress(record.ipHeader.dAddr.bytes);

                            lock (servers)
                            {
                                if (servers.TryGetValue(serverName, out var value))
                                    value.Add(dAddr);
                                else
                                    servers[serverName] = new HashSet<IPAddress> { dAddr };
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
                    // Do something with the exception maybe
                }
            }
        }

        public override void WriteOutput(string destinationRoot, ref bool searchAborted)
        {
            var sb = new StringBuilder();

            foreach (var kvp in servers)
            {
                sb.AppendLine(kvp.Key);

                foreach (var value in kvp.Value)
                    sb.AppendLine(value.ToString());

                sb.AppendLine();
            }

            var fileName = GetFileName(destinationRoot);
            File.WriteAllText(fileName, sb.ToString());
        }
    }
}
