using System;
using System.Collections.Generic;
using System.IO;

namespace aclogview.Tools.Scrapers
{
    class VendorBuySellAmountScraperC2S : Scraper
    {
        public override string Description => "Finds the vendor buy/sell item and amount maximums from actual players";

        private uint buyMaxItemCount;
        private uint buyMaxAmountSingle;
        private uint buyMaxAmountTotal;
        private uint sellMaxItemCount;
        private uint sellMaxAmountSingle;
        private uint sellMaxAmountTotal;

        public override void Reset()
        {
            buyMaxItemCount = 0;
            buyMaxAmountSingle = 0;
            buyMaxAmountTotal = 0;
            sellMaxItemCount = 0;
            sellMaxAmountSingle = 0;
            sellMaxAmountTotal = 0;
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
                        var messageCode = binaryReader.ReadUInt32();

                        if (messageCode == (uint)PacketOpcode.ORDERED_EVENT) // 0xF7B1 (Game Action)
                        {
                            var sequence = binaryReader.ReadUInt32();
                            var opCode = binaryReader.ReadUInt32();

                            if (opCode == (uint)PacketOpcode.Evt_Vendor__Buy_ID) // 0x005F
                            {
                                hits++;

                                var vendorGuid = binaryReader.ReadUInt32();
                                uint itemcount = binaryReader.ReadUInt32();

                                lock (this)
                                {
                                    if (itemcount > buyMaxItemCount)
                                        buyMaxItemCount = itemcount;

                                    uint total = 0;
                                    for (int i = 0; i < itemcount; i++)
                                    {
                                        var amount = binaryReader.ReadUInt32();
                                        var guid = binaryReader.ReadUInt32();

                                        total += amount;

                                        if (amount > buyMaxAmountSingle)
                                            buyMaxAmountSingle = amount;
                                    }

                                    if (total > buyMaxAmountTotal)
                                        buyMaxAmountTotal = total;
                                }
                            }
                            else if (opCode == (uint)PacketOpcode.Evt_Vendor__Sell_ID) // 0x0060
                            {
                                hits++;

                                var vendorGuid = binaryReader.ReadUInt32();
                                uint itemcount = binaryReader.ReadUInt32();

                                lock (this)
                                {
                                    if (itemcount > sellMaxItemCount)
                                        sellMaxItemCount = itemcount;

                                    uint total = 0;
                                    for (int i = 0; i < itemcount; i++)
                                    {
                                        var amount = binaryReader.ReadUInt32();
                                        var guid = binaryReader.ReadUInt32();

                                        total += amount;

                                        if (amount > sellMaxAmountSingle)
                                            sellMaxAmountSingle = amount;
                                    }

                                    if (total > sellMaxAmountTotal)
                                        sellMaxAmountTotal = total;
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
            var output = $"buyMaxItemCount: {buyMaxItemCount}, buyMaxAmountSingle: {buyMaxAmountSingle}, buyMaxAmountTotal: {buyMaxAmountTotal}, sellMaxItemCount: {sellMaxItemCount}, sellMaxAmountSingle: {sellMaxAmountSingle}, sellMaxAmountTotal: {sellMaxAmountTotal}";

            var fileName = GetFileName(destinationRoot);
            File.WriteAllText(fileName, output);
        }
    }
}
