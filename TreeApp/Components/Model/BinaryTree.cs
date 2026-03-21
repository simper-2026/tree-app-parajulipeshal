public class BinaryTree
{
    private Node? root;
    private int phantomId = 0;

    public string ToMermaid()
    {
        if (root == null)
            return "graph TD\n empty[\"(empty tree)\"]";

        if (root.Left == null && root.Right == null)
            return $"graph TD\n    {root.Value}[ {root.Value} h:{root.Height} ]";

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
        root = InsertNode(root, value, null);
    }

    private Node InsertNode(Node? current, int value, Node? parent)
    {
        if (current == null)
        {
            return new Node(value, null, null, parent);
        }

        if (value < current.Value)
        {
            current.Left = InsertNode(current.Left, value, current);
        }
        else if (value > current.Value)
        {
            current.Right = InsertNode(current.Right, value, current);
        }
        else
        {
            return current; //duplicates are ignored
        }

        //updating height
        UpdateHeight(current);

        int balance = GetBalance(current);

        //left-left
        if (balance > 1 && value < current.Left!.Value)
        {
            return RotateRight(current);
        }

        //right-right
        if (balance < -1 && value > current.Right!.Value)
        {
            return RotateLeft(current);
        }

        //left-right
        if (balance > 1 && value > current.Left!.Value)
        {
            current.Left = RotateLeft(current.Left);
            return RotateRight(current);
        }

        //right-left
        if (balance < -1 && value < current.Right!.Value)
        {
            current.Right = RotateRight(current.Right);
            return RotateLeft(current);
        }

        return current;
    }

    private void UpdateHeight(Node node)
    {
        int leftHeight = node.Left?.Height ?? -1;
        int rightHeight = node.Right?.Height ?? -1;
        node.Height = Math.Max(leftHeight, rightHeight) + 1;
    }

    private int GetBalance(Node node)
    {
        int leftHeight = node.Left?.Height ?? -1;
        int rightHeight = node.Right?.Height ?? -1;
        return leftHeight - rightHeight;
    }

    private Node RotateRight(Node z)
    {
        Node y = z.Left!;
        Node? t3 = y.Right;

        y.Right = z;
        z.Left = t3;

        //updating parent reference
        y.Parent = z.Parent;
        z.Parent = y;
        if (t3 != null)
            t3.Parent = z;

        // updating heights
        UpdateHeight(z);
        UpdateHeight(y);

        return y;
    }

    private Node RotateLeft(Node z)
    {
        Node y = z.Right!;
        Node? t2 = y.Left;

        y.Left = z;
        z.Right = t2;

        //updating parent reference
        y.Parent = z.Parent;
        z.Parent = y;
        if (t2 != null)
            t2.Parent = z;

        // updating height
        UpdateHeight(z);
        UpdateHeight(y);

        return y;
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
        return root?.Height ?? -1;
    }

    private void BuildGraph(Node? node, List<string> edges, List<int> phantomIds)
    {
        if (node == null)
            return;

        string nodeLabel = $"{node.Value}[ {node.Value} h:{node.Height} ]";

        if (node.Left != null)
        {
            string leftLabel = $"{node.Left.Value}[ {node.Left.Value} h:{node.Left.Height} ]";
            edges.Add($"    {nodeLabel} --> {leftLabel}");
            BuildGraph(node.Left, edges, phantomIds);
        }
        else if (node.Right != null)
        {
            AddPhantomNode(edges, phantomIds, nodeLabel);
        }

        if (node.Right != null)
        {
            string rightLabel = $"{node.Right.Value}[ {node.Right.Value} h:{node.Right.Height} ]";
            edges.Add($"    {nodeLabel} --> {rightLabel}");
            BuildGraph(node.Right, edges, phantomIds);
        }
        else if (node.Left != null)
        {
            AddPhantomNode(edges, phantomIds, nodeLabel);
        }
    }

    private void AddPhantomNode(List<string> edges, List<int> phantomIds, string parentLabel)
    {
        int id = phantomId++;
        edges.Add($"    {parentLabel} --> _ph{id}[ ]");
        phantomIds.Add(id);
    }
}
