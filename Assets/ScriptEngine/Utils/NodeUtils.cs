using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ScriptEngine.Elements;

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

        public static Tuple<int, int> FindScope(Collection<Node> nodes, int startIdx, NodeType end)
        {
            for (var i = startIdx; i < nodes.Count; i++)
                if (nodes[i].NodeType == end)
                    return new Tuple<int, int>(startIdx, i - 1);

            return new Tuple<int, int>(startIdx, -1);
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
    }
}