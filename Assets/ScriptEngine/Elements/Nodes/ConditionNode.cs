using System.Collections.ObjectModel;

namespace ScriptEngine.Elements.Nodes
{
    public class ConditionNode : Node
    {
        public Collection<Node> InternalConditions;

        public ConditionNode(string value) : base(NodeType.If, value)
        {
            InternalConditions = new Collection<Node>();
        }
    }
}