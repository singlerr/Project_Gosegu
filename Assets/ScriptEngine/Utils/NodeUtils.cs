using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ScriptEngine.Elements;
using ScriptEngine.Elements.Nodes;
using ScriptEngine.Table;

namespace ScriptEngine.Utils
{
    public static class NodeUtils
    {
        public static int FindStartIndex(Collection<Node> nodes, NodeType nodeType)
        {
            for (var i = 0; i < nodes.Count; i++)
                if (nodes[i].NodeType == nodeType)
                    return i;

            return -1;
        }

        public static int FindStartIndex(List<Node> nodes, NodeType nodeType)
        {
            for (var i = 0; i < nodes.Count; i++)
                if (nodes[i].NodeType == nodeType)
                    return i;

            return -1;
        }

        public static int FindStartIndex(Collection<Node> nodes, params NodeType[] nodeTypes)
        {
            for (var i = 0; i < nodes.Count; i++)
                if (nodeTypes.Contains(nodes[i].NodeType))
                    return i;

            return -1;
        }

        public static Tuple<int, int> FindScope(Collection<Node> nodes, int startIdx, NodeType end)
        {
            for (var i = startIdx; i < nodes.Count; i++)
                if (nodes[i].NodeType == end)
                    return new Tuple<int, int>(startIdx, i - 1);

            return new Tuple<int, int>(startIdx, -1);
        }

        public static Node GetRawOrVariableValue(Node node)
        {
            if (node.NodeType == NodeType.Variable)
            {
                if (!VariableTable.VariableExists(node.Value))
                    return null;

                return VariableTable.GetVariable(node.Value);
            }

            return node;
        }

        public static Collection<T> Slice<T>(this Collection<T> collection, int startIdx, int endIdx)
        {
            var newCol = new Collection<T>();
            for (var i = startIdx; i <= endIdx; i++) newCol.Add(collection[i]);

            return newCol;
        }

        public static bool IsOperator(char c)
        {
            return c == NodeType.Add.GetExpression()[0] || c == NodeType.Sub.GetExpression()[0] ||
                   c == NodeType.Div.GetExpression()[0] || c == NodeType.Mul.GetExpression()[0];
        }

        public static string JoinNodeToString(Collection<Node> nodes)
        {
            var builder = new StringBuilder();
            foreach (var node in nodes) builder.Append(node.Value);

            return builder.ToString();
        }

        public static Tuple<int, Node> FindFirstInBound(Collection<LineNode> nodes, int currentIdx, NodeType nodeType,
            bool isUpper)
        {
            if (isUpper)
                for (var i = currentIdx; i >= 0; i--)
                {
                    var a = i;
                    if (nodes[i].GetHeadNodeType() == nodeType)
                        return new Tuple<int, Node>(i, nodes[i]);
                }
            else
                for (var i = currentIdx; i < nodes.Count; i++)
                    if (nodes[i].GetHeadNodeType() == nodeType)
                        return new Tuple<int, Node>(i, nodes[i]);

            return new Tuple<int, Node>(-1, null);
        }
    }
}