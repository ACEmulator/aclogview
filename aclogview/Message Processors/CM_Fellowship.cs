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
                    ContextInfo.AddToList(new ContextInfo { DataType = DataType.ServerToClientHeader });
                    break;
                }

            case PacketOpcode.Evt_Fellowship__Create_ID:
                {
                    FellowshipCreate message = FellowshipCreate.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Fellowship__Quit_ID:
                {
                    FellowshipQuit message = FellowshipQuit.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Fellowship__Dismiss_ID:
                {
                    FellowshipDismiss message = FellowshipDismiss.read(messageDataReader);
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
                    UpdateFellow message = UpdateFellow.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Fellowship__AssignNewLeader_ID:
                {
                    AssignNewLeader message = AssignNewLeader.read(messageDataReader);
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
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_name = " + i_name);
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Serialized_AsciiString, Length = i_name.Length });
            rootNode.Nodes.Add("i_share_xp = " + i_share_xp);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            treeView.Nodes.Add(rootNode);
        }
    }

    // Client to server and server to client message
    public class FellowshipQuit : Message
    {
        // Player ID is actually a "disband" boolean when the client sends this message
        public uint player_id_or_disband;
        public bool clientToServer;

        public static FellowshipQuit read(BinaryReader binaryReader)
        {
            FellowshipQuit newObj = new FellowshipQuit();
            newObj.clientToServer = binaryReader.BaseStream.Position == 12;
            newObj.player_id_or_disband = binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = clientToServer ? DataType.ClientToServerHeader : DataType.ServerToClientHeader });
            if (clientToServer)
                rootNode.Nodes.Add("disband = " + player_id_or_disband);
            else
                rootNode.Nodes.Add("player_id = " + Utility.FormatHex(player_id_or_disband));
            ContextInfo.AddToList(new ContextInfo { DataType = clientToServer ? DataType.Undefined: DataType.ObjectID, Length = 4 });
            treeView.Nodes.Add(rootNode);
        }
    }

    // Client to server and server to client message
    public class FellowshipDismiss : Message
    {
        public uint player_id;
        public bool clientToServer;

        public static FellowshipDismiss read(BinaryReader binaryReader)
        {
            FellowshipDismiss newObj = new FellowshipDismiss();
            newObj.clientToServer = binaryReader.BaseStream.Position == 12;
            newObj.player_id = binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = clientToServer ? DataType.ClientToServerHeader : DataType.ServerToClientHeader });
            rootNode.Nodes.Add("player_id = " + Utility.FormatHex(player_id));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
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
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("player_id = " + Utility.FormatHex(player_id));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
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
        public PackableHashTable<PStringChar, LockedFellowshipList> unk = new PackableHashTable<PStringChar, LockedFellowshipList>();

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
            newObj.unk = PackableHashTable<PStringChar, LockedFellowshipList>.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ServerToClientHeader });
            TreeNode FellowshipTableNode = rootNode.Nodes.Add("_fellowship_table");
            ContextInfo.AddToList(new ContextInfo { Length = _fellowship_table.Length }, updateDataIndex: false);
            ContextInfo.DataIndex += 4;
            foreach (KeyValuePair<uint, Fellow> element in _fellowship_table.hashTable)
            {
                TreeNode FellowNode = FellowshipTableNode.Nodes.Add($"fellow {Utility.FormatHex(element.Key)} = ");
                ContextInfo.AddToList(new ContextInfo { Length = element.Value.Length + 4 }, updateDataIndex: false);
                ContextInfo.DataIndex += 4;
                element.Value.contributeToTreeNode(FellowNode);
            }

            rootNode.Nodes.Add("_name = " + _name);
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Serialized_AsciiString, Length = _name.Length });
            rootNode.Nodes.Add("_leader = " + Utility.FormatHex(_leader));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            rootNode.Nodes.Add("_share_xp = " + _share_xp);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            rootNode.Nodes.Add("_even_xp_split = " + _even_xp_split);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            rootNode.Nodes.Add("_open_fellow = " + _open_fellow);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            rootNode.Nodes.Add("_locked = " + _locked);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            TreeNode FellowsDepartedNode = rootNode.Nodes.Add("_fellows_departed = ");
            ContextInfo.AddToList(new ContextInfo { Length = _fellows_departed.Length }, updateDataIndex: false);
            ContextInfo.DataIndex += 4;
            foreach (KeyValuePair<uint, int> element in _fellows_departed.hashTable)
            {
                TreeNode FellowNode = FellowsDepartedNode.Nodes.Add("fellow = ");
                ContextInfo.AddToList(new ContextInfo { Length = 8 }, updateDataIndex: false);
                FellowNode.Nodes.Add("fellow_id = " + Utility.FormatHex(element.Key));
                ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
                FellowNode.Nodes.Add("return_time = " + element.Value);
                ContextInfo.AddToList(new ContextInfo { Length = 4 });
            }

            TreeNode UnknownNode = rootNode.Nodes.Add("LockedFellowshipList = ");
            ContextInfo.AddToList(new ContextInfo { Length = unk.Length }, updateDataIndex: false);
            ContextInfo.DataIndex += 4;
            foreach (KeyValuePair<PStringChar, LockedFellowshipList> element in unk.hashTable)
            {
                TreeNode UnknownSubNode = UnknownNode.Nodes.Add("lock_name = " + element.Key);
                ContextInfo.AddToList(new ContextInfo { Length = element.Key.Length });
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
        public int Length;

        public static Fellow read(BinaryReader binaryReader)
        {
            Fellow newObj = new Fellow();
            var startPosition = binaryReader.BaseStream.Position;
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
            newObj.Length = (int)(binaryReader.BaseStream.Position - startPosition);
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
            node.Nodes.Add("_cp_cache = " + _cp_cache);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_lum_cache = " + _lum_cache);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_level = " + _level);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_max_health = " + _max_health);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_max_stamina = " + _max_stamina);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_max_mana = " + _max_mana);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_current_health = " + _current_health);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_current_stamina = " + _current_stamina);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_current_mana = " + _current_mana);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_share_loot = " + _share_loot);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_name = " + _name);
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Serialized_AsciiString, Length = _name.Length });
        }
    }

    /// <summary>
    /// This is not unpacked in the client. Values/structures are guesses.
    /// </summary>
    public class LockedFellowshipList
    {
        // unknown_1 was always 0 in pcaps
        public uint unknown_1;
        public uint unknown_2;
        public uint unknown_3;
        public uint timestamp;
        public uint unknown_4;

        public static LockedFellowshipList read(BinaryReader binaryReader)
        {
            LockedFellowshipList newObj = new LockedFellowshipList();
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
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("unknown_2 = " + Utility.FormatHex(unknown_2));
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("unknown_3 = " + Utility.FormatHex(unknown_3));
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("unknown_timestamp = " + timestamp);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("unknown_sequence = " + unknown_4);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
        }
    }

    public enum FellowshipUpdateType
    {
        UNDEF,
        FELLOWSHIP_UPDATE_FULL,
        FELLOWSHIP_UPDATE_STATS,
        FELLOWSHIP_UPDATE_VITALS,
    }

    public class UpdateFellow : Message
    {
        public uint i_iidPlayer;
        public Fellow fellow;
        public uint i_uiUpdateType;

        public static UpdateFellow read(BinaryReader binaryReader)
        {
            UpdateFellow newObj = new UpdateFellow();
            newObj.i_iidPlayer = binaryReader.ReadUInt32();
            newObj.fellow = Fellow.read(binaryReader);
            newObj.i_uiUpdateType = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ServerToClientHeader });
            rootNode.Nodes.Add("i_iidPlayer = " + Utility.FormatHex(i_iidPlayer));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            TreeNode FellowNode = rootNode.Nodes.Add("Fellow");
            FellowNode.Expand();
            ContextInfo.AddToList(new ContextInfo { Length = fellow.Length }, updateDataIndex: false);
            fellow.contributeToTreeNode(FellowNode);

            rootNode.Nodes.Add("i_uiUpdateType = " + (FellowshipUpdateType)i_uiUpdateType);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            treeView.Nodes.Add(rootNode);
        }
    }
    public class AssignNewLeader : Message
    {
        public uint i_target;

        public static AssignNewLeader read(BinaryReader binaryReader)
        {
            AssignNewLeader newObj = new AssignNewLeader();
            newObj.i_target = binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_target = " + Utility.FormatHex(i_target));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
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
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_open = " + i_open);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
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
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_on = " + i_on);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            treeView.Nodes.Add(rootNode);
        }
    }
}
