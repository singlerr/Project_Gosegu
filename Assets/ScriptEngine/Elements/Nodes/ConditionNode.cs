using System.Collections.ObjectModel;

namespace ScriptEngine.Elements.Nodes
{
    public class ConditionNode : Node
    {
        public Collection<LineNode> FalseLineNodes;
        public Collection<Node> InternalConditions;

        public Collection<LineNode> TrueLineNodes;

        public ConditionNode(NodeType nodeType, string value) : base(nodeType, value)
        {
            InternalConditions = new Collection<Node>();
            TrueLineNodes = new Collection<LineNode>();
            FalseLineNodes = new Collection<LineNode>();
        }
    }
}