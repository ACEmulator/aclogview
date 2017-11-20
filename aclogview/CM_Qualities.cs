using aclogview;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class CM_Qualities : MessageProcessor {

    public override bool acceptMessageData(BinaryReader messageDataReader, TreeView outputTreeView) {
        bool handled = true;

        PacketOpcode opcode = Util.readOpcode(messageDataReader);
        switch (opcode) {
            case PacketOpcode.Evt_Qualities__PrivateRemoveIntEvent_ID: {
                    PrivateRemoveQualityEvent<STypeInt> message = PrivateRemoveQualityEvent<STypeInt>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__RemoveIntEvent_ID: {
                    RemoveQualityEvent<STypeInt> message = RemoveQualityEvent<STypeInt>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__PrivateRemoveBoolEvent_ID: {
                    PrivateRemoveQualityEvent<STypeBool> message = PrivateRemoveQualityEvent<STypeBool>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__RemoveBoolEvent_ID: {
                    RemoveQualityEvent<STypeBool> message = RemoveQualityEvent<STypeBool>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__PrivateRemoveFloatEvent_ID: {
                    PrivateRemoveQualityEvent<STypeFloat> message = PrivateRemoveQualityEvent<STypeFloat>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__RemoveFloatEvent_ID: {
                    RemoveQualityEvent<STypeFloat> message = RemoveQualityEvent<STypeFloat>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__PrivateRemoveStringEvent_ID: {
                    PrivateRemoveQualityEvent<STypeString> message = PrivateRemoveQualityEvent<STypeString>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__RemoveStringEvent_ID: {
                    RemoveQualityEvent<STypeString> message = RemoveQualityEvent<STypeString>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__PrivateRemoveDataIDEvent_ID: {
                    PrivateRemoveQualityEvent<STypeDID> message = PrivateRemoveQualityEvent<STypeDID>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__RemoveDataIDEvent_ID: {
                    RemoveQualityEvent<STypeDID> message = RemoveQualityEvent<STypeDID>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__PrivateRemoveInstanceIDEvent_ID: {
                    PrivateRemoveQualityEvent<STypeIID> message = PrivateRemoveQualityEvent<STypeIID>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__RemoveInstanceIDEvent_ID: {
                    RemoveQualityEvent<STypeIID> message = RemoveQualityEvent<STypeIID>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__PrivateRemovePositionEvent_ID: {
                    PrivateRemoveQualityEvent<STypePosition> message = PrivateRemoveQualityEvent<STypePosition>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__RemovePositionEvent_ID: {
                    RemoveQualityEvent<STypePosition> message = RemoveQualityEvent<STypePosition>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__PrivateRemoveInt64Event_ID: {
                    PrivateRemoveQualityEvent<STypeInt64> message = PrivateRemoveQualityEvent<STypeInt64>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__RemoveInt64Event_ID: {
                    RemoveQualityEvent<STypeInt64> message = RemoveQualityEvent<STypeInt64>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__PrivateUpdateInt_ID: {
                    PrivateUpdateQualityEvent<STypeInt, int> message = PrivateUpdateQualityEvent<STypeInt, int>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__UpdateInt_ID: {
                    UpdateQualityEvent<STypeInt, int> message = UpdateQualityEvent<STypeInt, int>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__PrivateUpdateInt64_ID: {
                    PrivateUpdateQualityEvent<STypeInt64, long> message = PrivateUpdateQualityEvent<STypeInt64, long>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__UpdateInt64_ID: {
                    UpdateQualityEvent<STypeInt64, long> message = UpdateQualityEvent<STypeInt64, long>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__PrivateUpdateBool_ID: {
                    PrivateUpdateQualityEvent<STypeBool, int> message = PrivateUpdateQualityEvent<STypeBool, int>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__UpdateBool_ID: {
                    UpdateQualityEvent<STypeBool, int> message = UpdateQualityEvent<STypeBool, int>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__PrivateUpdateFloat_ID: {
                    PrivateUpdateQualityEvent<STypeFloat, double> message = PrivateUpdateQualityEvent<STypeFloat, double>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__UpdateFloat_ID: {
                    UpdateQualityEvent<STypeFloat, double> message = UpdateQualityEvent<STypeFloat, double>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__PrivateUpdateString_ID: {
                    PrivateUpdateStringEvent message = PrivateUpdateStringEvent.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__UpdateString_ID: {
                    UpdateStringEvent message = UpdateStringEvent.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__PrivateUpdateDataID_ID: {
                    PrivateUpdateQualityEvent<STypeDID, uint> message = PrivateUpdateQualityEvent<STypeDID, uint>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__UpdateDataID_ID: {
                    UpdateQualityEvent<STypeDID, uint> message = UpdateQualityEvent<STypeDID, uint>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__PrivateUpdateInstanceID_ID: {
                    PrivateUpdateQualityEvent<STypeIID, uint> message = PrivateUpdateQualityEvent<STypeIID, uint>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__UpdateInstanceID_ID: {
                    UpdateQualityEvent<STypeIID, uint> message = UpdateQualityEvent<STypeIID, uint>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__PrivateUpdatePosition_ID: {
                    PrivateUpdateQualityEvent<STypePosition, Position> message = PrivateUpdateQualityEvent<STypePosition, Position>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__UpdatePosition_ID: {
                    UpdateQualityEvent<STypePosition, Position> message = UpdateQualityEvent<STypePosition, Position>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__PrivateUpdateSkill_ID: {
                    PrivateUpdateQualityEvent<STypeSkill, Skill> message = PrivateUpdateQualityEvent<STypeSkill, Skill>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__UpdateSkill_ID: {
                    UpdateQualityEvent<STypeSkill, Skill> message = UpdateQualityEvent<STypeSkill, Skill>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__PrivateUpdateSkillLevel_ID: {
                    PrivateUpdateQualityEvent<STypeSkill, int> message = PrivateUpdateQualityEvent<STypeSkill, int>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__UpdateSkillLevel_ID: {
                    UpdateQualityEvent<STypeSkill, int> message = UpdateQualityEvent<STypeSkill, int>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__PrivateUpdateSkillAC_ID: {
                    PrivateUpdateQualityEvent<STypeSkill, SKILL_ADVANCEMENT_CLASS> message = PrivateUpdateQualityEvent<STypeSkill, SKILL_ADVANCEMENT_CLASS>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__UpdateSkillAC_ID: {
                    UpdateQualityEvent<STypeSkill, SKILL_ADVANCEMENT_CLASS> message = UpdateQualityEvent<STypeSkill, SKILL_ADVANCEMENT_CLASS>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__PrivateUpdateAttribute_ID: {
                    PrivateUpdateQualityEvent<STypeAttribute, Attribute> message = PrivateUpdateQualityEvent<STypeAttribute, Attribute>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__UpdateAttribute_ID: {
                    UpdateQualityEvent<STypeAttribute, Attribute> message = UpdateQualityEvent<STypeAttribute, Attribute>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__PrivateUpdateAttributeLevel_ID: {
                    PrivateUpdateQualityEvent<STypeAttribute, int> message = PrivateUpdateQualityEvent<STypeAttribute, int>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__UpdateAttributeLevel_ID: {
                    UpdateQualityEvent<STypeAttribute, int> message = UpdateQualityEvent<STypeAttribute, int>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__PrivateUpdateAttribute2nd_ID: {
                    PrivateUpdateQualityEvent<STypeAttribute2nd, SecondaryAttribute> message = PrivateUpdateQualityEvent<STypeAttribute2nd, SecondaryAttribute>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__UpdateAttribute2nd_ID: {
                    UpdateQualityEvent<STypeAttribute2nd, SecondaryAttribute> message = UpdateQualityEvent<STypeAttribute2nd, SecondaryAttribute>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__PrivateUpdateAttribute2ndLevel_ID: {
                    PrivateUpdateQualityEvent<STypeAttribute2nd, int> message = PrivateUpdateQualityEvent<STypeAttribute2nd, int>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__UpdateAttribute2ndLevel_ID: {
                    UpdateQualityEvent<STypeAttribute2nd, int> message = UpdateQualityEvent<STypeAttribute2nd, int>.read(opcode, messageDataReader);
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

    public class PrivateRemoveQualityEvent<TSType> : Message {
        public PacketOpcode opcode;
        public byte wts;
        public TSType stype;

        public static PrivateRemoveQualityEvent<TSType> read(PacketOpcode opcode, BinaryReader binaryReader) {
            PrivateRemoveQualityEvent<TSType> newObj = new PrivateRemoveQualityEvent<TSType>();
            newObj.opcode = opcode;
            newObj.wts = binaryReader.ReadByte();
            newObj.stype = (TSType)Enum.ToObject(typeof(TSType), binaryReader.ReadUInt32());
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("opcode = " + opcode);
            rootNode.Nodes.Add("wts = " + wts);
            rootNode.Nodes.Add("stype = " + stype);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class RemoveQualityEvent<TSType> : Message {
        public PacketOpcode opcode;
        public byte wts;
        public uint sender;
        public TSType stype;

        public static RemoveQualityEvent<TSType> read(PacketOpcode opcode, BinaryReader binaryReader) {
            RemoveQualityEvent<TSType> newObj = new RemoveQualityEvent<TSType>();
            newObj.opcode = opcode;
            newObj.wts = binaryReader.ReadByte();
            newObj.sender = binaryReader.ReadUInt32();
            newObj.stype = (TSType)Enum.ToObject(typeof(TSType), binaryReader.ReadUInt32());
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("opcode = " + opcode);
            rootNode.Nodes.Add("wts = " + wts);
            rootNode.Nodes.Add("sender = " + sender);
            rootNode.Nodes.Add("stype = " + stype);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class PrivateUpdateQualityEvent<TSType, T> : Message {
        public PacketOpcode opcode;
        public byte wts;
        public TSType stype;
        public T val;

        public static PrivateUpdateQualityEvent<TSType, T> read(PacketOpcode opcode, BinaryReader binaryReader) {
            PrivateUpdateQualityEvent<TSType, T> newObj = new PrivateUpdateQualityEvent<TSType, T>();
            newObj.opcode = opcode;
            newObj.wts = binaryReader.ReadByte();
            newObj.stype = (TSType)Enum.ToObject(typeof(TSType), binaryReader.ReadUInt32());
            newObj.val = Util.readers[typeof(T)](binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("opcode = " + opcode);
            rootNode.Nodes.Add("wts = " + wts);
            rootNode.Nodes.Add("stype = " + stype);
            // TODO: Need to contribute to this node for types that are capable of doing so
            if (val is uint)
            {
                rootNode.Nodes.Add("val = " + Utility.FormatHex((uint)(object)val));
            }
            else
            {
                rootNode.Nodes.Add("val = " + val); 
            }
            treeView.Nodes.Add(rootNode);
        }
    }

    public class UpdateQualityEvent<TSType, T> : Message {
        public PacketOpcode opcode;
        public byte wts;
        public uint sender;
        public TSType stype;
        public T val;

        public static UpdateQualityEvent<TSType, T> read(PacketOpcode opcode, BinaryReader binaryReader) {
            UpdateQualityEvent<TSType, T> newObj = new UpdateQualityEvent<TSType, T>();
            newObj.opcode = opcode;
            newObj.wts = binaryReader.ReadByte();
            newObj.sender = binaryReader.ReadUInt32();
            newObj.stype = (TSType)Enum.ToObject(typeof(TSType), binaryReader.ReadUInt32());
            newObj.val = Util.readers[typeof(T)](binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("opcode = " + opcode);
            rootNode.Nodes.Add("wts = " + wts);
            rootNode.Nodes.Add("sender = " + Utility.FormatHex(sender));
            rootNode.Nodes.Add("stype = " + stype);
            // TODO: Need to contribute to this node for types that are capable of doing so
            if (val is uint)
            {
                rootNode.Nodes.Add("val = " + Utility.FormatHex((uint)(object)val));
            }
            else
            {
                rootNode.Nodes.Add("val = " + val);
            }
            treeView.Nodes.Add(rootNode);
        }
    }

    // Note: I could not find this message in any pcaps but this code should parse the message based on the client code structure.
    public class PrivateUpdateStringEvent : Message
    {
        public PacketOpcode opcode;
        public byte wts;
        public STypeString stype;
        public PStringChar val;

        public static PrivateUpdateStringEvent read(PacketOpcode opcode, BinaryReader binaryReader)
        {
            PrivateUpdateStringEvent newObj = new PrivateUpdateStringEvent();
            newObj.opcode = opcode;
            newObj.wts = binaryReader.ReadByte();
            newObj.stype = (STypeString)binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            newObj.val = PStringChar.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("opcode = " + opcode);
            rootNode.Nodes.Add("wts = " + wts);
            rootNode.Nodes.Add("stype = " + stype);
            rootNode.Nodes.Add("val = " + val);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class UpdateStringEvent : Message
    {
        public PacketOpcode opcode;
        public byte wts;
        public STypeString stype;
        public uint sender;
        public PStringChar val;

        public static UpdateStringEvent read(PacketOpcode opcode, BinaryReader binaryReader)
        {
            UpdateStringEvent newObj = new UpdateStringEvent();
            newObj.opcode = opcode;
            newObj.wts = binaryReader.ReadByte();
            newObj.stype = (STypeString)binaryReader.ReadUInt32();
            newObj.sender = binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            newObj.val = PStringChar.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("opcode = " + opcode);
            rootNode.Nodes.Add("wts = " + wts);
            rootNode.Nodes.Add("stype = " + stype);
            rootNode.Nodes.Add("sender = " + Utility.FormatHex(sender));
            rootNode.Nodes.Add("val = " + val);
            treeView.Nodes.Add(rootNode);
        }
    }
}
