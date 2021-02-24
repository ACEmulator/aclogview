using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aclogview.Tools.Scrapers
{
    class CombatDamageGiven
    {

        public string Description => "Exports Damage given to a creature by a player from a melee or missile weapon";

        private uint damageDone = 0;
        private string damageType = "";
        private bool crititcalHit = false;
        private string creatureName = "";
        private string combatInfo = "";
        private bool haveCreatureName = false;
        public void Reset()
        {
            damageDone = 0;
            damageType = "";
            crititcalHit = false;
            //haveCreatureName = false;
        }
    
        public (int hits, int messageExceptions, string combatInfo) ProcessFileRecords(string fileName, List<PacketRecord> records, List<string> creatureNames, ref bool searchAborted)
        {
            int hits = 0;
            int messageExceptions = 0;
            string combatInfoResults = "";
            //if (haveCreatureName == false)
            //{
            //    using (CreatureName form = new CreatureName())
            //    {
            //        DialogResult dr = form.ShowDialog();
            //        if (dr == DialogResult.OK)
            //        {
            //            creatureName = form.creatureName;
            //            haveCreatureName = true;
            //            if (creatureName == "")
            //                return (hits, messageExceptions);
            //        }
            //        else
            //        {
            //            searchAborted = true;
            //            //return (hits, messageExceptions);
            //        }
            //    }
            //}

            foreach (PacketRecord record in records)
            {
                if (searchAborted)
                    return (hits, messageExceptions, combatInfoResults);

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

                        if (messageCode == (uint)PacketOpcode.WEENIE_ORDERED_EVENT) // 0x7BD
                        {

                            var character = binaryReader.ReadUInt32(); // Character
                            var sequence = binaryReader.ReadUInt32(); // Sequence
                            var _event = binaryReader.ReadUInt32(); // Event

                            if (_event == (uint)PacketOpcode.ATTACKER_NOTIFICATION_EVENT) // Tell
                            {

                                var parsedCombatAttack = CM_Combat.AttackerNotificationEvent.read(binaryReader);

                                lock (this)
                                {
                                    //if (parsedCombatAttack.defenders_name.ToString() == creatureName)
                                    if (creatureNames.Contains(parsedCombatAttack.defenders_name.ToString()) == true)
                                    {
                                        hits++;
                                        damageDone = parsedCombatAttack.damage;
                                        damageType = DamageType(parsedCombatAttack.damage_type);
                                        if (parsedCombatAttack.critical == 1)
                                            crititcalHit = true;
                                        combatInfo += $"{parsedCombatAttack.defenders_name},{damageDone},{damageType},{crititcalHit}\r\n";

                                    }
                                }
                            }
                            Reset();
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

            return (hits, messageExceptions, combatInfo);
        }

        public void WriteOutput(string destinationRoot, string scrapeResults, string fileNameHeading, ref bool writeOutputAborted)
        {
            var sb = new StringBuilder();
            string header = $"Combat Damage \r\n" +
                            $"Creature,Damage,DamageType,Critical\r\n";
            //string combatInfo = $"{damageDone},{damageType},{crititcalHit}";

            var fileName = GetFileNameCombat(destinationRoot, fileNameHeading, ".csv");
            //if (creatureName != "")
                File.WriteAllText(fileName, header + scrapeResults);

            haveCreatureName = false;
        }

        private string DamageType (uint dtype)
        {
            string damageType = "";
            switch (dtype)
            {
                case 1:
                    damageType = "Slashing";
                    break;
                case 2:
                    damageType = "Piercing";
                    break;
                case 4:
                    damageType = "Bludgeoning";
                    break;
                case 8:
                    damageType = "Cold";
                    break;
                case 16:
                    damageType = "Fire";
                    break;
                case 32:
                    damageType = "Acid";
                    break;
                case 64:
                    damageType = "Electric";
                    break;
                default:
                    damageType = "Unknown";
                    break;
            }
            return damageType;
        }
        protected string GetFileNameCombat(string destinationRoot, string creature, string extension = ".txt")
        {
            if (creature == "")
                creature = "BlankCreature";
            return Path.Combine(destinationRoot, creature + "-" + DateTime.UtcNow.ToString("yyyy-MM-dd HH-mm-ss") + " " + GetType().Name + extension);
        }
    }
}
