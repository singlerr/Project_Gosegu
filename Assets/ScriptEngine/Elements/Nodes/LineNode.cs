using System.Collections.ObjectModel;

namespace ScriptEngine.Elements.Nodes
{
    public class LineNode : Node
    {
        public Collection<Node> InternalNodes;

        public LineNode(string line) : base(NodeType.Line, line)
        {
            InternalNodes = new Collection<Node>();
        }
    }
}