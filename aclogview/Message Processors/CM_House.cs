using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using aclogview;

public class CM_House : MessageProcessor {

    public override bool acceptMessageData(BinaryReader messageDataReader, TreeView outputTreeView) {
        bool handled = true;

        PacketOpcode opcode = Util.readOpcode(messageDataReader);
        switch (opcode) {
            case PacketOpcode.Evt_House__QueryHouse_ID:
            case PacketOpcode.Evt_House__AbandonHouse_ID:
            case PacketOpcode.Evt_House__RemoveAllStoragePermission_ID:
            case PacketOpcode.Evt_House__RequestFullGuestList_Event_ID:
            case PacketOpcode.Evt_House__AddAllStoragePermission_ID:
            case PacketOpcode.Evt_House__RemoveAllPermanentGuests_Event_ID:
            case PacketOpcode.Evt_House__BootEveryone_Event_ID:
            case PacketOpcode.Evt_House__TeleToHouse_Event_ID:
            case PacketOpcode.Evt_House__TeleToMansion_Event_ID: {
                    EmptyMessage message = new EmptyMessage(opcode);
                    message.contributeToTreeView(outputTreeView);
                    ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
                    break;
                }
            // TODO: PacketOpcode.Evt_House__DumpHouse_ID (retired)
            case PacketOpcode.Evt_House__BuyHouse_ID: {
                    BuyHouse message = BuyHouse.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_House__Recv_HouseProfile_ID: {
                    Recv_HouseProfile message = Recv_HouseProfile.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            // TODO: PacketOpcode.Evt_House__StealHouse_ID: (retired)
            case PacketOpcode.Evt_House__RentHouse_ID: {
                    RentHouse message = RentHouse.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            // TODO: PacketOpcode.Evt_House__LinkToHouse_ID (retired)
            // TODO: PacketOpcode.Evt_House__ReCacheHouse_ID (retired)
            case PacketOpcode.Evt_House__Recv_HouseData_ID: {
                    Recv_HouseData message = Recv_HouseData.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_House__Recv_HouseStatus_ID: {
                    Recv_HouseStatus message = Recv_HouseStatus.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_House__Recv_UpdateRentTime_ID: {
                    Recv_UpdateRentTime message = Recv_UpdateRentTime.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_House__Recv_UpdateRentPayment_ID: {
                    Recv_UpdateRentPayment message = Recv_UpdateRentPayment.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_House__AddPermanentGuest_Event_ID: {
                    AddPermanentGuest_Event message = AddPermanentGuest_Event.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_House__RemovePermanentGuest_Event_ID: {
                    RemovePermanentGuest_Event message = RemovePermanentGuest_Event.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_House__SetOpenHouseStatus_Event_ID: {
                    SetOpenHouseStatus_Event message = SetOpenHouseStatus_Event.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_House__Recv_UpdateRestrictions_ID:
                {
                    Recv_UpdateRestrictions message = Recv_UpdateRestrictions.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_House__ChangeStoragePermission_Event_ID: {
                    ChangeStoragePermission_Event message = ChangeStoragePermission_Event.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_House__BootSpecificHouseGuest_Event_ID: {
                    BootSpecificHouseGuest_Event message = BootSpecificHouseGuest_Event.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            // TODO: PacketOpcode.Evt_House__BootAllUninvitedGuests_Event_ID (retired)
            // TODO: PacketOpcode.Evt_House__RentPay_ID (retired)
            // TODO: PacketOpcode.Evt_House__RentWarn_ID (retired)
            // TODO: PacketOpcode.Evt_House__RentDue_ID (retired)
            case PacketOpcode.Evt_House__Recv_UpdateHAR_ID:
                {
                    Recv_UpdateHAR message = Recv_UpdateHAR.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_House__QueryLord_ID: {
                    QueryLord message = QueryLord.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_House__Recv_HouseTransaction_ID: {
                    Recv_HouseTransaction message = Recv_HouseTransaction.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            // TODO: PacketOpcode.Evt_House__RentOverDue_ID (retired)
            // TODO: PacketOpcode.Evt_House__QueryHouseOwner_ID (retired)
            // TODO: PacketOpcode.Evt_House__AdminTeleToHouse_ID
            // TODO: PacketOpcode.Evt_House__PayRentForAllHouses_ID (retired)
            case PacketOpcode.Evt_House__SetHooksVisibility_ID: {
                    SetHooksVisibility message = SetHooksVisibility.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_House__ModifyAllegianceGuestPermission_ID: {
                    ModifyAllegianceGuestPermission message = ModifyAllegianceGuestPermission.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_House__ModifyAllegianceStoragePermission_ID: {
                    ModifyAllegianceStoragePermission message = ModifyAllegianceStoragePermission.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_House__ListAvailableHouses_ID: {
                    ListAvailableHouses message = ListAvailableHouses.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_House__Recv_AvailableHouses_ID: {
                    Recv_AvailableHouses message = Recv_AvailableHouses.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            // TODO: PacketOpcode.Evt_House__SetMaintenanceFree_ID (retired)
            // TODO: PacketOpcode.Evt_House__DumpHouseAccess_ID (retired)
            default: {
                    handled = false;
                    break;
                }
        }

        return handled;
    }

    public class BuyHouse : Message {
        public uint i_slumlord;
        public PList<uint> i_stuff;

        public static BuyHouse read(BinaryReader binaryReader) {
            BuyHouse newObj = new BuyHouse();
            newObj.i_slumlord = binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            newObj.i_stuff = PList<uint>.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_slumlord = " + Utility.FormatHex(i_slumlord));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            TreeNode stuffNode = rootNode.Nodes.Add("i_stuff = ");
            ContextInfo.AddToList(new ContextInfo { Length = i_stuff.Length }, updateDataIndex: false);
            ContextInfo.DataIndex += 4;
            for (int i = 0; i < i_stuff.list.Count; i++) {
                stuffNode.Nodes.Add(Utility.FormatHex(i_stuff.list[i]));
                ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            }
            treeView.Nodes.Add(rootNode);
            rootNode.ExpandAll();
        }
    }

    public class HousePayment {
        public int num;
        public int paid;
        public uint wcid;
        public PStringChar name;
        public PStringChar pname;
        public int Length;

        public static HousePayment read(BinaryReader binaryReader) {
            HousePayment newObj = new HousePayment();
            var startPosition = binaryReader.BaseStream.Position;
            newObj.num = binaryReader.ReadInt32();
            newObj.paid = binaryReader.ReadInt32();
            newObj.wcid = binaryReader.ReadUInt32();
            newObj.name = PStringChar.read(binaryReader);
            newObj.pname = PStringChar.read(binaryReader);
            newObj.Length = (int)(binaryReader.BaseStream.Position - startPosition);
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node) {
            node.Nodes.Add("num = " + num);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("paid = " + paid);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("wcid = " + wcid);
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.WCID, Length = 4 });
            node.Nodes.Add("name = " + name);
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Serialized_AsciiString, Length = name.Length });
            node.Nodes.Add("pname = " + pname);
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Serialized_AsciiString, Length = pname.Length });
        }
    }

    public class HouseProfile {
        public uint _id;
        public uint _owner;
        public uint _bitmask;
        public int _min_level;
        public int _max_level;
        public int _min_alleg_rank;
        public int _max_alleg_rank;
        public uint _maintenance_free;
        public HouseType _type;
        public PStringChar _name;
        public PList<HousePayment> _buy;
        public PList<HousePayment> _rent;
        public int Length;

        public static HouseProfile read(BinaryReader binaryReader) {
            HouseProfile newObj = new HouseProfile();
            var startPosition = binaryReader.BaseStream.Position;
            newObj._id = binaryReader.ReadUInt32();
            newObj._owner = binaryReader.ReadUInt32();
            newObj._bitmask = binaryReader.ReadUInt32();
            newObj._min_level = binaryReader.ReadInt32();
            newObj._max_level = binaryReader.ReadInt32();
            newObj._min_alleg_rank = binaryReader.ReadInt32();
            newObj._max_alleg_rank = binaryReader.ReadInt32();
            newObj._maintenance_free = binaryReader.ReadUInt32();
            newObj._type = (HouseType)binaryReader.ReadUInt32();
            newObj._name = PStringChar.read(binaryReader);
            newObj._buy = PList<HousePayment>.read(binaryReader);
            newObj._rent = PList<HousePayment>.read(binaryReader);
            newObj.Length = (int)(binaryReader.BaseStream.Position - startPosition);
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node) {
            node.Nodes.Add("_id = " + _id);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_owner = " + Utility.FormatHex(_owner));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            var bitmaskNode = node.Nodes.Add("_bitmask = " + Utility.FormatHex(_bitmask));
            ContextInfo.AddToList(new ContextInfo { Length = 4 }, updateDataIndex: false);
            foreach (HouseBitmask element in Enum.GetValues(typeof(HouseBitmask)))
            {
                if ((_bitmask & (uint)element) != 0)
                {
                    bitmaskNode.Nodes.Add(Enum.GetName(typeof(HouseBitmask), element));
                    ContextInfo.AddToList(new ContextInfo { Length = 4 }, updateDataIndex: false);
                }
            }
            ContextInfo.DataIndex += 4;
            node.Nodes.Add("_min_level = " + _min_level);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_max_level = " + _max_level);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_min_alleg_rank = " + _min_alleg_rank);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_max_alleg_rank = " + _max_alleg_rank);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_maintenance_free = " + _maintenance_free);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_type = " + _type);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_name = " + _name);
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Serialized_AsciiString, Length = _name.Length });
            TreeNode buyNode = node.Nodes.Add("_buy = ");
            ContextInfo.AddToList(new ContextInfo { Length = _buy.Length }, updateDataIndex: false);
            ContextInfo.DataIndex += 4;
            for (int i = 0; i < _buy.list.Count; i++) {
                TreeNode itemNode = buyNode.Nodes.Add("_item = ");
                ContextInfo.AddToList(new ContextInfo { Length = _buy.list[i].Length }, updateDataIndex: false);
                _buy.list[i].contributeToTreeNode(itemNode);
            }
            TreeNode rentNode = node.Nodes.Add("_rent = ");
            ContextInfo.AddToList(new ContextInfo { Length = _rent.Length }, updateDataIndex: false);
            ContextInfo.DataIndex += 4;
            for (int i = 0; i < _rent.list.Count; i++) {
                TreeNode itemNode = rentNode.Nodes.Add("_item = ");
                ContextInfo.AddToList(new ContextInfo { Length = _rent.list[i].Length }, updateDataIndex: false);
                _rent.list[i].contributeToTreeNode(itemNode);
            }
        }
    }

    public class Recv_HouseProfile : Message {
        public uint lord;
        public HouseProfile prof;

        public static Recv_HouseProfile read(BinaryReader binaryReader) {
            Recv_HouseProfile newObj = new Recv_HouseProfile();
            newObj.lord = binaryReader.ReadUInt32();
            newObj.prof = HouseProfile.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ServerToClientHeader });
            rootNode.Nodes.Add("lord = " + Utility.FormatHex(lord));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            TreeNode profNode = rootNode.Nodes.Add("prof = ");
            ContextInfo.AddToList(new ContextInfo { Length = prof.Length }, updateDataIndex: false);
            prof.contributeToTreeNode(profNode);
            rootNode.ExpandAll();
            treeView.Nodes.Add(rootNode);
        }
    }

    public class RentHouse : Message {
        public uint i_slumlord;
        public PList<uint> i_stuff;

        public static RentHouse read(BinaryReader binaryReader) {
            RentHouse newObj = new RentHouse();
            newObj.i_slumlord = binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            newObj.i_stuff = PList<uint>.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_slumlord = " + Utility.FormatHex(i_slumlord));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            TreeNode stuffNode = rootNode.Nodes.Add("i_stuff = ");
            ContextInfo.AddToList(new ContextInfo { Length = i_stuff.Length }, updateDataIndex: false);
            ContextInfo.DataIndex += 4;
            for (int i = 0; i < i_stuff.list.Count; i++) {
                stuffNode.Nodes.Add(Utility.FormatHex(i_stuff.list[i]));
                ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            }
            rootNode.ExpandAll();
            treeView.Nodes.Add(rootNode);
        }
    }

    public class HouseData {
        public int m_buy_time;
        public int m_rent_time;
        public HouseType m_type;
        public uint m_maintenance_free;
        public PList<HousePayment> m_buy;
        public PList<HousePayment> m_rent;
        public Position m_pos;
        public int Length;

        public static HouseData read(BinaryReader binaryReader) {
            HouseData newObj = new HouseData();
            var startPosition = binaryReader.BaseStream.Position;
            newObj.m_buy_time = binaryReader.ReadInt32();
            newObj.m_rent_time = binaryReader.ReadInt32();
            newObj.m_type = (HouseType)binaryReader.ReadUInt32();
            newObj.m_maintenance_free = binaryReader.ReadUInt32();
            newObj.m_buy = PList<HousePayment>.read(binaryReader);
            newObj.m_rent = PList<HousePayment>.read(binaryReader);
            newObj.m_pos = Position.read(binaryReader);
            newObj.Length = (int)(binaryReader.BaseStream.Position - startPosition);

            return newObj;
        }

        public void contributeToTreeNode(TreeNode node) {
            node.Nodes.Add("m_buy_time = " + m_buy_time);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("m_rent_time = " + m_rent_time);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("m_type = " + m_type);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("m_maintenance_free = " + m_maintenance_free);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            TreeNode buyNode = node.Nodes.Add("m_buy = ");
            ContextInfo.AddToList(new ContextInfo { Length = m_buy.Length }, updateDataIndex: false);
            ContextInfo.DataIndex += 4;
            for (int i = 0; i<m_buy.list.Count; i++)
            {
                HousePayment ele = m_buy.list[i];
                TreeNode subNode = buyNode.Nodes.Add(ele.ToString());
                ContextInfo.AddToList(new ContextInfo { Length = ele.Length }, updateDataIndex: false);
                ele.contributeToTreeNode(subNode);
            }
            TreeNode rentNode = node.Nodes.Add("m_rent = ");
            ContextInfo.AddToList(new ContextInfo { Length = m_rent.Length }, updateDataIndex: false);
            ContextInfo.DataIndex += 4;
            for (int i = 0; i < m_rent.list.Count; i++)
            {
                HousePayment ele = m_rent.list[i];
                TreeNode subNode = rentNode.Nodes.Add(ele.ToString());
                ContextInfo.AddToList(new ContextInfo { Length = ele.Length }, updateDataIndex: false);
                ele.contributeToTreeNode(subNode);
            }
            TreeNode posNode = node.Nodes.Add("m_pos = ");
            ContextInfo.AddToList(new ContextInfo { Length = m_pos.Length }, updateDataIndex: false);
            m_pos.contributeToTreeNode(posNode);
        }
    }

    public class Recv_HouseData : Message {
        public HouseData data;

        public static Recv_HouseData read(BinaryReader binaryReader) {
            Recv_HouseData newObj = new Recv_HouseData();
            newObj.data = HouseData.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ServerToClientHeader });
            TreeNode dataNode = rootNode.Nodes.Add("data = ");
            ContextInfo.AddToList(new ContextInfo { Length = data.Length }, updateDataIndex: false);
            data.contributeToTreeNode(dataNode);
            dataNode.Expand();
            treeView.Nodes.Add(rootNode);
        }
    }

    public class Recv_HouseStatus : Message {
        public uint etype;

        public static Recv_HouseStatus read(BinaryReader binaryReader) {
            Recv_HouseStatus newObj = new Recv_HouseStatus();
            newObj.etype = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ServerToClientHeader });
            rootNode.Nodes.Add("etype = " + (WERROR)etype);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class Recv_UpdateRentTime : Message {
        public int rent_time;

        public static Recv_UpdateRentTime read(BinaryReader binaryReader) {
            Recv_UpdateRentTime newObj = new Recv_UpdateRentTime();
            newObj.rent_time = binaryReader.ReadInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ServerToClientHeader });
            rootNode.Nodes.Add("rent_time = " + rent_time);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class Recv_UpdateRentPayment : Message {
        public PList<HousePayment> list;

        public static Recv_UpdateRentPayment read(BinaryReader binaryReader) {
            Recv_UpdateRentPayment newObj = new Recv_UpdateRentPayment();
            newObj.list = PList<HousePayment>.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ServerToClientHeader });
            TreeNode listNode = rootNode.Nodes.Add("list = ");
            ContextInfo.AddToList(new ContextInfo { Length = list.Length }, updateDataIndex: false);
            ContextInfo.DataIndex += 4;
            for (int i = 0; i < list.list.Count; i++) {
                TreeNode itemNode = listNode.Nodes.Add("_item = ");
                ContextInfo.AddToList(new ContextInfo { Length = list.list[i].Length }, updateDataIndex: false);
                list.list[i].contributeToTreeNode(itemNode);
            }
            rootNode.ExpandAll();
            treeView.Nodes.Add(rootNode);
        }
    }

    public class AddPermanentGuest_Event : Message {
        public PStringChar i_guest_name;

        public static AddPermanentGuest_Event read(BinaryReader binaryReader) {
            AddPermanentGuest_Event newObj = new AddPermanentGuest_Event();
            newObj.i_guest_name = PStringChar.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_guest_name = " + i_guest_name);
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Serialized_AsciiString, Length = i_guest_name.Length });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class RemovePermanentGuest_Event : Message {
        public PStringChar i_guest_name;

        public static RemovePermanentGuest_Event read(BinaryReader binaryReader) {
            RemovePermanentGuest_Event newObj = new RemovePermanentGuest_Event();
            newObj.i_guest_name = PStringChar.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_guest_name = " + i_guest_name);
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Serialized_AsciiString, Length = i_guest_name.Length });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class RestrictionDB {
        // The latest client and server used 0x10000002 for the version variable.
        public uint version;
        public uint _bitmask;
        public uint _monarch_iid;
        public PackableHashTable<uint,uint> _table;
        public int Length;

        public static RestrictionDB read(BinaryReader binaryReader)
        {
            RestrictionDB newObj = new RestrictionDB();
            var startPosition = binaryReader.BaseStream.Position;
            newObj.version = binaryReader.ReadUInt32();
            newObj._bitmask = binaryReader.ReadUInt32();
            newObj._monarch_iid = binaryReader.ReadUInt32();
            newObj._table = PackableHashTable<uint, uint>.read(binaryReader);
            newObj.Length = (int)(binaryReader.BaseStream.Position - startPosition);
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
            node.Nodes.Add("version = " + Utility.FormatHex(version));
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_bitmask = " + (RDBBitmask)_bitmask);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_monarch_iid = " + Utility.FormatHex(_monarch_iid));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            TreeNode tableNode = node.Nodes.Add("_table = ");
            ContextInfo.AddToList(new ContextInfo { Length = _table.Length }, updateDataIndex: false);
            ContextInfo.DataIndex += 4;
            foreach (KeyValuePair<uint, uint> element in _table.hashTable)
            {
                TreeNode guestNode = tableNode.Nodes.Add($"guest = ");
                ContextInfo.AddToList(new ContextInfo { Length = 8 }, updateDataIndex: false);
                guestNode.Nodes.Add("_char_object_id = " + Utility.FormatHex(element.Key));
                ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
                guestNode.Nodes.Add("_item_storage_permission = " + element.Value);
                ContextInfo.AddToList(new ContextInfo { Length = 4 });
            }
        }
    }

    public class GuestInfo
    {
        public int _item_storage_permission;
        PStringChar _char_name;

        public static GuestInfo read(BinaryReader binaryReader)
        {
            GuestInfo newObj = new GuestInfo();
            newObj._item_storage_permission = binaryReader.ReadInt32();
            newObj._char_name = PStringChar.read(binaryReader);
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
            node.Nodes.Add("_item_storage_permission = " + _item_storage_permission);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("_char_name = " + _char_name);
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Serialized_AsciiString, Length = _char_name.Length });
        }
    }

    public class HAR {
        public uint _bitmask;
        public uint _monarch_iid;
        public PackableHashTable<uint,GuestInfo> _guest_table;
        // Note: the _roommate_list is a list of object IDs of other characters on your account.
        public PList<uint> _roommate_list;

        public static HAR read(BinaryReader binaryReader)
        {
            HAR newObj = new HAR();
            newObj._bitmask = binaryReader.ReadUInt32();
            newObj._monarch_iid = binaryReader.ReadUInt32();
            newObj._guest_table = PackableHashTable<uint,GuestInfo>.read(binaryReader);
            newObj._roommate_list = new PList<uint>();
            newObj._roommate_list = PList<uint>.read(binaryReader);
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
            TreeNode bitmaskNode = node.Nodes.Add("_bitmask = " + Utility.FormatHex(_bitmask));
            ContextInfo.AddToList(new ContextInfo { Length = 4 }, updateDataIndex: false);
            foreach (HARBitmask e in Enum.GetValues(typeof(HARBitmask)))
            {
                if ((_bitmask & (uint)e) == (uint)e && (uint)e != 0)
                {
                    bitmaskNode.Nodes.Add($"{Enum.GetName(typeof(HARBitmask), e)}");
                    ContextInfo.AddToList(new ContextInfo { Length = 4 }, updateDataIndex: false);
                }
            }
            ContextInfo.DataIndex += 4;
            node.Nodes.Add("_monarch_iid = " + Utility.FormatHex(_monarch_iid));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            TreeNode guestTableNode = node.Nodes.Add("_guest_table = ");
            ContextInfo.AddToList(new ContextInfo { Length = _guest_table.Length }, updateDataIndex: false);
            ContextInfo.DataIndex += 4;
            foreach (KeyValuePair<uint,GuestInfo> element in _guest_table.hashTable)
            {
                var characterNode = guestTableNode.Nodes.Add("char_object_id = " + Utility.FormatHex(element.Key));
                ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
                element.Value.contributeToTreeNode(characterNode);
            }
            TreeNode roommateNode = node.Nodes.Add("_roommate_list = ");
            ContextInfo.AddToList(new ContextInfo { Length = _roommate_list.Length }, updateDataIndex: false);
            ContextInfo.DataIndex += 4;
            for (int i = 0; i < _roommate_list.list.Count; i++)
            {
                roommateNode.Nodes.Add("_char_object_id = " + Utility.FormatHex(_roommate_list.list[i]));
                ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            }
        }
    }

    public class Recv_UpdateRestrictions : Message {
        public byte _house_ts;
        public uint object_id;
        public RestrictionDB db;

        public static Recv_UpdateRestrictions read(BinaryReader binaryReader)
        {
            Recv_UpdateRestrictions newObj = new Recv_UpdateRestrictions();
            newObj._house_ts = binaryReader.ReadByte();
            newObj.object_id = binaryReader.ReadUInt32();
            newObj.db = RestrictionDB.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Opcode });
            rootNode.Nodes.Add("_house_ts = " + _house_ts);
            ContextInfo.AddToList(new ContextInfo { Length = 1 });
            rootNode.Nodes.Add("object_id = " + Utility.FormatHex(object_id));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            TreeNode dbNode = rootNode.Nodes.Add("db = ");
            ContextInfo.AddToList(new ContextInfo { Length = db.Length }, updateDataIndex: false);
            db.contributeToTreeNode(dbNode);
            rootNode.ExpandAll();
            treeView.Nodes.Add(rootNode);
        }
    }

    public class SetOpenHouseStatus_Event : Message {
        public int i_open_house;

        public static SetOpenHouseStatus_Event read(BinaryReader binaryReader) {
            SetOpenHouseStatus_Event newObj = new SetOpenHouseStatus_Event();
            newObj.i_open_house = binaryReader.ReadInt32();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_open_house = " + i_open_house);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class ChangeStoragePermission_Event : Message {
        public PStringChar i_guest_name;
        public int i_has_permission;

        public static ChangeStoragePermission_Event read(BinaryReader binaryReader) {
            ChangeStoragePermission_Event newObj = new ChangeStoragePermission_Event();
            newObj.i_guest_name = PStringChar.read(binaryReader);
            newObj.i_has_permission = binaryReader.ReadInt32();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_guest_name = " + i_guest_name);
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Serialized_AsciiString, Length = i_guest_name.Length });
            rootNode.Nodes.Add("i_has_permission = " + i_has_permission);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class BootSpecificHouseGuest_Event : Message {
        public PStringChar i_guest_name;

        public static BootSpecificHouseGuest_Event read(BinaryReader binaryReader) {
            BootSpecificHouseGuest_Event newObj = new BootSpecificHouseGuest_Event();
            newObj.i_guest_name = PStringChar.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_guest_name = " + i_guest_name);
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Serialized_AsciiString, Length = i_guest_name.Length });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class Recv_UpdateHAR : Message
    {
        public uint version;
        public HAR har;
        public static Recv_UpdateHAR read(BinaryReader binaryReader)
        {
            Recv_UpdateHAR newObj = new Recv_UpdateHAR();
            newObj.version = binaryReader.ReadUInt32();
            newObj.har = HAR.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ServerToClientHeader });
            rootNode.Nodes.Add("version = " + Utility.FormatHex(version));
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            har.contributeToTreeNode(rootNode);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class QueryLord : Message {
        public uint i_lord;

        public static QueryLord read(BinaryReader binaryReader) {
            QueryLord newObj = new QueryLord();
            newObj.i_lord = binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_lord = " + Utility.FormatHex(i_lord));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class Recv_HouseTransaction : Message {
        public uint etype;

        public static Recv_HouseTransaction read(BinaryReader binaryReader) {
            Recv_HouseTransaction newObj = new Recv_HouseTransaction();
            newObj.etype = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ServerToClientHeader });
            rootNode.Nodes.Add("etype = " + (WERROR)etype);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class SetHooksVisibility : Message {
        public int i_bVisible;

        public static SetHooksVisibility read(BinaryReader binaryReader) {
            SetHooksVisibility newObj = new SetHooksVisibility();
            newObj.i_bVisible = binaryReader.ReadInt32();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_bVisible = " + i_bVisible);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class ModifyAllegianceGuestPermission : Message {
        public int i_add;

        public static ModifyAllegianceGuestPermission read(BinaryReader binaryReader) {
            ModifyAllegianceGuestPermission newObj = new ModifyAllegianceGuestPermission();
            newObj.i_add = binaryReader.ReadInt32();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_add = " + i_add);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class ModifyAllegianceStoragePermission : Message {
        public int i_add;

        public static ModifyAllegianceStoragePermission read(BinaryReader binaryReader) {
            ModifyAllegianceStoragePermission newObj = new ModifyAllegianceStoragePermission();
            newObj.i_add = binaryReader.ReadInt32();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_add = " + i_add);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class ListAvailableHouses : Message {
        public HouseType i_houseType;

        public static ListAvailableHouses read(BinaryReader binaryReader) {
            ListAvailableHouses newObj = new ListAvailableHouses();
            newObj.i_houseType = (HouseType)binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_houseType = " + i_houseType);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class Recv_AvailableHouses : Message {
        public HouseType i_houseType;
        public PList<uint> houses;
        public int nHouses;

        public static Recv_AvailableHouses read(BinaryReader binaryReader) {
            Recv_AvailableHouses newObj = new Recv_AvailableHouses();
            newObj.i_houseType = (HouseType)binaryReader.ReadUInt32();
            newObj.houses = PList<uint>.read(binaryReader);
            newObj.nHouses = binaryReader.ReadInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ServerToClientHeader });
            rootNode.Nodes.Add("i_houseType = " + i_houseType);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            TreeNode housesNode = rootNode.Nodes.Add("houses = ");
            ContextInfo.AddToList(new ContextInfo { Length = houses.Length }, updateDataIndex: false);
            ContextInfo.DataIndex += 4;
            for (int i = 0; i < houses.list.Count; i++) {
                housesNode.Nodes.Add(Utility.FormatHex(houses.list[i]));
                ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            }
            rootNode.Nodes.Add("nHouses = " + nHouses);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            rootNode.ExpandAll();
            treeView.Nodes.Add(rootNode);
        }
    }
}
