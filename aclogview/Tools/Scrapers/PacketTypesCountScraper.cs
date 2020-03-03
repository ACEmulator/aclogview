using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;

namespace aclogview.Tools.Scrapers
{
    class PacketTypesCountScraper : Scraper
    {
        public override string Description => "Counts the number of packets seen by Opcode";

        readonly OrderedDictionary opcodeOccurrences = new OrderedDictionary();

        public override void Reset()
        {
            opcodeOccurrences.Clear();

            foreach (PacketOpcode opcode in Enum.GetValues(typeof(PacketOpcode)))
                opcodeOccurrences[opcode] = 0;
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
                foreach (PacketOpcode opcode in record.opcodes)
                {
                    lock (opcodeOccurrences)
                    {
                        hits++;

                        if (opcodeOccurrences.Contains(opcode))
                            opcodeOccurrences[opcode] = (Int32)opcodeOccurrences[opcode] + 1;
                        else
                            opcodeOccurrences[opcode] = 1;
                    }
                }
            }

            return (hits, messageExceptions);
        }

        public override void WriteOutput(string destinationRoot, ref bool writeOutputAborted)
        {
            long totalCount = 0;

            StringBuilder occurencesString = new StringBuilder();

            foreach (DictionaryEntry entry in opcodeOccurrences)
            {
                occurencesString.Append(entry.Key);
                occurencesString.Append(" = ");
                occurencesString.Append(entry.Value);
                occurencesString.Append("\r\n");

                totalCount += (Int32)entry.Value;
            }

            occurencesString.Append("\r\n\r\nTotal Count = ");
            occurencesString.Append(totalCount);
            occurencesString.Append("\r\n");

            var fileName = GetFileName(destinationRoot);
            File.WriteAllText(fileName, occurencesString.ToString());
        }
    }
}
