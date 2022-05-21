using System.Collections.Generic;
using System.Collections.ObjectModel;
using Autodesk.Revit.DB;


namespace CleanCode.Comments.Engineering
{
    public class TreeNode
    {
        public string Name { get; set; }
        public ObservableCollection<TreeNode> Nodes { get; set; }
        public int? ParentId { get; set; }
        public int? Id { get; set; }

        public TreeNode Parent;
        public bool IsExpanded { get; set; }
        private bool? _isChecked;

        public bool? IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
            }
        }

        public List<Element> AllElements;
        public string Tooltip { get; set; }
        
        public TreeNode(string name, int? id, int? parentId, List<Element> elems)
        {
            Name = name;
            Id = id;
            ParentId = parentId;
            Nodes = null;
            IsExpanded = false;
            IsChecked = false;

            AllElements = elems;
        }

        public List<TreeNode> GetChildren()
        {
            List<TreeNode> Children = new List<TreeNode>();
            if (Nodes != null)
            {
                foreach (TreeNode node in Nodes)
                    Children.Add(node);
            }

            return Children;
        }
    }
}