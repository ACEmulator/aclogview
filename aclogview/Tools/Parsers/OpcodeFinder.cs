using System;
using System.Collections.Generic;

namespace aclogview.Tools.Parsers
{
    class OpcodeFinderResult
    {
        public string FileName;
        public int Hits;
        public int Exceptions;

        public readonly List<string> SpecialOutput = new List<string>();
    }

    class OpcodeFinder 
    {
        public OpcodeFinderResult ProcessFileRecords(string fileName, List<PacketRecord> records, ref bool searchAborted, int opCodeToSearchFor)
        {
            var result = new OpcodeFinderResult { FileName = fileName };

            foreach (PacketRecord record in records)
            {
                if (searchAborted)
                    return result;

                if (record.opcodes.Contains((PacketOpcode)opCodeToSearchFor))
                    result.Hits++;

                // See SampleParser.cs for examples on how to parse more information
            }

            return result;
        }
    }
}
