using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using aclogview;

public class CM_Magic : MessageProcessor {

    public override bool acceptMessageData(BinaryReader messageDataReader, TreeView outputTreeView) {
        bool handled = true;

        PacketOpcode opcode = Util.readOpcode(messageDataReader);
        switch (opcode) {
            case PacketOpcode.Evt_Magic__PurgeEnchantments_ID:
            case PacketOpcode.Evt_Magic__PurgeBadEnchantments_ID: {
                    EmptyMessage message = new EmptyMessage(opcode);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Magic__CastUntargetedSpell_ID: {
                    CastUntargetedSpell message = CastUntargetedSpell.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Magic__CastTargetedSpell_ID: {
                    CastTargetedSpell message = CastTargetedSpell.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            // TODO: Evt_Magic__ResearchSpell_ID
            //case PacketOpcode.UPDATE_SPELL_EVENT: {
            //        UpdateSpell message = UpdateSpell.read(messageDataReader);
            //        message.contributeToTreeView(outputTreeView);
            //        break;
            //    }
            //case PacketOpcode.REMOVE_SPELL_EVENT: {
            //        RemoveSpell message = RemoveSpell.read(messageDataReader);
            //        message.contributeToTreeView(outputTreeView);
            //        break;
            //    }
            //case PacketOpcode.UPDATE_ENCHANTMENT_EVENT: {
            //        UpdateEnchantment message = UpdateEnchantment.read(messageDataReader);
            //        message.contributeToTreeView(outputTreeView);
            //        break;
            //    }
            //case PacketOpcode.REMOVE_ENCHANTMENT_EVENT: {
            //        RemoveEnchantment message = RemoveEnchantment.read(messageDataReader);
            //        message.contributeToTreeView(outputTreeView);
            //        break;
            //    }
            case PacketOpcode.Evt_Magic__RemoveSpell_ID:
                {
                    RemoveSpell message = RemoveSpell.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Magic__UpdateMultipleEnchantments_ID: {
                    UpdateMultipleEnchantments message = UpdateMultipleEnchantments.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Magic__RemoveMultipleEnchantments_ID: {
                    RemoveMultipleEnchantments message = RemoveMultipleEnchantments.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Magic__DispelEnchantment_ID: {
                    DispelEnchantment message = DispelEnchantment.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Magic__DispelMultipleEnchantments_ID: {
                    DispelMultipleEnchantments message = DispelMultipleEnchantments.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Magic__UpdateSpell_ID:
                {
                    UpdateSpell message = UpdateSpell.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Magic__UpdateEnchantment_ID:
                {
                    UpdateEnchantment message = UpdateEnchantment.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.Evt_Magic__RemoveEnchantment_ID:
                {
                    RemoveEnchantment message = RemoveEnchantment.read(messageDataReader);
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

    public class CastTargetedSpell : Message {
        public uint i_target;
        public uint i_spell_id;
        
        public static CastTargetedSpell read(BinaryReader binaryReader) {
            CastTargetedSpell newObj = new CastTargetedSpell();
            newObj.i_target = binaryReader.ReadUInt32();
            newObj.i_spell_id = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_target = " + Utility.FormatHex(this.i_target));
            rootNode.Nodes.Add("i_spell_id = " + "(" + i_spell_id + ") " + (SpellID)i_spell_id);       
            treeView.Nodes.Add(rootNode);
        }
    }

    public class CastUntargetedSpell : Message {
        public uint i_spell_id;

        public static CastUntargetedSpell read(BinaryReader binaryReader) {
            CastUntargetedSpell newObj = new CastUntargetedSpell();
            newObj.i_spell_id = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();          
            rootNode.Nodes.Add("i_spell_id = " + "(" + i_spell_id + ") " + (SpellID)i_spell_id);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class RemoveSpell : Message {
        public uint i_spell_id;

        public static RemoveSpell read(BinaryReader binaryReader) {
            RemoveSpell newObj = new RemoveSpell();
            newObj.i_spell_id = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_spell_id = " + "(" + i_spell_id + ") " + (SpellID)i_spell_id);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class UpdateSpell : Message {
        public uint i_spell_id;

        public static UpdateSpell read(BinaryReader binaryReader) {
            UpdateSpell newObj = new UpdateSpell();
            newObj.i_spell_id = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_spell_id = " + "(" + i_spell_id + ") " + (SpellID)i_spell_id);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class StatMod {
        public byte table;
        public byte main_cat;
        public byte sub_cat;
        public byte effect;
        public uint key;
        public float val;

        public static StatMod read(BinaryReader binaryReader) {
            StatMod newObj = new StatMod();
            newObj.table = binaryReader.ReadByte();
            newObj.main_cat = binaryReader.ReadByte();
            newObj.sub_cat = binaryReader.ReadByte();
            newObj.effect = binaryReader.ReadByte();
            newObj.key = binaryReader.ReadUInt32();
            newObj.val = binaryReader.ReadSingle();
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node) {
            node.Nodes.Add("table = " + (STables)table);
            node.Nodes.Add("main_category = " + (SMainCat)main_cat);
            node.Nodes.Add("sub_category = " + (SSubCat)sub_cat);
            node.Nodes.Add("effect = " + (STypeEffect)effect);
            switch (table)
            {
                case 0x01:
                    node.Nodes.Add("key = " + (STypeAttribute)key);
                    break;
                case 0x02:
                    node.Nodes.Add("key = " + (STypeAttribute2nd)key);
                    break;
                case 0x04:
                    node.Nodes.Add("key = " + (STypeInt)key);
                    break;
                case 0x08:
                    node.Nodes.Add("key = " + (STypeFloat)key);
                    break;
                case 0x10:
                    node.Nodes.Add("key = " + (STypeSkill)key);
                    break;
                default:
                    node.Nodes.Add("key = " + key);
                    break;
            }
            node.Nodes.Add("val = " + val);
        }
    }

    public class Enchantment {
        public ushort i_spell_id;
        public ushort layer;
        public ushort spell_category;
        public ushort has_spell_set_id;
        public uint power_level;
        public double start_time;
        public double duration;
        public uint caster;
        public float degrade_modifier;
        public float degrade_limit;
        public double last_time_degraded;
        public StatMod smod;
        public uint spell_set_id;

        public static Enchantment read(BinaryReader binaryReader) {
            Enchantment newObj = new Enchantment();
            newObj.i_spell_id = binaryReader.ReadUInt16();
            newObj.layer = binaryReader.ReadUInt16();
            newObj.spell_category = binaryReader.ReadUInt16();
            newObj.has_spell_set_id = binaryReader.ReadUInt16();
            newObj.power_level = binaryReader.ReadUInt32();
            newObj.start_time = binaryReader.ReadDouble();
            newObj.duration = binaryReader.ReadDouble();
            newObj.caster = binaryReader.ReadUInt32();
            newObj.degrade_modifier = binaryReader.ReadSingle();
            newObj.degrade_limit = binaryReader.ReadSingle();
            newObj.last_time_degraded = binaryReader.ReadDouble();
            newObj.smod = StatMod.read(binaryReader);
            newObj.spell_set_id = binaryReader.ReadUInt32();
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node) {
            node.Nodes.Add("i_spell_id = " +  "(" + i_spell_id + ") " + (SpellID)i_spell_id);
            node.Nodes.Add("layer = " + layer);
            node.Nodes.Add("spell_category = " + Utility.FormatHex(spell_category));
            node.Nodes.Add("has_spell_set_id = " + has_spell_set_id);
            node.Nodes.Add("power_level = " + power_level);
            node.Nodes.Add("start_time = " + start_time);
            if (duration == -1) {
                node.Nodes.Add("duration = " + duration + " (indefinite)");
            }
            else {
                node.Nodes.Add("duration = " + duration + " seconds");
            }
            node.Nodes.Add("caster = " + Utility.FormatHex(caster));
            node.Nodes.Add("degrade_modifier = " + degrade_modifier);
            node.Nodes.Add("degrade_limit = " + degrade_limit);
            node.Nodes.Add("last_time_degraded = " + last_time_degraded);
            TreeNode statModNode = node.Nodes.Add("statmod = ");
            smod.contributeToTreeNode(statModNode);
            node.Nodes.Add("spell_set_id = " + (SpellSetID)spell_set_id);
        }
    }

    public class EnchantmentID {
        public ushort i_spell_id;
        public ushort layer;
        
        public static EnchantmentID read(BinaryReader binaryReader)
        {
            EnchantmentID newObj = new EnchantmentID();
            newObj.i_spell_id = binaryReader.ReadUInt16();
            newObj.layer = binaryReader.ReadUInt16();
            return newObj;
        }

        public void contributeToTreeNode(TreeNode treeView)
        {
            treeView.Nodes.Add("i_spell_id = " + "(" + i_spell_id + ") " + (SpellID)i_spell_id);
            treeView.Nodes.Add("layer = " + layer);
        }
    }

    

    public class DispelEnchantment : Message {
        public ushort i_spell_id;
        public ushort layer;

        public static DispelEnchantment read(BinaryReader binaryReader) {
            DispelEnchantment newObj = new DispelEnchantment();
            newObj.i_spell_id = binaryReader.ReadUInt16();
            newObj.layer = binaryReader.ReadUInt16();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_spell_id = " + "(" + i_spell_id + ") " + (SpellID)i_spell_id);
            rootNode.Nodes.Add("layer = " + layer);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class RemoveEnchantment : Message {
        public ushort i_spell_id;
        public ushort layer;

        public static RemoveEnchantment read(BinaryReader binaryReader) {
            RemoveEnchantment newObj = new RemoveEnchantment();
            newObj.i_spell_id = binaryReader.ReadUInt16();
            newObj.layer = binaryReader.ReadUInt16();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_spell_id = " + "(" + i_spell_id + ") " + (SpellID)i_spell_id);
            rootNode.Nodes.Add("layer = " + layer);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class UpdateEnchantment : Message {
        public Enchantment enchant;

        public static UpdateEnchantment read(BinaryReader binaryReader) {
            UpdateEnchantment newObj = new UpdateEnchantment();
            newObj.enchant = Enchantment.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            TreeNode enchantmentNode = rootNode.Nodes.Add("enchantment = ");
            enchant.contributeToTreeNode(enchantmentNode);
            enchantmentNode.ExpandAll();
            treeView.Nodes.Add(rootNode);
        }
    }

    public class DispelMultipleEnchantments : Message {
        public PList<EnchantmentID> enchantmentList;

        public static DispelMultipleEnchantments read(BinaryReader binaryReader) {
            DispelMultipleEnchantments newObj = new DispelMultipleEnchantments();
            newObj.enchantmentList = PList<EnchantmentID>.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            for (int i = 0; i < enchantmentList.list.Count; i++) {
                TreeNode listNode = rootNode.Nodes.Add($"enchantment {i+1} = ");
                var enchantment = enchantmentList.list[i];
                enchantment.contributeToTreeNode(listNode);
                listNode.Expand();
            }
            treeView.Nodes.Add(rootNode);
        }
    }

    public class RemoveMultipleEnchantments : Message {
        public PList<EnchantmentID> enchantmentList;

        public static RemoveMultipleEnchantments read(BinaryReader binaryReader) {
            RemoveMultipleEnchantments newObj = new RemoveMultipleEnchantments();
            newObj.enchantmentList = PList<EnchantmentID>.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            for (int i = 0; i < enchantmentList.list.Count; i++) {
                TreeNode listNode = rootNode.Nodes.Add($"enchantment {i+1} = ");
                var enchantment = enchantmentList.list[i];
                enchantment.contributeToTreeNode(listNode);
                listNode.Expand();
            }
            treeView.Nodes.Add(rootNode);
        }
    }

    // This message does not appear to be used. It was not found in any pcaps.
    public class UpdateMultipleEnchantments : Message { 
        public PList<Enchantment> list;

        public static UpdateMultipleEnchantments read(BinaryReader binaryReader) {
            UpdateMultipleEnchantments newObj = new UpdateMultipleEnchantments();
            newObj.list = PList<Enchantment>.read(binaryReader);
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            TreeNode listNode = rootNode.Nodes.Add("list = ");
            list.contributeToTreeNode(listNode);
            treeView.Nodes.Add(rootNode);
        }
    }
}
