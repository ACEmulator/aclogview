using aclogview;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using PropertyInt = aclogview.Enum_Helpers.PropertyInt;

public class CM_Qualities : MessageProcessor {

    public override bool acceptMessageData(BinaryReader messageDataReader, TreeView outputTreeView) {
        var handled = true;

        var opcode = Util.readOpcode(messageDataReader);
        switch (opcode) {
            case PacketOpcode.Evt_Qualities__PrivateRemoveIntEvent_ID: {
                    var message = PrivateRemoveQualityEvent<STypeInt>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__RemoveIntEvent_ID: {
                    var message = RemoveQualityEvent<STypeInt>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__PrivateRemoveBoolEvent_ID: {
                    var message = PrivateRemoveQualityEvent<PropertyBool>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__RemoveBoolEvent_ID: {
                    var message = RemoveQualityEvent<PropertyBool>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__PrivateRemoveFloatEvent_ID: {
                    var message = PrivateRemoveQualityEvent<PropertyFloat>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__RemoveFloatEvent_ID: {
                    var message = RemoveQualityEvent<PropertyFloat>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__PrivateRemoveStringEvent_ID: {
                    var message = PrivateRemoveQualityEvent<PropertyString>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__RemoveStringEvent_ID: {
                    var message = RemoveQualityEvent<PropertyString>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__PrivateRemoveDataIDEvent_ID: {
                    var message = PrivateRemoveQualityEvent<PropertyDataId>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__RemoveDataIDEvent_ID: {
                    var message = RemoveQualityEvent<PropertyDataId>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__PrivateRemoveInstanceIDEvent_ID: {
                    var message = PrivateRemoveQualityEvent<PropertyInstanceId>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__RemoveInstanceIDEvent_ID: {
                    var message = RemoveQualityEvent<PropertyInstanceId>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__PrivateRemovePositionEvent_ID: {
                    var message = PrivateRemoveQualityEvent<PositionType>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__RemovePositionEvent_ID: {
                    var message = RemoveQualityEvent<PositionType>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__PrivateRemoveInt64Event_ID: {
                    var message = PrivateRemoveQualityEvent<PropertyInt64>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__RemoveInt64Event_ID: {
                    var message = RemoveQualityEvent<PropertyInt64>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__PrivateUpdateInt_ID: {
                    var message = PrivateUpdateQualityEvent<STypeInt, int>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__UpdateInt_ID: {
                    var message = UpdateQualityEvent<STypeInt, int>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__PrivateUpdateInt64_ID: {
                    var message = PrivateUpdateQualityEvent<PropertyInt64, long>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__UpdateInt64_ID: {
                    var message = UpdateQualityEvent<PropertyInt64, long>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__PrivateUpdateBool_ID: {
                    var message = PrivateUpdateQualityEvent<PropertyBool, int>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__UpdateBool_ID: {
                    var message = UpdateQualityEvent<PropertyBool, int>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__PrivateUpdateFloat_ID: {
                    var message = PrivateUpdateQualityEvent<PropertyFloat, double>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__UpdateFloat_ID: {
                    var message = UpdateQualityEvent<PropertyFloat, double>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__PrivateUpdateString_ID: {
                    var message = PrivateUpdateStringEvent.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__UpdateString_ID: {
                    var message = UpdateStringEvent.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__PrivateUpdateDataID_ID: {
                    var message = PrivateUpdateQualityEvent<PropertyDataId, uint>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__UpdateDataID_ID: {
                    var message = UpdateQualityEvent<PropertyDataId, uint>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__PrivateUpdateInstanceID_ID: {
                    var message = PrivateUpdateQualityEvent<PropertyInstanceId, uint>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__UpdateInstanceID_ID: {
                    var message = UpdateQualityEvent<PropertyInstanceId, uint>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__PrivateUpdatePosition_ID: {
                    var message = PrivateUpdateQualityEvent<PositionType, Position>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__UpdatePosition_ID: {
                    var message = UpdateQualityEvent<PositionType, Position>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__PrivateUpdateSkill_ID: {
                    var message = PrivateUpdateQualityEvent<STypeSkill, Skill>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__UpdateSkill_ID: {
                    var message = UpdateQualityEvent<STypeSkill, Skill>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__PrivateUpdateSkillLevel_ID: {
                    var message = PrivateUpdateQualityEvent<STypeSkill, int>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__UpdateSkillLevel_ID: {
                    var message = UpdateQualityEvent<STypeSkill, int>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__PrivateUpdateSkillAC_ID: {
                    var message = PrivateUpdateQualityEvent<STypeSkill, SkillStatus>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__UpdateSkillAC_ID: {
                    var message = UpdateQualityEvent<STypeSkill, SkillStatus>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__PrivateUpdateAttribute_ID: {
                    var message = PrivateUpdateQualityEvent<PropertyAttribute, Attribute>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__UpdateAttribute_ID: {
                    var message = UpdateQualityEvent<PropertyAttribute, Attribute>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__PrivateUpdateAttributeLevel_ID: {
                    var message = PrivateUpdateQualityEvent<PropertyAttribute, int>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__UpdateAttributeLevel_ID: {
                    var message = UpdateQualityEvent<PropertyAttribute, int>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__PrivateUpdateAttribute2nd_ID: {
                    var message = PrivateUpdateQualityEvent<PropertyAttribute2nd, SecondaryAttribute>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__UpdateAttribute2nd_ID: {
                    var message = UpdateQualityEvent<PropertyAttribute2nd, SecondaryAttribute>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__PrivateUpdateAttribute2ndLevel_ID: {
                    var message = PrivateUpdateQualityEvent<PropertyAttribute2nd, int>.read(opcode, messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Qualities__UpdateAttribute2ndLevel_ID: {
                    var message = UpdateQualityEvent<PropertyAttribute2nd, int>.read(opcode, messageDataReader);
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
            var newObj = new PrivateRemoveQualityEvent<TSType>();
            newObj.opcode = opcode;
            newObj.wts = binaryReader.ReadByte();
            newObj.stype = (TSType)Enum.ToObject(typeof(TSType), binaryReader.ReadUInt32());
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            var rootNode = new TreeNode(opcode.ToString());
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo{ DataType = DataType.Opcode });
            rootNode.Nodes.Add("wts = " + wts);
            ContextInfo.AddToList(new ContextInfo{ Length = 1 } );
            rootNode.Nodes.Add("stype = " + stype);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class RemoveQualityEvent<TSType> : Message {
        public PacketOpcode opcode;
        public byte wts;
        public uint sender;
        public TSType stype;

        public static RemoveQualityEvent<TSType> read(PacketOpcode opcode, BinaryReader binaryReader) {
            var newObj = new RemoveQualityEvent<TSType>();
            newObj.opcode = opcode;
            newObj.wts = binaryReader.ReadByte();
            newObj.sender = binaryReader.ReadUInt32();
            newObj.stype = (TSType)Enum.ToObject(typeof(TSType), binaryReader.ReadUInt32());
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            var rootNode = new TreeNode(opcode.ToString());
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Opcode });
            rootNode.Nodes.Add("wts = " + wts);
            ContextInfo.AddToList(new ContextInfo { Length = 1 });
            rootNode.Nodes.Add("sender = " + sender);
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            rootNode.Nodes.Add("stype = " + stype);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class PrivateUpdateQualityEvent<TSType, T> : Message {
        public PacketOpcode opcode;
        public byte wts;
        public TSType stype;
        public T val;
        public int valLength;

        public static PrivateUpdateQualityEvent<TSType, T> read(PacketOpcode opcode, BinaryReader binaryReader) {
            var newObj = new PrivateUpdateQualityEvent<TSType, T>();
            newObj.opcode = opcode;
            newObj.wts = binaryReader.ReadByte();
            newObj.stype = (TSType)Enum.ToObject(typeof(TSType), binaryReader.ReadUInt32());
            var valStartPosition = binaryReader.BaseStream.Position;
            newObj.val = Util.readers[typeof(T)](binaryReader);
            newObj.valLength = (int)(binaryReader.BaseStream.Position - valStartPosition);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            var rootNode = new TreeNode(opcode.ToString());
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Opcode });
            rootNode.Nodes.Add("wts = " + wts);
            ContextInfo.AddToList(new ContextInfo { Length = 1 });
            rootNode.Nodes.Add("stype = " + stype);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            var updateValues = new QualityValues{opcode = opcode, stype = stype, val = val, valLength = valLength};
            updateValues.ContributeValuesToTreeView(rootNode);
            rootNode.ExpandAll();
            treeView.Nodes.Add(rootNode);
        }
    }

    public class UpdateQualityEvent<TSType, T> : Message {
        public PacketOpcode opcode;
        public byte wts;
        public uint sender;
        public TSType stype;
        public T val;
        public int valLength;

        public static UpdateQualityEvent<TSType, T> read(PacketOpcode opcode, BinaryReader binaryReader) {
            var newObj = new UpdateQualityEvent<TSType, T>();
            newObj.opcode = opcode;
            newObj.wts = binaryReader.ReadByte();
            newObj.sender = binaryReader.ReadUInt32();
            newObj.stype = (TSType)Enum.ToObject(typeof(TSType), binaryReader.ReadUInt32());
            var valStartPosition = binaryReader.BaseStream.Position;
            newObj.val = Util.readers[typeof(T)](binaryReader);
            newObj.valLength = (int)(binaryReader.BaseStream.Position - valStartPosition);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            var rootNode = new TreeNode(opcode.ToString());
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Opcode });
            rootNode.Nodes.Add("wts = " + wts);
            ContextInfo.AddToList(new ContextInfo { Length = 1 });
            rootNode.Nodes.Add("sender = " + Utility.FormatHex(sender));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            rootNode.Nodes.Add("stype = " + stype);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            var updateValues = new QualityValues { opcode = opcode, stype = stype, val = val, valLength = valLength };
            updateValues.ContributeValuesToTreeView(rootNode);
            rootNode.ExpandAll();
            treeView.Nodes.Add(rootNode);
        }
    }

    public class QualityValues
    {
        public PacketOpcode opcode;
        public object stype;
        public object val;
        public int valLength;

        public void ContributeValuesToTreeView(TreeNode rootNode)
        {
            switch (opcode)
            {
                case PacketOpcode.Evt_Qualities__PrivateUpdateInt_ID:
                case PacketOpcode.Evt_Qualities__UpdateInt_ID:
                    PropertyInt.contributeToTreeNode(rootNode, this);
                    break;
                case PacketOpcode.Evt_Qualities__PrivateUpdateInt64_ID:
                case PacketOpcode.Evt_Qualities__UpdateInt64_ID:
                    rootNode.Nodes.Add("val = " + (long)val);
                    ContextInfo.AddToList(new ContextInfo { Length = 8 });
                    break;
                case PacketOpcode.Evt_Qualities__PrivateUpdateBool_ID:
                case PacketOpcode.Evt_Qualities__UpdateBool_ID:
                    rootNode.Nodes.Add("val = " + Convert.ToBoolean(val));
                    ContextInfo.AddToList(new ContextInfo { Length = 4 });
                    break;
                case PacketOpcode.Evt_Qualities__PrivateUpdateFloat_ID:
                case PacketOpcode.Evt_Qualities__UpdateFloat_ID:
                    rootNode.Nodes.Add("val = " + (double)val);
                    ContextInfo.AddToList(new ContextInfo { Length = 8 });
                    break;
                case PacketOpcode.Evt_Qualities__PrivateUpdateDataID_ID:
                case PacketOpcode.Evt_Qualities__UpdateDataID_ID:
                    rootNode.Nodes.Add("val = " + Utility.FormatHex((uint)val));
                    ContextInfo.AddToList(new ContextInfo { Length = 4, DataType = DataType.DataID });
                    break;
                case PacketOpcode.Evt_Qualities__PrivateUpdateInstanceID_ID:
                case PacketOpcode.Evt_Qualities__UpdateInstanceID_ID:
                    rootNode.Nodes.Add("val = " + Utility.FormatHex((uint)val));
                    ContextInfo.AddToList(new ContextInfo { Length = 4, DataType = DataType.ObjectID });
                    break;
                case PacketOpcode.Evt_Qualities__PrivateUpdateSkillLevel_ID:
                case PacketOpcode.Evt_Qualities__UpdateSkillLevel_ID:
                case PacketOpcode.Evt_Qualities__PrivateUpdateAttributeLevel_ID:
                case PacketOpcode.Evt_Qualities__UpdateAttributeLevel_ID:
                case PacketOpcode.Evt_Qualities__PrivateUpdateAttribute2ndLevel_ID:
                case PacketOpcode.Evt_Qualities__UpdateAttribute2ndLevel_ID:
                    rootNode.Nodes.Add("val = " + (int)val);
                    ContextInfo.AddToList(new ContextInfo { Length = 4 });
                    break;
                case PacketOpcode.Evt_Qualities__PrivateUpdateSkillAC_ID:
                case PacketOpcode.Evt_Qualities__UpdateSkillAC_ID:
                    rootNode.Nodes.Add("val = " + val);
                    ContextInfo.AddToList(new ContextInfo { Length = 4 });
                    break;
                default:
                    var valNode = rootNode.Nodes.Add(val.GetType().Name + " = ");
                    ContextInfo.AddToList(new ContextInfo { Length = valLength }, updateDataIndex: false);
                    var methodInfo = val.GetType().GetMethod("contributeToTreeNode");
                    var args = new object[] { valNode };
                    methodInfo.Invoke(val, args);
                    break;
            }
        }
    }

    // Note: I could not find this message in any pcaps but this code should parse the message based on the client code structure.
    public class PrivateUpdateStringEvent : Message
    {
        public PacketOpcode opcode;
        public byte wts;
        public PropertyString stype;
        public byte padding;
        public PStringChar val;

        public static PrivateUpdateStringEvent read(PacketOpcode opcode, BinaryReader binaryReader)
        {
            var newObj = new PrivateUpdateStringEvent();
            newObj.opcode = opcode;
            newObj.wts = binaryReader.ReadByte();
            newObj.stype = (PropertyString)binaryReader.ReadUInt32();
            newObj.padding = Util.readToAlign(binaryReader);
            newObj.val = PStringChar.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            var rootNode = new TreeNode(opcode.ToString());
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Opcode });
            rootNode.Nodes.Add("wts = " + wts);
            ContextInfo.AddToList(new ContextInfo { Length = 1 });
            rootNode.Nodes.Add("stype = " + stype);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            // Skip padding
            ContextInfo.DataIndex += padding;
            rootNode.Nodes.Add("val = " + val);
            ContextInfo.AddToList(new ContextInfo { Length = val.Length, DataType = DataType.Serialized_AsciiString });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class UpdateStringEvent : Message
    {
        public PacketOpcode opcode;
        public byte wts;
        public PropertyString stype;
        public uint sender;
        public byte padding;
        public PStringChar val;

        public static UpdateStringEvent read(PacketOpcode opcode, BinaryReader binaryReader)
        {
            var newObj = new UpdateStringEvent();
            newObj.opcode = opcode;
            newObj.wts = binaryReader.ReadByte();
            newObj.stype = (PropertyString)binaryReader.ReadUInt32();
            newObj.sender = binaryReader.ReadUInt32();
            newObj.padding = Util.readToAlign(binaryReader);
            newObj.val = PStringChar.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            var rootNode = new TreeNode(opcode.ToString());
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Opcode });
            rootNode.Nodes.Add("wts = " + wts);
            ContextInfo.AddToList(new ContextInfo { Length = 1 });
            rootNode.Nodes.Add("stype = " + stype);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            rootNode.Nodes.Add("sender = " + Utility.FormatHex(sender));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            // Skip padding
            ContextInfo.DataIndex += padding;
            rootNode.Nodes.Add("val = " + val);
            ContextInfo.AddToList(new ContextInfo { Length = val.Length, DataType = DataType.Serialized_AsciiString });
            treeView.Nodes.Add(rootNode);
        }
    }
}
