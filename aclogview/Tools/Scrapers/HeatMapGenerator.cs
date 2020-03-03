using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;

namespace aclogview.Tools.Scrapers
{
    class HeatMapGenerator : Scraper
    {
        public override string Description => "Generates a PNG showing all the pcapped locations over Dereth";

        long packetCount;
        long messageCount;

        uint[,] heatmap = new uint[256, 256];

        public override void Reset()
        {
            packetCount = 0;
            messageCount = 0;
            heatmap = new uint[256, 256];
        }

        public override (int hits, int messageExceptions) ProcessFileRecords(string fileName, List<PacketRecord> records, ref bool searchAborted)
        {
            int hits = 0;
            int messageExceptions = 0;

            foreach (PacketRecord record in records)
            {
                Interlocked.Increment(ref packetCount);

                foreach (BlobFrag frag in record.frags)
                {
                    if (frag.memberHeader_.blobNum == 0)
                        Interlocked.Increment(ref messageCount);

                    if (frag.dat_.Length > 20)
                    {
                        //BinaryReader fragDataReader = new BinaryReader(new MemoryStream(frag.dat_));
                        //fragDataReader.ReadUInt32();
                        //fragDataReader.ReadUInt32();
                        //if ((PacketOpcode)fragDataReader.ReadUInt32() == PacketOpcode.Evt_Movement__AutonomousPosition_ID)
                        if ((PacketOpcode)BitConverter.ToInt32(frag.dat_, 8) == PacketOpcode.Evt_Movement__AutonomousPosition_ID)
                        {
                            hits++;

                            uint objcell_id = unchecked((uint)BitConverter.ToInt32(frag.dat_, 12));//fragDataReader.ReadUInt32();

                            uint x = (objcell_id >> 24) & 0xFF;
                            uint y = 255 - ((objcell_id >> 16) & 0xFF);

                            lock (heatmap)
                                heatmap[x, y] = 1;
                        }
                    }
                }
            }

            return (hits, messageExceptions);
        }

        public override void WriteOutput(string destinationRoot, ref bool writeOutputAborted)
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();

            using (Stream imageStream = assembly.GetManifestResourceStream("aclogview.Tools.map.png"))
            using (Bitmap heatmapImg = new Bitmap(imageStream))
            {
                for (int y = 0; y < 256; ++y)
                {
                    for (int x = 0; x < 256; ++x)
                    {
                        if (heatmap[x, y] > 0)
                        {
                            Color curColor = heatmapImg.GetPixel(x, y);
                            heatmapImg.SetPixel(x, y, Color.FromArgb(255, Math.Min(255, 200 + curColor.R), curColor.G, curColor.B));
                        }
                    }
                }

                var fileName = GetFileName(destinationRoot, ".png");
                heatmapImg.Save(fileName);

                var output = "Coverage Map - " + packetCount + " packets, " + messageCount + " messages";

                var fileName2 = GetFileName(destinationRoot);
                File.WriteAllText(fileName2, output);
            }
        }
    }
}
