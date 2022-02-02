namespace ScriptEngine.Elements.Nodes
{
    public class LineNode : Node
    {
        public Node[] InternalNodes;
        public LineNode(string line) : base(NodeType.Line,line)
        {
        }
    }
}