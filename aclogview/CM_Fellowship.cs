using aclogview;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class CM_Fellowship : MessageProcessor {

    public override bool acceptMessageData(BinaryReader messageDataReader, TreeView outputTreeView) {
        bool handled = true;

        PacketOpcode opcode = Util.readOpcode(messageDataReader);
        switch (opcode) {
            // TODO: PacketOpcode.Evt_Fellowship__Create_ID = 162,
            // TODO: PacketOpcode.Evt_Fellowship__Quit_ID = 163, // Bidirectional
            // TODO: PacketOpcode.Evt_Fellowship__Dismiss_ID = 164, // Bidirectional
            // TODO: PacketOpcode.Evt_Fellowship__Recruit_ID = 165,,
            // TODO: PacketOpcode.Evt_Fellowship__UpdateRequest_ID = 166,
            // TODO: PacketOpcode.RECV_QUIT_FELLOW_EVENT = 167,
            // TODO: PacketOpcode.RECV_FELLOWSHIP_UPDATE_EVENT = 175,
            // TODO: PacketOpcode.RECV_UPDATE_FELLOW_EVENT = 176,
            // TODO: PacketOpcode.RECV_DISMISS_FELLOW_EVENT = 177,
            // TODO: PacketOpcode.RECV_LOGOFF_FELLOW_EVENT = 178,
            // TODO: PacketOpcode.RECV_DISBAND_FELLOWSHIP_EVENT = 179,
            // TODO: PacketOpcode.Evt_Fellowship__Appraise_ID = 202, 
            // TODO: PacketOpcode.Evt_Fellowship__FellowUpdateDone_ID = 457,
            // TODO: PacketOpcode.Evt_Fellowship__FellowStatsDone_ID = 458,
            // TODO: PacketOpcode.Evt_Fellowship__ChangeFellowOpeness_ID = 657,
            // TODO: PacketOpcode.Evt_Fellowship__FullUpdate_ID = 702,
            // TODO: PacketOpcode.Evt_Fellowship__Disband_ID = 703,
            // TODO: PacketOpcode.Evt_Fellowship__UpdateFellow_ID = 704,
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
            case PacketOpcode.Evt_Fellowship__FullUpdate_ID:
                {
                    FellowshipFullUpdate message = FellowshipFullUpdate.read(messageDataReader);
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

    public class FellowshipFullUpdate : Message
    {
        public PackableHashTable<uint, Fellow> _fellowship_table = new PackableHashTable<uint, Fellow>();
        public PStringChar _name;
        public uint _leader;
        public uint _share_xp;
        public uint _even_xp_split;
        public uint _open_fellow;
        public uint _locked;
        public PackableHashTable<uint, uint> _fellows_departed = new PackableHashTable<uint, uint>();

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
            newObj._fellows_departed = PackableHashTable<uint, uint>.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();

            TreeNode FellowshipTableNode = rootNode.Nodes.Add("_fellowship_table");
            _fellowship_table.contributeToTreeNode(FellowshipTableNode);
            rootNode.Nodes.Add("_name = " + _name);
            rootNode.Nodes.Add("_leader = " + Utility.FormatGuid(_leader));
            rootNode.Nodes.Add("_share_xp = " + _share_xp);
            rootNode.Nodes.Add("_even_xp_split = " + _even_xp_split);
            rootNode.Nodes.Add("_open_fellow = " + _open_fellow);
            rootNode.Nodes.Add("_locked = " + _locked);
            TreeNode FellowsDepartedNode = rootNode.Nodes.Add("_fellows_departed");
            _fellows_departed.contributeToTreeNode(FellowsDepartedNode);
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
}
