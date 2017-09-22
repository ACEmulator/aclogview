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
            case PacketOpcode.UPDATE_SPELL_EVENT: {
                    UpdateSpell message = UpdateSpell.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.REMOVE_SPELL_EVENT: {
                    RemoveSpell message = RemoveSpell.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.UPDATE_ENCHANTMENT_EVENT: {
                    UpdateEnchantment message = UpdateEnchantment.read(messageDataReader);
                    message.contributeToTreeView(outputTreeView);
                    break;
                }
            case PacketOpcode.REMOVE_ENCHANTMENT_EVENT: {
                    RemoveEnchantment message = RemoveEnchantment.read(messageDataReader);
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
            default: {
                    handled = false;
                    break;
                }
        }

        return handled;
    }

    public class CastTargetedSpell : Message {
        public uint i_target;
        public SpellID i_spell_id;
        
        public static CastTargetedSpell read(BinaryReader binaryReader) {
            CastTargetedSpell newObj = new CastTargetedSpell();
            newObj.i_target = binaryReader.ReadUInt32();
            newObj.i_spell_id = (SpellID)binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_target = " + Utility.FormatGuid(this.i_target));
            rootNode.Nodes.Add("i_spell_id = " + i_spell_id);           
            treeView.Nodes.Add(rootNode);
        }
    }

    public class CastUntargetedSpell : Message {
        public SpellID i_spell_id;

        public static CastUntargetedSpell read(BinaryReader binaryReader) {
            CastUntargetedSpell newObj = new CastUntargetedSpell();
            newObj.i_spell_id = (SpellID)binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();          
            rootNode.Nodes.Add("i_spell_id = " + Utility.FormatGuid((uint)this.i_spell_id));
            treeView.Nodes.Add(rootNode);
        }
    }

    public class RemoveSpell : Message {
        public SpellID i_spell_id;

        public static RemoveSpell read(BinaryReader binaryReader) {
            RemoveSpell newObj = new RemoveSpell();
            newObj.i_spell_id = (SpellID)binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_spell_id = " + Utility.FormatGuid((uint)this.i_spell_id));
            treeView.Nodes.Add(rootNode);
        }
    }

    public class UpdateSpell : Message {
        public SpellID i_spell_id;

        public static UpdateSpell read(BinaryReader binaryReader) {
            UpdateSpell newObj = new UpdateSpell();
            newObj.i_spell_id = (SpellID)binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("i_spell_id = " + Utility.FormatGuid((uint)this.i_spell_id));
            treeView.Nodes.Add(rootNode);
        }
    }

    public class StatMod {
        public uint type;
        public uint key;
        public float val;

        public static StatMod read(BinaryReader binaryReader) {
            StatMod newObj = new StatMod();
            newObj.type = binaryReader.ReadUInt32();
            newObj.key = binaryReader.ReadUInt32();
            newObj.val = binaryReader.ReadSingle();
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node) {
            node.Nodes.Add("type = " + type);
            node.Nodes.Add("key = " + key);
            node.Nodes.Add("val = " + val);
        }
    }

    public class Enchantment {
        public uint _id;
        public uint _spell_category;
        public uint _power_level;
        public double _start_time;
        public double _duration;
        public uint _caster;
        public float _degrade_modifier;
        public float _degrade_limit;
        public double _last_time_degraded;
        public StatMod _smod;
        public SpellSetID m_SpellSetID;

        public static Enchantment read(BinaryReader binaryReader) {
            Enchantment newObj = new Enchantment();
            newObj._id = binaryReader.ReadUInt32();
            newObj._spell_category = binaryReader.ReadUInt32();
            newObj._power_level = binaryReader.ReadUInt32();
            newObj._start_time = binaryReader.ReadDouble();
            newObj._duration = binaryReader.ReadDouble();
            newObj._caster = binaryReader.ReadUInt32();
            newObj._degrade_modifier = binaryReader.ReadSingle();
            newObj._degrade_limit = binaryReader.ReadSingle();
            newObj._last_time_degraded = binaryReader.ReadDouble();
            newObj._smod = StatMod.read(binaryReader);
            if ((newObj._spell_category >> 16) >= 1) {
                newObj.m_SpellSetID = (SpellSetID)binaryReader.ReadUInt32();
            }
            return newObj;
        }

        public void contributeToTreeNode(TreeNode node) {
            node.Nodes.Add("_id = " + _id);
            node.Nodes.Add("_spell_category = " + _spell_category);
            node.Nodes.Add("_power_level = " + _power_level);
            node.Nodes.Add("_start_time = " + _start_time);
            node.Nodes.Add("_duration = " + _duration);
            node.Nodes.Add("_caster = " + _caster);
            node.Nodes.Add("_degrade_modifier = " + _degrade_modifier);
            node.Nodes.Add("_degrade_limit = " + _degrade_limit);
            node.Nodes.Add("_last_time_degraded = " + _last_time_degraded);
            TreeNode statModNode = node.Nodes.Add("_smod = ");
            _smod.contributeToTreeNode(statModNode);
            node.Nodes.Add("m_SpellSetID = " + m_SpellSetID);
        }
    }

    public class DispelEnchantment : Message {
        public uint eid;

        public static DispelEnchantment read(BinaryReader binaryReader) {
            DispelEnchantment newObj = new DispelEnchantment();
            newObj.eid = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("eid = " + eid);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class RemoveEnchantment : Message {
        public uint eid;

        public static RemoveEnchantment read(BinaryReader binaryReader) {
            RemoveEnchantment newObj = new RemoveEnchantment();
            newObj.eid = binaryReader.ReadUInt32();
            return newObj;
        }

        public override void contributeToTreeView(TreeView treeView) {
            TreeNode rootNode = new TreeNode(this.GetType().Name);
            rootNode.Expand();
            rootNode.Nodes.Add("eid = " + eid);
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
            TreeNode enchantmentNode = rootNode.Nodes.Add("enchant = ");
            enchant.contributeToTreeNode(enchantmentNode);
            treeView.Nodes.Add(rootNode);
        }
    }

    public class DispelMultipleEnchantments : Message {
        public PList<uint> list;

        public static DispelMultipleEnchantments read(BinaryReader binaryReader) {
            DispelMultipleEnchantments newObj = new DispelMultipleEnchantments();
            newObj.list = PList<uint>.read(binaryReader);
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

    public class RemoveMultipleEnchantments : Message {
        public PList<uint> list;

        public static RemoveMultipleEnchantments read(BinaryReader binaryReader) {
            RemoveMultipleEnchantments newObj = new RemoveMultipleEnchantments();
            newObj.list = PList<uint>.read(binaryReader);
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
