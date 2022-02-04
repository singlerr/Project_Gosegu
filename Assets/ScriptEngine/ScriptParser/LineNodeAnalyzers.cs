using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ScriptEngine.Elements;
using ScriptEngine.Elements.Nodes;
using ScriptEngine.Table;
using ScriptEngine.Utils;

namespace ScriptEngine.ScriptParser
{
    public static class LineNodeAnalyzers
    {
        #region CalcConstants

        public static readonly Func<List<Node>, Node> CalcConstants = delegate(List<Node> nodes)
        {
            var operators = new[] { NodeType.Mul, NodeType.Div, NodeType.Add, NodeType.Sub };
            foreach (var op in operators)
                //Do until there is no current operator.
                while (NodeUtils.FindStartIndex(nodes, op) != -1)
                {
                    var safeList = nodes.ToList();
                    for (var i = 0; i < safeList.Count; i++)
                    {
                        var left = i - 1 >= 0 ? safeList[i - 1] : null;
                        var current = safeList[i];
                        var right = i + 1 <= safeList.Count ? safeList[i + 1] : null;

                        if (current.NodeType == op)
                        {
                            if (left == null || right == null)
                                throw new Exception(
                                    $"Cannot operate '{op.ToString()}' because at least one of both side is null.");
                            var combined = ExecOperator(op, left, right);

                            nodes.RemoveRange(i - 1, 3);
                            //a(i-1), b(i), c(i+1) 이 있을 때 합친 결과를 i-1에 집어넣고, b, c제거하면 3개가 하나로 합쳐진 것과 같다 
                            nodes.Insert(i - 1, combined);
                            break;
                        }
                    }
                }

            return nodes.Count == 1
                ? nodes[0]
                : throw new Exception(
                    $"Some error occurred in operation. CalcConstants expected nodes.Count is 1 but found {nodes.Count}");
        };

        #endregion

        #region ExecOperator

        private static readonly Func<NodeType, Node, Node, Node> ExecOperator =
            delegate(NodeType opType, Node first, Node second)
            {
                var firstValue = first.NodeType == NodeType.Variable ? VariableTable.GetVariable(first.Value) : first;
                var secondValue = second.NodeType == NodeType.Variable
                    ? VariableTable.GetVariable(second.Value)
                    : second;

                if (firstValue == null)
                    throw new Exception($"Cannot resolve symbol {first.Value}");
                if (secondValue == null)
                    throw new Exception($"Cannot resolve symbol {second.Value}");

                switch (opType)
                {
                    case NodeType.Add:
                        if (firstValue.NodeType == NodeType.String || secondValue.NodeType == NodeType.String)
                            return new ElementNode(NodeType.String, firstValue.Value + secondValue.Value);
                        if (firstValue.NodeType == NodeType.Double || secondValue.NodeType == NodeType.Double)
                            return new ElementNode(NodeType.Double,
                                (double.Parse(firstValue.Value) + double.Parse(secondValue.Value)).ToString("g2"));
                        return new ElementNode(NodeType.Int,
                            (int.Parse(firstValue.Value) + int.Parse(secondValue.Value)).ToString());
                    case NodeType.Sub:
                        if (firstValue.NodeType == NodeType.String || secondValue.NodeType == NodeType.String)
                            throw new Exception("Sub operation does not support string.");
                        if (firstValue.NodeType == NodeType.Double || secondValue.NodeType == NodeType.Double)
                            return new ElementNode(NodeType.Double,
                                (double.Parse(firstValue.Value) - double.Parse(secondValue.Value)).ToString("g2"));
                        return new ElementNode(NodeType.Int,
                            (int.Parse(firstValue.Value) - int.Parse(secondValue.Value)).ToString());
                    case NodeType.Mul:
                        if (firstValue.NodeType == NodeType.String || secondValue.NodeType == NodeType.String)
                            throw new Exception("Mul operation does not support string.");
                        if (firstValue.NodeType == NodeType.Double || secondValue.NodeType == NodeType.Double)
                            return new ElementNode(NodeType.Double,
                                (double.Parse(firstValue.Value) * double.Parse(secondValue.Value)).ToString("g2"));
                        return new ElementNode(NodeType.Int,
                            (int.Parse(firstValue.Value) * int.Parse(secondValue.Value)).ToString());
                    case NodeType.Div:
                        if (firstValue.NodeType == NodeType.String || secondValue.NodeType == NodeType.String)
                            throw new Exception("Div operation does not support string.");
                        if (firstValue.NodeType == NodeType.Double || secondValue.NodeType == NodeType.Double)
                            return new ElementNode(NodeType.Double,
                                (double.Parse(firstValue.Value) / double.Parse(secondValue.Value)).ToString("g2"));
                        return new ElementNode(NodeType.Int,
                            (int.Parse(firstValue.Value) / int.Parse(secondValue.Value)).ToString());
                    default:
                        throw new Exception($"Unsupported operation type {opType.ToString()}");
                }
            };

