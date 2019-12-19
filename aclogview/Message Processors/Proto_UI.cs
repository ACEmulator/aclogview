using System;
using System.Collections.Generic;
using System.Drawing;
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
                    ContextInfo.AddToList(new ContextInfo { DataType = DataType.Opcode });
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
                    AdminFriends message = AdminFriends.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Admin__AdminRestoreCharacter_ID: {
                    AdminRestoreCharacter message = AdminRestoreCharacter.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.ACCOUNT_BOOTED_EVENT:
                {
                    AccountBootedEvent message = AccountBootedEvent.read(messageDataReader);
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
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Opcode });
            rootNode.Nodes.Add("verificationResponse = " + verificationResponse);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            if (verificationResponse == CG_VERIFICATION_RESPONSE.CG_VERIFICATION_RESPONSE_OK)
            {
                TreeNode identNode = rootNode.Nodes.Add("ident = ");
                ContextInfo.AddToList(new ContextInfo { Length = ident.Length }, updateDataIndex: false);
                ident.contributeToTreeNode(identNode);
            }
            treeView.Nodes.Add(rootNode);
        }
    }

    // This is bidirectional: client-to-sever has a gid; server-to-client does not
    public class LogOff : Message {
        public uint gid;
        public bool clientMessage = true;

        public static LogOff read(BinaryReader binaryReader) {
            LogOff newObj = new LogOff();
            if (binaryReader.BaseStream.Length == 8) // Client message
            {
                newObj.gid = binaryReader.ReadUInt32();
            }
            else // Server message
            {
                newObj.clientMessage = false;
            }   
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Opcode });
            if (clientMessage)
            {
                rootNode.Nodes.Add("gid = " + Utility.FormatHex(gid));
                ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            }
            treeView.Nodes.Add(rootNode);
        }
    }

    // This is bidirectional: client-to-sever has an account and a slot; server-to-client does not
    public class CharacterDelete : Message {
        public PStringChar account;
        public int slot;
        public bool clientMessage = true;

        public static CharacterDelete read(BinaryReader binaryReader) {
            CharacterDelete newObj = new CharacterDelete();
            // Client message length seems to always be 36 but just in case the string is of a 
            // different length we will check against the server message length.
            if (binaryReader.BaseStream.Length > 4) // Client message
            {
                newObj.account = PStringChar.read(binaryReader);
                newObj.slot = binaryReader.ReadInt32();
            }
            else // Server message
            {
                newObj.clientMessage = false;
            }
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Opcode });
            if (clientMessage)
            {
                rootNode.Nodes.Add("account = " + account);
                ContextInfo.AddToList(new ContextInfo { Length = account.Length, DataType = DataType.Serialized_AsciiString });
                rootNode.Nodes.Add("slot = " + slot);
                ContextInfo.AddToList(new ContextInfo { Length = 4 });
            }
            treeView.Nodes.Add(rootNode);
        }
    }

    public class ACCharGenResult {
        public uint packVersion__guessedname;
        public HeritageGroup heritageGroup;
        public uint gender;
        public int eyesStrip;
        public int noseStrip;
        public int mouthStrip;
        public int hairColor;
        public int eyeColor;
        public int hairStyle;
        public int headgearStyle;
        public uint headgearColor;
        public int shirtStyle;
        public uint shirtColor;
        public int trousersStyle;
        public uint trousersColor;
        public int footwearStyle;
        public uint footwearColor;
        public double skinShade;
        public double hairShade;
        public double headgearShade;
        public double shirtShade;
        public double trousersShade;
        public double footwearShade;
        public int templateNum;
        public int strength;
        public int endurance;
        public int coordination;
        public int quickness;
        public int focus;
        public int self;
        public int slot;
        public uint classID;
        public PList<SKILL_ADVANCEMENT_CLASS> skillAdvancementClasses;
        public PStringChar name;
        public uint startArea;
        public int isAdmin;
        public int isEnvoy;
        // Note: The following field is the sum of the following fields: heritageGroup, gender, eyesStrip, noseStrip, mouthStrip, hairColor, eyeColor,
        // hairStyle, headgearStyle, shirtStyle, trousersStyle, footwearStyle, templateNum, strength, endurance, coordination, quickness, focus, and self
        public int validationChecksum__guessedname;
        public int Length;

        public static ACCharGenResult read(BinaryReader binaryReader) {
            ACCharGenResult newObj = new ACCharGenResult();
            var startPosition = binaryReader.BaseStream.Position;
            newObj.packVersion__guessedname = binaryReader.ReadUInt32();
            newObj.heritageGroup = (HeritageGroup)binaryReader.ReadUInt32();
            newObj.gender = binaryReader.ReadUInt32();
            newObj.eyesStrip = binaryReader.ReadInt32();
            newObj.noseStrip = binaryReader.ReadInt32();
            newObj.mouthStrip = binaryReader.ReadInt32();
            newObj.hairColor = binaryReader.ReadInt32();
            newObj.eyeColor = binaryReader.ReadInt32();
            newObj.hairStyle = binaryReader.ReadInt32();
            newObj.headgearStyle = binaryReader.ReadInt32();
            newObj.headgearColor = binaryReader.ReadUInt32();
            newObj.shirtStyle = binaryReader.ReadInt32();
            newObj.shirtColor = binaryReader.ReadUInt32();
            newObj.trousersStyle = binaryReader.ReadInt32();
            newObj.trousersColor = binaryReader.ReadUInt32();
            newObj.footwearStyle = binaryReader.ReadInt32();
            newObj.footwearColor = binaryReader.ReadUInt32();
            newObj.skinShade = binaryReader.ReadDouble();
            newObj.hairShade = binaryReader.ReadDouble();
            newObj.headgearShade = binaryReader.ReadDouble();
            newObj.shirtShade = binaryReader.ReadDouble();
            newObj.trousersShade = binaryReader.ReadDouble();
            newObj.footwearShade = binaryReader.ReadDouble();
            newObj.templateNum = binaryReader.ReadInt32();
            newObj.strength = binaryReader.ReadInt32();
            newObj.endurance = binaryReader.ReadInt32();
            newObj.coordination = binaryReader.ReadInt32();
            newObj.quickness = binaryReader.ReadInt32();
            newObj.focus = binaryReader.ReadInt32();
            newObj.self = binaryReader.ReadInt32();
            newObj.slot = binaryReader.ReadInt32();
            newObj.classID = binaryReader.ReadUInt32();
            newObj.skillAdvancementClasses = PList<SKILL_ADVANCEMENT_CLASS>.read(binaryReader);
            newObj.name = PStringChar.read(binaryReader);
            newObj.startArea = binaryReader.ReadUInt32();
            newObj.isAdmin = binaryReader.ReadInt32();
            newObj.isEnvoy = binaryReader.ReadInt32();
            newObj.validationChecksum__guessedname = binaryReader.ReadInt32();
            newObj.Length = (int)(binaryReader.BaseStream.Position - startPosition);
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node) {
            node.Nodes.Add("packVersion__guessedname = " + packVersion__guessedname);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("heritageGroup = " + heritageGroup);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("gender = " + (Gender)gender);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("eyesStrip = " + eyesStrip);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("noseStrip = " + noseStrip);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("mouthStrip = " + mouthStrip);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("hairColor = " + hairColor);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("eyeColor = " + eyeColor);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("hairStyle = " + hairStyle);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("headgearStyle = " + headgearStyle);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("headgearColor = " + headgearColor);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("shirtStyle = " + shirtStyle);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("shirtColor = " + shirtColor);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("trousersStyle = " + trousersStyle);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("trousersColor = " + trousersColor);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("footwearStyle = " + footwearStyle);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("footwearColor = " + footwearColor);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("skinShade = " + skinShade);
            ContextInfo.AddToList(new ContextInfo { Length = 8 });
            node.Nodes.Add("hairShade = " + hairShade);
            ContextInfo.AddToList(new ContextInfo { Length = 8 });
            node.Nodes.Add("headgearShade = " + headgearShade);
            ContextInfo.AddToList(new ContextInfo { Length = 8 });
            node.Nodes.Add("shirtShade = " + shirtShade);
            ContextInfo.AddToList(new ContextInfo { Length = 8 });
            node.Nodes.Add("trousersShade = " + trousersShade);
            ContextInfo.AddToList(new ContextInfo { Length = 8 });
            node.Nodes.Add("footwearShade = " + footwearShade);
            ContextInfo.AddToList(new ContextInfo { Length = 8 });
            node.Nodes.Add("templateNum = " + (CG_Profession)templateNum);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("strength = " + strength);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("endurance = " + endurance);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("coordination = " + coordination);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("quickness = " + quickness);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("focus = " + focus);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("self = " + self);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("slot = " + slot);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("classID = " + (WCLASSID)classID);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            TreeNode sacsNode = node.Nodes.Add("skillAdvancementClasses = ");
            ContextInfo.AddToList(new ContextInfo { Length = skillAdvancementClasses.Length }, updateDataIndex: false);
            // Skip PList count dword
            ContextInfo.DataIndex += 4;
            for (int i = 0; i < skillAdvancementClasses.list.Count; i++)
            {
                sacsNode.Nodes.Add($"{(STypeSkill)i} = " + skillAdvancementClasses.list[i]);
                ContextInfo.AddToList(new ContextInfo { Length = 4 });
            }
            node.Nodes.Add("name = " + name);
            ContextInfo.AddToList(new ContextInfo { Length = name.Length, DataType = DataType.Serialized_AsciiString });
            node.Nodes.Add("startArea = " + (CG_Town)startArea);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("isAdmin = " + isAdmin);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("isEnvoy = " + isEnvoy);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            node.Nodes.Add("validationChecksum__guessedname = " + validationChecksum__guessedname);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
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
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Opcode });
            rootNode.Nodes.Add("account = " + account);
            ContextInfo.AddToList(new ContextInfo { Length = account.Length, DataType = DataType.Serialized_AsciiString });
            TreeNode resultNode = rootNode.Nodes.Add("_charGenResult = ");
            ContextInfo.AddToList(new ContextInfo { Length = _charGenResult.Length }, updateDataIndex: false);
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
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Opcode });
            rootNode.Nodes.Add("gid = " + Utility.FormatHex(gid));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            rootNode.Nodes.Add("account = " + account.m_buffer);
            ContextInfo.AddToList(new ContextInfo { Length = account.Length, DataType = DataType.Serialized_AsciiString });
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
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Opcode });
            rootNode.Nodes.Add("object_id = " + Utility.FormatHex(this.object_id));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class AdminFriends : Message {
        // This command is sent by the client typing @friends old in chat.
        // The field cmd is always 0 and i_player is a null string.
        public uint cmd;
        public PStringChar i_player;

        public static AdminFriends read(BinaryReader binaryReader) {
            AdminFriends newObj = new AdminFriends();
            newObj.cmd = binaryReader.ReadUInt32();
            newObj.i_player = PStringChar.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Opcode });
            rootNode.Nodes.Add("cmd = " + cmd);
            ContextInfo.AddToList(new ContextInfo { Length = 4 });
            rootNode.Nodes.Add("i_player = " + i_player);
            ContextInfo.AddToList(new ContextInfo { Length = i_player.Length });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class AdminRestoreCharacter : Message {
        // The fields i_restoredCharName and i_acctToRestoreTo are both sent from the client as null strings.
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
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Opcode });
            rootNode.Nodes.Add("iid = " + Utility.FormatHex(iid));
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.ObjectID });
            rootNode.Nodes.Add("i_restoredCharName = " + i_restoredCharName);
            ContextInfo.AddToList(new ContextInfo { Length = i_restoredCharName.Length });
            rootNode.Nodes.Add("i_acctToRestoreTo = " + i_acctToRestoreTo);
            ContextInfo.AddToList(new ContextInfo { Length = i_acctToRestoreTo.Length });
            treeView.Nodes.Add(rootNode);
        }
    }

    public class AccountBootedEvent : Message
    {
        public PStringChar additionalReasonText;

        public static AccountBootedEvent read(BinaryReader binaryReader)
        {
            AccountBootedEvent newObj = new AccountBootedEvent();
            newObj.additionalReasonText = PStringChar.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView)
        {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            ContextInfo.AddToList(new ContextInfo { DataType = DataType.Opcode });
            rootNode.Nodes.Add("additionalReasonText = " + additionalReasonText);
            if (additionalReasonText.Length == 0)
                rootNode.Nodes[rootNode.Nodes.Count - 1].ForeColor = Color.DimGray;
            ContextInfo.AddToList(new ContextInfo { Length = additionalReasonText.Length });
            treeView.Nodes.Add(rootNode);
        }
    }
}
