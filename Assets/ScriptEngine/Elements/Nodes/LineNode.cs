using System.Collections.Generic;
using System.Collections.ObjectModel;
using ScriptEngine.Utils;

namespace ScriptEngine.Elements.Nodes
{
    public class LineNode : Node
    {
        public Collection<Node> InternalNodes;

        public LineNode(string line) : base(NodeType.Line, line)
        {
            InternalNodes = new Collection<Node>();
        }

        public LineNode(string line, IEnumerable<Node> internalNodes) : this(line)
        {
            InternalNodes.AddRange(internalNodes);
        }

        public static NodeType GetHeadNodeType(Collection<Node> nodes)
        {
            return nodes[0].NodeType;
        }
    }

    public static class LineNodeExt
    {
        public static NodeType GetHeadNodeType(this LineNode node)
        {
            return node.InternalNodes[0].NodeType;
        }

        public static Node GetHeadNode(this LineNode node)
        {
            return node.InternalNodes[0];
        }
    }
}