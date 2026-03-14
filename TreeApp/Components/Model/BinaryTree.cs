public class BinaryTree
{
    private Node? root;
    private int phantomId = 0;

    public string ToMermaid()
    {
        if (root == null)
            return "graph TD\n empty[\"(empty tree)\"]";

        if (root.Left == null && root.Right == null)
            return $"graph TD\n    {root.Value}";

        phantomId = 0;
        List<string> edges = new List<string>();
        List<int> phantomIds = new List<int>();

        BuildGraph(root, edges, phantomIds);

        List<string> output = new List<string> { "graph TD" };
        output.AddRange(edges);

        foreach (var id in phantomIds)
        {
            output.Add($"    style _ph{id} fill:none,stroke:none,color:none");
        }

        return string.Join("\n", output);
    }

    public void Insert(int value)
    {
        root = InsertNode(root, value);
    }

    private Node InsertNode(Node? current, int value)
    {
        if (current == null)
        {
            return new Node(value);
        }
        if (value < current.Value)
        {
            current.Left = InsertNode(current.Left, value);
        }
        else if (value > current.Value)
        {
            current.Right = InsertNode(current.Right, value);
        }
        return current;
    }

    public string Inorder()
    {
        List<int> values = new List<int>();
        TraverseInOrder(root, values);
        return string.Join(" ", values);
    }

    private void TraverseInOrder(Node? node, List<int> values)
    {
        if (node == null)
            return;

        TraverseInOrder(node.Left, values);
        values.Add(node.Value);
        TraverseInOrder(node.Right, values);
    }

    public int Height()
    {
        return GetHeight(root);
    }

    private int GetHeight(Node? node)
    {
        if (node == null)
            return -1;

        int left = GetHeight(node.Left);
        int right = GetHeight(node.Right);

        return Math.Max(left, right) + 1;
    }

    private void BuildGraph(Node? node, List<string> edges, List<int> phantomIds)
    {
        if (node == null)
            return;

        if (node.Left != null)
        {
            edges.Add($"    {node.Value} --> {node.Left.Value}");
            BuildGraph(node.Left, edges, phantomIds);
        }
        else if (node.Right != null)
        {
            AddPhantomNode(edges, phantomIds, node.Value);
        }

        if (node.Right != null)
        {
            edges.Add($"    {node.Value} --> {node.Right.Value}");
            BuildGraph(node.Right, edges, phantomIds);
        }
        else if (node.Left != null)
        {
            AddPhantomNode(edges, phantomIds, node.Value);
        }
    }

    private void AddPhantomNode(List<string> edges, List<int> phantomIds, int parentValue)
    {
        int id = phantomId++;
        edges.Add($"    {parentValue} --> _ph{id}[ ]");
        phantomIds.Add(id);
    }
}
