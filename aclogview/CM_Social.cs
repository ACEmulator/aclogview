﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class CM_Social : MessageProcessor {

    public override bool acceptMessageData(BinaryReader messageDataReader, TreeView outputTreeView) {
        bool handled = true;

        PacketOpcode opcode = Util.readOpcode(messageDataReader);
        switch (opcode) {
            case PacketOpcode.Evt_Social__ClearFriends_ID: {
                    EmptyMessage message = new EmptyMessage(opcode);
                    message.contributeToTreeView(outputTreeView);
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
            rootNode.Nodes.Add("i_friendID = " + i_friendID);
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
            rootNode.Nodes.Add("i_friend_name = " + i_friend_name);
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

        public static FriendData read(BinaryReader binaryReader) {
            FriendData newObj = new FriendData();
            newObj.m_id = binaryReader.ReadUInt32();
            newObj.m_online = binaryReader.ReadUInt32();
            newObj.m_appearOffline = binaryReader.ReadUInt32();
            newObj.m_name = PStringChar.read(binaryReader);
            newObj.m_friendsList = PList<uint>.read(binaryReader);
            newObj.m_friendOfList = PList<uint>.read(binaryReader);
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node) {
            node.Nodes.Add("m_id = " + m_id);
            node.Nodes.Add("m_online = " + m_online);
            node.Nodes.Add("m_appearOffline = " + m_appearOffline);
            node.Nodes.Add("m_name = " + m_name);
            TreeNode friendsListNode = node.Nodes.Add("m_friendsList = ");
            m_friendsList.contributeToTreeNode(friendsListNode);
            TreeNode friendOfListNode = node.Nodes.Add("m_friendOfList = ");
            m_friendOfList.contributeToTreeNode(friendOfListNode);
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
            TreeNode friendDataListNode = rootNode.Nodes.Add("friendDataList = ");
            friendDataList.contributeToTreeNode(friendDataListNode);
            rootNode.Nodes.Add("updateType = " + updateType);
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
            rootNode.Nodes.Add("mDisplayTitle = " + mDisplayTitle);
            TreeNode titleListNode = rootNode.Nodes.Add("mTitleList = ");
            mTitleList.contributeToTreeNode(titleListNode);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class AddOrSetCharacterTitle : Message {
        public CharacterTitle newTitle;
        public int bSetAsDisplayTitle;

        public static AddOrSetCharacterTitle read(BinaryReader binaryReader) {
            AddOrSetCharacterTitle newObj = new AddOrSetCharacterTitle();
            uint constantOne = binaryReader.ReadUInt32();
            newObj.newTitle = (CharacterTitle)binaryReader.ReadUInt32();
            newObj.bSetAsDisplayTitle = binaryReader.ReadInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("newTitle = " + newTitle);
            rootNode.Nodes.Add("bSetAsDisplayTitle = " + bSetAsDisplayTitle);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class SetDisplayCharacterTitle : Message {
        public CharacterTitle i_i_title;

        public static SetDisplayCharacterTitle read(BinaryReader binaryReader) {
            SetDisplayCharacterTitle newObj = new SetDisplayCharacterTitle();
            uint constantOne = binaryReader.ReadUInt32();
            newObj.i_i_title = (CharacterTitle)binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_i_title = " + i_i_title);
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
            node.Nodes.Add("_contract_id = " + _contract_id);
            node.Nodes.Add("_contract_stage = " + _contract_stage);
            node.Nodes.Add("_time_when_done = " + _time_when_done);
            node.Nodes.Add("_time_when_repeats = " + _time_when_repeats);
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
            rootNode.Expand();
            TreeNode trackerTableNode = rootNode.Nodes.Add("_contractTrackerHash = ");
            _contractTrackerHash.contributeToTreeNode(trackerTableNode);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class SendClientContractTracker : Message {
        public CContractTracker contractTracker;
        public int bDeleteContract;
        public int bSetAsDisplayContract;

        public static SendClientContractTracker read(BinaryReader binaryReader) {
            SendClientContractTracker newObj = new SendClientContractTracker();
            newObj.contractTracker = CContractTracker.read(binaryReader);
            newObj.bDeleteContract = binaryReader.ReadInt32();
            newObj.bSetAsDisplayContract = binaryReader.ReadInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            TreeNode trackerNode = rootNode.Nodes.Add("contractTracker = ");
            contractTracker.contributeToTreeNode(trackerNode);
            rootNode.Nodes.Add("bDeleteContract = " + bDeleteContract);
            rootNode.Nodes.Add("bSetAsDisplayContract = " + bSetAsDisplayContract);
            treeView.Nodes.Add(rootNode);
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
            rootNode.Nodes.Add("i_contract_id = " + i_contract_id);
            treeView.Nodes.Add(rootNode);
        }
    }
}
