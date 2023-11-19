namespace BookSmart
{
    public class Tree<T> //Represents generic tree structure with a root node.
    {
        public TreeNode<T> Root { get; set; }

        public Tree(T rootValue)
        {
            Root = new TreeNode<T>(rootValue);
        }
    }
}