        #endregion

        #region SetVariable

        public static readonly Func<LineNode, Collection<object>> SetVariable = delegate(LineNode node)
        {
            var args = new Collection<object>();

            var internalNodes = node.InternalNodes;

            var headNode = internalNodes[0];

            if (headNode.NodeType != NodeType.SetVariable)
                throw new Exception($"Called SetVariable processor but found {headNode.NodeType.ToString()}");


            var startBracketIdx = NodeUtils.FindStartIndex(internalNodes, NodeType.StartBracket);
            if (startBracketIdx == -1)
                throw new Exception("SetVariable starts with {");

            for (var i = startBracketIdx; i < internalNodes.Count; i++)
            {
                var currentNode = internalNodes[i];
                //Check this node is NameAndVarSeparator
                //If true, then check each side of this node are both string.
                if (currentNode.NodeType == NodeType.NameAndVarSeparator)
                {
                    var leftNode = i - 1 > startBracketIdx ? internalNodes[i - 1] : null;
                    //Why i+1 < .... , not <= ? Because last node must be }.
                    var valueScope = NodeUtils.FindScope(internalNodes, i + 1, NodeType.Comma);

                    //If value is just one(no operators exist)
                    if (valueScope.Item2 == -1)
                    {
                        var bracketScope = NodeUtils.FindScope(internalNodes, i + 1, NodeType.EndBracket);
                        if (bracketScope.Item2 - bracketScope.Item1 > 0)
                        {
                            //There are mutiple values
                            var rightNodes = internalNodes.Slice(bracketScope.Item1, bracketScope.Item2);
                            var rightNode = CalcConstants(rightNodes.ToList());
                            if (leftNode == null || rightNode == null)
                                throw new Exception($"SetVariable requires key:value shape, but found {node.Value}");

                            //Now, leftNode becomes var name, and right node var value
                            //But, var name node must be string.
                            if (leftNode.NodeType != NodeType.String)
                                throw new Exception(
                                    $"Variable name must be string,but found {leftNode.NodeType.ToString()}");

                            var varName = leftNode.Value;
                            var valueNode = rightNode.NodeType == NodeType.Variable &&
                                            VariableTable.VariableExists(varName)
                                ? VariableTable.GetVariable(varName)
                                : rightNode;

                            VariableTable.PutVariable(varName, valueNode);
                        }
                        else
                        {
                            var rightNode = i + 1 < internalNodes.Count ? internalNodes[i + 1] : null;
                            if (leftNode == null || rightNode == null)
                                throw new Exception($"SetVariable requires key:value shape, but found {node.Value}");

                            //Now, leftNode becomes var name, and right node var value
                            //But, var name node must be string.
                            if (leftNode.NodeType != NodeType.String)
                                throw new Exception(
                                    $"Variable name must be string,but found {leftNode.NodeType.ToString()}");

                            var varName = leftNode.Value;
                            var valueNode = rightNode.NodeType == NodeType.Variable &&
                                            VariableTable.VariableExists(varName)
                                ? VariableTable.GetVariable(varName)
                                : rightNode;

                            VariableTable.PutVariable(varName, valueNode);
                        }
                    }
                    else
                    {
                        //There are mutiple values
                        var rightNodes = internalNodes.Slice(valueScope.Item1, valueScope.Item2);
                        var rightNode = CalcConstants(rightNodes.ToList());
                        if (leftNode == null || rightNode == null)
                            throw new Exception($"SetVariable requires key:value shape, but found {node.Value}");

                        //Now, leftNode becomes var name, and right node var value
                        //But, var name node must be string.
                        if (leftNode.NodeType != NodeType.String)
                            throw new Exception(
                                $"Variable name must be string,but found {leftNode.NodeType.ToString()}");

                        var varName = leftNode.Value;
                        var valueNode = rightNode.NodeType == NodeType.Variable && VariableTable.VariableExists(varName)
                            ? VariableTable.GetVariable(varName)
                            : rightNode;

                        VariableTable.PutVariable(varName, valueNode);
                    }
                }
            }

            return args;
        };

        #endregion

        #region UpdateState

        public static readonly Func<LineNode, Collection<object>> UpdateState = delegate(LineNode node)
        {
            var args = new Collection<object>();

            var internalNodes = node.InternalNodes;

            var headNode = internalNodes[0];

            if (headNode.NodeType != NodeType.UpdateState)
                throw new Exception($"Called UpdateState processor but found {headNode.NodeType.ToString()}");

            return args;
        };

        #endregion
    }
}