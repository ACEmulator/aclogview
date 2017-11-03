using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aclogview
{
    /// <summary>
    /// This is a class to save and restore treeview states.
    /// </summary>
    /// Code was taken from https://stackoverflow.com/questions/8308258/expand-selected-node-after-refresh-treeview-in-c-sharp and slightly modified.
    /// Note that simply getting the TopNode before updating the treeview and setting it after the update did not work correctly.
    /// A node identifier tag had to be saved and searched for after the treeview was updated.
    public static class TreeviewState
    {
        public static List<string> GetExpansionState(this TreeNodeCollection nodes)
        {

         return nodes.Descendants()
                    .Where(n => n.IsExpanded)
                    .Select(n => n.Tag.ToString())
                    .ToList();
        }

        public static void SetExpansionState(this TreeNodeCollection nodes, List<string> savedExpansionState)
        {
            foreach (var node in nodes.Descendants()
                                      .Where(n => savedExpansionState.Any(i => i == n.Tag.ToString() )))
            {
                node.Expand();
            }
        }

        public static string GetTopNode(this TreeView treeView)
        {
            return treeView.TopNode.Tag.ToString();
        }

        public static void SetTopNode(this TreeView treeView, string previousTopNode)
        {
            foreach (var node in treeView.Nodes.Descendants())
            {
                if (node.Tag.ToString() == previousTopNode)
                {
                    treeView.TopNode = node;
                    return;
                }
            }
        }

        public static IEnumerable<TreeNode> Descendants(this TreeNodeCollection c)
        {
            foreach (var node in c.OfType<TreeNode>())
            {
                yield return node;

                foreach (var child in node.Nodes.Descendants())
                {
                    yield return child;
                }
            }
        }
    }
}
