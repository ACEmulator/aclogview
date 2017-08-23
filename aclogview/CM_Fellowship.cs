using aclogview;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

public class CM_Fellowship : MessageProcessor {

    public override bool acceptMessageData(BinaryReader messageDataReader, TreeView outputTreeView) {
        bool handled = true;

        PacketOpcode opcode = Util.readOpcode(messageDataReader);
        switch (opcode) {

            // TODO: PacketOpcode.RECV_QUIT_FELLOW_EVENT = 167, - RETIRED
            // TODO: PacketOpcode.RECV_FELLOWSHIP_UPDATE_EVENT = 175, - RETIRED
            // TODO: PacketOpcode.RECV_UPDATE_FELLOW_EVENT = 176 / 0xB0,
            // TODO: PacketOpcode.RECV_DISMISS_FELLOW_EVENT = 177 / 0xB1,
            // TODO: PacketOpcode.RECV_LOGOFF_FELLOW_EVENT = 178 / 0xB2,
            // TODO: PacketOpcode.RECV_DISBAND_FELLOWSHIP_EVENT = 179 / 0xB3,
            // TODO: PacketOpcode.Evt_Fellowship__Appraise_ID = 202 / 0xCA, 
            // TODO: PacketOpcode.Evt_Fellowship__FellowUpdateDone_ID = 457 / 0x1C9 - NO LOGS FOUND
            // TODO: PacketOpcode.Evt_Fellowship__FellowStatsDone_ID = 458 / 0x1CA - NO LOGS FOUND

            case PacketOpcode.Evt_Fellowship__UpdateRequest_ID:
                {
                    UpdateRequest message = UpdateRequest.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Fellowship__Disband_ID:
                {
                    EmptyMessage message = new EmptyMessage(opcode);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }

            case PacketOpcode.Evt_Fellowship__Create_ID:
                {
                    FellowshipCreate message = FellowshipCreate.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Fellowship__Quit_ID:
            case PacketOpcode.Evt_Fellowship__Dismiss_ID:
                {
                    FellowshipQuit message = FellowshipQuit.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Fellowship__Recruit_ID:
                {
                    FellowshipRecruit message = FellowshipRecruit.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Fellowship__FullUpdate_ID:
                {
                    FellowshipFullUpdate message = FellowshipFullUpdate.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Fellowship__UpdateFellow_ID:
                {
                    FellowshipUpdate message = FellowshipUpdate.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Fellowship__ChangeFellowOpeness_ID:
                {
                    FellowshipChangeOpenness message = FellowshipChangeOpenness.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            default: {
                    handled = false;
                    break;
                }
        }

        return handled;
    }

    public class FellowshipCreate : Message
    {
        public PStringChar i_name;
        public uint i_share_xp;

        public static FellowshipCreate read(BinaryReader binaryReader)
        {
            FellowshipCreate newObj = new FellowshipCreate();
            newObj.i_name = PStringChar.read(binaryReader);
            newObj.i_share_xp = binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_name = " + i_name);
            rootNode.Nodes.Add("i_share_xp = " + i_share_xp);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class FellowshipQuit : Message
    {
        public uint player_id;

        public static FellowshipQuit read(BinaryReader binaryReader)
        {
            FellowshipQuit newObj = new FellowshipQuit();
            newObj.player_id = binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("player_id = " + Utility.FormatGuid(player_id));
            treeView.Nodes.Add(rootNode);
        }
    }

    public class FellowshipRecruit : Message
    {
        public uint player_id;

        public static FellowshipRecruit read(BinaryReader binaryReader)
        {
            FellowshipRecruit newObj = new FellowshipRecruit();
            newObj.player_id = binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("player_id = " + Utility.FormatGuid(player_id));
            treeView.Nodes.Add(rootNode);
        }
    }

    public class FellowshipFullUpdate : Message
    {
        public PackableHashTable<uint, Fellow> _fellowship_table = new PackableHashTable<uint, Fellow>();
        public PStringChar _name;
        public uint _leader;
        public uint _share_xp;
        public uint _even_xp_split;
        public uint _open_fellow;
        public uint _locked;
        public PackableHashTable<uint, int> _fellows_departed = new PackableHashTable<uint, int>();

        // This is not unpacked or defined in the client. From server end, it might be acceptable to leave off or, to ensure compatability with aclogview, send an empty PackableHashTable
        public PackableHashTable<PStringChar, FellowshipLock__GuessedName> unk = new PackableHashTable<PStringChar, FellowshipLock__GuessedName>();

        public static FellowshipFullUpdate read(BinaryReader binaryReader)
        {
            FellowshipFullUpdate newObj = new FellowshipFullUpdate();
            newObj._fellowship_table = PackableHashTable<uint, Fellow>.read(binaryReader);
            newObj._name = PStringChar.read(binaryReader);
            newObj._leader = binaryReader.ReadUInt32();
            newObj._share_xp = binaryReader.ReadUInt32();
            newObj._even_xp_split = binaryReader.ReadUInt32();
            newObj._open_fellow = binaryReader.ReadUInt32();
            newObj._locked = binaryReader.ReadUInt32();
            newObj._fellows_departed = PackableHashTable<uint, int>.read(binaryReader);
            newObj.unk = PackableHashTable<PStringChar, FellowshipLock__GuessedName>.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();

            TreeNode FellowshipTableNode = rootNode.Nodes.Add("_fellowship_table");
            foreach (KeyValuePair<uint, Fellow> element in _fellowship_table.hashTable)
            {
                TreeNode FellowNode = FellowshipTableNode.Nodes.Add("Fellow");
                element.Value.contributeToTreeNode(FellowNode);
            }

            rootNode.Nodes.Add("_name = " + _name);
            rootNode.Nodes.Add("_leader = " + Utility.FormatGuid(_leader));
            rootNode.Nodes.Add("_share_xp = " + _share_xp);
            rootNode.Nodes.Add("_even_xp_split = " + _even_xp_split);
            rootNode.Nodes.Add("_open_fellow = " + _open_fellow);
            rootNode.Nodes.Add("_locked = " + _locked);
            TreeNode FellowsDepartedNode = rootNode.Nodes.Add("_fellows_departed");
            foreach (KeyValuePair<uint, int> element in _fellows_departed.hashTable)
            {
                TreeNode FellowNode = FellowsDepartedNode.Nodes.Add(Utility.FormatGuid(element.Key) + " = " + element.Value);
            }

            TreeNode UnknownNode = rootNode.Nodes.Add("FellowshipLock__GuessedName");
            foreach (KeyValuePair<PStringChar, FellowshipLock__GuessedName> element in unk.hashTable)
            {
                TreeNode UnknownSubNode = UnknownNode.Nodes.Add(element.Key.ToString());
                element.Value.contributeToTreeNode(UnknownSubNode);
            }

            treeView.Nodes.Add(rootNode);
        }
    }

    public class Fellow
    {
        public uint _cp_cache;
        public uint _lum_cache;
        public uint _level;
        public uint _max_health;
        public uint _max_stamina;
        public uint _max_mana;
        public uint _current_health;
        public uint _current_stamina;
        public uint _current_mana;
        public uint _share_loot;
        public PStringChar _name;

        public static Fellow read(BinaryReader binaryReader)
        {
            Fellow newObj = new Fellow();
            newObj._cp_cache = binaryReader.ReadUInt32();
            newObj._lum_cache = binaryReader.ReadUInt32();
            newObj._level = binaryReader.ReadUInt32();

            newObj._max_health = binaryReader.ReadUInt32();
            newObj._max_stamina = binaryReader.ReadUInt32();
            newObj._max_mana = binaryReader.ReadUInt32();

            newObj._current_health = binaryReader.ReadUInt32();
            newObj._current_stamina = binaryReader.ReadUInt32();
            newObj._current_mana = binaryReader.ReadUInt32();

            newObj._share_loot = binaryReader.ReadUInt32();
            newObj._name = PStringChar.read(binaryReader);
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
            node.Nodes.Add("_cp_cache = " + _cp_cache);
            node.Nodes.Add("_lum_cache = " + _lum_cache);
            node.Nodes.Add("_level = " + _level);
            node.Nodes.Add("_max_health = " + _max_health);
            node.Nodes.Add("_max_stamina = " + _max_stamina);
            node.Nodes.Add("_max_mana = " + _max_mana);
            node.Nodes.Add("_current_health = " + _current_health);
            node.Nodes.Add("_current_stamina = " + _current_stamina);
            node.Nodes.Add("_current_mana = " + _current_mana);
            node.Nodes.Add("_share_loot = " + _share_loot);
            node.Nodes.Add("_name = " + _name);
        }
    }

    /// <summary>
    /// This is not unpacked in the client. Values/structures are gusses.
    /// </summary>
    public class FellowshipLock__GuessedName
    {
        public uint unknown_1;
        public uint unknown_2;
        public uint unknown_3;
        public uint timestamp;
        public uint unknown_4;

        public static FellowshipLock__GuessedName read(BinaryReader binaryReader)
        {
            FellowshipLock__GuessedName newObj = new FellowshipLock__GuessedName();
            newObj.unknown_1 = binaryReader.ReadUInt32();
            newObj.unknown_2 = binaryReader.ReadUInt32();
            newObj.unknown_3 = binaryReader.ReadUInt32();
            newObj.timestamp = binaryReader.ReadUInt32();
            newObj.unknown_4 = binaryReader.ReadUInt32();
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
            node.Nodes.Add("unknown_1 = " + unknown_1);
            node.Nodes.Add("unknown_2 = " + unknown_2);
            node.Nodes.Add("unknown_3 = " + unknown_3);
            node.Nodes.Add("timestamp__guessedname = " + timestamp);
            node.Nodes.Add("unknown_4 = " + unknown_4);
        }
    }

    public class FellowshipUpdate : Message
    {
        public uint i_iidPlayer;
        public Fellow fellow;
        public uint i_uiUpdateType;

        public static FellowshipUpdate read(BinaryReader binaryReader)
        {
            FellowshipUpdate newObj = new FellowshipUpdate();
            newObj.i_iidPlayer = binaryReader.ReadUInt32();
            newObj.fellow = Fellow.read(binaryReader);
            newObj.i_uiUpdateType = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();

            rootNode.Nodes.Add("i_iidPlayer = " + Utility.FormatGuid(i_iidPlayer));

            TreeNode FellowNode = rootNode.Nodes.Add("Fellow");
            FellowNode.Expand();
            fellow.contributeToTreeNode(FellowNode);

            rootNode.Nodes.Add("i_uiUpdateType = " + i_uiUpdateType);

            treeView.Nodes.Add(rootNode);
        }
    }
    public class FellowshipChangeOpenness : Message
    {
        // NOTE: Client incorrectly spells this as "Openess" (only one "N")
        public uint i_open;

        public static FellowshipChangeOpenness read(BinaryReader binaryReader)
        {
            FellowshipChangeOpenness newObj = new FellowshipChangeOpenness();
            newObj.i_open = binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_open = " + i_open);
            treeView.Nodes.Add(rootNode);
        }
    }
    public class UpdateRequest : Message
    {
        public uint i_on;

        public static UpdateRequest read(BinaryReader binaryReader)
        {
            UpdateRequest newObj = new UpdateRequest();
            newObj.i_on = binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_on = " + i_on);
            treeView.Nodes.Add(rootNode);
        }
    }
}
