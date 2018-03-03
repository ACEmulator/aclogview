using aclogview;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class CM_Vendor : MessageProcessor {

    public override bool acceptMessageData(BinaryReader messageDataReader, TreeView outputTreeView) {
        bool handled = true;

        PacketOpcode opcode = Util.readOpcode(messageDataReader);
        switch (opcode) {
            case PacketOpcode.Evt_Vendor__Buy_ID: {
                    Buy message = Buy.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Vendor__Sell_ID:
                {
                    Sell message = Sell.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            // (I could find no instance of this event in logs. Possibly a retired event? - OptimShi)
            case PacketOpcode.Evt_Vendor__RequestVendorInfo_ID: {
                    handled = false;
                    break;
                }
            case PacketOpcode.VENDOR_INFO_EVENT:
                {
                    gmVendorUI message = gmVendorUI.read(messageDataReader);
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

    public class gmVendorUI : Message
    {
        public uint shopVendorID;
        public VendorProfile shopVendorProfile;
        public PList<ItemProfile> shopItemProfileList;

        public static gmVendorUI read(BinaryReader binaryReader)
        {
            gmVendorUI newObj = new gmVendorUI();
            newObj.shopVendorID = binaryReader.ReadUInt32();
            newObj.shopVendorProfile = VendorProfile.read(binaryReader);
            newObj.shopItemProfileList = PList<ItemProfile>.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ServerToClientHeader });
            rootNode.Nodes.Add("shopVendorID = " + Utility.FormatHex(shopVendorID));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            TreeNode shopVendorProfileNode = rootNode.Nodes.Add("shopVendorProfile = ");
            ContextInfo.AddToList(new ContextInfo { Length = shopVendorProfile.Length }, updateDataIndex: false);
            shopVendorProfile.contributeToTreeNode(shopVendorProfileNode);
            TreeNode shopItemProfilesNode = rootNode.Nodes.Add("shopItemProfileList = ");
            ContextInfo.AddToList(new ContextInfo { Length = shopItemProfileList.Length }, updateDataIndex: false);
            // Skip PList count dword
            ContextInfo.DataIndex += 4;
            for (int i = 0; i < shopItemProfileList.list.Count; i++)
            {
                TreeNode itemProfileNode = shopItemProfilesNode.Nodes.Add("itemProfile = ");
                ItemProfile thisProfile = shopItemProfileList.list[i];
                ContextInfo.AddToList(new ContextInfo { Length = thisProfile.Length }, updateDataIndex: false);
                thisProfile.contributeToTreeNode(itemProfileNode);
            }

            treeView.Nodes.Add(rootNode);
        }
    }

    public class VendorProfile
    {
        public uint item_types;
        public uint min_value;
        public uint max_value;
        public uint magic;
        public float buy_price;
        public float sell_price;
        public uint trade_id;
        public uint trade_num;
        public PStringChar trade_name;
        public int Length;

        public static VendorProfile read(BinaryReader binaryReader)
        {
            VendorProfile newObj = new VendorProfile();
            var startPosition = binaryReader.BaseStream.Position;
            newObj.item_types = binaryReader.ReadUInt32();
            newObj.min_value = binaryReader.ReadUInt32();
            newObj.max_value = binaryReader.ReadUInt32();
            newObj.magic = binaryReader.ReadUInt32();
            newObj.buy_price = binaryReader.ReadSingle();
            newObj.sell_price = binaryReader.ReadSingle();
            newObj.trade_id = binaryReader.ReadUInt32();
            newObj.trade_num = binaryReader.ReadUInt32();
            newObj.trade_name = PStringChar.read(binaryReader);
            newObj.Length = (int)(binaryReader.BaseStream.Position - startPosition);
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
            TreeNode itemTypeNode = node.Nodes.Add("item_types = " + Utility.FormatHex(item_types));
            ContextInfo.AddToList(new ContextInfo { Length = 4 }, updateDataIndex: false);
            foreach (ITEM_TYPE e in Enum.GetValues(typeof(ITEM_TYPE)))
            {
                if ( (item_types & (uint)e) == (uint)e && (uint)e != 0 )
                {
                    itemTypeNode.Nodes.Add($"{Enum.GetName(typeof(ITEM_TYPE), e)}");
                    ContextInfo.AddToList(new ContextInfo { Length = 4 }, updateDataIndex: false);
                }
            }
            // Now skip over the item_types dword
            ContextInfo.DataIndex += 4;
            node.Nodes.Add("min_value = " + min_value);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("max_value = " + max_value);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("magic = " + magic);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("buy_price = " + buy_price);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("sell_price = " + sell_price);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("trade_id = " + trade_id);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("trade_num = " + trade_num);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("trade_name = " + trade_name);
            ContextInfo.AddToList(new ContextInfo { Length = trade_name.Length, DataType = DataType.Serialized_AsciiString });
        }
    }

    public class ItemProfile {
        public enum PWDType
        {
            PublicWeenieDesc = -1,
            OldPublicWeenieDesc = 1
        }
        public int packedAmount;
        public int amount;
        public int pwdType;
        public uint iid;
        public CM_Physics.PublicWeenieDesc pwd;
        public CM_Physics.OldPublicWeenieDesc opwd;
        public int Length;

        public static ItemProfile read(BinaryReader binaryReader) {
            ItemProfile newObj = new ItemProfile();
            var startPosition = binaryReader.BaseStream.Position;
            newObj.packedAmount = binaryReader.ReadInt32();
            newObj.amount = newObj.packedAmount & 0xFFFFFF;
            newObj.iid = binaryReader.ReadUInt32();
            newObj.pwdType = (newObj.packedAmount >> 24);
            if (newObj.pwdType == (int)PWDType.PublicWeenieDesc)
                newObj.pwd = CM_Physics.PublicWeenieDesc.read(binaryReader);
            // NOTE: I've not found an actual instance of this method being used.
            else if (newObj.pwdType == (int)PWDType.OldPublicWeenieDesc)
                newObj.opwd = CM_Physics.OldPublicWeenieDesc.read(binaryReader);
            newObj.Length = (int)(binaryReader.BaseStream.Position - startPosition);
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node) {
            TreeNode packedAmountNode = node.Nodes.Add("packedAmount = ");
            ContextInfo.AddToList(new ContextInfo {Length = 4}, updateDataIndex: false);
            if (amount == 0x00FFFFFF)
                packedAmountNode.Nodes.Add("amount = " + "-1 (unlimited)");
            else
                packedAmountNode.Nodes.Add("amount = " + amount);
            ContextInfo.AddToList(new ContextInfo { Length = 4 }, updateDataIndex: false);
            packedAmountNode.Nodes.Add("pwdType = " + (PWDType)pwdType);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("iid = " + Utility.FormatHex(iid));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            if (pwd != null)
            {
                TreeNode pwdNode = node.Nodes.Add("wdesc = ");
                ContextInfo.AddToList(new ContextInfo { Length = pwd.Length }, updateDataIndex: false);
                pwd.contributeToTreeNode(pwdNode);
            }
            if (opwd != null)
            {
                TreeNode opwdNode = node.Nodes.Add("oldwdesc = ");
                // Context info has not been added to the old weenie description class as it is not used
                opwd.contributeToTreeNode(opwdNode);
            }
        }
    }

    public class Buy : Message {
        public uint i_vendorID;
        public PList<ItemProfile> i_stuff;
        public uint i_alternateCurrencyID;

        public static Buy read(BinaryReader binaryReader) {
            Buy newObj = new Buy();
            newObj.i_vendorID = binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            newObj.i_stuff = PList<ItemProfile>.read(binaryReader);
            newObj.i_alternateCurrencyID = binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo{ DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_vendorID = " + Utility.FormatHex(i_vendorID));
            ContextInfo.AddToList(new ContextInfo{ DataType = DataType.ObjectID });
            TreeNode stuffNode = rootNode.Nodes.Add("i_stuff = ");
            ContextInfo.AddToList(new ContextInfo{ Length = i_stuff.Length }, updateDataIndex: false);
            // Now skip PList count dword
            ContextInfo.DataIndex += 4;
            for (int i = 0; i < i_stuff.list.Count; i++)
            {
                TreeNode itemProfileNode = stuffNode.Nodes.Add("itemProfile = ");
                ItemProfile thisProfile = i_stuff.list[i];
                ContextInfo.AddToList(new ContextInfo { Length = thisProfile.Length }, updateDataIndex: false);
                thisProfile.contributeToTreeNode(itemProfileNode);
            }
            rootNode.Nodes.Add("i_alternateCurrencyID = " + Utility.FormatHex(i_alternateCurrencyID));
            ContextInfo.AddToList(new ContextInfo { Length = 4, DataType = DataType.WCID });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class Sell : Message {
        public uint i_vendorID;
        public PList<ItemProfile> i_stuff;

        public static Sell read(BinaryReader binaryReader) {
            Sell newObj = new Sell();
            newObj.i_vendorID = binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            newObj.i_stuff = PList<ItemProfile>.read(binaryReader);
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ClientToServerHeader });
            rootNode.Nodes.Add("i_vendorID = " + Utility.FormatHex(i_vendorID));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            TreeNode stuffNode = rootNode.Nodes.Add("i_stuff = ");
            ContextInfo.AddToList(new ContextInfo { Length = i_stuff.Length }, updateDataIndex: false);
            // Now skip PList count dword
            ContextInfo.DataIndex += 4;
            for (int i = 0; i < i_stuff.list.Count; i++)
            {
                TreeNode itemProfileNode = stuffNode.Nodes.Add("itemProfile = ");
                ItemProfile thisProfile = i_stuff.list[i];
                ContextInfo.AddToList(new ContextInfo { Length = thisProfile.Length }, updateDataIndex: false);
                thisProfile.contributeToTreeNode(itemProfileNode);
            }
            treeView.Nodes.Add(rootNode);
        }
    }
}
