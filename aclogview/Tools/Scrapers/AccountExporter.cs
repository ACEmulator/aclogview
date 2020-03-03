using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace aclogview.Tools.Scrapers
{
    class AccountExporter :Scraper
    {
        public override string Description => "Exports character names and id's grouped by account";

        private readonly Dictionary<string, Dictionary<uint, string>> accounts = new Dictionary<string, Dictionary<uint, string>>();

        public override void Reset()
        {
            accounts.Clear();
        }

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

                    if (record.isSend)
                        continue;

                    using (var memoryStream = new MemoryStream(record.data))
                    using (var binaryReader = new BinaryReader(memoryStream))
                    {
                        var messageCode = binaryReader.ReadUInt32();

                        if (messageCode == (uint)PacketOpcode.Evt_Login__CharacterSet_ID) // 0xF658
                        {
                            var message = CM_Login.Login__CharacterSet.read(binaryReader);

                            lock (accounts)
                            {
                                if (!accounts.TryGetValue(message.account_.m_buffer, out var account))
                                {
                                    hits++;

                                    account = new Dictionary<uint, string>();
                                    accounts[message.account_.m_buffer] = account;
                                }

                                foreach (var character in message.set_)
                                {
                                    if (!account.ContainsKey(character.gid_))
                                        account[character.gid_] = character.name_.m_buffer;
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

            sb.AppendLine("AccountID CharacterGUID CharacterName");

            using (var md5 = MD5.Create())
            {
                foreach (var account in accounts)
                {
                    // We hash the account to help protect user names that weren't hashed originally. I don't know why or how that happened.
                    var accountHash = md5.ComputeHash(Encoding.UTF8.GetBytes(account.Key));

                    string accountHashString = null;
                    foreach (var b in accountHash)
                        accountHashString += b.ToString("X2");

                    foreach (var character in account.Value)
                        sb.AppendLine(accountHashString + " " + character.Key.ToString("X8") + " " + character.Value);

                    sb.AppendLine();
                }
            }

            var fileName = GetFileName(destinationRoot);
            File.WriteAllText(fileName, sb.ToString());
        }
    }
}
