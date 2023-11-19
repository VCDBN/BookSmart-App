using System.Collections.Generic;

namespace BookSmart
{
    public class TreeNode<T> //Represents a node (part of tree) with list of child nodes
    {
        public T Value { get; set; }
        public List<TreeNode<T>> Children { get; set; }

        public TreeNode(T value)
        {
            Value = value;
            Children = new List<TreeNode<T>>();
        }

        public void AddChild(TreeNode<T> child) //Assigns another node as a child to this node
        {
            Children.Add(child);
        }
    }
}
