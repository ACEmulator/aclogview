using System;
using System.Collections.Generic;
using System.Text;

namespace aclogview.Tools.Parsers
{
    class TextSearcherResult
    {
        public string FileName;
        public int Hits;
        public int Exceptions;

        public readonly List<string> SpecialOutput = new List<string>();
    }

    class TextSearcher
    {
        public TextSearcherResult ProcessFileRecords(string fileName, List<PacketRecord> records, ref bool searchAborted, string textToSearchFor, bool caseSensitive)
        {
            var result = new TextSearcherResult { FileName = fileName };

            foreach (PacketRecord record in records)
            {
                if (searchAborted)
                    return result;

                try
                {
                    if (record.data.Length <= 4 || (textToSearchFor.Length > record.data.Length))
                        continue;

                    SearchForText(record, result, textToSearchFor, caseSensitive);

                    // See SampleParser.cs for examples on how to parse more information
                }
                catch
                {
                    // Do something with the exception maybe
                    result.Exceptions++;
                }
            }

            return result;
        }

        private static void SearchForText(PacketRecord record, TextSearcherResult result, string textToSearch, bool caseSensitive = true)
        {
            try
            {
                if (record.data.Length <= 4)
                    return;

                if (caseSensitive)
                {
                    int asciiResult = SearchBytePattern(Encoding.ASCII.GetBytes(textToSearch), record.data);
                    int unicodeResult = SearchBytePattern(Encoding.Unicode.GetBytes(textToSearch), record.data);
                    if (asciiResult > 0 || unicodeResult > 0)
                        result.Hits++;
                }
                else
                {
                    string asciiStringData = Encoding.ASCII.GetString(record.data);
                    string unicodeStringData = Encoding.Unicode.GetString(record.data);
                    // Shift the byte stream by 1 to catch any Unicode strings not on the previous two byte boundary
                    string unicodeStringData2 = Encoding.Unicode.GetString(record.data, 1, record.data.Length - 1);//(int)fragDataReader.BaseStream.Length - 1);
                    int asciiResultCI = asciiStringData.IndexOf(textToSearch, StringComparison.OrdinalIgnoreCase);
                    int unicodeResultCI = unicodeStringData.IndexOf(textToSearch, StringComparison.OrdinalIgnoreCase);
                    int unicodeResultCI2 = unicodeStringData2.IndexOf(textToSearch, StringComparison.OrdinalIgnoreCase);

                    if (asciiResultCI != -1 || unicodeResultCI != -1 || unicodeResultCI2 != -1)
                        result.Hits++;
                }
            }
            catch
            {
                // Do something with the exception maybe
                result.Exceptions++;
            }
        }

        private static int SearchBytePattern(byte[] pattern, byte[] bytes)
        {
            int matches = 0;

            for (int i = 0; i < bytes.Length; i++)
            {
                if (pattern[0] == bytes[i] && bytes.Length - i >= pattern.Length)
                {
                    bool ismatch = true;

                    for (int j = 1; j < pattern.Length && ismatch; j++)
                    {
                        if (bytes[i + j] != pattern[j])
                            ismatch = false;
                    }

                    if (ismatch)
                    {
                        matches++;
                        i += pattern.Length - 1;
                    }
                }
            }

            return matches;
        }
    }
}
