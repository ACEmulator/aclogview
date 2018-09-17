using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using aclogview;

public class CM_Social : MessageProcessor {

    public override bool acceptMessageData(BinaryReader messageDataReader, TreeView outputTreeView) {
        bool handled = true;

        PacketOpcode opcode = Util.readOpcode(messageDataReader);
        switch (opcode) {
            case PacketOpcode.Evt_Social__ClearFriends_ID: {
                    EmptyMessage message = new EmptyMessage(opcode);
                    message.contributeToTreeView(outputTreeView);
                    ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
                    break;
                }
            case PacketOpcode.Evt_Social__RemoveFriend_ID: {
                    RemoveFriend message = RemoveFriend.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Social__AddFriend_ID: {
                    AddFriend message = AddFriend.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Social__FriendsUpdate_ID: {
                    FriendsUpdate message = FriendsUpdate.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            // TODO: AddCharacterTitle
            case PacketOpcode.Evt_Social__CharacterTitleTable_ID: {
                    CharacterTitleTable message = CharacterTitleTable.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Social__AddOrSetCharacterTitle_ID: {
                    AddOrSetCharacterTitle message = AddOrSetCharacterTitle.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Social__SetDisplayCharacterTitle_ID: {
                    SetDisplayCharacterTitle message = SetDisplayCharacterTitle.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Social__SendClientContractTrackerTable_ID: {
                    SendClientContractTrackerTable message = SendClientContractTrackerTable.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Social__SendClientContractTracker_ID: {
                    SendClientContractTracker message = SendClientContractTracker.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Social__AbandonContract_ID: {
                    AbandonContract message = AbandonContract.read(messageDataReader);
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

    public class RemoveFriend : Message {
        public uint i_friendID;

        public static RemoveFriend read(BinaryReader binaryReader) {
            RemoveFriend newObj = new RemoveFriend();
            newObj.i_friendID = binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo{ DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_friendID = " + Utility.FormatHex(i_friendID));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class AddFriend : Message {
        public PStringChar i_friend_name;

        public static AddFriend read(BinaryReader binaryReader) {
            AddFriend newObj = new AddFriend();
            newObj.i_friend_name = PStringChar.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_friend_name = " + i_friend_name);
            ContextInfo.AddToList(new ContextInfo { Length = i_friend_name.Length, DataType = DataType.Serialized_AsciiString });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class FriendData {
        public uint m_id;
        public uint m_online;
        public uint m_appearOffline;
        public PStringChar m_name;
        public PList<uint> m_friendsList;
        public PList<uint> m_friendOfList;
        public int Length;

        public static FriendData read(BinaryReader binaryReader) {
            FriendData newObj = new FriendData();
            var startPosition = binaryReader.BaseStream.Position;
            newObj.m_id = binaryReader.ReadUInt32();
            newObj.m_online = binaryReader.ReadUInt32();
            newObj.m_appearOffline = binaryReader.ReadUInt32();
            newObj.m_name = PStringChar.read(binaryReader);
            newObj.m_friendsList = PList<uint>.read(binaryReader);
            newObj.m_friendOfList = PList<uint>.read(binaryReader);
            newObj.Length = (int)(binaryReader.BaseStream.Position - startPosition);
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node) {
            node.Nodes.Add("m_id = " + Utility.FormatHex(m_id));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            node.Nodes.Add("m_online = " + m_online);
            ContextInfo.AddToList(new ContextInfo{ Length = 4 });
            node.Nodes.Add("m_appearOffline = " + m_appearOffline);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("m_name = " + m_name);
            ContextInfo.AddToList(new ContextInfo { Length = m_name.Length, DataType = DataType.Serialized_AsciiString });
            TreeNode friendsListNode = node.Nodes.Add("m_friendsList = ");
            ContextInfo.AddToList(new ContextInfo { Length = m_friendsList.Length }, updateDataIndex: false);
            // Skip plist header
            ContextInfo.DataIndex += 4;
            for (int i = 0; i< m_friendsList.list.Count; i++) {
                friendsListNode.Nodes.Add("m_id = " + Utility.FormatHex(m_friendsList.list[i]));
                ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            }
            TreeNode friendOfListNode = node.Nodes.Add("m_friendOfList = ");
            ContextInfo.AddToList(new ContextInfo { Length = m_friendsList.Length }, updateDataIndex: false);
            // Skip plist header
            ContextInfo.DataIndex += 4;
            for (int i = 0; i < m_friendOfList.list.Count; i++) {
                friendOfListNode.Nodes.Add("m_id = " + Utility.FormatHex(m_friendOfList.list[i]));
                ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            }
        }
    }

    public class FriendsUpdate : Message {
        public PList<FriendData> friendDataList;
        public FriendsUpdateType updateType;

        public static FriendsUpdate read(BinaryReader binaryReader) {
            FriendsUpdate newObj = new FriendsUpdate();
            newObj.friendDataList = PList<FriendData>.read(binaryReader);
            newObj.updateType = (FriendsUpdateType)binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ServerToClientHeader });
            TreeNode friendDataListNode = rootNode.Nodes.Add("friendDataList = ");
            ContextInfo.AddToList(new ContextInfo { Length = friendDataList.Length }, updateDataIndex: false);
            // Skip plist header
            ContextInfo.DataIndex += 4;
            for (int i = 0; i < friendDataList.list.Count; i++) {
                TreeNode friendNode = friendDataListNode.Nodes.Add($"friend \"{friendDataList.list[i].m_name}\" = ");
                ContextInfo.AddToList(new ContextInfo { Length = friendDataList.list[i].Length }, updateDataIndex: false);
                friendDataList.list[i].contributeToTreeNode(friendNode);
            }
            rootNode.Nodes.Add("updateType = " + updateType);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class CharacterTitleTable : Message {
        public CharacterTitle mDisplayTitle;
        public PList<CharacterTitle> mTitleList = new PList<CharacterTitle>();

        public static CharacterTitleTable read(BinaryReader binaryReader) {
            CharacterTitleTable newObj = new CharacterTitleTable();
            uint constantOne = binaryReader.ReadUInt32();
            newObj.mDisplayTitle = (CharacterTitle)binaryReader.ReadUInt32();
            newObj.mTitleList = PList<CharacterTitle>.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ServerToClientHeader });
            rootNode.Nodes.Add("constantOne = " + Utility.FormatHex(1));
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            rootNode.Nodes.Add("mDisplayTitle = " + mDisplayTitle);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            TreeNode titleListNode = rootNode.Nodes.Add("mTitleList = ");
            ContextInfo.AddToList(new ContextInfo { Length = mTitleList.Length }, updateDataIndex: false);
            // Skip plist header
            ContextInfo.DataIndex += 4;
            for (int i = 0; i < mTitleList.list.Count; i++)
            {
                titleListNode.Nodes.Add(mTitleList.list[i].ToString());
                ContextInfo.AddToList(new ContextInfo { Length = 4 });
            }
            treeView.Nodes.Add(rootNode);
        }
    }

    public class AddOrSetCharacterTitle : Message {
        public CharacterTitle newTitle;
        public int bSetAsDisplayTitle;

        public static AddOrSetCharacterTitle read(BinaryReader binaryReader) {
            AddOrSetCharacterTitle newObj = new AddOrSetCharacterTitle();
            newObj.newTitle = (CharacterTitle)binaryReader.ReadUInt32();
            newObj.bSetAsDisplayTitle = binaryReader.ReadInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ServerToClientHeader });
            rootNode.Nodes.Add("newTitle = " + newTitle);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            rootNode.Nodes.Add("bSetAsDisplayTitle = " + bSetAsDisplayTitle);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class SetDisplayCharacterTitle : Message {
        public CharacterTitle i_i_title;

        public static SetDisplayCharacterTitle read(BinaryReader binaryReader) {
            SetDisplayCharacterTitle newObj = new SetDisplayCharacterTitle();
            newObj.i_i_title = (CharacterTitle)binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_i_title = " + i_i_title);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class CContractTracker {
        public uint _version;
        public uint _contract_id;
        public uint _contract_stage;
        public double _time_when_done;
        public double _time_when_repeats;

        public static CContractTracker read(BinaryReader binaryReader) {
            CContractTracker newObj = new CContractTracker();
            newObj._version = binaryReader.ReadUInt32();
            newObj._contract_id = binaryReader.ReadUInt32();
            newObj._contract_stage = binaryReader.ReadUInt32();
            newObj._time_when_done = binaryReader.ReadDouble();
            newObj._time_when_repeats = binaryReader.ReadDouble();
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node) {
            node.Nodes.Add("_version = " + _version);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_contract_id = " + (ContractName)_contract_id);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_contract_stage = " + _contract_stage);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_time_when_done = " + _time_when_done);
            ContextInfo.AddToList(new ContextInfo { Length = 8 });
            node.Nodes.Add("_time_when_repeats = " + _time_when_repeats);
            ContextInfo.AddToList(new ContextInfo { Length = 8 });
        }
    }

    public class SendClientContractTrackerTable : Message {
        public PackableHashTable<uint, CContractTracker> _contractTrackerHash = new PackableHashTable<uint, CContractTracker>();

        public static SendClientContractTrackerTable read(BinaryReader binaryReader) {
            SendClientContractTrackerTable newObj = new SendClientContractTrackerTable();
            newObj._contractTrackerHash = PackableHashTable<uint, CContractTracker>.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ServerToClientHeader });
            TreeNode trackerTableNode = rootNode.Nodes.Add("_contractTrackerHash = ");
            ContextInfo.AddToList(new ContextInfo { Length = _contractTrackerHash.Length }, updateDataIndex: false);
            // Skip PackableHashTable header
            ContextInfo.DataIndex += 4;
            foreach (uint key in _contractTrackerHash.hashTable.Keys) {
                TreeNode contractNode = trackerTableNode.Nodes.Add($"contract {(ContractName)key} = ");
                ContextInfo.AddToList(new ContextInfo { Length = 32 }, updateDataIndex: false);
                // Skip key field
                ContextInfo.DataIndex += 4;
                _contractTrackerHash.hashTable[key].contributeToTreeNode(contractNode);
            }
            treeView.Nodes.Add(rootNode);
            rootNode.Expand();
            rootNode.NextVisibleNode.Expand();
            treeView.Nodes[0].EnsureVisible();
        }
    }

    public class SendClientContractTracker : Message {
        public CContractTracker contractTracker;
        public int bDeleteContract;
        public int bSetAsDisplayContract;
        public uint unused1;
        public uint unused2;
        public uint unused3;
        public bool retailPcap = false;
        public int Length;

        public static SendClientContractTracker read(BinaryReader binaryReader) {
            SendClientContractTracker newObj = new SendClientContractTracker();
            var startPosition = binaryReader.BaseStream.Position;
            newObj.contractTracker = CContractTracker.read(binaryReader);
            newObj.bDeleteContract = binaryReader.ReadInt32();
            newObj.bSetAsDisplayContract = binaryReader.ReadInt32();
            // NOTE: Retail pcaps have three extra dwords that are not used by the client 
            // at the end of the message (message length equals 64). 
            // ACE servers will not be sending these so we need to check for that.
            if (binaryReader.BaseStream.Length == 64) {
                newObj.retailPcap = true;
                newObj.unused1 = binaryReader.ReadUInt32();
                newObj.unused2 = binaryReader.ReadUInt32();
                newObj.unused3 = binaryReader.ReadUInt32();
            }
            newObj.Length = (int)(binaryReader.BaseStream.Position - startPosition);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ServerToClientHeader });
            TreeNode trackerNode = rootNode.Nodes.Add("contractTracker = ");
            ContextInfo.AddToList(new ContextInfo { Length = 28 }, updateDataIndex: false);
            contractTracker.contributeToTreeNode(trackerNode);
            rootNode.Nodes.Add("bDeleteContract = " + bDeleteContract);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            rootNode.Nodes.Add("bSetAsDisplayContract = " + bSetAsDisplayContract);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            if (retailPcap == true) {
                rootNode.Nodes.Add("unused1 = " + Utility.FormatHex(unused1));
                ContextInfo.AddToList(new ContextInfo { Length = 4 });
                rootNode.Nodes.Add("unused2 = " + Utility.FormatHex(unused2));
                ContextInfo.AddToList(new ContextInfo { Length = 4 });
                rootNode.Nodes.Add("unused3 = " + Utility.FormatHex(unused3));
                ContextInfo.AddToList(new ContextInfo { Length = 4 });
            }
            treeView.Nodes.Add(rootNode);
            rootNode.ExpandAll();
        }
    }

    public class AbandonContract : Message {
        public uint i_contract_id;

        public static AbandonContract read(BinaryReader binaryReader) {
            AbandonContract newObj = new AbandonContract();
            newObj.i_contract_id = binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_contract_id = " + (ContractName)i_contract_id);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            treeView.Nodes.Add(rootNode);
        }
    }
}
