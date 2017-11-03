using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using aclogview;

class CM_Movement : MessageProcessor {

    public override bool acceptMessageData(BinaryReader messageDataReader, TreeView outputTreeView) {
        bool handled = true;

        PacketOpcode opcode = Util.readOpcode(messageDataReader);
        switch (opcode) {
            case PacketOpcode.LIFESTONE_MATERIALIZE:
                {
                    LifestoneMaterialize message = LifestoneMaterialize.read(messageDataReader);
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
        public Vector3 velocity;
        public ushort instance_timestamp;
        public ushort server_control_timestamp;
        public ushort teleport_timestamp;
        public ushort force_position_ts;

        public static JumpPack read(BinaryReader binaryReader) {
            JumpPack newObj = new JumpPack();
            newObj.velocity = Vector3.read(binaryReader);
            newObj.instance_timestamp = binaryReader.ReadUInt16();
            newObj.server_control_timestamp = binaryReader.ReadUInt16();
            newObj.teleport_timestamp = binaryReader.ReadUInt16();
            newObj.force_position_ts = binaryReader.ReadUInt16();
            Util.readToAlign(binaryReader);
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node) {
            node.Nodes.Add("velocity = " + velocity);
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

    public class UpdatePosition : Message {
        public uint object_id;
        public uint flags;
        public Position position;
        public Vector3 velocity;
        public uint placement_id;
        public bool has_contact;
        public ushort instance_timestamp;
        public ushort position_timestamp;
        public ushort teleport_timestamp;
        public ushort force_position_timestamp;

        public static UpdatePosition read(BinaryReader binaryReader) {
            UpdatePosition newObj = new UpdatePosition();
            newObj.object_id = binaryReader.ReadUInt32();
            newObj.flags = binaryReader.ReadUInt32();
            newObj.position = Position.readOrigin(binaryReader);

            if ((newObj.flags & 0x8) == 0) {
                newObj.position.frame.qw = binaryReader.ReadSingle();
            }
            if ((newObj.flags & 0x10) == 0) {
                newObj.position.frame.qx = binaryReader.ReadSingle();
            }
            if ((newObj.flags & 0x20) == 0) {
                newObj.position.frame.qy = binaryReader.ReadSingle();
            }
            if ((newObj.flags & 0x40) == 0) {
                newObj.position.frame.qz = binaryReader.ReadSingle();
            }

            newObj.position.frame.cache();

            if ((newObj.flags & 0x1) != 0) {
                newObj.velocity = Vector3.read(binaryReader);
            }

            if ((newObj.flags & 0x2) != 0) {
                newObj.placement_id = binaryReader.ReadUInt32();
            }

            newObj.has_contact = (newObj.flags & 0x4) != 0;

            newObj.instance_timestamp = binaryReader.ReadUInt16();
            newObj.position_timestamp = binaryReader.ReadUInt16();
            newObj.teleport_timestamp = binaryReader.ReadUInt16();
            newObj.force_position_timestamp = binaryReader.ReadUInt16();

            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("object_id = " + Utility.FormatHex(this.object_id));
            rootNode.Nodes.Add("flags = " + Utility.FormatHex(this.flags));
        
            TreeNode positionNode = rootNode.Nodes.Add("position = ");
            position.contributeToTreeNode(positionNode);
            if ((flags & 0x1) != 0) {
                rootNode.Nodes.Add("velocity = " + velocity);
            }
            if ((flags & 0x2) != 0) {
                rootNode.Nodes.Add("placement_id = " + placement_id);
            }
            rootNode.Nodes.Add("has_contact = " + has_contact);
            rootNode.Nodes.Add("instance_timestamp = " + instance_timestamp);
            rootNode.Nodes.Add("position_timestamp = " + position_timestamp);
            rootNode.Nodes.Add("teleport_timestamp = " + teleport_timestamp);
            rootNode.Nodes.Add("force_position_timestamp = " + force_position_timestamp);
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
        public MotionStyle current_style = MotionStyle.Motion_NonCombat;
        public MotionStyle forward_command = MotionStyle.Motion_Ready;
        public MotionStyle sidestep_command;
        public MotionStyle turn_command;
        public float forward_speed = 1.0f;
        public float sidestep_speed = 1.0f;
        public float turn_speed = 1.0f;
        public List<MovementAction> actions = new List<MovementAction>();

        public static InterpretedMotionState read(BinaryReader binaryReader) {
            InterpretedMotionState newObj = new InterpretedMotionState();
            newObj.bitfield = binaryReader.ReadUInt32();
            if ((newObj.bitfield & (uint)PackBitfield.current_style) != 0) {
                newObj.current_style = (MotionStyle)command_ids[binaryReader.ReadUInt16()];
            }
            if ((newObj.bitfield & (uint)PackBitfield.forward_command) != 0) {
                newObj.forward_command = (MotionStyle)command_ids[binaryReader.ReadUInt16()];
            }
            if ((newObj.bitfield & (uint)PackBitfield.sidestep_command) != 0) {
                newObj.sidestep_command = (MotionStyle)command_ids[binaryReader.ReadUInt16()];
            }
            if ((newObj.bitfield & (uint)PackBitfield.turn_command) != 0) {
                newObj.turn_command = (MotionStyle)command_ids[binaryReader.ReadUInt16()];
            }
            if ((newObj.bitfield & (uint)PackBitfield.forward_speed) != 0) {
                newObj.forward_speed = binaryReader.ReadSingle();
            }
            if ((newObj.bitfield & (uint)PackBitfield.sidestep_speed) != 0) {
                newObj.sidestep_speed = binaryReader.ReadSingle();
            }
            if ((newObj.bitfield & (uint)PackBitfield.turn_speed) != 0) {
                newObj.turn_speed = binaryReader.ReadSingle();
            }

            uint numActions = (newObj.bitfield >> 7) & 0x1F;
            for (int i = 0; i < numActions; ++i) {
                newObj.actions.Add(MovementAction.read(binaryReader));
            }

            Util.readToAlign(binaryReader);

            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
            node.Nodes.Add("bitfield = " + bitfield);
            node.Nodes.Add("current_style = " + current_style);
            node.Nodes.Add("forward_command = " + forward_command);
            node.Nodes.Add("sidestep_command = " + sidestep_command);
            node.Nodes.Add("turn_command = " + turn_command);
            node.Nodes.Add("forward_speed = " + forward_speed);
            node.Nodes.Add("sidestep_speed = " + sidestep_speed);
            node.Nodes.Add("turn_speed = " + turn_speed);

            TreeNode actionsNode = node.Nodes.Add("Actions");
            for (int i = 0; i < actions.Count; ++i)
            {
                TreeNode actionNode = actionsNode.Nodes.Add("action");
                actions[i].contributeToTreeNode(actionNode);
            }
            actionsNode.ExpandAll();
        }
    }

    public class MovementAction
    {
        public MotionStyle style;
        public uint sequence;
        public float speed;

        public static MovementAction read(BinaryReader binaryReader)
        {
            MovementAction newObj = new MovementAction();
            ushort command_id = binaryReader.ReadUInt16();
            newObj.style = (MotionStyle)command_ids[command_id];
            newObj.sequence = binaryReader.ReadUInt16();  // TODO - This is a packed word
            newObj.speed = binaryReader.ReadSingle();
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node)
        {
            node.Nodes.Add("style = " + style);
            node.Nodes.Add("sequence = " + sequence);
            node.Nodes.Add("speed = " + speed);
        }
    }

    public class MovementParameters {
        public uint bitfield;
        public float distance_to_object;
        public float min_distance;
        public float fail_distance;
        public float speed;
        public float walk_run_threshhold;
        public float desired_heading;

        public static MovementParameters read(MovementTypes.Type type, BinaryReader binaryReader) {
            MovementParameters newObj = new MovementParameters();
            switch (type) {
                case MovementTypes.Type.MoveToObject:
                case MovementTypes.Type.MoveToPosition: {
                        newObj.bitfield = binaryReader.ReadUInt32();
                        newObj.distance_to_object = binaryReader.ReadSingle();
                        newObj.min_distance = binaryReader.ReadSingle();
                        newObj.fail_distance = binaryReader.ReadSingle();
                        newObj.speed = binaryReader.ReadSingle();
                        newObj.walk_run_threshhold = binaryReader.ReadSingle();
                        newObj.desired_heading = binaryReader.ReadSingle();
                        break;
                    }
                case MovementTypes.Type.TurnToObject:
                case MovementTypes.Type.TurnToHeading: {
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
            node.Nodes.Add("bitfield = " + bitfield);
            node.Nodes.Add("distance_to_object = " + distance_to_object);
            node.Nodes.Add("min_distance = " + min_distance);
            node.Nodes.Add("fail_distance = " + fail_distance);
            node.Nodes.Add("speed = " + speed);
            node.Nodes.Add("walk_run_threshhold = " + walk_run_threshhold);
            node.Nodes.Add("desired_heading = " + desired_heading);
        }
    }

    public class MovementEvent : Message {
        public uint object_id;
        public ushort instance_timestamp;
        public ushort server_control_timestamp;
        public ushort movement_timestamp;
        public byte autonomous;
        public MovementTypes.Type movementType__guessedname;
        public MovementParameters movementParams__guessedname = new MovementParameters();
        public MotionStyle style;
        public InterpretedMotionState motionState__guessedname = new InterpretedMotionState();
        public uint stickToObject;
        public uint moveToObject;
        public Position moveToPos = new Position();
        public float my_run_rate;
        public uint turnToObject;
        public float turnToHeading;

        public static MovementEvent read(BinaryReader binaryReader) {
            MovementEvent newObj = new MovementEvent();
            newObj.object_id = binaryReader.ReadUInt32();
            newObj.instance_timestamp = binaryReader.ReadUInt16();
            newObj.server_control_timestamp = binaryReader.ReadUInt16();
            newObj.movement_timestamp = binaryReader.ReadUInt16();
            newObj.autonomous = binaryReader.ReadByte();

            Util.readToAlign(binaryReader);

            newObj.movementType__guessedname = (MovementTypes.Type)binaryReader.ReadUInt16();
            newObj.style = (MotionStyle)command_ids[binaryReader.ReadUInt16()];

            switch (newObj.movementType__guessedname) {
                // TODO: This now needs to mimic the rest of MovementManager::unpack_movement here
                case MovementTypes.Type.Invalid: {
                        newObj.motionState__guessedname = InterpretedMotionState.read(binaryReader);
                        if (((ushort)newObj.movementType__guessedname & 0x100) != 0) { // TODO: Double check that this is the correct mask here
                            newObj.stickToObject = binaryReader.ReadUInt32();
                        }
                        break;
                    }
                case MovementTypes.Type.MoveToObject: {
                        newObj.moveToObject = binaryReader.ReadUInt32();
                        newObj.moveToPos = Position.readOrigin(binaryReader);
                        newObj.movementParams__guessedname = MovementParameters.read(newObj.movementType__guessedname, binaryReader);
                        newObj.my_run_rate = binaryReader.ReadSingle();
                        break;
                    }
                case MovementTypes.Type.MoveToPosition: {
                        newObj.moveToPos = Position.readOrigin(binaryReader);
                        newObj.movementParams__guessedname = MovementParameters.read(newObj.movementType__guessedname, binaryReader);
                        newObj.my_run_rate = binaryReader.ReadSingle();
                        break;
                    }
                case MovementTypes.Type.TurnToObject: {
                        newObj.turnToObject = binaryReader.ReadUInt32();
                        newObj.turnToHeading = binaryReader.ReadSingle();
                        newObj.movementParams__guessedname = MovementParameters.read(newObj.movementType__guessedname, binaryReader);
                        break;
                    }
                case MovementTypes.Type.TurnToHeading: {
                        newObj.movementParams__guessedname = MovementParameters.read(newObj.movementType__guessedname, binaryReader);
                        break;
                    }
                default: {
                        break;
                    }
            }

            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("object_id = " + Utility.FormatHex(this.object_id));
            rootNode.Nodes.Add("instance_timestamp = " + instance_timestamp);
            rootNode.Nodes.Add("server_control_timestamp = " + server_control_timestamp);
            rootNode.Nodes.Add("movement_timestamp = " + movement_timestamp);
            rootNode.Nodes.Add("autonomous = " + autonomous);
            rootNode.Nodes.Add("movementType__guessedname = " + movementType__guessedname);
            TreeNode moveParamsNode = rootNode.Nodes.Add("movementParams__guessedname = ");
            movementParams__guessedname.contributeToTreeNode(moveParamsNode);
            rootNode.Nodes.Add("style = " + style);
            TreeNode motionStateNode = rootNode.Nodes.Add("motionState__guessedname = ");
            motionState__guessedname.contributeToTreeNode(motionStateNode);
            rootNode.Nodes.Add("stickToObject = " + Utility.FormatHex(stickToObject));
            rootNode.Nodes.Add("moveToObject = " + Utility.FormatHex(moveToObject));
            TreeNode posNode = rootNode.Nodes.Add("moveToPos = ");
            moveToPos.contributeToTreeNode(posNode);
            rootNode.Nodes.Add("my_run_rate = " + my_run_rate);
            rootNode.Nodes.Add("turnToObject = " + turnToObject);
            rootNode.Nodes.Add("turnToHeading = " + turnToHeading);
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
        public HoldKey current_holdkey;
        public MotionStyle current_style = MotionStyle.Motion_NonCombat;
        public MotionStyle forward_command = MotionStyle.Motion_Ready;
        public HoldKey forward_holdkey;
        public float forward_speed = 1.0f;
        public MotionStyle sidestep_command;
        public HoldKey sidestep_holdkey;
        public float sidestep_speed = 1.0f;
        public MotionStyle turn_command;
        public HoldKey turn_holdkey;
        public float turn_speed = 1.0f;

        public static RawMotionState read(BinaryReader binaryReader) {
            RawMotionState newObj = new RawMotionState();
            newObj.bitfield = binaryReader.ReadUInt32();
            if ((newObj.bitfield & (uint)PackBitfield.current_holdkey) != 0) {
                newObj.current_holdkey = (HoldKey)binaryReader.ReadUInt32();
            }
            if ((newObj.bitfield & (uint)PackBitfield.current_style) != 0) {
                newObj.current_style = (MotionStyle)binaryReader.ReadUInt32();
            }
            if ((newObj.bitfield & (uint)PackBitfield.forward_command) != 0) {
                newObj.forward_command = (MotionStyle)binaryReader.ReadUInt32();
            }
            if ((newObj.bitfield & (uint)PackBitfield.forward_holdkey) != 0) {
                newObj.forward_holdkey = (HoldKey)binaryReader.ReadUInt32();
            }
            if ((newObj.bitfield & (uint)PackBitfield.forward_speed) != 0) {
                newObj.forward_speed = binaryReader.ReadSingle();
            }
            if ((newObj.bitfield & (uint)PackBitfield.sidestep_command) != 0) {
                newObj.sidestep_command = (MotionStyle)binaryReader.ReadUInt32();
            }
            if ((newObj.bitfield & (uint)PackBitfield.sidestep_holdkey) != 0) {
                newObj.sidestep_holdkey = (HoldKey)binaryReader.ReadUInt32();
            }
            if ((newObj.bitfield & (uint)PackBitfield.sidestep_speed) != 0) {
                newObj.sidestep_speed = binaryReader.ReadSingle();
            }
            if ((newObj.bitfield & (uint)PackBitfield.turn_command) != 0) {
                newObj.turn_command = (MotionStyle)binaryReader.ReadUInt32();
            }
            if ((newObj.bitfield & (uint)PackBitfield.turn_holdkey) != 0) {
                newObj.turn_holdkey = (HoldKey)binaryReader.ReadUInt32();
            }
            if ((newObj.bitfield & (uint)PackBitfield.turn_speed) != 0) {
                newObj.turn_speed = binaryReader.ReadSingle();
            }

            uint numActions = (newObj.bitfield >> 11);
            for (int i = 0; i < numActions; ++i) {
                // TODO: Do actual stuff here!
                uint thing1 = command_ids[binaryReader.ReadUInt16()];
                uint thing2 = binaryReader.ReadUInt16();
                uint thing3 = binaryReader.ReadUInt32();
            }

            Util.readToAlign(binaryReader);

            return newObj;
        }

        public void contributeToTreeNode(TreeNode node) {
            node.Nodes.Add("bitfield = " + bitfield);
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

    public class LifestoneMaterialize : Message
    {
        public uint ObjectId;
        public int unknown1;
        public Position Position;
        public int unknown2;
        public int unknown3;
        public int unknown4;
        public int unknown5;
        public int unknown6;
        public int unknown7;

        public static LifestoneMaterialize read(BinaryReader binaryReader)
        {
            LifestoneMaterialize newObj = new LifestoneMaterialize();
            newObj.ObjectId = binaryReader.ReadUInt32();
            newObj.unknown1 = binaryReader.ReadInt32();
            newObj.Position = Position.read(binaryReader);
            newObj.unknown2 = binaryReader.ReadInt32();
            newObj.unknown3 = binaryReader.ReadInt32();
            newObj.unknown4 = binaryReader.ReadInt32();
            newObj.unknown5 = binaryReader.ReadInt32();
            newObj.unknown6 = binaryReader.ReadInt32();
            newObj.unknown7 = binaryReader.ReadInt32();

            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("player_id = " + ObjectId);
            rootNode.Nodes.Add("unknown1 = " + unknown1);
            TreeNode posNode = rootNode.Nodes.Add("position = ");
            Position.contributeToTreeNode(posNode);
            posNode.ExpandAll();
            rootNode.Nodes.Add("unknown2 = " + unknown2);
            rootNode.Nodes.Add("unknown3 = " + unknown3);
            rootNode.Nodes.Add("unknown4 = " + unknown4);
            rootNode.Nodes.Add("unknown5 = " + unknown5);
            rootNode.Nodes.Add("unknown6 = " + unknown6);
            rootNode.Nodes.Add("unknown7 = " + unknown7);
            treeView.Nodes.Add(rootNode);
        }
    }

    static uint[] command_ids = {
        2147483648, // Invalid
        2231369729, // HoldRun
        2231369730, // SideStep
        (uint)MotionStyle.Motion_Ready,
        (uint)MotionStyle.Motion_Stop,
        (uint)MotionStyle.Motion_WalkForward,
        (uint)MotionStyle.Motion_WalkBackwards,
        (uint)MotionStyle.Motion_RunForward,
        (uint)MotionStyle.Motion_Fallen,
        (uint)MotionStyle.Motion_Interpolating,
        (uint)MotionStyle.Motion_Hover,
        (uint)MotionStyle.Motion_On,
        (uint)MotionStyle.Motion_Off,
        (uint)MotionStyle.Motion_TurnRight,
        (uint)MotionStyle.Motion_TurnLeft,
        (uint)MotionStyle.Motion_SideStepRight,
        (uint)MotionStyle.Motion_SideStepLeft,
        (uint)MotionStyle.Motion_Dead,
        (uint)MotionStyle.Motion_Crouch,
        (uint)MotionStyle.Motion_Sitting,
        (uint)MotionStyle.Motion_Sleeping,
        (uint)MotionStyle.Motion_Falling,
        (uint)MotionStyle.Motion_Reload,
        (uint)MotionStyle.Motion_Unload,
        (uint)MotionStyle.Motion_Pickup,
        (uint)MotionStyle.Motion_StoreInBackpack,
        (uint)MotionStyle.Motion_Eat,
        (uint)MotionStyle.Motion_Drink,
        (uint)MotionStyle.Motion_Reading,
        (uint)MotionStyle.Motion_JumpCharging,
        (uint)MotionStyle.Motion_AimLevel,
        (uint)MotionStyle.Motion_AimHigh15,
        (uint)MotionStyle.Motion_AimHigh30,
        (uint)MotionStyle.Motion_AimHigh45,
        (uint)MotionStyle.Motion_AimHigh60,
        (uint)MotionStyle.Motion_AimHigh75,
        (uint)MotionStyle.Motion_AimHigh90,
        (uint)MotionStyle.Motion_AimLow15,
        (uint)MotionStyle.Motion_AimLow30,
        (uint)MotionStyle.Motion_AimLow45,
        (uint)MotionStyle.Motion_AimLow60,
        (uint)MotionStyle.Motion_AimLow75,
        (uint)MotionStyle.Motion_AimLow90,
        (uint)MotionStyle.Motion_MagicBlast,
        (uint)MotionStyle.Motion_MagicSelfHead,
        (uint)MotionStyle.Motion_MagicSelfHeart,
        (uint)MotionStyle.Motion_MagicBonus,
        (uint)MotionStyle.Motion_MagicClap,
        (uint)MotionStyle.Motion_MagicHarm,
        (uint)MotionStyle.Motion_MagicHeal,
        (uint)MotionStyle.Motion_MagicThrowMissile,
        (uint)MotionStyle.Motion_MagicRecoilMissile,
        (uint)MotionStyle.Motion_MagicPenalty,
        (uint)MotionStyle.Motion_MagicTransfer,
        (uint)MotionStyle.Motion_MagicVision,
        (uint)MotionStyle.Motion_MagicEnchantItem,
        (uint)MotionStyle.Motion_MagicPortal,
        (uint)MotionStyle.Motion_MagicPray,
        (uint)MotionStyle.Motion_StopTurning,
        (uint)MotionStyle.Motion_Jump,
        2147483708, // HandCombat
        2147483709, // NonCombat
        2147483710, // SwordCombat
        2147483711, // BowCombat
        2147483712, // SwordShieldCombat
        2147483713, // CrossbowCombat
        2147483714, // UnusedCombat
        2147483715, // SlingCombat
        2147483716, // TwoHandedSwordCombat
        2147483717, // TwoHandedStaffCombat
        2147483718, // DualWieldCombat
        2147483719, // ThrownWeaponCombat
        2147483720, // Graze
        2147483721, // Magi
        (uint)MotionStyle.Motion_Hop,
        (uint)MotionStyle.Motion_Jumpup,
        (uint)MotionStyle.Motion_Cheer,
        (uint)MotionStyle.Motion_ChestBeat,
        (uint)MotionStyle.Motion_TippedLeft,
        (uint)MotionStyle.Motion_TippedRight,
        (uint)MotionStyle.Motion_FallDown,
        (uint)MotionStyle.Motion_Twitch1,
        (uint)MotionStyle.Motion_Twitch2,
        (uint)MotionStyle.Motion_Twitch3,
        (uint)MotionStyle.Motion_Twitch4,
        (uint)MotionStyle.Motion_StaggerBackward,
        (uint)MotionStyle.Motion_StaggerForward,
        (uint)MotionStyle.Motion_Sanctuary,
        (uint)MotionStyle.Motion_ThrustMed,
        (uint)MotionStyle.Motion_ThrustLow,
        (uint)MotionStyle.Motion_ThrustHigh,
        (uint)MotionStyle.Motion_SlashHigh,
        (uint)MotionStyle.Motion_SlashMed,
        (uint)MotionStyle.Motion_SlashLow,
        (uint)MotionStyle.Motion_BackhandHigh,
        (uint)MotionStyle.Motion_BackhandMed,
        (uint)MotionStyle.Motion_BackhandLow,
        (uint)MotionStyle.Motion_Shoot,
        (uint)MotionStyle.Motion_AttackHigh1,
        (uint)MotionStyle.Motion_AttackMed1,
        (uint)MotionStyle.Motion_AttackLow1,
        (uint)MotionStyle.Motion_AttackHigh2,
        (uint)MotionStyle.Motion_AttackMed2,
        (uint)MotionStyle.Motion_AttackLow2,
        (uint)MotionStyle.Motion_AttackHigh3,
        (uint)MotionStyle.Motion_AttackMed3,
        (uint)MotionStyle.Motion_AttackLow3,
        (uint)MotionStyle.Motion_HeadThrow,
        (uint)MotionStyle.Motion_FistSlam,
        (uint)MotionStyle.Motion_BreatheFlame_,
        (uint)MotionStyle.Motion_SpinAttack,
        (uint)MotionStyle.Motion_MagicPowerUp01,
        (uint)MotionStyle.Motion_MagicPowerUp02,
        (uint)MotionStyle.Motion_MagicPowerUp03,
        (uint)MotionStyle.Motion_MagicPowerUp04,
        (uint)MotionStyle.Motion_MagicPowerUp05,
        (uint)MotionStyle.Motion_MagicPowerUp06,
        (uint)MotionStyle.Motion_MagicPowerUp07,
        (uint)MotionStyle.Motion_MagicPowerUp08,
        (uint)MotionStyle.Motion_MagicPowerUp09,
        (uint)MotionStyle.Motion_MagicPowerUp10,
        (uint)MotionStyle.Motion_ShakeFist,
        (uint)MotionStyle.Motion_Beckon,
        (uint)MotionStyle.Motion_BeSeeingYou,
        (uint)MotionStyle.Motion_BlowKiss,
        (uint)MotionStyle.Motion_BowDeep,
        (uint)MotionStyle.Motion_ClapHands,
        (uint)MotionStyle.Motion_Cry,
        (uint)MotionStyle.Motion_Laugh,
        (uint)MotionStyle.Motion_MimeEat,
        (uint)MotionStyle.Motion_MimeDrink,
        (uint)MotionStyle.Motion_Nod,
        (uint)MotionStyle.Motion_Point,
        (uint)MotionStyle.Motion_ShakeHead,
        (uint)MotionStyle.Motion_Shrug,
        (uint)MotionStyle.Motion_Wave,
        (uint)MotionStyle.Motion_Akimbo,
        (uint)MotionStyle.Motion_HeartyLaugh,
        (uint)MotionStyle.Motion_Salute,
        (uint)MotionStyle.Motion_ScratchHead,
        (uint)MotionStyle.Motion_SmackHead,
        (uint)MotionStyle.Motion_TapFoot,
        (uint)MotionStyle.Motion_WaveHigh,
        (uint)MotionStyle.Motion_WaveLow,
        (uint)MotionStyle.Motion_YawnStretch,
        (uint)MotionStyle.Motion_Cringe,
        (uint)MotionStyle.Motion_Kneel,
        (uint)MotionStyle.Motion_Plead,
        (uint)MotionStyle.Motion_Shiver,
        (uint)MotionStyle.Motion_Shoo,
        (uint)MotionStyle.Motion_Slouch,
        (uint)MotionStyle.Motion_Spit,
        (uint)MotionStyle.Motion_Surrender,
        (uint)MotionStyle.Motion_Woah,
        (uint)MotionStyle.Motion_Winded,
        (uint)MotionStyle.Motion_YMCA,
        (uint)MotionStyle.Motion_EnterGame,
        (uint)MotionStyle.Motion_ExitGame,
        (uint)MotionStyle.Motion_OnCreation,
        (uint)MotionStyle.Motion_OnDestruction,
        (uint)MotionStyle.Motion_EnterPortal,
        (uint)MotionStyle.Motion_ExitPortal,
        (uint)MotionStyle.Command_Cancel,
        (uint)MotionStyle.Command_UseSelected,
        (uint)MotionStyle.Command_AutosortSelected,
        (uint)MotionStyle.Command_DropSelected,
        (uint)MotionStyle.Command_GiveSelected,
        (uint)MotionStyle.Command_SplitSelected,
        (uint)MotionStyle.Command_ExamineSelected,
        (uint)MotionStyle.Command_CreateShortcutToSelected,
        (uint)MotionStyle.Command_PreviousCompassItem,
        (uint)MotionStyle.Command_NextCompassItem,
        (uint)MotionStyle.Command_ClosestCompassItem,
        (uint)MotionStyle.Command_PreviousSelection,
        (uint)MotionStyle.Command_LastAttacker,
        (uint)MotionStyle.Command_PreviousFellow,
        (uint)MotionStyle.Command_NextFellow,
        (uint)MotionStyle.Command_ToggleCombat,
        (uint)MotionStyle.Command_HighAttack,
        (uint)MotionStyle.Command_MediumAttack,
        (uint)MotionStyle.Command_LowAttack,
        (uint)MotionStyle.Command_EnterChat,
        (uint)MotionStyle.Command_ToggleChat,
        (uint)MotionStyle.Command_SavePosition,
        (uint)MotionStyle.Command_OptionsPanel,
        (uint)MotionStyle.Command_ResetView,
        (uint)MotionStyle.Command_CameraLeftRotate,
        (uint)MotionStyle.Command_CameraRightRotate,
        (uint)MotionStyle.Command_CameraRaise,
        (uint)MotionStyle.Command_CameraLower,
        (uint)MotionStyle.Command_CameraCloser,
        (uint)MotionStyle.Command_CameraFarther,
        (uint)MotionStyle.Command_FloorView,
        (uint)MotionStyle.Command_MouseLook,
        (uint)MotionStyle.Command_PreviousItem,
        (uint)MotionStyle.Command_NextItem,
        (uint)MotionStyle.Command_ClosestItem,
        (uint)MotionStyle.Command_ShiftView,
        (uint)MotionStyle.Command_MapView,
        (uint)MotionStyle.Command_AutoRun,
        (uint)MotionStyle.Command_DecreasePowerSetting,
        (uint)MotionStyle.Command_IncreasePowerSetting,
        (uint)MotionStyle.Motion_Pray,
        (uint)MotionStyle.Motion_Mock,
        (uint)MotionStyle.Motion_Teapot,
        (uint)MotionStyle.Motion_SpecialAttack1,
        (uint)MotionStyle.Motion_SpecialAttack2,
        (uint)MotionStyle.Motion_SpecialAttack3,
        (uint)MotionStyle.Motion_MissileAttack1,
        (uint)MotionStyle.Motion_MissileAttack2,
        (uint)MotionStyle.Motion_MissileAttack3,
        (uint)MotionStyle.Motion_CastSpell,
        (uint)MotionStyle.Motion_Flatulence,
        (uint)MotionStyle.Command_FirstPersonView,
        (uint)MotionStyle.Command_AllegiancePanel,
        (uint)MotionStyle.Command_FellowshipPanel,
        (uint)MotionStyle.Command_SpellbookPanel,
        (uint)MotionStyle.Command_SpellComponentsPanel,
        (uint)MotionStyle.Command_HousePanel,
        (uint)MotionStyle.Command_AttributesPanel,
        (uint)MotionStyle.Command_SkillsPanel,
        (uint)MotionStyle.Command_MapPanel,
        (uint)MotionStyle.Command_InventoryPanel,
        (uint)MotionStyle.Motion_Demonet,
        (uint)MotionStyle.Motion_UseMagicStaff,
        (uint)MotionStyle.Motion_UseMagicWand,
        (uint)MotionStyle.Motion_Blink,
        (uint)MotionStyle.Motion_Bite,
        (uint)MotionStyle.Motion_TwitchSubstate1,
        (uint)MotionStyle.Motion_TwitchSubstate2,
        (uint)MotionStyle.Motion_TwitchSubstate3,
        (uint)MotionStyle.Command_CaptureScreenshotToFile,
        2147483880, // BowNoAmmo
        2147483881, // CrossBowNoAmmo
        (uint)MotionStyle.Motion_ShakeFistState,
        (uint)MotionStyle.Motion_PrayState,
        (uint)MotionStyle.Motion_BowDeepState,
        (uint)MotionStyle.Motion_ClapHandsState,
        (uint)MotionStyle.Motion_CrossArmsState,
        (uint)MotionStyle.Motion_ShiverState,
        (uint)MotionStyle.Motion_PointState,
        (uint)MotionStyle.Motion_WaveState,
        (uint)MotionStyle.Motion_AkimboState,
        (uint)MotionStyle.Motion_SaluteState,
        (uint)MotionStyle.Motion_ScratchHeadState,
        (uint)MotionStyle.Motion_TapFootState,
        (uint)MotionStyle.Motion_LeanState,
        (uint)MotionStyle.Motion_KneelState,
        (uint)MotionStyle.Motion_PleadState,
        (uint)MotionStyle.Motion_ATOYOT,
        (uint)MotionStyle.Motion_SlouchState,
        (uint)MotionStyle.Motion_SurrenderState,
        (uint)MotionStyle.Motion_WoahState,
        (uint)MotionStyle.Motion_WindedState,
        (uint)MotionStyle.Command_AutoCreateShortcuts,
        (uint)MotionStyle.Command_AutoRepeatAttacks,
        (uint)MotionStyle.Command_AutoTarget,
        (uint)MotionStyle.Command_AdvancedCombatInterface,
        (uint)MotionStyle.Command_IgnoreAllegianceRequests,
        (uint)MotionStyle.Command_IgnoreFellowshipRequests,
        (uint)MotionStyle.Command_InvertMouseLook,
        (uint)MotionStyle.Command_LetPlayersGiveYouItems,
        (uint)MotionStyle.Command_AutoTrackCombatTargets,
        (uint)MotionStyle.Command_DisplayTooltips,
        (uint)MotionStyle.Command_AttemptToDeceivePlayers,
        (uint)MotionStyle.Command_RunAsDefaultMovement,
        (uint)MotionStyle.Command_StayInChatModeAfterSend,
        (uint)MotionStyle.Command_RightClickToMouseLook,
        (uint)MotionStyle.Command_VividTargetIndicator,
        (uint)MotionStyle.Command_SelectSelf,
        (uint)MotionStyle.Motion_SkillHealSelf,
        (uint)MotionStyle.Command_NextMonster,
        (uint)MotionStyle.Command_PreviousMonster,
        (uint)MotionStyle.Command_ClosestMonster,
        (uint)MotionStyle.Command_NextPlayer,
        (uint)MotionStyle.Command_PreviousPlayer,
        (uint)MotionStyle.Command_ClosestPlayer,
        (uint)MotionStyle.Motion_SnowAngelState,
        (uint)MotionStyle.Motion_WarmHands,
        (uint)MotionStyle.Motion_CurtseyState,
        (uint)MotionStyle.Motion_AFKState,
        (uint)MotionStyle.Motion_MeditateState,
        (uint)MotionStyle.Command_TradePanel,
        (uint)MotionStyle.Motion_LogOut,
        (uint)MotionStyle.Motion_DoubleSlashLow,
        (uint)MotionStyle.Motion_DoubleSlashMed,
        (uint)MotionStyle.Motion_DoubleSlashHigh,
        (uint)MotionStyle.Motion_TripleSlashLow,
        (uint)MotionStyle.Motion_TripleSlashMed,
        (uint)MotionStyle.Motion_TripleSlashHigh,
        (uint)MotionStyle.Motion_DoubleThrustLow,
        (uint)MotionStyle.Motion_DoubleThrustMed,
        (uint)MotionStyle.Motion_DoubleThrustHigh,
        (uint)MotionStyle.Motion_TripleThrustLow,
        (uint)MotionStyle.Motion_TripleThrustMed,
        (uint)MotionStyle.Motion_TripleThrustHigh,
        (uint)MotionStyle.Motion_MagicPowerUp01Purple,
        (uint)MotionStyle.Motion_MagicPowerUp02Purple,
        (uint)MotionStyle.Motion_MagicPowerUp03Purple,
        (uint)MotionStyle.Motion_MagicPowerUp04Purple,
        (uint)MotionStyle.Motion_MagicPowerUp05Purple,
        (uint)MotionStyle.Motion_MagicPowerUp06Purple,
        (uint)MotionStyle.Motion_MagicPowerUp07Purple,
        (uint)MotionStyle.Motion_MagicPowerUp08Purple,
        (uint)MotionStyle.Motion_MagicPowerUp09Purple,
        (uint)MotionStyle.Motion_MagicPowerUp10Purple,
        (uint)MotionStyle.Motion_Helper,
        (uint)MotionStyle.Motion_Pickup5,
        (uint)MotionStyle.Motion_Pickup10,
        (uint)MotionStyle.Motion_Pickup15,
        (uint)MotionStyle.Motion_Pickup20,
        (uint)MotionStyle.Motion_HouseRecall,
        2147483960,
        2147483961,
        (uint)MotionStyle.Motion_SitState,
        (uint)MotionStyle.Motion_SitCrossleggedState,
        (uint)MotionStyle.Motion_SitBackState,
        (uint)MotionStyle.Motion_PointLeftState,
        (uint)MotionStyle.Motion_PointRightState,
        (uint)MotionStyle.Motion_TalktotheHandState,
        (uint)MotionStyle.Motion_PointDownState,
        (uint)MotionStyle.Motion_DrudgeDanceState,
        (uint)MotionStyle.Motion_PossumState,
        (uint)MotionStyle.Motion_ReadState,
        (uint)MotionStyle.Motion_ThinkerState,
        (uint)MotionStyle.Motion_HaveASeatState,
        (uint)MotionStyle.Motion_AtEaseState,
        (uint)MotionStyle.Motion_NudgeLeft,
        (uint)MotionStyle.Motion_NudgeRight,
        (uint)MotionStyle.Motion_PointLeft,
        (uint)MotionStyle.Motion_PointRight,
        (uint)MotionStyle.Motion_PointDown,
        (uint)MotionStyle.Motion_Knock,
        (uint)MotionStyle.Motion_ScanHorizon,
        (uint)MotionStyle.Motion_DrudgeDance,
        (uint)MotionStyle.Motion_HaveASeat,
        (uint)MotionStyle.Motion_LifestoneRecall,
        (uint)MotionStyle.Command_CharacterOptionsPanel,
        (uint)MotionStyle.Command_SoundAndGraphicsPanel,
        (uint)MotionStyle.Command_HelpfulSpellsPanel,
        (uint)MotionStyle.Command_HarmfulSpellsPanel,
        (uint)MotionStyle.Command_CharacterInformationPanel,
        (uint)MotionStyle.Command_LinkStatusPanel,
        (uint)MotionStyle.Command_VitaePanel,
        (uint)MotionStyle.Command_ShareFellowshipXP,
        (uint)MotionStyle.Command_ShareFellowshipLoot,
        (uint)MotionStyle.Command_AcceptCorpseLooting,
        (uint)MotionStyle.Command_IgnoreTradeRequests,
        (uint)MotionStyle.Command_DisableWeather,
        (uint)MotionStyle.Command_DisableHouseEffect,
        (uint)MotionStyle.Command_SideBySideVitals,
        (uint)MotionStyle.Command_ShowRadarCoordinates,
        (uint)MotionStyle.Command_ShowSpellDurations,
        (uint)MotionStyle.Command_MuteOnLosingFocus,
        (uint)MotionStyle.Motion_Fishing,
        (uint)MotionStyle.Motion_MarketplaceRecall,
        (uint)MotionStyle.Motion_EnterPKLite,
        (uint)MotionStyle.Command_AllegianceChat,
        (uint)MotionStyle.Command_AutomaticallyAcceptFellowshipRequests,
        (uint)MotionStyle.Command_Reply,
        (uint)MotionStyle.Command_MonarchReply,
        (uint)MotionStyle.Command_PatronReply,
        (uint)MotionStyle.Command_ToggleCraftingChanceOfSuccessDialog,
        (uint)MotionStyle.Command_UseClosestUnopenedCorpse,
        (uint)MotionStyle.Command_UseNextUnopenedCorpse,
        (uint)MotionStyle.Command_IssueSlashCommand,
        (uint)MotionStyle.Motion_AllegianceHometownRecall,
        (uint)MotionStyle.Motion_PKArenaRecall,
        (uint)MotionStyle.Motion_OffhandSlashHigh,
        (uint)MotionStyle.Motion_OffhandSlashMed,
        (uint)MotionStyle.Motion_OffhandSlashLow,
        (uint)MotionStyle.Motion_OffhandThrustHigh,
        (uint)MotionStyle.Motion_OffhandThrustMed,
        (uint)MotionStyle.Motion_OffhandThrustLow,
        (uint)MotionStyle.Motion_OffhandDoubleSlashLow,
        (uint)MotionStyle.Motion_OffhandDoubleSlashMed,
        (uint)MotionStyle.Motion_OffhandDoubleSlashHigh,
        (uint)MotionStyle.Motion_OffhandTripleSlashLow,
        (uint)MotionStyle.Motion_OffhandTripleSlashMed,
        (uint)MotionStyle.Motion_OffhandTripleSlashHigh,
        (uint)MotionStyle.Motion_OffhandDoubleThrustLow,
        (uint)MotionStyle.Motion_OffhandDoubleThrustMed,
        (uint)MotionStyle.Motion_OffhandDoubleThrustHigh,
        (uint)MotionStyle.Motion_OffhandTripleThrustLow,
        (uint)MotionStyle.Motion_OffhandTripleThrustMed,
        (uint)MotionStyle.Motion_OffhandTripleThrustHigh,
        (uint)MotionStyle.Motion_OffhandKick,
        (uint)MotionStyle.Motion_AttackHigh4,
        (uint)MotionStyle.Motion_AttackMed4,
        (uint)MotionStyle.Motion_AttackLow4,
        (uint)MotionStyle.Motion_AttackHigh5,
        (uint)MotionStyle.Motion_AttackMed5,
        (uint)MotionStyle.Motion_AttackLow5,
        (uint)MotionStyle.Motion_AttackHigh6,
        (uint)MotionStyle.Motion_AttackMed6,
        (uint)MotionStyle.Motion_AttackLow6,
        (uint)MotionStyle.Motion_PunchFastHigh,
        (uint)MotionStyle.Motion_PunchFastMed,
        (uint)MotionStyle.Motion_PunchFastLow,
        (uint)MotionStyle.Motion_PunchSlowHigh,
        (uint)MotionStyle.Motion_PunchSlowMed,
        (uint)MotionStyle.Motion_PunchSlowLow,
        (uint)MotionStyle.Motion_OffhandPunchFastHigh,
        (uint)MotionStyle.Motion_OffhandPunchFastMed,
        (uint)MotionStyle.Motion_OffhandPunchFastLow,
        (uint)MotionStyle.Motion_OffhandPunchSlowHigh,
        (uint)MotionStyle.Motion_OffhandPunchSlowMed,
        (uint)MotionStyle.Motion_OffhandPunchSlowLow
    };
}
