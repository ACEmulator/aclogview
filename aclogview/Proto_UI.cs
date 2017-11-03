using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using aclogview;

public class Proto_UI : MessageProcessor {

    public override bool acceptMessageData(BinaryReader messageDataReader, TreeView outputTreeView) {
        bool handled = true;

        PacketOpcode opcode = Util.readOpcode(messageDataReader);
        switch (opcode) {
            case PacketOpcode.CLIENT_REQUEST_ENTER_GAME_EVENT:
            case PacketOpcode.Evt_Admin__GetServerVersion_ID: {
                    EmptyMessage message = new EmptyMessage(opcode);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.CHARACTER_GENERATION_VERIFICATION_RESPONSE_EVENT: {
                    CharGenVerificationResponse message = CharGenVerificationResponse.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.CHARACTER_EXIT_GAME_EVENT: {
                    LogOff message = LogOff.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            // TODO: CHARACTER_PREVIEW_EVENT
            case PacketOpcode.CHARACTER_DELETE_EVENT: {
                    CharacterDelete message = CharacterDelete.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.CHARACTER_CREATE_EVENT: {
                    CharacterCreate message = CharacterCreate.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.CHARACTER_ENTER_GAME_EVENT: {
                    EnterWorld message = EnterWorld.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.CONTROL_FORCE_OBJDESC_SEND_EVENT: {
                    ForceObjdesc message = ForceObjdesc.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Admin__Friends_ID: {
                    Friends message = Friends.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Admin__AdminRestoreCharacter_ID: {
                    AdminRestoreCharacter message = AdminRestoreCharacter.read(messageDataReader);
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
    
    public class CharGenVerificationResponse : Message {
        public CG_VERIFICATION_RESPONSE verificationResponse;
        public CM_Login.CharacterIdentity ident = new CM_Login.CharacterIdentity();

        public static CharGenVerificationResponse read(BinaryReader binaryReader) {
            CharGenVerificationResponse newObj = new CharGenVerificationResponse();
            newObj.verificationResponse = (CG_VERIFICATION_RESPONSE)binaryReader.ReadUInt32();
            switch (newObj.verificationResponse) {
                case CG_VERIFICATION_RESPONSE.CG_VERIFICATION_RESPONSE_OK: {
                        newObj.ident = CM_Login.CharacterIdentity.read(binaryReader);
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
            rootNode.Nodes.Add("verificationResponse = " + verificationResponse);
            TreeNode identNode = rootNode.Nodes.Add("ident = ");
            ident.contributeToTreeNode(identNode);
            treeView.Nodes.Add(rootNode);
        }
    }

    // TODO: This is bidirectional: client-to-sever has a gid; server-to-client does not
    public class LogOff : Message {
        public uint gid;

        public static LogOff read(BinaryReader binaryReader) {
            LogOff newObj = new LogOff();
            newObj.gid = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("gid = " + gid);
            treeView.Nodes.Add(rootNode);
        }
    }

    // TODO: This is bidirectional: client-to-sever has a slot; server-to-client does not
    public class CharacterDelete : Message {
        public PStringChar account;
        public int slot;

        public static CharacterDelete read(BinaryReader binaryReader) {
            CharacterDelete newObj = new CharacterDelete();
            newObj.account = PStringChar.read(binaryReader);
            newObj.slot = binaryReader.ReadInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("account = " + account);
            rootNode.Nodes.Add("slot = " + slot);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class ACCharGenResult {
        public uint unkConstOne;
        public HeritageGroup heritageGroup;
        public uint gender;
        public uint eyesStrip;
        public uint noseStrip;
        public uint mouthStrip;
        public uint hairColor;
        public uint eyeColor;
        public uint hairStyle;
        public uint headgearStyle;
        public uint headgearColor;
        public uint shirtStyle;
        public uint shirtColor;
        public uint trousersStyle;
        public uint trousersColor;
        public uint footwearStyle;
        public uint footwearColor;
        public double skinShade;
        public double hairShade;
        public double headgearShade;
        public double shirtShade;
        public double trousersShade;
        public double footwearShade;
        public uint templateNum;
        public uint strength;
        public uint endurance;
        public uint coordination;
        public uint quickness;
        public uint focus;
        public uint self;
        public uint slot;
        public uint classID;
        public PList<SKILL_ADVANCEMENT_CLASS> sacs__guessedname;
        public PStringChar name;
        public uint startArea;
        public uint isAdmin;
        public uint isEnvoy;
        public uint totalSkillPts__guessedname;

        public static ACCharGenResult read(BinaryReader binaryReader) {
            ACCharGenResult newObj = new ACCharGenResult();
            newObj.unkConstOne = binaryReader.ReadUInt32();
            newObj.heritageGroup = (HeritageGroup)binaryReader.ReadUInt32();
            newObj.gender = binaryReader.ReadUInt32();
            newObj.eyesStrip = binaryReader.ReadUInt32();
            newObj.noseStrip = binaryReader.ReadUInt32();
            newObj.mouthStrip = binaryReader.ReadUInt32();
            newObj.hairColor = binaryReader.ReadUInt32();
            newObj.eyeColor = binaryReader.ReadUInt32();
            newObj.hairStyle = binaryReader.ReadUInt32();
            newObj.headgearStyle = binaryReader.ReadUInt32();
            newObj.headgearColor = binaryReader.ReadUInt32();
            newObj.shirtStyle = binaryReader.ReadUInt32();
            newObj.shirtColor = binaryReader.ReadUInt32();
            newObj.trousersStyle = binaryReader.ReadUInt32();
            newObj.trousersColor = binaryReader.ReadUInt32();
            newObj.footwearStyle = binaryReader.ReadUInt32();
            newObj.footwearColor = binaryReader.ReadUInt32();
            newObj.skinShade = binaryReader.ReadDouble();
            newObj.hairShade = binaryReader.ReadDouble();
            newObj.headgearShade = binaryReader.ReadDouble();
            newObj.shirtShade = binaryReader.ReadDouble();
            newObj.trousersShade = binaryReader.ReadDouble();
            newObj.footwearShade = binaryReader.ReadDouble();
            newObj.templateNum = binaryReader.ReadUInt32();
            newObj.strength = binaryReader.ReadUInt32();
            newObj.endurance = binaryReader.ReadUInt32();
            newObj.coordination = binaryReader.ReadUInt32();
            newObj.quickness = binaryReader.ReadUInt32();
            newObj.focus = binaryReader.ReadUInt32();
            newObj.self = binaryReader.ReadUInt32();
            newObj.slot = binaryReader.ReadUInt32();
            newObj.classID = binaryReader.ReadUInt32();
            newObj.sacs__guessedname = PList<SKILL_ADVANCEMENT_CLASS>.read(binaryReader);
            newObj.name = PStringChar.read(binaryReader);
            newObj.startArea = binaryReader.ReadUInt32();
            newObj.isAdmin = binaryReader.ReadUInt32();
            newObj.isEnvoy = binaryReader.ReadUInt32();
            newObj.totalSkillPts__guessedname = binaryReader.ReadUInt32();
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node) {
            node.Nodes.Add("unkConstOne = " + unkConstOne);
            node.Nodes.Add("heritageGroup = " + heritageGroup);
            node.Nodes.Add("gender = " + gender);
            node.Nodes.Add("eyesStrip = " + eyesStrip);
            node.Nodes.Add("noseStrip = " + noseStrip);
            node.Nodes.Add("mouthStrip = " + mouthStrip);
            node.Nodes.Add("hairColor = " + hairColor);
            node.Nodes.Add("eyeColor = " + eyeColor);
            node.Nodes.Add("hairStyle = " + hairStyle);
            node.Nodes.Add("headgearStyle = " + headgearStyle);
            node.Nodes.Add("headgearColor = " + headgearColor);
            node.Nodes.Add("shirtStyle = " + shirtStyle);
            node.Nodes.Add("shirtColor = " + shirtColor);
            node.Nodes.Add("trousersStyle = " + trousersStyle);
            node.Nodes.Add("trousersColor = " + trousersColor);
            node.Nodes.Add("footwearStyle = " + footwearStyle);
            node.Nodes.Add("footwearColor = " + footwearColor);
            node.Nodes.Add("skinShade = " + skinShade);
            node.Nodes.Add("hairShade = " + hairShade);
            node.Nodes.Add("headgearShade = " + headgearShade);
            node.Nodes.Add("shirtShade = " + shirtShade);
            node.Nodes.Add("trousersShade = " + trousersShade);
            node.Nodes.Add("footwearShade = " + footwearShade);
            node.Nodes.Add("templateNum = " + templateNum);
            node.Nodes.Add("strength = " + strength);
            node.Nodes.Add("endurance = " + endurance);
            node.Nodes.Add("coordination = " + coordination);
            node.Nodes.Add("quickness = " + quickness);
            node.Nodes.Add("focus = " + focus);
            node.Nodes.Add("self = " + self);
            node.Nodes.Add("slot = " + slot);
            node.Nodes.Add("classID = " + classID);
            TreeNode sacsNode = node.Nodes.Add("sacs__guessedname = ");
            sacs__guessedname.contributeToTreeNode(sacsNode);
            node.Nodes.Add("name = " + name);
            node.Nodes.Add("startArea = " + startArea);
            node.Nodes.Add("isAdmin = " + isAdmin);
            node.Nodes.Add("isEnvoy = " + isEnvoy);
            node.Nodes.Add("totalSkillPts__guessedname = " + totalSkillPts__guessedname);
        }
    }

    public class CharacterCreate : Message {
        public PStringChar account;
        public ACCharGenResult _charGenResult;

        public static CharacterCreate read(BinaryReader binaryReader) {
            CharacterCreate newObj = new CharacterCreate();
            newObj.account = PStringChar.read(binaryReader);
            newObj._charGenResult = ACCharGenResult.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("account = " + account);
            TreeNode resultNode = rootNode.Nodes.Add("_charGenResult = ");
            _charGenResult.contributeToTreeNode(resultNode);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class EnterWorld : Message {
        public uint gid;
        public PStringChar account;

        public static EnterWorld read(BinaryReader binaryReader) {
            EnterWorld newObj = new EnterWorld();
            newObj.gid = binaryReader.ReadUInt32();
            newObj.account = PStringChar.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("gid = " + gid);
            rootNode.Nodes.Add("account = " + account.m_buffer);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class ForceObjdesc : Message {
        public uint object_id;

        public static ForceObjdesc read(BinaryReader binaryReader) {
            ForceObjdesc newObj = new ForceObjdesc();
            newObj.object_id = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("object_id = " + Utility.FormatHex(this.object_id));
            treeView.Nodes.Add(rootNode);
        }
    }

    public class Friends : Message {
        public uint cmd; // TODO: Perhaps a FriendsUpdateType??
        public PStringChar i_player;

        public static Friends read(BinaryReader binaryReader) {
            Friends newObj = new Friends();
            newObj.cmd = binaryReader.ReadUInt32();
            newObj.i_player = PStringChar.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("cmd = " + cmd);
            rootNode.Nodes.Add("i_player = " + i_player);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class AdminRestoreCharacter : Message {
        public uint iid;
        public PStringChar i_restoredCharName;
        public PStringChar i_acctToRestoreTo;

        public static AdminRestoreCharacter read(BinaryReader binaryReader) {
            AdminRestoreCharacter newObj = new AdminRestoreCharacter();
            newObj.iid = binaryReader.ReadUInt32();
            newObj.i_restoredCharName = PStringChar.read(binaryReader);
            newObj.i_acctToRestoreTo = PStringChar.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("iid = " + iid);
            rootNode.Nodes.Add("i_restoredCharName = " + i_restoredCharName);
            rootNode.Nodes.Add("i_acctToRestoreTo = " + i_acctToRestoreTo);
            treeView.Nodes.Add(rootNode);
        }
    }
}
