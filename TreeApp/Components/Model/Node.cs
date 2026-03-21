public class Node
{
    public int Value {get; private set;}
    public Node? Left {get; set;}
    public Node? Right {get; set;}
    public Node? Parent {get; set;}
    public int Height {get; set;} = 0;
    
    public Node (int value, Node? left=null, Node? right=null, Node? parent=null)
    {
        Value = value;
        Left = left;
        Right = right;
        Parent = parent;
        Height = 0;
    }
}