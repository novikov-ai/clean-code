using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CleanCode.Comments.Engineering
{
    public class Tree
    {
        public TreeNode Root;
        public Tree(TreeNode root) {
            Root = root;
        }
        public void AddChild(TreeNode newNode, TreeNode parent) {
            if (parent.Nodes is null)
                parent.Nodes = new ObservableCollection<TreeNode>();

            parent.Nodes.Add(newNode);
            newNode.Parent = parent;
        }

        public List<TreeNode> GetChildren(TreeNode node) {
            List<TreeNode> allChildren = new List<TreeNode>();
            if (node.Nodes != null)
            {
                foreach (TreeNode item in node.Nodes)
                {
                    allChildren.Add(item);
                    allChildren.AddRange(GetChildren(item));
                }
            }
            return allChildren;
        }

        public List<TreeNode> GetAllNodes() {
            List<TreeNode> allNodes = new List<TreeNode>();

            if (Root != null)
            {
                allNodes.Add(Root);
                allNodes.AddRange(GetChildren(Root));
            }
            return allNodes;
        }
        public TreeNode FindNodeByValue(int? id) {
            if (Root != null)
            {
                List<TreeNode> allNodes = GetAllNodes();
                foreach (TreeNode item in allNodes)
                {
                    if (item.Id == id)
                        return item;
                }
            }
            return null;
        }
    }
}
