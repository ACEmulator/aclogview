using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace aclogview.Tools.Scrapers
{
    class PacketSizeScraperC2S : Scraper
    {
        public override string Description => "Finds all packet sizes for every Opcode";

        private readonly ConcurrentDictionary<uint, uint> codeHits = new ConcurrentDictionary<uint, uint>();
        private readonly ConcurrentDictionary<uint, uint> codeMaxLengths = new ConcurrentDictionary<uint, uint>();
        private readonly ConcurrentDictionary<uint, HashSet<int>> codeLengths = new ConcurrentDictionary<uint, HashSet<int>>();

        private readonly ConcurrentDictionary<uint, uint> gameActionHits = new ConcurrentDictionary<uint, uint>();
        private readonly ConcurrentDictionary<uint, uint> gameActionMaxLengths = new ConcurrentDictionary<uint, uint>();
        private readonly ConcurrentDictionary<uint, HashSet<int>> gameActionLengths = new ConcurrentDictionary<uint, HashSet<int>>();

        public override void Reset()
        {
            codeHits.Clear();
            codeMaxLengths.Clear();
            codeLengths.Clear();

            gameActionHits.Clear();
            gameActionMaxLengths.Clear();
            gameActionLengths.Clear();
        }

        /// <summary>
        /// This can be called by multiple thread simultaneously
        /// </summary>
        public override (int hits, int messageExceptions) ProcessFileRecords(string fileName, List<PacketRecord> records, ref bool searchAborted)
        {
            int hits = 0;
            int messageExceptions = 0;

            foreach (PacketRecord record in records)
            {
                if (searchAborted)
                    return (hits, messageExceptions);

                try
                {
                    if (record.data.Length <= 4)
                        continue;

                    if (!record.isSend)
                        continue;

                    using (var memoryStream = new MemoryStream(record.data))
                    using (var binaryReader = new BinaryReader(memoryStream))
                    {
                        hits++;

                        var messageCode = binaryReader.ReadUInt32();

                        if (!codeHits.ContainsKey(messageCode))
                        {
                            codeHits[messageCode] = 1;
                            codeMaxLengths[messageCode] = (uint)record.data.Length;
                            codeLengths[messageCode] = new HashSet<int> { record.data.Length };
                        }
                        else
                        {
                            codeHits[messageCode]++;
                            codeMaxLengths[messageCode] = Math.Max((uint)record.data.Length, codeMaxLengths[messageCode]);
                            codeLengths[messageCode].Add(record.data.Length);
                        }

                        if (messageCode == (uint)PacketOpcode.ORDERED_EVENT) // 0xF7B1 (Game Action)
                        {
                            if (messageCode == 0xF7B1) // Game Action
                            {
                                var sequence = binaryReader.ReadUInt32();
                                var opCode = binaryReader.ReadUInt32();

                                // We subtract 12 bytes from the record.data.Length to remove the header information
                                var recordLength = record.data.Length - 12;

                                if (!gameActionHits.ContainsKey(opCode))
                                {
                                    gameActionHits[opCode] = 1;
                                    gameActionMaxLengths[opCode] = (uint)recordLength;
                                    gameActionLengths[opCode] = new HashSet<int> { recordLength };
                                }
                                else
                                {
                                    gameActionHits[opCode]++;
                                    gameActionMaxLengths[opCode] = Math.Max((uint)recordLength, gameActionMaxLengths[opCode]);
                                    gameActionLengths[opCode].Add(recordLength);
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

            sb.AppendLine("Codes");

            var sortedKeys = codeLengths.Keys.ToList();
            sortedKeys.Sort();

            foreach (var key in sortedKeys)
            {
                sb.Append($"0x{key:X4}, hits: {codeHits[key].ToString("N0").PadLeft(10)}, maxLength: {codeMaxLengths[key].ToString("N0").PadLeft(6)}, lengths: ");

                var sortedLengths = codeLengths[key].ToList();
                sortedLengths.Sort();

                foreach (var length in sortedLengths)
                    sb.Append(length.ToString().PadLeft(3) + " ");
                sb.AppendLine();
            }

            sb.AppendLine();
            sb.AppendLine("GameActions");

            sortedKeys = gameActionLengths.Keys.ToList();
            sortedKeys.Sort();

            foreach (var key in sortedKeys)
            {
                sb.Append($"0x{key:X4}, hits: {gameActionHits[key].ToString("N0").PadLeft(10)}, maxLength: {gameActionMaxLengths[key].ToString("N0").PadLeft(6)}, lengths: ");

                var sortedLengths = gameActionLengths[key].ToList();
                sortedLengths.Sort();

                foreach (var length in sortedLengths)
                    sb.Append(length.ToString().PadLeft(3) + " ");
                sb.AppendLine();
            }


            var fileName = GetFileName(destinationRoot);
            File.WriteAllText(fileName, sb.ToString());
        }
    }
}
