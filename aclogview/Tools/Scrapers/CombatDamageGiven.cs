using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aclogview.Tools.Scrapers
{
    class CombatDamageGiven
    {
        public string Description => "Exports Damage given to a creature by a player from a melee or missile weapon";

        // List for Character Names
        private readonly Dictionary<string, Dictionary<uint, string>> accounts = new Dictionary<string, Dictionary<uint, string>>();
        private Dictionary<uint, string> charNames = new Dictionary<uint, string>();
        private Dictionary<uint, string> wieldedItemNames = new Dictionary<uint, string>();

        // Lists for Magic Verbs (for getting Magic Damage Type)
        List<string> SlashVerbs = new List<string>() { "mangle", "mangles", "slash", "slashes", "cut", "cuts", "scratch", "scratches" };
        List<string> PierceVerbs = new List<string>() { "gore", "gores", "impale", "impales", "stab", "stabs", "nick", "nicks" };
        List<string> BludgeVerbs = new List<string>() { "crush", "crushes", "smash", "smashes", "bash", "bashes", "graze", "grazes" };
        List<string> FireVerbs = new List<string>() { "incinerate", "incinerates", "burn", "burns", "scorch", "scorches", "singe", "singes" };
        List<string> ColdVerbs = new List<string>() { "freeze", "freezes", "frost", "frosts", "chill", "chills", "numb", "numbs" };        
        List<string> AcidVerbs = new List<string>() { "dissolve", "dissolves", "corrode", "corrodes", "sear", "sears", "blister", "blisters" };
        List<string> ElectricVerbs = new List<string>() { "blast", "blasts", "jolt", "jolts", "shock", "shocks", "spark", "sparks" };        
        List<string> NetherVerbs = new List<string>() { "eradicate", "eradicates", "wither", "withers", "twist", "twists", "scar", "scars" };
        List<string> HealthVerbs = new List<string>() { "deplete", "depletes", "siphon", "siphons", "exhaust", "exhausts", "drain", "drains" };

        string rxPattern = @"^You (\w+) (.+) for (.+) point.* with.+$";
        string rxPatternCrit = @"^Critical hit! You (\w+) (.+) for (.+) point.* with .+$";
        // string rxPatternBoth = @" ^ Critical hit! You(\w+) (.+) for (.+) point.* with.+$|^You (\w+) (.+) for (.+) point.* with.+$";

        private uint damageDone = 0;
        private string damageType = "";
        private bool crititcalHit = false;
        private string combatInfo = "";

        // private bool useCharList = false;

        //private bool haveCreatureName = false;
        //private string creatureName = "";
        private uint defenderCharID = 0;
        private uint wieldedItemID = 0;

        public void Reset()
        {
            damageDone = 0;
            damageType = "";
            crititcalHit = false;
            //haveCreatureName = false;
        }
    
        public (int hits, int messageExceptions, string combatInfo) ProcessFileRecords(string fileName, List<PacketRecord> records, List<string> creatureNames, ref bool useCharList, ref bool searchAborted)
        {
            int hits = 0;
            int messageExceptions = 0;
            string combatInfoResults = "";
            

            // Loads Character List from file if checked
            if (useCharList)
                GetCharList();

            foreach (PacketRecord record in records)
            {
                if (searchAborted)
                    return (hits, messageExceptions, combatInfoResults);

                try
                {
                    if (record.data.Length <= 4)
                        continue;

                    //if (record.isSend)
                    //    continue;

                    using (var memoryStream = new MemoryStream(record.data))
                    using (var binaryReader = new BinaryReader(memoryStream))
                    {
                        var messageCode = binaryReader.ReadUInt32();
                        // Getting Account and Character Info
                        if (useCharList == false)
                        {
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
                                        {
                                            account[character.gid_] = character.name_.m_buffer;
                                            charNames.Add(ConvertToUinteger(character.gid_.ToString()), character.name_.m_buffer);
                                        }
                                    }
                                }
                            }
                        }
                        // Getting Wield Item - Only be shown if item has been switched.
                        if (messageCode == 63409) //(uint)PacketOpcode.ORDERED_EVENT) // 0xF7B1)
                        {
                            var sequence = binaryReader.ReadUInt32(); // Sequence
                            var _event = binaryReader.ReadUInt32(); // Event
                            if (_event == (uint)PacketOpcode.Evt_Inventory__GetAndWieldItem_ID) // Seeing if event matches 
                            {
                                var parsedWieldInfo = CM_Inventory.GetAndWieldItem.read(binaryReader);

                                if ((parsedWieldInfo.i_equipMask == 1048576) || (parsedWieldInfo.i_equipMask == 4194304) || (parsedWieldInfo.i_equipMask == 16777216))
                                {
                                    wieldedItemID = parsedWieldInfo.i_item;
                                }
                                
                            }
                        }
                        // Getting Melee/Missile Damage
                        if (messageCode == (uint)PacketOpcode.WEENIE_ORDERED_EVENT) // 0xF7B0)
                        {

                        var character = binaryReader.ReadUInt32(); // Character
                        var sequence = binaryReader.ReadUInt32(); // Sequence
                        var _event = binaryReader.ReadUInt32(); // Event

                        if (_event == (uint)PacketOpcode.ATTACKER_NOTIFICATION_EVENT) // Seeing if event matches 
                        {

                            var parsedCombatAttack = CM_Combat.AttackerNotificationEvent.read(binaryReader);

                            lock (this)
                            {
                                if (creatureNames.Contains(parsedCombatAttack.defenders_name.ToString()) == true)  // Seeing if creature Name matches list of creatures searching for.
                                {
                                    hits++;
                                    damageDone = parsedCombatAttack.damage;
                                    damageType = DamageType(parsedCombatAttack.damage_type);
                                    if (parsedCombatAttack.critical == 1)
                                        crititcalHit = true;

                                    combatInfo += $"{GetCharacterName(character)},{"Melee/Missile"},{GetWieldedItemName(wieldedItemID)},{damageType},{damageDone},{parsedCombatAttack.defenders_name},{crititcalHit}\r\n";
                                    Reset();
                                }
                            }
                        }
                        // for getting charID for Magic chars
                        if (_event == (uint)PacketOpcode.DEFENDER_NOTIFICATION_EVENT) // Seeing if event matches
                        {
                            defenderCharID = character;

                        }

                        }
                        // Getting Character ID from 0x01BF: Combat_QueryHealth 
                        //if (messageCode == (uint)PacketOpcode.Evt_Combat__QueryHealth_ID) // 0xF7E0
                        //{
                        //    var character = binaryReader.ReadUInt32(); // Character
                        //    var sequence = binaryReader.ReadUInt32(); // Sequence
                        //    var _event = binaryReader.ReadUInt32(); // Event

                        //    var parsedHealth = CM_Combat.QueryHealth.read(binaryReader);


                        //}


                        // Getting Magic Attack Damage
                        if (messageCode == (uint)PacketOpcode.Evt_Communication__TextboxString_ID) // 0xF7E0
                        {
                            var parsedChat = CM_Communication.TextBoxString.read(binaryReader); // Chat
                            var chatType = parsedChat.ChatMessageType;

                            if (chatType == 7)  // Magic Message
                            {
                                // Non Critical Magic Hits
                                Regex regex = new Regex(rxPattern);
                                if (regex.IsMatch(parsedChat.MessageText.ToString())) // Seeing if Magic Message has magic damage
                                {
                                    var decodedMagicChat = DecodeMagicDamageMessage(parsedChat.MessageText.ToString());
                                    if (creatureNames.Contains(decodedMagicChat.creatureName) == true)  // Seeing if creature Name matches list of creatures searching for.
                                    {
                                        hits++;
                                        string magicCharName = "Not Found";
                                        if (defenderCharID != 0)
                                            magicCharName = GetCharacterName(defenderCharID);
                                        combatInfo += $"{magicCharName},{"Magic"},{GetWieldedItemName(wieldedItemID)},{decodedMagicChat.magicDamageType},{decodedMagicChat.damageAmount},{decodedMagicChat.creatureName},{decodedMagicChat.crit}\r\n";
                                    }
                                }
                            }
                            if (chatType == 7)   // Magic Message
                            {
                                // Critical Magic Hits
                                Regex regex = new Regex(rxPatternCrit);
                                if (regex.IsMatch(parsedChat.MessageText.ToString())) // Seeing if Magic Message has magic damage
                                {
                                    var decodedMagicChat = DecodeMagicDamageCriticalMessage(parsedChat.MessageText.ToString());
                                    if (creatureNames.Contains(decodedMagicChat.creatureName) == true)  // Seeing if creature Name matches list of creatures searching for.
                                    {
                                        hits++;
                                        string magicCharName = "Not Found";
                                        if (defenderCharID != 0)
                                            magicCharName = GetCharacterName(defenderCharID);
                                        combatInfo += $"{magicCharName},{"Magic"},{GetWieldedItemName(wieldedItemID)},{decodedMagicChat.magicDamageType},{decodedMagicChat.damageAmount},{decodedMagicChat.creatureName},{decodedMagicChat.crit}\r\n";
                                    }
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
            
            return (hits, messageExceptions, combatInfo);
        }

        public void WriteOutput(string destinationRoot, string scrapeResults, string fileNameHeading, ref bool writeOutputAborted)
        {
            var sb = new StringBuilder();
            string header = $"Combat Damage Given to a creature from a player \r\n" +
                            $"CharName,Attack,WeaponName,DamageType,Damage,Creature,Critical\r\n";
            //string combatInfo = $"{damageDone},{damageType},{crititcalHit}";

            var fileName = GetFileNameCombat(destinationRoot, fileNameHeading, ".csv");
            //if (creatureName != "")
                File.WriteAllText(fileName, header + scrapeResults);

            // haveCreatureName = false;
        }

        private string DamageType (uint dtype)
        {
            // Figuring out Damage Type for Melee/Missile Attacks

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
        protected (string magicDamageType, string creatureName, int damageAmount, bool crit) DecodeMagicDamageMessage(string magicMessage)
        {
            // Decoding Magic Message via RegEx
            string magicDamageType = "Undefined";
            string creatureName = "Unknown";
            int damageAmount = 0;
            bool critHit = false;

            Regex magicNonCrit = new Regex(rxPattern);
            if (magicNonCrit.IsMatch(magicMessage))
            {
                Match rxMatch = Regex.Match(magicMessage, rxPattern);

                magicDamageType = GetMagicDamageType(rxMatch.Groups[1].ToString());
                creatureName = rxMatch.Groups[2].ToString();
                damageAmount = ConvertToInteger(rxMatch.Groups[3].ToString());
                return (magicDamageType, creatureName, damageAmount, critHit);
            }

            return (magicDamageType, creatureName, damageAmount, critHit);
        }

        protected (string magicDamageType, string creatureName, int damageAmount, bool crit) DecodeMagicDamageCriticalMessage(string magicMessage)
        {
            string magicDamageType = "Undefined";
            string creatureName = "Unknown";
            int damageAmount = 0;
            bool critHit = false;

            Regex magicCrit = new Regex(rxPatternCrit);
            if (magicCrit.IsMatch(magicMessage))
            {
                Match rxMatch = Regex.Match(magicMessage, rxPatternCrit);

                magicDamageType = GetMagicDamageType(rxMatch.Groups[1].ToString());
                creatureName = rxMatch.Groups[2].ToString();
                damageAmount = ConvertToInteger(rxMatch.Groups[3].ToString());
                critHit = true;
                return (magicDamageType, creatureName, damageAmount, critHit);
            }

            return (magicDamageType, creatureName, damageAmount, critHit);
        }

        protected string GetMagicDamageType(string magicVerb)
        {
            // Determining Magic Damage Type

            string magicDamageType = "Unknown";

            if (SlashVerbs.Contains(magicVerb) == true)
            {
                magicDamageType = "Slash";
                return magicDamageType;
            }
            if (PierceVerbs.Contains(magicVerb) == true)
            {
                magicDamageType = "Pierce";
                return magicDamageType;
            }
            if (BludgeVerbs.Contains(magicVerb) == true)
            {
                magicDamageType = "Bludge";
                return magicDamageType;
            }
            if (FireVerbs.Contains(magicVerb) == true)
            {
                magicDamageType = "Fire";
                return magicDamageType;
            }
            if (ColdVerbs.Contains(magicVerb) == true)
            {
                magicDamageType = "Cold";
                return magicDamageType;
            }
            if (AcidVerbs.Contains(magicVerb) == true)
            {
                magicDamageType = "Acid";
                return magicDamageType;
            }
            if (ElectricVerbs.Contains(magicVerb) == true)
            {
                magicDamageType = "Electric";
                return magicDamageType;
            }
            if (NetherVerbs.Contains(magicVerb) == true)
            {
                magicDamageType = "Nether";
                return magicDamageType;
            }
            if (HealthVerbs.Contains(magicVerb) == true)
                magicDamageType = "HealthDrain";

            return magicDamageType;

        }

        private void GetCharList()
        {
            uint charID = 0;
            string charValue = "";

            foreach (string line in File.ReadLines(@"Lists\CharacterList.txt"))
            {
                string[] listValues = line.Split(',');

                charNames.Add(ConvertToUinteger(listValues[0]), listValues[1]);
            }

        }
        private string GetCharacterName (uint charID)
        {
            string charName = "Not Found";            
            if (charNames.TryGetValue(charID, out string nameValue))
            {
                charName = nameValue;
            }

            return charName;
        }

        private string GetWieldedItemName(uint wieldedItemID)
        {
            string wieldedItemName = "Not Found";
            if (charNames.TryGetValue(wieldedItemID, out string nameValue))
            {
                wieldedItemName = nameValue;
            }

            // For now just sending ID as text
            wieldedItemName = wieldedItemID.ToString();

            return wieldedItemName;
        }

        public static int ConvertToInteger(string text)
        {
            int i = 0;
            Int32.TryParse(text, out i);            
            return i;
        }
        public static uint ConvertToUinteger(string text)
        {
            uint i = 0;
            //Int32.TryParse(text, out i);
            UInt32.TryParse(text, out i);
            return i;
        }
    }
}
