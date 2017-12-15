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
            // TODO: PacketOpcode.Evt_Vendor__RequestVendorInfo_ID - (I could find no instance of this event in logs. Possibly a retired event? - OptimShi)
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

    public class gmVendorUI
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

        public void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("shopVendorID = " + Utility.FormatHex(shopVendorID));
            TreeNode shopVendorProfileNode = rootNode.Nodes.Add("shopVendorProfile = ");
            shopVendorProfile.contributeToTreeNode(shopVendorProfileNode);
            TreeNode shopItemProfilesNode = rootNode.Nodes.Add("shopItemProfileList = ");
            for (int i = 0; i < shopItemProfileList.list.Count; i++)
            {
                TreeNode itemProfileNode = shopItemProfilesNode.Nodes.Add("itemProfile = ");
                ItemProfile thisProfile = shopItemProfileList.list[i];
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

        public static VendorProfile read(BinaryReader binaryReader)
        {
            VendorProfile newObj = new VendorProfile();
            newObj.item_types = binaryReader.ReadUInt32();
            newObj.min_value = binaryReader.ReadUInt32();
            newObj.max_value = binaryReader.ReadUInt32();
            newObj.magic = binaryReader.ReadUInt32();
            newObj.buy_price = binaryReader.ReadSingle();
            newObj.sell_price = binaryReader.ReadSingle();
            newObj.trade_id = binaryReader.ReadUInt32();
            newObj.trade_num = binaryReader.ReadUInt32();
            newObj.trade_name = PStringChar.read(binaryReader);
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
            TreeNode itemTypeNode = node.Nodes.Add("item_types = " + Utility.FormatHex(item_types));
            foreach (ITEM_TYPE e in Enum.GetValues(typeof(ITEM_TYPE)))
            {
                if ( (item_types & (uint)e) == (uint)e && (uint)e != 0 )
                {
                    itemTypeNode.Nodes.Add($"{Enum.GetName(typeof(ITEM_TYPE), e)}");
                }
            }
            node.Nodes.Add("min_value = " + min_value);
            node.Nodes.Add("max_value = " + max_value);
            node.Nodes.Add("magic = " + magic);
            node.Nodes.Add("buy_price = " + buy_price);
            node.Nodes.Add("sell_price = " + sell_price);
            node.Nodes.Add("trade_id = " + trade_id);
            node.Nodes.Add("trade_num = " + trade_num);
            node.Nodes.Add("trade_name = " + trade_name);
        }
    }

    public class ItemProfile {
        public int amount;
        public uint iid;
        public CM_Physics.PublicWeenieDesc pwd;
        public CM_Physics.OldPublicWeenieDesc opwd;

        public static ItemProfile read(BinaryReader binaryReader) {
            ItemProfile newObj = new ItemProfile();
            newObj.amount = binaryReader.ReadInt32();
            newObj.iid = binaryReader.ReadUInt32();
            int descType = (newObj.amount >> 24);
            if (descType == -1)
                newObj.pwd = CM_Physics.PublicWeenieDesc.read(binaryReader);
            else if (descType == 1)
                newObj.opwd = CM_Physics.OldPublicWeenieDesc.read(binaryReader); // NOTE: I've not found an actual instance of this method being used.
            newObj.amount = newObj.amount & 0xFFFFFF;
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node) {
            node.Nodes.Add("amount = " + amount);
            node.Nodes.Add("iid = " + Utility.FormatHex(iid));
            if (pwd != null)
            {
                TreeNode pwdNode = node.Nodes.Add("wdesc = ");
                pwd.contributeToTreeNode(pwdNode);
            }
            if (opwd != null)
            {
                TreeNode opwdNode = node.Nodes.Add("oldwdesc = ");
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
            rootNode.Nodes.Add("i_vendorID = " + Utility.FormatHex(i_vendorID));
            TreeNode stuffNode = rootNode.Nodes.Add("i_stuff = ");
            for (int i = 0; i < i_stuff.list.Count; i++)
            {
                TreeNode itemProfileNode = stuffNode.Nodes.Add("itemProfile = ");
                ItemProfile thisProfile = i_stuff.list[i];
                thisProfile.contributeToTreeNode(itemProfileNode);
            }
            rootNode.Nodes.Add("i_alternateCurrencyID = " + Utility.FormatHex(i_alternateCurrencyID));
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
            rootNode.Nodes.Add("i_vendorID = " + Utility.FormatHex(i_vendorID));
            TreeNode stuffNode = rootNode.Nodes.Add("i_stuff = ");
            for (int i = 0; i < i_stuff.list.Count; i++)
            {
                TreeNode itemProfileNode = stuffNode.Nodes.Add("itemProfile = ");
                ItemProfile thisProfile = i_stuff.list[i];
                thisProfile.contributeToTreeNode(itemProfileNode);
            }
            treeView.Nodes.Add(rootNode);
        }
    }
}
