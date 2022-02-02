namespace ScriptEngine.Elements
{
    public abstract class Node
    {
        public NodeType NodeType;
        public string Value;

        public Node(NodeType nodeType, string value)
        {
            NodeType = nodeType;
            Value = value;
        }
    }
}