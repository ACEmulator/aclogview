using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aclogview.Tools.Scrapers
{
    class CombatDamageGiven :Scraper
    {

        public override string Description => "Exports Damage given to a creature by a player";

        private uint damageDone = 0;
        private uint damageType = 0;
        private bool crititcalHit = false;
        private string creatureName = "Rynthid Minion";

        public override void Reset()
        {
            damageDone = 0;
            damageType = 0;
            crititcalHit = false;
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

                        // if (messageCode == (uint)PacketOpcode.ATTACKER_NOTIFICATION_EVENT) // 0x01B1
                        if (messageCode == (uint)PacketOpcode.WEENIE_ORDERED_EVENT) // 0x01B1
                        {
                            // var message = CM_Login.Login__CharacterSet.read(binaryReader);
                            // var message = CM_Combat.AttackerNotificationEvent.read(binaryReader);
                            var message = CM_Combat.AttackerNotificationEvent.read(binaryReader);

                            lock (this)
                            {
                                if (message.defenders_name.ToString() == creatureName)
                                {
                                    hits++;
                                    damageDone = message.damage;
                                    damageType = message.damage_type;
                                    if (message.critical == 1)
                                        crititcalHit = true;
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
            string header = $"Combat Damage for {creatureName} \r\n" +
                            $"Damage,DamageType,Critical\r\n";
            string combatInfo = $"{damageDone},{damageType},{crititcalHit}";

            var fileName = GetFileName(destinationRoot, ".csv");
            File.WriteAllText(fileName, header + combatInfo);
        }

    }
}
