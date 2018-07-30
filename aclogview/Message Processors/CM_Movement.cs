using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using aclogview;

using ACE.Entity.Enum;

public class CM_Movement : MessageProcessor {

    public override bool acceptMessageData(BinaryReader messageDataReader, TreeView outputTreeView) {
        bool handled = true;

        PacketOpcode opcode = Util.readOpcode(messageDataReader);
        switch (opcode) {
            case PacketOpcode.Evt_Movement__PositionAndMovement:
                {
                    PositionAndMovement message = PositionAndMovement.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Movement__Jump_ID: {
                    Jump message = Jump.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Movement__MoveToState_ID: {
                    MoveToState message = MoveToState.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Movement__DoMovementCommand_ID: {
                    DoMovementCommand message = DoMovementCommand.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            // TODO: PacketOpcode.Evt_Movement__TurnEvent_ID
            // TODO: PacketOpcode.Evt_Movement__TurnToEvent_ID
            case PacketOpcode.Evt_Movement__StopMovementCommand_ID: {
                    StopMovementCommand message = StopMovementCommand.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Movement__UpdatePosition_ID: {
                    UpdatePosition message = UpdatePosition.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Movement__MovementEvent_ID: {
                    MovementEvent message = MovementEvent.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Movement__AutonomyLevel_ID: {
                    AutonomyLevel message = AutonomyLevel.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Movement__AutonomousPosition_ID: {
                    AutonomousPosition message = AutonomousPosition.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Movement__Jump_NonAutonomous_ID: {
                    Jump_NonAutonomous message = Jump_NonAutonomous.read(messageDataReader);
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

    public class JumpPack {
        public float extent;
        public Vector3 velocity;
        public Position position;
        public ushort instance_timestamp;
        public ushort server_control_timestamp;
        public ushort teleport_timestamp;
        public ushort force_position_ts;

        public static JumpPack read(BinaryReader binaryReader) {
            JumpPack newObj = new JumpPack();
            newObj.extent = binaryReader.ReadSingle();
            newObj.velocity = Vector3.read(binaryReader);
            newObj.position = Position.read(binaryReader);
            newObj.instance_timestamp = binaryReader.ReadUInt16();
            newObj.server_control_timestamp = binaryReader.ReadUInt16();
            newObj.teleport_timestamp = binaryReader.ReadUInt16();
            newObj.force_position_ts = binaryReader.ReadUInt16();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node) {
            node.Nodes.Add("extent = " + extent);
            node.Nodes.Add("velocity = " + velocity);
            TreeNode positionNode = node.Nodes.Add("position = ");
            position.contributeToTreeNode(positionNode);
            node.Nodes.Add("instance_timestamp = " + instance_timestamp);
            node.Nodes.Add("server_control_timestamp = " + server_control_timestamp);
            node.Nodes.Add("teleport_timestamp = " + teleport_timestamp);
            node.Nodes.Add("force_position_ts = " + force_position_ts);
        }
    }

    public class Jump : Message {
        public JumpPack i_jp;

        public static Jump read(BinaryReader binaryReader) {
            Jump newObj = new Jump();
            newObj.i_jp = JumpPack.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            TreeNode jumpNode = rootNode.Nodes.Add("i_jp = ");
            i_jp.contributeToTreeNode(jumpNode);
            jumpNode.Expand();
            treeView.Nodes.Add(rootNode);
        }
    }

    public class MoveToState : Message {
        public RawMotionState raw_motion_state;
        public Position position;
        public ushort instance_timestamp;
        public ushort server_control_timestamp;
        public ushort teleport_timestamp;
        public ushort force_position_ts;
        public bool contact;
        public bool longjump_mode;

        public static MoveToState read(BinaryReader binaryReader) {
            MoveToState newObj = new MoveToState();
            newObj.raw_motion_state = RawMotionState.read(binaryReader);
            newObj.position = Position.read(binaryReader);
            newObj.instance_timestamp = binaryReader.ReadUInt16();
            newObj.server_control_timestamp = binaryReader.ReadUInt16();
            newObj.teleport_timestamp = binaryReader.ReadUInt16();
            newObj.force_position_ts = binaryReader.ReadUInt16();

            byte flags = binaryReader.ReadByte();

            newObj.contact = (flags & (1 << 0)) != 0;
            newObj.longjump_mode = (flags & (1 << 1)) != 0;

            Util.readToAlign(binaryReader);

            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            TreeNode rawMotionStateNode = rootNode.Nodes.Add("raw_motion_state = ");
            raw_motion_state.contributeToTreeNode(rawMotionStateNode);
            TreeNode posNode = rootNode.Nodes.Add("position = ");
            position.contributeToTreeNode(posNode);
            rootNode.Nodes.Add("instance_timestamp = " + instance_timestamp);
            rootNode.Nodes.Add("server_control_timestamp = " + server_control_timestamp);
            rootNode.Nodes.Add("teleport_timestamp = " + teleport_timestamp);
            rootNode.Nodes.Add("force_position_ts = " + force_position_ts);
            rootNode.Nodes.Add("contact = " + contact);
            rootNode.Nodes.Add("longjump_mode = " + longjump_mode);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class DoMovementCommand : Message {
        public uint i_motion;
        public float i_speed;
        public HoldKey i_hold_key;

        public static DoMovementCommand read(BinaryReader binaryReader) {
            DoMovementCommand newObj = new DoMovementCommand();
            newObj.i_motion = binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            newObj.i_speed = binaryReader.ReadSingle();
            Util.readToAlign(binaryReader);
            newObj.i_hold_key = (HoldKey)binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_motion = " + i_motion);
            rootNode.Nodes.Add("i_speed = " + i_speed);
            rootNode.Nodes.Add("i_hold_key = " + i_hold_key);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class StopMovementCommand : Message {
        public uint i_motion;
        public HoldKey i_hold_key;

        public static StopMovementCommand read(BinaryReader binaryReader) {
            StopMovementCommand newObj = new StopMovementCommand();
            newObj.i_motion = binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            newObj.i_hold_key = (HoldKey)binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_motion = " + i_motion);
            rootNode.Nodes.Add("i_hold_key = " + i_hold_key);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class PositionPack
    {
        public enum PackBitfield
        {
            HasVelocity       = 0x1,
            HasPlacementID    = 0x2,
            IsGrounded        = 0x4,
            OrientationHasNoW = 0x8,
            OrientationHasNoX = 0x10,
            OrientationHasNoY = 0x20,
            OrientationHasNoZ = 0x40,
        }

        public uint bitfield;
        public Position position;
        public Vector3 velocity;
        public uint placement_id;
        public bool has_contact;
        public ushort instance_timestamp;
        public ushort position_timestamp;
        public ushort teleport_timestamp;
        public ushort force_position_timestamp;
        public List<string> packedItems; // For display purposes

        public static PositionPack read(BinaryReader binaryReader)
        {
            PositionPack newObj = new PositionPack();
            newObj.packedItems = new List<string>();
            newObj.bitfield = binaryReader.ReadUInt32();
            newObj.position = Position.readOrigin(binaryReader);

            if ((newObj.bitfield & (uint)PackBitfield.OrientationHasNoW) == 0)
            {
                newObj.position.frame.qw = binaryReader.ReadSingle();
            }
            else
            {
                newObj.packedItems.Add(PackBitfield.OrientationHasNoW.ToString());
            }
            if ((newObj.bitfield & (uint)PackBitfield.OrientationHasNoX) == 0)
            {
                newObj.position.frame.qx = binaryReader.ReadSingle();
            }
            else
            {
                newObj.packedItems.Add(PackBitfield.OrientationHasNoX.ToString());
            }
            if ((newObj.bitfield & (uint)PackBitfield.OrientationHasNoY) == 0)
            {
                newObj.position.frame.qy = binaryReader.ReadSingle();
            }
            else
            {
                newObj.packedItems.Add(PackBitfield.OrientationHasNoY.ToString());
            }
            if ((newObj.bitfield & (uint)PackBitfield.OrientationHasNoZ) == 0)
            {
                newObj.position.frame.qz = binaryReader.ReadSingle();
            }
            else
            {
                newObj.packedItems.Add(PackBitfield.OrientationHasNoZ.ToString());
            }

            newObj.position.frame.cache();

            if ((newObj.bitfield & (uint)PackBitfield.HasVelocity) != 0)
            {
                newObj.velocity = Vector3.read(binaryReader);
                newObj.packedItems.Add(PackBitfield.HasVelocity.ToString());
            }

            if ((newObj.bitfield & (uint)PackBitfield.HasPlacementID) != 0)
            {
                newObj.placement_id = binaryReader.ReadUInt32();
                newObj.packedItems.Add(PackBitfield.HasPlacementID.ToString());
            }

            newObj.has_contact = (newObj.bitfield & (uint)PackBitfield.IsGrounded) != 0;
            if (newObj.has_contact)
                newObj.packedItems.Add(PackBitfield.IsGrounded.ToString());

            newObj.instance_timestamp = binaryReader.ReadUInt16();
            newObj.position_timestamp = binaryReader.ReadUInt16();
            newObj.teleport_timestamp = binaryReader.ReadUInt16();
            newObj.force_position_timestamp = binaryReader.ReadUInt16();
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
            TreeNode bitfieldNode = node.Nodes.Add("bitfield = " + Utility.FormatHex(this.bitfield));
            for (int i = 0; i < packedItems.Count; i++)
            {
                bitfieldNode.Nodes.Add(packedItems[i]);
            }

            TreeNode positionNode = node.Nodes.Add("position = ");
            position.contributeToTreeNode(positionNode);
            if ((bitfield & (uint)PackBitfield.HasVelocity) != 0)
            {
                node.Nodes.Add("velocity = " + velocity);
            }
            if ((bitfield & (uint)PackBitfield.HasPlacementID) != 0)
            {
                node.Nodes.Add("placement_id = " + placement_id);
            }
            node.Nodes.Add("has_contact = " + has_contact);
            node.Nodes.Add("instance_timestamp = " + instance_timestamp);
            node.Nodes.Add("position_timestamp = " + position_timestamp);
            node.Nodes.Add("teleport_timestamp = " + teleport_timestamp);
            node.Nodes.Add("force_position_timestamp = " + force_position_timestamp);
        }
    }

    public class UpdatePosition : Message {
        public uint object_id;
        public PositionPack positionPack;

        public static UpdatePosition read(BinaryReader binaryReader) {
            UpdatePosition newObj = new UpdatePosition();
            newObj.object_id = binaryReader.ReadUInt32();
            newObj.positionPack = PositionPack.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("object_id = " + Utility.FormatHex(this.object_id));
            TreeNode positionPackNode = rootNode.Nodes.Add("PositionPack = ");
            positionPack.contributeToTreeNode(positionPackNode);
            positionPackNode.Expand();
            treeView.Nodes.Add(rootNode);
        }
    }

    public class InterpretedMotionState {
        public enum PackBitfield {
            current_style = (1 << 0),
            forward_command = (1 << 1),
            forward_speed = (1 << 2),
            sidestep_command = (1 << 3),
            sidestep_speed = (1 << 4),
            turn_command = (1 << 5),
            turn_speed = (1 << 6)
        }

        public uint bitfield;
        public MotionCommand current_style = MotionCommand.NonCombat;
        public MotionCommand forward_command = MotionCommand.Ready;
        public MotionCommand sidestep_command;
        public MotionCommand turn_command;
        public float forward_speed = 1.0f;
        public float sidestep_speed = 1.0f;
        public float turn_speed = 1.0f;
        public List<ActionNode> actions = new List<ActionNode>();
        public List<string> packedItems = new List<string>(); // For display purposes

        public static InterpretedMotionState read(BinaryReader binaryReader) {
            InterpretedMotionState newObj = new InterpretedMotionState();
            newObj.bitfield = binaryReader.ReadUInt32();
            if ((newObj.bitfield & (uint)PackBitfield.current_style) != 0) {
                newObj.current_style = (MotionCommand)command_ids[binaryReader.ReadUInt16()];
                newObj.packedItems.Add(PackBitfield.current_style.ToString());
            }
            if ((newObj.bitfield & (uint)PackBitfield.forward_command) != 0) {
                newObj.forward_command = (MotionCommand)command_ids[binaryReader.ReadUInt16()];
                newObj.packedItems.Add(PackBitfield.forward_command.ToString());
            }
            if ((newObj.bitfield & (uint)PackBitfield.sidestep_command) != 0) {
                newObj.sidestep_command = (MotionCommand)command_ids[binaryReader.ReadUInt16()];
                newObj.packedItems.Add(PackBitfield.sidestep_command.ToString());
            }
            if ((newObj.bitfield & (uint)PackBitfield.turn_command) != 0) {
                newObj.turn_command = (MotionCommand)command_ids[binaryReader.ReadUInt16()];
                newObj.packedItems.Add(PackBitfield.turn_command.ToString());
            }
            if ((newObj.bitfield & (uint)PackBitfield.forward_speed) != 0) {
                newObj.forward_speed = binaryReader.ReadSingle();
                newObj.packedItems.Add(PackBitfield.forward_speed.ToString());
            }
            if ((newObj.bitfield & (uint)PackBitfield.sidestep_speed) != 0) {
                newObj.sidestep_speed = binaryReader.ReadSingle();
                newObj.packedItems.Add(PackBitfield.sidestep_speed.ToString());
            }
            if ((newObj.bitfield & (uint)PackBitfield.turn_speed) != 0) {
                newObj.turn_speed = binaryReader.ReadSingle();
                newObj.packedItems.Add(PackBitfield.turn_speed.ToString());
            }

            uint num_actions = (newObj.bitfield >> 7) & 0x1F;
            newObj.packedItems.Add("num_actions = " + num_actions);
            for (int i = 0; i < num_actions; ++i) {
                newObj.actions.Add(ActionNode.read(binaryReader));
            }

            Util.readToAlign(binaryReader);

            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
            TreeNode bitfieldNode = node.Nodes.Add("bitfield = " + Utility.FormatHex(bitfield));
            for (int i = 0; i < packedItems.Count; i++)
            {
                bitfieldNode.Nodes.Add(packedItems[i]);
            }
            node.Nodes.Add("current_style = " + current_style);
            node.Nodes.Add("forward_command = " + forward_command);
            node.Nodes.Add("sidestep_command = " + sidestep_command);
            node.Nodes.Add("turn_command = " + turn_command);
            node.Nodes.Add("forward_speed = " + forward_speed);
            node.Nodes.Add("sidestep_speed = " + sidestep_speed);
            node.Nodes.Add("turn_speed = " + turn_speed);
            if (actions.Count > 0)
            {
                TreeNode actionsNode = node.Nodes.Add("actions = ");
                for (int i = 0; i < actions.Count; ++i)
                {
                    TreeNode actionNode = actionsNode.Nodes.Add($"action {i + 1}");
                    actions[i].contributeToTreeNode(actionNode);
                }
            } 
        }
    }

    public class ActionNode
    {
        public uint action;
        public uint stamp;
        public int autonomous;
        public float speed;

        public static ActionNode read(BinaryReader binaryReader)
        {
            ActionNode newObj = new ActionNode();
            newObj.action = command_ids[binaryReader.ReadUInt16()];
            uint packedSequence = binaryReader.ReadUInt16();
            newObj.stamp = packedSequence & 0x7FFF;
            newObj.autonomous = (int)((packedSequence >> 15) & 1);
            newObj.speed = binaryReader.ReadSingle();
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
            node.Nodes.Add("action = " + (MotionCommand)action);
            node.Nodes.Add("stamp = " + stamp);
            node.Nodes.Add("autonomous = " + autonomous);
            node.Nodes.Add("speed = " + speed);
        }
    }

    public class MovementParameters {
        public enum PackBitfield
        {
            can_walk = (1 << 0),
            can_run = (1 << 1),
            can_sidestep = (1 << 2),
            can_walk_backwards = (1 << 3),
            can_charge = (1 << 4),
            fail_walk = (1 << 5),
            use_final_heading = (1 << 6),
            sticky = (1 << 7),
            move_away = (1 << 8),
            move_towards = (1 << 9),
            use_spheres = (1 << 10),
            set_hold_key = (1 << 11),
            autonomous = (1 << 12),
            modify_raw_state = (1 << 13),
            modify_interpreted_state = (1 << 14),
            cancel_moveto = (1 << 15),
            stop_completely = (1 << 16),
            disable_jump_during_link = (1 << 17),
        }

        public uint bitfield;
        public float distance_to_object;
        public float min_distance;
        public float fail_distance;
        public float speed;
        public float walk_run_threshhold;
        public float desired_heading;

        public static MovementParameters read(MovementTypes type, BinaryReader binaryReader) {
            MovementParameters newObj = new MovementParameters();
            switch (type) {
                case MovementTypes.MoveToObject:
                case MovementTypes.MoveToPosition: {
                        newObj.bitfield = binaryReader.ReadUInt32();
                        newObj.distance_to_object = binaryReader.ReadSingle();
                        newObj.min_distance = binaryReader.ReadSingle();
                        newObj.fail_distance = binaryReader.ReadSingle();
                        newObj.speed = binaryReader.ReadSingle();
                        newObj.walk_run_threshhold = binaryReader.ReadSingle();
                        newObj.desired_heading = binaryReader.ReadSingle();
                        break;
                    }
                case MovementTypes.TurnToObject:
                case MovementTypes.TurnToHeading: {
                        newObj.bitfield = binaryReader.ReadUInt32();
                        newObj.speed = binaryReader.ReadSingle();
                        newObj.desired_heading = binaryReader.ReadSingle();
                        break;
                    }
                default: {
                        break;
                    }
            }
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node) {
            TreeNode bitfieldNode = node.Nodes.Add("bitfield = " + Utility.FormatHex(bitfield));
            foreach (PackBitfield e in Enum.GetValues(typeof(PackBitfield)))
            {
                if (((uint)bitfield & (uint)e) == (uint)e && (uint)e != 0)
                {
                    bitfieldNode.Nodes.Add($"{Enum.GetName(typeof(PackBitfield), e)}");
                }
            }
            node.Nodes.Add("distance_to_object = " + distance_to_object);
            node.Nodes.Add("min_distance = " + min_distance);
            node.Nodes.Add("fail_distance = " + fail_distance);
            node.Nodes.Add("speed = " + speed);
            node.Nodes.Add("walk_run_threshhold = " + walk_run_threshhold);
            node.Nodes.Add("desired_heading = " + desired_heading);
        }
    }
    // This class does not appear in the client but is added for convenience
    public class MovementData
    {
        public ushort movement_timestamp;
        public ushort server_control_timestamp;
        public byte autonomous;
        public MovementDataUnpack movementData_Unpack;

        public static MovementData read(BinaryReader binaryReader)
        {
            MovementData newObj = new MovementData();
            newObj.movement_timestamp = binaryReader.ReadUInt16();
            newObj.server_control_timestamp = binaryReader.ReadUInt16();
            newObj.autonomous = binaryReader.ReadByte();

            Util.readToAlign(binaryReader);

            newObj.movementData_Unpack = MovementDataUnpack.read(binaryReader);
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
            node.Nodes.Add("movement_timestamp = " + movement_timestamp);
            node.Nodes.Add("server_control_timestamp = " + server_control_timestamp);
            node.Nodes.Add("autonomous = " + autonomous);
            movementData_Unpack.contributeToTreeNode(node);
        }
    }

    // A class that mimics MovementManager::unpack_movement
    public class MovementDataUnpack
    {
        public MovementTypes movement_type;
        public ushort movement_options;
        public MovementParameters movement_params = new MovementParameters();
        public MotionCommand style;
        public InterpretedMotionState interpretedMotionState = new InterpretedMotionState();
        public uint stickToObject;
        public bool standing_longjump = false;
        public uint moveToObject;
        public Position moveToPos = new Position();
        public float my_run_rate;
        public uint turnToObject;
        public float desiredHeading;

        public static MovementDataUnpack read(BinaryReader binaryReader)
        {
            MovementDataUnpack newObj = new MovementDataUnpack();
            ushort pack_word = binaryReader.ReadUInt16();
            newObj.movement_options = (ushort)(pack_word & 0xFF00);
            newObj.movement_type = (MovementTypes)((ushort)(pack_word & 0x00FF));
            newObj.style = (MotionCommand)command_ids[binaryReader.ReadUInt16()];
            switch (newObj.movement_type)
            {
                case MovementTypes.General:
                    {
                        newObj.interpretedMotionState = InterpretedMotionState.read(binaryReader);
                        if ((newObj.movement_options & 0x100) != 0)
                        {
                            newObj.stickToObject = binaryReader.ReadUInt32();
                        }
                        if ((newObj.movement_options & 0x200) != 0)
                        {
                            newObj.standing_longjump = true;
                        }
                        break;
                    }
                case MovementTypes.MoveToObject:
                    {
                        newObj.moveToObject = binaryReader.ReadUInt32();
                        newObj.moveToPos = Position.readOrigin(binaryReader);
                        newObj.movement_params = MovementParameters.read(newObj.movement_type, binaryReader);
                        newObj.my_run_rate = binaryReader.ReadSingle();
                        break;
                    }
                case MovementTypes.MoveToPosition:
                    {
                        newObj.moveToPos = Position.readOrigin(binaryReader);
                        newObj.movement_params = MovementParameters.read(newObj.movement_type, binaryReader);
                        newObj.my_run_rate = binaryReader.ReadSingle();
                        break;
                    }
                case MovementTypes.TurnToObject:
                    {
                        newObj.turnToObject = binaryReader.ReadUInt32();
                        newObj.desiredHeading = binaryReader.ReadSingle();
                        newObj.movement_params = MovementParameters.read(newObj.movement_type, binaryReader);
                        break;
                    }
                case MovementTypes.TurnToHeading:
                    {
                        newObj.movement_params = MovementParameters.read(newObj.movement_type, binaryReader);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
            node.Nodes.Add("movement_type = " + movement_type);
            node.Nodes.Add("style = " + style);
            if (movement_type == MovementTypes.General)
            {
                TreeNode optionsNode = node.Nodes.Add("movement_options = " + Utility.FormatHex(movement_options));
                if ((movement_options & 0x100) != 0)
                {
                    optionsNode.Nodes.Add("stickToObject = " + Utility.FormatHex(stickToObject));
                }
                optionsNode.Nodes.Add("standing_longjump = " + standing_longjump);
                TreeNode motionStateNode = node.Nodes.Add("interpretedMotionState = ");
                interpretedMotionState.contributeToTreeNode(motionStateNode);
            }
            else if (movement_type == MovementTypes.MoveToObject)
            {
                node.Nodes.Add("moveToObject = " + Utility.FormatHex(moveToObject));
                TreeNode posNode = node.Nodes.Add("moveToPos = ");
                moveToPos.contributeToTreeNode(posNode);
                TreeNode moveParamsNode = node.Nodes.Add("movement_params = ");
                movement_params.contributeToTreeNode(moveParamsNode);
                node.Nodes.Add("my_run_rate = " + my_run_rate);
            }
            else if (movement_type == MovementTypes.MoveToPosition)
            {
                TreeNode posNode = node.Nodes.Add("moveToPos = ");
                moveToPos.contributeToTreeNode(posNode);
                TreeNode moveParamsNode = node.Nodes.Add("movement_params = ");
                movement_params.contributeToTreeNode(moveParamsNode);
                node.Nodes.Add("my_run_rate = " + my_run_rate);
            }
            else if (movement_type == MovementTypes.TurnToObject)
            {
                node.Nodes.Add("turnToObject = " + Utility.FormatHex(turnToObject));
                node.Nodes.Add("desiredHeading = " + desiredHeading);
                TreeNode moveParamsNode = node.Nodes.Add("movement_params = ");
                movement_params.contributeToTreeNode(moveParamsNode);
            }
            else if (movement_type == MovementTypes.TurnToHeading)
            {
                TreeNode moveParamsNode = node.Nodes.Add("movement_params = ");
                movement_params.contributeToTreeNode(moveParamsNode);
            }
        }
    }

    public class MovementEvent : Message {
        public uint object_id;
        public ushort instance_timestamp;
        public MovementData movement_data;

        public static MovementEvent read(BinaryReader binaryReader) {
            MovementEvent newObj = new MovementEvent();
            newObj.object_id = binaryReader.ReadUInt32();
            newObj.instance_timestamp = binaryReader.ReadUInt16();
            newObj.movement_data = MovementData.read(binaryReader);

            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("object_id = " + Utility.FormatHex(this.object_id));
            rootNode.Nodes.Add("instance_timestamp = " + instance_timestamp);
            movement_data.contributeToTreeNode(rootNode);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class AutonomyLevel : Message {
        public uint i_autonomy_level;

        public static AutonomyLevel read(BinaryReader binaryReader) {
            AutonomyLevel newObj = new AutonomyLevel();
            newObj.i_autonomy_level = binaryReader.ReadUInt32();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_autonomy_level = " + i_autonomy_level);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class RawMotionState {
        public enum PackBitfield {
            current_holdkey = (1 << 0),
            current_style = (1 << 1),
            forward_command = (1 << 2),
            forward_holdkey = (1 << 3),
            forward_speed = (1 << 4),
            sidestep_command = (1 << 5),
            sidestep_holdkey = (1 << 6),
            sidestep_speed = (1 << 7),
            turn_command = (1 << 8),
            turn_holdkey = (1 << 9),
            turn_speed = (1 << 10),
        }

        public uint bitfield;
        public List<string> packedItems; // For display purposes
        public HoldKey current_holdkey;
        public MotionCommand current_style = MotionCommand.NonCombat;
        public MotionCommand forward_command = MotionCommand.Ready;
        public HoldKey forward_holdkey;
        public float forward_speed = 1.0f;
        public MotionCommand sidestep_command;
        public HoldKey sidestep_holdkey;
        public float sidestep_speed = 1.0f;
        public MotionCommand turn_command;
        public HoldKey turn_holdkey;
        public float turn_speed = 1.0f;
        public List<ActionNode> actions;

        public static RawMotionState read(BinaryReader binaryReader) {
            RawMotionState newObj = new RawMotionState();
            newObj.packedItems = new List<string>();
            newObj.bitfield = binaryReader.ReadUInt32();
            if ((newObj.bitfield & (uint)PackBitfield.current_holdkey) != 0) {
                newObj.current_holdkey = (HoldKey)binaryReader.ReadUInt32();
                newObj.packedItems.Add(PackBitfield.current_holdkey.ToString());
            }
            if ((newObj.bitfield & (uint)PackBitfield.current_style) != 0) {
                newObj.current_style = (MotionCommand)binaryReader.ReadUInt32();
                newObj.packedItems.Add(PackBitfield.current_style.ToString());
            }
            if ((newObj.bitfield & (uint)PackBitfield.forward_command) != 0) {
                newObj.forward_command = (MotionCommand)binaryReader.ReadUInt32();
                newObj.packedItems.Add(PackBitfield.forward_command.ToString());
            }
            if ((newObj.bitfield & (uint)PackBitfield.forward_holdkey) != 0) {
                newObj.forward_holdkey = (HoldKey)binaryReader.ReadUInt32();
                newObj.packedItems.Add(PackBitfield.forward_holdkey.ToString());
            }
            if ((newObj.bitfield & (uint)PackBitfield.forward_speed) != 0) {
                newObj.forward_speed = binaryReader.ReadSingle();
                newObj.packedItems.Add(PackBitfield.forward_speed.ToString());
            }
            if ((newObj.bitfield & (uint)PackBitfield.sidestep_command) != 0) {
                newObj.sidestep_command = (MotionCommand)binaryReader.ReadUInt32();
                newObj.packedItems.Add(PackBitfield.sidestep_command.ToString());
            }
            if ((newObj.bitfield & (uint)PackBitfield.sidestep_holdkey) != 0) {
                newObj.sidestep_holdkey = (HoldKey)binaryReader.ReadUInt32();
                newObj.packedItems.Add(PackBitfield.sidestep_holdkey.ToString());
            }
            if ((newObj.bitfield & (uint)PackBitfield.sidestep_speed) != 0) {
                newObj.sidestep_speed = binaryReader.ReadSingle();
                newObj.packedItems.Add(PackBitfield.sidestep_speed.ToString());
            }
            if ((newObj.bitfield & (uint)PackBitfield.turn_command) != 0) {
                newObj.turn_command = (MotionCommand)binaryReader.ReadUInt32();
                newObj.packedItems.Add(PackBitfield.turn_command.ToString());
            }
            if ((newObj.bitfield & (uint)PackBitfield.turn_holdkey) != 0) {
                newObj.turn_holdkey = (HoldKey)binaryReader.ReadUInt32();
                newObj.packedItems.Add(PackBitfield.turn_holdkey.ToString());
            }
            if ((newObj.bitfield & (uint)PackBitfield.turn_speed) != 0) {
                newObj.turn_speed = binaryReader.ReadSingle();
                newObj.packedItems.Add(PackBitfield.turn_speed.ToString());
            }

            uint num_actions = (newObj.bitfield >> 11);
            newObj.packedItems.Add("num_actions = " + num_actions);
            newObj.actions = new List<ActionNode>();
            for (int i = 0; i < num_actions; ++i) {
                newObj.actions.Add(ActionNode.read(binaryReader));
            }

            Util.readToAlign(binaryReader);

            return newObj;
        }

        public void contributeToTreeNode(TreeNode node) {
            TreeNode bitfieldNode = node.Nodes.Add("bitfield = " + Utility.FormatHex(bitfield));
            for (int i = 0; i < packedItems.Count; i++)
            {
                bitfieldNode.Nodes.Add(packedItems[i]);
            }
            node.Nodes.Add("current_holdkey = " + current_holdkey);
            node.Nodes.Add("current_style = " + current_style);
            node.Nodes.Add("forward_command = " + forward_command);
            node.Nodes.Add("forward_holdkey = " + forward_holdkey);
            node.Nodes.Add("forward_speed = " + forward_speed);
            node.Nodes.Add("sidestep_command = " + sidestep_command);
            node.Nodes.Add("sidestep_holdkey = " + sidestep_holdkey);
            node.Nodes.Add("sidestep_speed = " + sidestep_speed);
            node.Nodes.Add("turn_command = " + turn_command);
            node.Nodes.Add("turn_holdkey = " + turn_holdkey);
            node.Nodes.Add("turn_speed = " + turn_speed);
            if (actions.Count > 0)
            {
                TreeNode actionsNode = node.Nodes.Add("actions = ");
                for (int i = 0; i < actions.Count; i++)
                {
                    TreeNode actionNode = actionsNode.Nodes.Add($"action {i+1}");
                    actions[i].contributeToTreeNode(actionNode);
                }
            }
        }
    }

    public class AutonomousPosition : Message {
        public Position position;
        public ushort instance_timestamp;
        public ushort server_control_timestamp;
        public ushort teleport_timestamp;
        public ushort force_position_timestamp;
        public bool contact;

        public static AutonomousPosition read(BinaryReader binaryReader) {
            AutonomousPosition newObj = new AutonomousPosition();

            newObj.position = Position.read(binaryReader);

            newObj.instance_timestamp = binaryReader.ReadUInt16();
            newObj.server_control_timestamp = binaryReader.ReadUInt16();
            newObj.teleport_timestamp = binaryReader.ReadUInt16();
            newObj.force_position_timestamp = binaryReader.ReadUInt16();

            newObj.contact = binaryReader.ReadByte() != 0;

            Util.readToAlign(binaryReader);

            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            TreeNode positionNode = rootNode.Nodes.Add("position = ");
            position.contributeToTreeNode(positionNode);
            rootNode.Nodes.Add("instance_timestamp = " + instance_timestamp);
            rootNode.Nodes.Add("server_control_timestamp = " + server_control_timestamp);
            rootNode.Nodes.Add("teleport_timestamp = " + teleport_timestamp);
            rootNode.Nodes.Add("force_position_timestamp = " + force_position_timestamp);
            rootNode.Nodes.Add("contact = " + contact);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class Jump_NonAutonomous : Message {
        public float i_extent;

        public static Jump_NonAutonomous read(BinaryReader binaryReader) {
            Jump_NonAutonomous newObj = new Jump_NonAutonomous();
            newObj.i_extent = binaryReader.ReadSingle();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_extent = " + i_extent);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class PositionAndMovement : Message
    {
        public uint object_id;
        public PositionPack positionPack;
        public MovementData movementData;

        public static PositionAndMovement read(BinaryReader binaryReader)
        {
            PositionAndMovement newObj = new PositionAndMovement();
            newObj.object_id = binaryReader.ReadUInt32();
            newObj.positionPack = PositionPack.read(binaryReader);
            newObj.movementData = MovementData.read(binaryReader);

            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("object_id = " + Utility.FormatHex(object_id));
            TreeNode positionPackNode = rootNode.Nodes.Add("PositionPack = ");
            positionPack.contributeToTreeNode(positionPackNode);
            TreeNode movementDataNode = rootNode.Nodes.Add("MovementData = ");
            movementData.contributeToTreeNode(movementDataNode);
            treeView.Nodes.Add(rootNode);
        }
    }

    // Note: These IDs are from the last version of the client. Earlier versions of the client had a different array order and values.
    static uint[] command_ids = {
        (uint)MotionCommand.Invalid,
        (uint)MotionCommand.HoldRun,
        (uint)MotionCommand.HoldSidestep,
        (uint)MotionCommand.Ready,
        (uint)MotionCommand.Stop,
        (uint)MotionCommand.WalkForward,
        (uint)MotionCommand.WalkBackwards,
        (uint)MotionCommand.RunForward,
        (uint)MotionCommand.Fallen,
        (uint)MotionCommand.Interpolating,
        (uint)MotionCommand.Hover,
        (uint)MotionCommand.On,
        (uint)MotionCommand.Off,
        (uint)MotionCommand.TurnRight,
        (uint)MotionCommand.TurnLeft,
        (uint)MotionCommand.SideStepRight,
        (uint)MotionCommand.SideStepLeft,
        (uint)MotionCommand.Dead,
        (uint)MotionCommand.Crouch,
        (uint)MotionCommand.Sitting,
        (uint)MotionCommand.Sleeping,
        (uint)MotionCommand.Falling,
        (uint)MotionCommand.Reload,
        (uint)MotionCommand.Unload,
        (uint)MotionCommand.Pickup,
        (uint)MotionCommand.StoreInBackpack,
        (uint)MotionCommand.Eat,
        (uint)MotionCommand.Drink,
        (uint)MotionCommand.Reading,
        (uint)MotionCommand.JumpCharging,
        (uint)MotionCommand.AimLevel,
        (uint)MotionCommand.AimHigh15,
        (uint)MotionCommand.AimHigh30,
        (uint)MotionCommand.AimHigh45,
        (uint)MotionCommand.AimHigh60,
        (uint)MotionCommand.AimHigh75,
        (uint)MotionCommand.AimHigh90,
        (uint)MotionCommand.AimLow15,
        (uint)MotionCommand.AimLow30,
        (uint)MotionCommand.AimLow45,
        (uint)MotionCommand.AimLow60,
        (uint)MotionCommand.AimLow75,
        (uint)MotionCommand.AimLow90,
        (uint)MotionCommand.MagicBlast,
        (uint)MotionCommand.MagicSelfHead,
        (uint)MotionCommand.MagicSelfHeart,
        (uint)MotionCommand.MagicBonus,
        (uint)MotionCommand.MagicClap,
        (uint)MotionCommand.MagicHarm,
        (uint)MotionCommand.MagicHeal,
        (uint)MotionCommand.MagicThrowMissile,
        (uint)MotionCommand.MagicRecoilMissile,
        (uint)MotionCommand.MagicPenalty,
        (uint)MotionCommand.MagicTransfer,
        (uint)MotionCommand.MagicVision,
        (uint)MotionCommand.MagicEnchantItem,
        (uint)MotionCommand.MagicPortal,
        (uint)MotionCommand.MagicPray,
        (uint)MotionCommand.StopTurning,
        (uint)MotionCommand.Jump,
        (uint)MotionCommand.HandCombat,
        (uint)MotionCommand.NonCombat,
        (uint)MotionCommand.SwordCombat,
        (uint)MotionCommand.BowCombat,
        (uint)MotionCommand.SwordShieldCombat,
        (uint)MotionCommand.CrossbowCombat,
        (uint)MotionCommand.UnusedCombat,
        (uint)MotionCommand.SlingCombat,
        (uint)MotionCommand.TwoHandedSwordCombat,
        (uint)MotionCommand.TwoHandedStaffCombat,
        (uint)MotionCommand.DualWieldCombat,
        (uint)MotionCommand.ThrownWeaponCombat,
        (uint)MotionCommand.Graze,
        (uint)MotionCommand.Magic,
        (uint)MotionCommand.Hop,
        (uint)MotionCommand.Jumpup,
        (uint)MotionCommand.Cheer,
        (uint)MotionCommand.ChestBeat,
        (uint)MotionCommand.TippedLeft,
        (uint)MotionCommand.TippedRight,
        (uint)MotionCommand.FallDown,
        (uint)MotionCommand.Twitch1,
        (uint)MotionCommand.Twitch2,
        (uint)MotionCommand.Twitch3,
        (uint)MotionCommand.Twitch4,
        (uint)MotionCommand.StaggerBackward,
        (uint)MotionCommand.StaggerForward,
        (uint)MotionCommand.Sanctuary,
        (uint)MotionCommand.ThrustMed,
        (uint)MotionCommand.ThrustLow,
        (uint)MotionCommand.ThrustHigh,
        (uint)MotionCommand.SlashHigh,
        (uint)MotionCommand.SlashMed,
        (uint)MotionCommand.SlashLow,
        (uint)MotionCommand.BackhandHigh,
        (uint)MotionCommand.BackhandMed,
        (uint)MotionCommand.BackhandLow,
        (uint)MotionCommand.Shoot,
        (uint)MotionCommand.AttackHigh1,
        (uint)MotionCommand.AttackMed1,
        (uint)MotionCommand.AttackLow1,
        (uint)MotionCommand.AttackHigh2,
        (uint)MotionCommand.AttackMed2,
        (uint)MotionCommand.AttackLow2,
        (uint)MotionCommand.AttackHigh3,
        (uint)MotionCommand.AttackMed3,
        (uint)MotionCommand.AttackLow3,
        (uint)MotionCommand.HeadThrow,
        (uint)MotionCommand.FistSlam,
        (uint)MotionCommand.BreatheFlame_,
        (uint)MotionCommand.SpinAttack,
        (uint)MotionCommand.MagicPowerUp01,
        (uint)MotionCommand.MagicPowerUp02,
        (uint)MotionCommand.MagicPowerUp03,
        (uint)MotionCommand.MagicPowerUp04,
        (uint)MotionCommand.MagicPowerUp05,
        (uint)MotionCommand.MagicPowerUp06,
        (uint)MotionCommand.MagicPowerUp07,
        (uint)MotionCommand.MagicPowerUp08,
        (uint)MotionCommand.MagicPowerUp09,
        (uint)MotionCommand.MagicPowerUp10,
        (uint)MotionCommand.ShakeFist,
        (uint)MotionCommand.Beckon,
        (uint)MotionCommand.BeSeeingYou,
        (uint)MotionCommand.BlowKiss,
        (uint)MotionCommand.BowDeep,
        (uint)MotionCommand.ClapHands,
        (uint)MotionCommand.Cry,
        (uint)MotionCommand.Laugh,
        (uint)MotionCommand.MimeEat,
        (uint)MotionCommand.MimeDrink,
        (uint)MotionCommand.Nod,
        (uint)MotionCommand.Point,
        (uint)MotionCommand.ShakeHead,
        (uint)MotionCommand.Shrug,
        (uint)MotionCommand.Wave,
        (uint)MotionCommand.Akimbo,
        (uint)MotionCommand.HeartyLaugh,
        (uint)MotionCommand.Salute,
        (uint)MotionCommand.ScratchHead,
        (uint)MotionCommand.SmackHead,
        (uint)MotionCommand.TapFoot,
        (uint)MotionCommand.WaveHigh,
        (uint)MotionCommand.WaveLow,
        (uint)MotionCommand.YawnStretch,
        (uint)MotionCommand.Cringe,
        (uint)MotionCommand.Kneel,
        (uint)MotionCommand.Plead,
        (uint)MotionCommand.Shiver,
        (uint)MotionCommand.Shoo,
        (uint)MotionCommand.Slouch,
        (uint)MotionCommand.Spit,
        (uint)MotionCommand.Surrender,
        (uint)MotionCommand.Woah,
        (uint)MotionCommand.Winded,
        (uint)MotionCommand.YMCA,
        (uint)MotionCommand.EnterGame,
        (uint)MotionCommand.ExitGame,
        (uint)MotionCommand.OnCreation,
        (uint)MotionCommand.OnDestruction,
        (uint)MotionCommand.EnterPortal,
        (uint)MotionCommand.ExitPortal,
        (uint)MotionCommand.Cancel,
        (uint)MotionCommand.UseSelected,
        (uint)MotionCommand.AutosortSelected,
        (uint)MotionCommand.DropSelected,
        (uint)MotionCommand.GiveSelected,
        (uint)MotionCommand.SplitSelected,
        (uint)MotionCommand.ExamineSelected,
        (uint)MotionCommand.CreateShortcutToSelected,
        (uint)MotionCommand.PreviousCompassItem,
        (uint)MotionCommand.NextCompassItem,
        (uint)MotionCommand.ClosestCompassItem,
        (uint)MotionCommand.PreviousSelection,
        (uint)MotionCommand.LastAttacker,
        (uint)MotionCommand.PreviousFellow,
        (uint)MotionCommand.NextFellow,
        (uint)MotionCommand.ToggleCombat,
        (uint)MotionCommand.HighAttack,
        (uint)MotionCommand.MediumAttack,
        (uint)MotionCommand.LowAttack,
        (uint)MotionCommand.EnterChat,
        (uint)MotionCommand.ToggleChat,
        (uint)MotionCommand.SavePosition,
        (uint)MotionCommand.OptionsPanel,
        (uint)MotionCommand.ResetView,
        (uint)MotionCommand.CameraLeftRotate,
        (uint)MotionCommand.CameraRightRotate,
        (uint)MotionCommand.CameraRaise,
        (uint)MotionCommand.CameraLower,
        (uint)MotionCommand.CameraCloser,
        (uint)MotionCommand.CameraFarther,
        (uint)MotionCommand.FloorView,
        (uint)MotionCommand.MouseLook,
        (uint)MotionCommand.PreviousItem,
        (uint)MotionCommand.NextItem,
        (uint)MotionCommand.ClosestItem,
        (uint)MotionCommand.ShiftView,
        (uint)MotionCommand.MapView,
        (uint)MotionCommand.AutoRun,
        (uint)MotionCommand.DecreasePowerSetting,
        (uint)MotionCommand.IncreasePowerSetting,
        (uint)MotionCommand.Pray,
        (uint)MotionCommand.Mock,
        (uint)MotionCommand.Teapot,
        (uint)MotionCommand.SpecialAttack1,
        (uint)MotionCommand.SpecialAttack2,
        (uint)MotionCommand.SpecialAttack3,
        (uint)MotionCommand.MissileAttack1,
        (uint)MotionCommand.MissileAttack2,
        (uint)MotionCommand.MissileAttack3,
        (uint)MotionCommand.CastSpell,
        (uint)MotionCommand.Flatulence,
        (uint)MotionCommand.FirstPersonView,
        (uint)MotionCommand.AllegiancePanel,
        (uint)MotionCommand.FellowshipPanel,
        (uint)MotionCommand.SpellbookPanel,
        (uint)MotionCommand.SpellComponentsPanel,
        (uint)MotionCommand.HousePanel,
        (uint)MotionCommand.AttributesPanel,
        (uint)MotionCommand.SkillsPanel,
        (uint)MotionCommand.MapPanel,
        (uint)MotionCommand.InventoryPanel,
        (uint)MotionCommand.Demonet,
        (uint)MotionCommand.UseMagicStaff,
        (uint)MotionCommand.UseMagicWand,
        (uint)MotionCommand.Blink,
        (uint)MotionCommand.Bite,
        (uint)MotionCommand.TwitchSubstate1,
        (uint)MotionCommand.TwitchSubstate2,
        (uint)MotionCommand.TwitchSubstate3,
        (uint)MotionCommand.CaptureScreenshotToFile,
        (uint)MotionCommand.BowNoAmmo,
        (uint)MotionCommand.CrossBowNoAmmo,
        (uint)MotionCommand.ShakeFistState,
        (uint)MotionCommand.PrayState,
        (uint)MotionCommand.BowDeepState,
        (uint)MotionCommand.ClapHandsState,
        (uint)MotionCommand.CrossArmsState,
        (uint)MotionCommand.ShiverState,
        (uint)MotionCommand.PointState,
        (uint)MotionCommand.WaveState,
        (uint)MotionCommand.AkimboState,
        (uint)MotionCommand.SaluteState,
        (uint)MotionCommand.ScratchHeadState,
        (uint)MotionCommand.TapFootState,
        (uint)MotionCommand.LeanState,
        (uint)MotionCommand.KneelState,
        (uint)MotionCommand.PleadState,
        (uint)MotionCommand.ATOYOT,
        (uint)MotionCommand.SlouchState,
        (uint)MotionCommand.SurrenderState,
        (uint)MotionCommand.WoahState,
        (uint)MotionCommand.WindedState,
        (uint)MotionCommand.AutoCreateShortcuts,
        (uint)MotionCommand.AutoRepeatAttacks,
        (uint)MotionCommand.AutoTarget,
        (uint)MotionCommand.AdvancedCombatInterface,
        (uint)MotionCommand.IgnoreAllegianceRequests,
        (uint)MotionCommand.IgnoreFellowshipRequests,
        (uint)MotionCommand.InvertMouseLook,
        (uint)MotionCommand.LetPlayersGiveYouItems,
        (uint)MotionCommand.AutoTrackCombatTargets,
        (uint)MotionCommand.DisplayTooltips,
        (uint)MotionCommand.AttemptToDeceivePlayers,
        (uint)MotionCommand.RunAsDefaultMovement,
        (uint)MotionCommand.StayInChatModeAfterSend,
        (uint)MotionCommand.RightClickToMouseLook,
        (uint)MotionCommand.VividTargetIndicator,
        (uint)MotionCommand.SelectSelf,
        (uint)MotionCommand.SkillHealSelf,
        (uint)MotionCommand.WoahDuplicate1,
        (uint)MotionCommand.MimeDrinkDuplicate1,
        (uint)MotionCommand.MimeDrinkDuplicate2,
        (uint)MotionCommand.NextMonster,
        (uint)MotionCommand.PreviousMonster,
        (uint)MotionCommand.ClosestMonster,
        (uint)MotionCommand.NextPlayer,
        (uint)MotionCommand.PreviousPlayer,
        (uint)MotionCommand.ClosestPlayer,
        (uint)MotionCommand.SnowAngelState,
        (uint)MotionCommand.WarmHands,
        (uint)MotionCommand.CurtseyState,
        (uint)MotionCommand.AFKState,
        (uint)MotionCommand.MeditateState,
        (uint)MotionCommand.TradePanel,
        (uint)MotionCommand.LogOut,
        (uint)MotionCommand.DoubleSlashLow,
        (uint)MotionCommand.DoubleSlashMed,
        (uint)MotionCommand.DoubleSlashHigh,
        (uint)MotionCommand.TripleSlashLow,
        (uint)MotionCommand.TripleSlashMed,
        (uint)MotionCommand.TripleSlashHigh,
        (uint)MotionCommand.DoubleThrustLow,
        (uint)MotionCommand.DoubleThrustMed,
        (uint)MotionCommand.DoubleThrustHigh,
        (uint)MotionCommand.TripleThrustLow,
        (uint)MotionCommand.TripleThrustMed,
        (uint)MotionCommand.TripleThrustHigh,
        (uint)MotionCommand.MagicPowerUp01Purple,
        (uint)MotionCommand.MagicPowerUp02Purple,
        (uint)MotionCommand.MagicPowerUp03Purple,
        (uint)MotionCommand.MagicPowerUp04Purple,
        (uint)MotionCommand.MagicPowerUp05Purple,
        (uint)MotionCommand.MagicPowerUp06Purple,
        (uint)MotionCommand.MagicPowerUp07Purple,
        (uint)MotionCommand.MagicPowerUp08Purple,
        (uint)MotionCommand.MagicPowerUp09Purple,
        (uint)MotionCommand.MagicPowerUp10Purple,
        (uint)MotionCommand.Helper,
        (uint)MotionCommand.Pickup5,
        (uint)MotionCommand.Pickup10,
        (uint)MotionCommand.Pickup15,
        (uint)MotionCommand.Pickup20,
        (uint)MotionCommand.HouseRecall,
        (uint)MotionCommand.AtlatlCombat,
        (uint)MotionCommand.ThrownShieldCombat,
        (uint)MotionCommand.SitState,
        (uint)MotionCommand.SitCrossleggedState,
        (uint)MotionCommand.SitBackState,
        (uint)MotionCommand.PointLeftState,
        (uint)MotionCommand.PointRightState,
        (uint)MotionCommand.TalktotheHandState,
        (uint)MotionCommand.PointDownState,
        (uint)MotionCommand.DrudgeDanceState,
        (uint)MotionCommand.PossumState,
        (uint)MotionCommand.ReadState,
        (uint)MotionCommand.ThinkerState,
        (uint)MotionCommand.HaveASeatState,
        (uint)MotionCommand.AtEaseState,
        (uint)MotionCommand.NudgeLeft,
        (uint)MotionCommand.NudgeRight,
        (uint)MotionCommand.PointLeft,
        (uint)MotionCommand.PointRight,
        (uint)MotionCommand.PointDown,
        (uint)MotionCommand.Knock,
        (uint)MotionCommand.ScanHorizon,
        (uint)MotionCommand.DrudgeDance,
        (uint)MotionCommand.HaveASeat,
        (uint)MotionCommand.LifestoneRecall,
        (uint)MotionCommand.CharacterOptionsPanel,
        (uint)MotionCommand.SoundAndGraphicsPanel,
        (uint)MotionCommand.HelpfulSpellsPanel,
        (uint)MotionCommand.HarmfulSpellsPanel,
        (uint)MotionCommand.CharacterInformationPanel,
        (uint)MotionCommand.LinkStatusPanel,
        (uint)MotionCommand.VitaePanel,
        (uint)MotionCommand.ShareFellowshipXP,
        (uint)MotionCommand.ShareFellowshipLoot,
        (uint)MotionCommand.AcceptCorpseLooting,
        (uint)MotionCommand.IgnoreTradeRequests,
        (uint)MotionCommand.DisableWeather,
        (uint)MotionCommand.DisableHouseEffect,
        (uint)MotionCommand.SideBySideVitals,
        (uint)MotionCommand.ShowRadarCoordinates,
        (uint)MotionCommand.ShowSpellDurations,
        (uint)MotionCommand.MuteOnLosingFocus,
        (uint)MotionCommand.Fishing,
        (uint)MotionCommand.MarketplaceRecall,
        (uint)MotionCommand.EnterPKLite,
        (uint)MotionCommand.AllegianceChat,
        (uint)MotionCommand.AutomaticallyAcceptFellowshipRequests,
        (uint)MotionCommand.Reply,
        (uint)MotionCommand.MonarchReply,
        (uint)MotionCommand.PatronReply,
        (uint)MotionCommand.ToggleCraftingChanceOfSuccessDialog,
        (uint)MotionCommand.UseClosestUnopenedCorpse,
        (uint)MotionCommand.UseNextUnopenedCorpse,
        (uint)MotionCommand.IssueSlashCommand,
        (uint)MotionCommand.AllegianceHometownRecall,
        (uint)MotionCommand.PKArenaRecall,
        (uint)MotionCommand.OffhandSlashHigh,
        (uint)MotionCommand.OffhandSlashMed,
        (uint)MotionCommand.OffhandSlashLow,
        (uint)MotionCommand.OffhandThrustHigh,
        (uint)MotionCommand.OffhandThrustMed,
        (uint)MotionCommand.OffhandThrustLow,
        (uint)MotionCommand.OffhandDoubleSlashLow,
        (uint)MotionCommand.OffhandDoubleSlashMed,
        (uint)MotionCommand.OffhandDoubleSlashHigh,
        (uint)MotionCommand.OffhandTripleSlashLow,
        (uint)MotionCommand.OffhandTripleSlashMed,
        (uint)MotionCommand.OffhandTripleSlashHigh,
        (uint)MotionCommand.OffhandDoubleThrustLow,
        (uint)MotionCommand.OffhandDoubleThrustMed,
        (uint)MotionCommand.OffhandDoubleThrustHigh,
        (uint)MotionCommand.OffhandTripleThrustLow,
        (uint)MotionCommand.OffhandTripleThrustMed,
        (uint)MotionCommand.OffhandTripleThrustHigh,
        (uint)MotionCommand.OffhandKick,
        (uint)MotionCommand.AttackHigh4,
        (uint)MotionCommand.AttackMed4,
        (uint)MotionCommand.AttackLow4,
        (uint)MotionCommand.AttackHigh5,
        (uint)MotionCommand.AttackMed5,
        (uint)MotionCommand.AttackLow5,
        (uint)MotionCommand.AttackHigh6,
        (uint)MotionCommand.AttackMed6,
        (uint)MotionCommand.AttackLow6,
        (uint)MotionCommand.PunchFastHigh,
        (uint)MotionCommand.PunchFastMed,
        (uint)MotionCommand.PunchFastLow,
        (uint)MotionCommand.PunchSlowHigh,
        (uint)MotionCommand.PunchSlowMed,
        (uint)MotionCommand.PunchSlowLow,
        (uint)MotionCommand.OffhandPunchFastHigh,
        (uint)MotionCommand.OffhandPunchFastMed,
        (uint)MotionCommand.OffhandPunchFastLow,
        (uint)MotionCommand.OffhandPunchSlowHigh,
        (uint)MotionCommand.OffhandPunchSlowMed,
        (uint)MotionCommand.OffhandPunchSlowLow,
        (uint)MotionCommand.WoahDuplicate2
    };
}
