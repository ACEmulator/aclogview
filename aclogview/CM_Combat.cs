using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using aclogview;

public class CM_Combat : MessageProcessor {

    public override bool acceptMessageData(BinaryReader messageDataReader, TreeView outputTreeView) {
        bool handled = true;

        PacketOpcode opcode = Util.readOpcode(messageDataReader);
        switch (opcode) {
            case PacketOpcode.Evt_Combat__CancelAttack_ID:
            case PacketOpcode.Evt_Combat__CommenceAttack_ID: {
                    EmptyMessage message = new EmptyMessage(opcode);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            // TODO: Evt_Combat__UntargetedMeleeAttack_ID
            case PacketOpcode.Evt_Combat__TargetedMeleeAttack_ID: {
                    TargetedMeleeAttack message = TargetedMeleeAttack.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Combat__TargetedMissileAttack_ID: {
                    TargetedMissileAttack message = TargetedMissileAttack.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            // TODO: Evt_Combat__UntargetedMissileAttack_ID
            case PacketOpcode.Evt_Combat__ChangeCombatMode_ID: {
                    ChangeCombatMode message = ChangeCombatMode.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.ATTACK_DONE_EVENT:
                {
                    AttackDoneEvent message = AttackDoneEvent.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Combat__QueryHealth_ID: {
                    QueryHealth message = QueryHealth.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Combat__QueryHealthResponse_ID: {
                    QueryHealthResponse message = QueryHealthResponse.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.ATTACKER_NOTIFICATION_EVENT:
                {
                    AttackerNotificationEvent message = AttackerNotificationEvent.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.DEFENDER_NOTIFICATION_EVENT:
                {
                    DefenderNotificationEvent message = DefenderNotificationEvent.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.EVASION_ATTACKER_NOTIFICATION_EVENT:
                {
                    EvasionAttackerNotificationEvent message = EvasionAttackerNotificationEvent.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.EVASION_DEFENDER_NOTIFICATION_EVENT:
                {
                    EvasionDefenderNotificationEvent message = EvasionDefenderNotificationEvent.read(messageDataReader);
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

    public class TargetedMeleeAttack : Message {
        public uint i_targetID;
        public ATTACK_HEIGHT i_ah;
        public float i_power_level;

        public static TargetedMeleeAttack read(BinaryReader binaryReader) {
            TargetedMeleeAttack newObj = new TargetedMeleeAttack();
            newObj.i_targetID = binaryReader.ReadUInt32();
            newObj.i_ah = (ATTACK_HEIGHT)binaryReader.ReadUInt32();
            newObj.i_power_level = binaryReader.ReadSingle();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_targetID = " + Utility.FormatHex(this.i_targetID));            
            
            rootNode.Nodes.Add("i_ah = " + i_ah);
            rootNode.Nodes.Add("i_power_level = " + i_power_level);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class TargetedMissileAttack : Message {
        public uint i_targetID;
        public ATTACK_HEIGHT i_ah;
        public float i_accuracy_level;

        public static TargetedMissileAttack read(BinaryReader binaryReader) {
            TargetedMissileAttack newObj = new TargetedMissileAttack();
            newObj.i_targetID = binaryReader.ReadUInt32();
            newObj.i_ah = (ATTACK_HEIGHT)binaryReader.ReadUInt32();
            newObj.i_accuracy_level = binaryReader.ReadSingle();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_target_id = " + Utility.FormatHex(this.i_targetID));
            rootNode.Nodes.Add("i_ah = " + i_ah);
            rootNode.Nodes.Add("i_accuracy_level = " + i_accuracy_level);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class ChangeCombatMode : Message {
        public COMBAT_MODE i_mode;

        public static ChangeCombatMode read(BinaryReader binaryReader) {
            ChangeCombatMode newObj = new ChangeCombatMode();
            newObj.i_mode = (COMBAT_MODE)binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_mode = " + i_mode);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class AttackDoneEvent : Message
    {
        public uint etype;

        public static AttackDoneEvent read(BinaryReader binaryReader)
        {
            AttackDoneEvent newObj = new AttackDoneEvent();
            newObj.etype = binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("etype = " + (WERROR)etype);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class QueryHealth : Message {
        public uint i_target;

        public static QueryHealth read(BinaryReader binaryReader) {
            QueryHealth newObj = new QueryHealth();
            newObj.i_target = binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_target_ = " + Utility.FormatHex(this.i_target));
            treeView.Nodes.Add(rootNode);
        }
    }

    public class QueryHealthResponse : Message {
        public uint target;
        public float health;

        public static QueryHealthResponse read(BinaryReader binaryReader) {
            QueryHealthResponse newObj = new QueryHealthResponse();
            newObj.target = binaryReader.ReadUInt32();
            newObj.health = binaryReader.ReadSingle();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("target = " +Utility.FormatHex(this.target));
            rootNode.Nodes.Add("health = " + health);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class AttackerNotificationEvent : Message
    {
        public PStringChar defenders_name;
        public uint damage_type;
        public double severity; // 0.0 to 1.0
        public uint damage;
        public uint critical;
        public ulong attack_conditions;

        public static AttackerNotificationEvent read(BinaryReader binaryReader)
        {
            AttackerNotificationEvent newObj = new AttackerNotificationEvent();
            newObj.defenders_name = PStringChar.read(binaryReader);
            newObj.damage_type = binaryReader.ReadUInt32();
            newObj.severity = binaryReader.ReadDouble();
            newObj.damage = binaryReader.ReadUInt32();
            newObj.critical = binaryReader.ReadUInt32();
            newObj.attack_conditions = binaryReader.ReadUInt64();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("defenders_name = " + defenders_name);
            rootNode.Nodes.Add("damage_type = " + (DAMAGE_TYPE)damage_type);
            rootNode.Nodes.Add("severity = " + severity);
            rootNode.Nodes.Add("damage = " + damage);
            rootNode.Nodes.Add("critical = " + critical);
            rootNode.Nodes.Add("attackConditions = " + (AttackConditions)attack_conditions);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class DefenderNotificationEvent : Message
    {
        public PStringChar attackers_name;
        public uint damage_type;
        public double severity; // 0.0 to 1.0
        public uint damage;
        public uint part;
        public uint critical;
        public ulong attack_conditions;

        public static DefenderNotificationEvent read(BinaryReader binaryReader)
        {
            DefenderNotificationEvent newObj = new DefenderNotificationEvent();
            newObj.attackers_name = PStringChar.read(binaryReader);
            newObj.damage_type = binaryReader.ReadUInt32();
            newObj.severity = binaryReader.ReadDouble();
            newObj.damage = binaryReader.ReadUInt32();
            newObj.part = binaryReader.ReadUInt32();
            newObj.critical = binaryReader.ReadUInt32();
            newObj.attack_conditions = binaryReader.ReadUInt64();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("attackers_name = " + attackers_name);
            rootNode.Nodes.Add("damage_type = " + (DAMAGE_TYPE)damage_type);
            rootNode.Nodes.Add("severity = " + severity);
            rootNode.Nodes.Add("damage = " + damage);
            rootNode.Nodes.Add("part = " + (BodyPart)part);
            rootNode.Nodes.Add("critical = " + critical);
            rootNode.Nodes.Add("attackConditions = " + (AttackConditions)attack_conditions);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class EvasionAttackerNotificationEvent : Message
    {
        public PStringChar defenders_name;

        public static EvasionAttackerNotificationEvent read(BinaryReader binaryReader)
        {
            EvasionAttackerNotificationEvent newObj = new EvasionAttackerNotificationEvent();
            newObj.defenders_name = PStringChar.read(binaryReader);
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("defenders_name = " + defenders_name);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class EvasionDefenderNotificationEvent : Message
    {
        public PStringChar attackers_name;

        public static EvasionDefenderNotificationEvent read(BinaryReader binaryReader)
        {
            EvasionDefenderNotificationEvent newObj = new EvasionDefenderNotificationEvent();
            newObj.attackers_name = PStringChar.read(binaryReader);
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("attackers_name = " + attackers_name);
            treeView.Nodes.Add(rootNode);
        }
    }
}
