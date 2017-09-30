using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using aclogview;

public class CM_Inventory : MessageProcessor {

    public override bool acceptMessageData(BinaryReader messageDataReader, TreeView outputTreeView) {
        bool handled = true;

        PacketOpcode opcode = Util.readOpcode(messageDataReader);
        switch (opcode) {
            case PacketOpcode.INVENTORY_PUT_OBJ_IN_CONTAINER_EVENT: //0x0022
                {
                    PutObjectInContainerEvent message = PutObjectInContainerEvent.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.INVENTORY_WIELD_OBJ_EVENT: // 0x0023
                {
                    WieldItem message = WieldItem.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.INVENTORY_REMOVE_OBJ_EVENT: // 0x0024
                {
                    RemoveObject message = RemoveObject.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Inventory__PutItemInContainer_ID: {
                    PutItemInContainer message = PutItemInContainer.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Inventory__GetAndWieldItem_ID: {
                    GetAndWieldItem message = GetAndWieldItem.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Inventory__DropItem_ID: {
                    DropItem message = DropItem.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Inventory__UseWithTargetEvent_ID: {
                    UseWithTargetEvent message = UseWithTargetEvent.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Inventory__UseEvent_ID: {
                    UseEvent message = UseEvent.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            // TODO: Evt_Inventory__CommenceViewingContents_ID
            case PacketOpcode.CLOSE_GROUND_CONTAINER_EVENT: // 0x0052
                {
                    CloseGroundContainer message = CloseGroundContainer.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Inventory__StackableMerge_ID: {
                    StackableMerge message = StackableMerge.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Inventory__StackableSplitToContainer_ID: {
                    StackableSplitToContainer message = StackableSplitToContainer.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Inventory__StackableSplitTo3D_ID: {
                    StackableSplitTo3D message = StackableSplitTo3D.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Inventory__GiveObjectRequest_ID: {
                    GiveObjectRequest message = GiveObjectRequest.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Inventory__NoLongerViewingContents_ID: {
                    NoLongerViewingContents message = NoLongerViewingContents.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Inventory__StackableSplitToWield_ID: {
                    StackableSplitToWield message = StackableSplitToWield.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Inventory__CreateTinkeringTool_ID: {
                    CreateTinkeringTool message = CreateTinkeringTool.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Inventory__Recv_SalvageOperationsResultData_ID: {
                    SalvageOperationsResultData message = SalvageOperationsResultData.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.STACKABLE_SET_STACKSIZE_EVENT: {
                    UpdateStackSize message = UpdateStackSize.read(messageDataReader);
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

    public class PutObjectInContainerEvent : Message
    {
        public uint i_objectId;
        public uint i_container;
        public uint i_slot;
        public uint i_type;

        public static PutObjectInContainerEvent read(BinaryReader binaryReader)
        {
            PutObjectInContainerEvent newObj = new PutObjectInContainerEvent();
            newObj.i_objectId = binaryReader.ReadUInt32();
            newObj.i_container = binaryReader.ReadUInt32();
            newObj.i_slot = binaryReader.ReadUInt32();
            newObj.i_type = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_objectId = " + Utility.FormatGuid(i_objectId));
            rootNode.Nodes.Add("i_container = " + Utility.FormatGuid(i_container));
            rootNode.Nodes.Add("i_slot = " + i_slot);
            rootNode.Nodes.Add("i_type = " + Utility.FormatGuid(i_type));
            treeView.Nodes.Add(rootNode);
        }
    }


    public class WieldItem : Message
    {
        public uint i_item;
        public uint i_equipMask;

        public static WieldItem read(BinaryReader binaryReader)
        {
            WieldItem newObj = new WieldItem();
            newObj.i_item = binaryReader.ReadUInt32();
            newObj.i_equipMask = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_item = " + Utility.FormatGuid(i_item));
            rootNode.Nodes.Add("i_equipMask = " + Utility.FormatGuid(i_equipMask));
            treeView.Nodes.Add(rootNode);
        }
    }

    public class RemoveObject : Message
    {
        public uint i_item;

        public static RemoveObject read(BinaryReader binaryReader)
        {
            RemoveObject newObj = new RemoveObject();
            newObj.i_item = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_item = " + Utility.FormatGuid(i_item));
            treeView.Nodes.Add(rootNode);
        }
    }

    public class PutItemInContainer : Message {
        public uint i_item;
        public uint i_container;
        public uint i_loc;

        public static PutItemInContainer read(BinaryReader binaryReader) {
            PutItemInContainer newObj = new PutItemInContainer();
            newObj.i_item = binaryReader.ReadUInt32();
            newObj.i_container = binaryReader.ReadUInt32();
            newObj.i_loc = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_item = " + i_item);
            rootNode.Nodes.Add("i_container = " + i_container);
            rootNode.Nodes.Add("i_loc = " + i_loc);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class GetAndWieldItem : Message {
        public uint i_item;
        public uint i_loc;

        public static GetAndWieldItem read(BinaryReader binaryReader) {
            GetAndWieldItem newObj = new GetAndWieldItem();
            newObj.i_item = binaryReader.ReadUInt32();
            newObj.i_loc = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_item = " + Utility.FormatGuid(i_item));
            rootNode.Nodes.Add("i_loc = " + i_loc);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class DropItem : Message {
        public uint i_item;

        public static DropItem read(BinaryReader binaryReader) {
            DropItem newObj = new DropItem();
            newObj.i_item = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_item = " + i_item);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class UseWithTargetEvent : Message {
        public uint i_object;
        public uint i_target;

        public static UseWithTargetEvent read(BinaryReader binaryReader) {
            UseWithTargetEvent newObj = new UseWithTargetEvent();
            newObj.i_object = binaryReader.ReadUInt32();
            newObj.i_target = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_object = " + Utility.FormatGuid(this.i_object));
            rootNode.Nodes.Add("i_target = " + Utility.FormatGuid(this.i_target));
            treeView.Nodes.Add(rootNode);
        }
    }

    public class UseEvent : Message {
        public uint i_object;

        public static UseEvent read(BinaryReader binaryReader) {
            UseEvent newObj = new UseEvent();
            newObj.i_object = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_object = " + Utility.FormatGuid(i_object));
            treeView.Nodes.Add(rootNode);
        }
    }

    public class CloseGroundContainer : Message
    {
        public uint ObjectID;

        public static CloseGroundContainer read(BinaryReader binaryReader)
        {
            CloseGroundContainer newObj = new CloseGroundContainer();
            newObj.ObjectID = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("ObjectID = " + Utility.FormatGuid(ObjectID));
            treeView.Nodes.Add(rootNode);
        }
    }

    public class StackableMerge : Message {
        public uint i_mergeFromID;
        public uint i_mergeToID;
        public int i_amount;

        public static StackableMerge read(BinaryReader binaryReader) {
            StackableMerge newObj = new StackableMerge();
            newObj.i_mergeFromID = binaryReader.ReadUInt32();
            newObj.i_mergeToID = binaryReader.ReadUInt32();
            newObj.i_amount = binaryReader.ReadInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_mergeFromID = " + i_mergeFromID);
            rootNode.Nodes.Add("i_mergeToID = " + i_mergeToID);
            rootNode.Nodes.Add("i_amount = " + i_amount);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class StackableSplitToContainer : Message {
        public uint i_stackID;
        public uint i_containerID;
        public int i_place;
        public int i_amount;

        public static StackableSplitToContainer read(BinaryReader binaryReader) {
            StackableSplitToContainer newObj = new StackableSplitToContainer();
            newObj.i_stackID = binaryReader.ReadUInt32();
            newObj.i_containerID = binaryReader.ReadUInt32();
            newObj.i_place = binaryReader.ReadInt32();
            newObj.i_amount = binaryReader.ReadInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_stackID = " + i_stackID);
            rootNode.Nodes.Add("i_containerID = " + i_containerID);
            rootNode.Nodes.Add("i_place = " + i_place);
            rootNode.Nodes.Add("i_amount = " + i_amount);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class StackableSplitTo3D : Message {
        public uint i_stackID;
        public int i_amount;

        public static StackableSplitTo3D read(BinaryReader binaryReader) {
            StackableSplitTo3D newObj = new StackableSplitTo3D();
            newObj.i_stackID = binaryReader.ReadUInt32();
            newObj.i_amount = binaryReader.ReadInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_stackID = " + i_stackID);
            rootNode.Nodes.Add("i_amount = " + i_amount);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class GiveObjectRequest : Message {
        public uint i_targetID;
        public uint i_objectID;
        public uint i_amount;

        public static GiveObjectRequest read(BinaryReader binaryReader) {
            GiveObjectRequest newObj = new GiveObjectRequest();
            newObj.i_targetID = binaryReader.ReadUInt32();
            newObj.i_objectID = binaryReader.ReadUInt32();
            newObj.i_amount = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_targetID = " + Utility.FormatGuid(this.i_targetID));
            rootNode.Nodes.Add("i_ObjectID = " + Utility.FormatGuid(this.i_objectID));
            rootNode.Nodes.Add("i_amount = " + i_amount);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class NoLongerViewingContents : Message {
        public uint i_container;

        public static NoLongerViewingContents read(BinaryReader binaryReader) {
            NoLongerViewingContents newObj = new NoLongerViewingContents();
            newObj.i_container = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_container = " + i_container);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class StackableSplitToWield : Message {
        public uint i_stackID;
        public uint i_loc;
        public int i_amount;

        public static StackableSplitToWield read(BinaryReader binaryReader) {
            StackableSplitToWield newObj = new StackableSplitToWield();
            newObj.i_stackID = binaryReader.ReadUInt32();
            newObj.i_loc = binaryReader.ReadUInt32();
            newObj.i_amount = binaryReader.ReadInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_stackID = " + i_stackID);
            rootNode.Nodes.Add("i_loc = " + i_loc);
            rootNode.Nodes.Add("i_amount = " + i_amount);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class CreateTinkeringTool : Message {
        public uint i_toolID;
        public PList<uint> i_gems;

        public static CreateTinkeringTool read(BinaryReader binaryReader) {
            CreateTinkeringTool newObj = new CreateTinkeringTool();
            newObj.i_toolID = binaryReader.ReadUInt32();
            newObj.i_gems = PList<uint>.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_toolID = " + i_toolID);
            TreeNode gemsNode = rootNode.Nodes.Add("i_gems = ");
            i_gems.contributeToTreeNode(gemsNode);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class SalvageResult {
        public MaterialType m_material;
        public double m_workmanship;
        public int m_units;

        public static SalvageResult read(BinaryReader binaryReader) {
            SalvageResult newObj = new SalvageResult();
            newObj.m_material = (MaterialType)binaryReader.ReadUInt32();
            newObj.m_workmanship = binaryReader.ReadDouble();
            newObj.m_units = binaryReader.ReadInt32();
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node) {
            node.Nodes.Add("m_material = " + m_material);
            node.Nodes.Add("m_workmanship = " + m_workmanship);
            node.Nodes.Add("m_units = " + m_units);
        }
    }

    // TODO: Double-check that this is really supposed to just be a SalvageOperationsResultData
    public class SalvageOperationsResultData : Message {
        public uint m_skillUsed;
        public PList<uint> m_notSalvagable;
        public PList<SalvageResult> m_salvageResults;
        public int m_augBonus;

        public static SalvageOperationsResultData read(BinaryReader binaryReader) {
            SalvageOperationsResultData newObj = new SalvageOperationsResultData();
            newObj.m_skillUsed = binaryReader.ReadUInt32();
            newObj.m_notSalvagable = PList<uint>.read(binaryReader);
            newObj.m_salvageResults = PList<SalvageResult>.read(binaryReader);
            newObj.m_augBonus = binaryReader.ReadInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("m_skillUsed = " + m_skillUsed);
            TreeNode notSalvageableNode = rootNode.Nodes.Add("m_notSalvagable = ");
            m_notSalvagable.contributeToTreeNode(notSalvageableNode);
            TreeNode salvageResultsNode = rootNode.Nodes.Add("m_salvageResults = ");
            m_salvageResults.contributeToTreeNode(salvageResultsNode);
            rootNode.Nodes.Add("m_augBonus = " + m_augBonus);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class UpdateStackSize : Message {
        public byte maxNumPages;
        public uint item;
        public uint amount;
        public uint newValue;

        public static UpdateStackSize read(BinaryReader binaryReader) {
            UpdateStackSize newObj = new UpdateStackSize();
            newObj.maxNumPages = binaryReader.ReadByte();
            newObj.item = binaryReader.ReadUInt32();
            newObj.amount = binaryReader.ReadUInt32();
            newObj.newValue = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("maxNumPages = " + maxNumPages);
            rootNode.Nodes.Add("item = " + item);
            rootNode.Nodes.Add("amount = " + amount);
            rootNode.Nodes.Add("newValue = " + newValue);
            treeView.Nodes.Add(rootNode);
        }
    }
}
