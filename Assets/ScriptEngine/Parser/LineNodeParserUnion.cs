using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Handler.Segments;
using ScriptEngine.Elements;
using ScriptEngine.Elements.Nodes;
using ScriptEngine.Table;
using ScriptEngine.Utils;
using Action = Handler.Wrappers.Action;

namespace ScriptEngine.ScriptParser
{
    public static class LineNodeParserUnion
    {
        public static readonly NodeType[] ComparisonOperators =
        {
            NodeType.LeftBig, NodeType.LeftBigEqual, NodeType.RightBig, NodeType.RightBigEqual, NodeType.Equal,
            NodeType.NotEqual
        };

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

        #region CalcCondition

        public static Func<Node, Node, Node, bool> CalcCondition = delegate(Node left, Node op, Node right)
        {
            if (left.NodeType == NodeType.String || right.NodeType == NodeType.String)
                throw new Exception("Cannot execute comparison operator with strings.");
            switch (op.NodeType)
            {
                case NodeType.LeftBig:
                    if (left.NodeType == NodeType.Double || right.NodeType == NodeType.Double)
                    {
                        var leftVar = double.Parse(left.Value);
                        var rightVar = double.Parse(right.Value);

                        return leftVar > rightVar;
                    }

                    var leftIntVar = int.Parse(left.Value);
                    var rightIntVar = int.Parse(right.Value);
                    return leftIntVar > rightIntVar;
                case NodeType.LeftBigEqual:
                    if (left.NodeType == NodeType.Double || right.NodeType == NodeType.Double)
                    {
                        var leftVar = double.Parse(left.Value);
                        var rightVar = double.Parse(right.Value);

                        return leftVar >= rightVar;
                    }

                    var leftIntVarLBE = int.Parse(left.Value);
                    var rightIntVarLBE = int.Parse(right.Value);
                    return leftIntVarLBE >= rightIntVarLBE;
                case NodeType.RightBig:
                    if (left.NodeType == NodeType.Double || right.NodeType == NodeType.Double)
                    {
                        var leftVar = double.Parse(left.Value);
                        var rightVar = double.Parse(right.Value);

                        return leftVar < rightVar;
                    }

                    var leftIntVarRB = int.Parse(left.Value);
                    var rightIntVarRB = int.Parse(right.Value);
                    return leftIntVarRB < rightIntVarRB;
                case NodeType.RightBigEqual:
                    if (left.NodeType == NodeType.Double || right.NodeType == NodeType.Double)
                    {
                        var leftVar = double.Parse(left.Value);
                        var rightVar = double.Parse(right.Value);

                        return leftVar <= rightVar;
                    }

                    var leftIntVarRBE = int.Parse(left.Value);
                    var rightIntVarRBE = int.Parse(right.Value);
                    return leftIntVarRBE <= rightIntVarRBE;
                case NodeType.Equal:
                    if (left.NodeType == NodeType.Double || right.NodeType == NodeType.Double)
                    {
                        var leftVar = double.Parse(left.Value);
                        var rightVar = double.Parse(right.Value);

                        return leftVar == rightVar;
                    }

                    var leftIntVarEQ = int.Parse(left.Value);
                    var rightIntVarEQ = int.Parse(right.Value);
                    return leftIntVarEQ == rightIntVarEQ;
                case NodeType.NotEqual:
                    if (left.NodeType == NodeType.Double || right.NodeType == NodeType.Double)
                    {
                        var leftVar = double.Parse(left.Value);
                        var rightVar = double.Parse(right.Value);

                        return leftVar != rightVar;
                    }

                    var leftIntVarNEQ = int.Parse(left.Value);
                    var rightIntVarNEQ = int.Parse(right.Value);
                    return leftIntVarNEQ != rightIntVarNEQ;
                default:
                    throw new Exception($"Unsupported comparison operator : {op.NodeType}");
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
                throw new Exception(
                    $"Called {NodeType.SetVariable} processor but found {headNode.NodeType.ToString()}");


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
                throw new Exception(
                    $"Called {NodeType.UpdateState} processor but found {headNode.NodeType.ToString()}");

            var state = internalNodes.Count > 2 && internalNodes[1].NodeType == NodeType.Variable
                ? internalNodes[1]
                : throw new Exception($"expected $variable but found {internalNodes[1]}");
            var startBracketIdx = NodeUtils.FindStartIndex(internalNodes, NodeType.StartBracket);
            var endBracketIdx = NodeUtils.FindStartIndex(internalNodes, NodeType.EndBracket);

            var variableStatements =
                internalNodes.ToList().GetRange(startBracketIdx, endBracketIdx - startBracketIdx + 1);

            var variables = ParseVariableStatement(new Collection<Node>(variableStatements));

            foreach (var keyValuePair in variables) VariableTable.PutStateVariable(state.Value, keyValuePair);
            return args;
        };

        #endregion

        #region ParseVariableStatement

        /// <summary>
        ///     Input contains { , }
        /// </summary>
        public static Func<Collection<Node>, Dictionary<string, Node>> ParseVariableStatement =
            delegate(Collection<Node> internalNodes)
            {
                var startBracketIdx = 0;
                var variables = new Dictionary<string, Node>();

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
                                    throw new Exception(
                                        $"SetVariable requires key:value shape, but found {NodeUtils.JoinNodeToString(internalNodes)}");

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


                                variables[varName] = valueNode;
                            }
                            else
                            {
                                var rightNode = i + 1 < internalNodes.Count ? internalNodes[i + 1] : null;
                                if (leftNode == null || rightNode == null)
                                    throw new Exception(
                                        $"SetVariable requires key:value shape, but found {NodeUtils.JoinNodeToString(internalNodes)}");

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

                                variables[varName] = valueNode;
                            }
                        }
                        else
                        {
                            //There are mutiple values
                            var rightNodes = internalNodes.Slice(valueScope.Item1, valueScope.Item2);
                            var rightNode = CalcConstants(rightNodes.ToList());
                            if (leftNode == null || rightNode == null)
                                throw new Exception(
                                    $"SetVariable requires key:value shape, but found {NodeUtils.JoinNodeToString(internalNodes)}");

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

                            variables[varName] = valueNode;
                        }
                    }
                }

                return variables;
            };

        #endregion

        #region If

        public static Func<LineNode, Collection<object>> If = delegate(LineNode node)
        {
            var args = new Collection<object>();

            if (node.GetHeadNodeType() != NodeType.If)
                throw new Exception($"Called If processor but found {node.GetHeadNodeType()}");

            var internalNodes = node.InternalNodes;


            var opIdx = NodeUtils.FindStartIndex(internalNodes, ComparisonOperators);

            if (opIdx == -1)
                throw new Exception("Expected comparison operator but not found.");

            var processable = internalNodes.ToList();

            var leftSection = processable.GetRange(1, opIdx - 1);


            var rightSection = processable.GetRange(opIdx + 1, processable.Count - opIdx);

            var leftValue = CalcConstants(leftSection);
            var rightValue = CalcConstants(rightSection);

            var result = CalcCondition(leftValue, processable[opIdx], rightValue);

            if (result)
            {
                var segments = new Collection<Segment>();
                foreach (var trueNode in (node.GetHeadNode() as ConditionNode).TrueLineNodes)
                    segments.Add(NodeParser.ParseSegment(trueNode));

                args.Add(segments);
            }
            else
            {
                var segments = new Collection<Segment>();
                foreach (var falseNode in (node.GetHeadNode() as ConditionNode).FalseLineNodes)
                    segments.Add(NodeParser.ParseSegment(falseNode));

                args.Add(segments);
            }

            return args;
        };

        #endregion

        #region CreateActionSelector

        public static Func<LineNode, Collection<object>> CreateActionSelector = delegate(LineNode node)
        {
            var args = new Collection<object>();

            if (node.GetHeadNodeType() != NodeType.CreateActionSelector)
                throw new Exception(
                    $"Called {NodeType.CreateActionSelector} processor but found {node.GetHeadNodeType()}");

            var internalNodes = node.InternalNodes;

            if (internalNodes.Count != 3)
                throw new Exception($"Required 3 parameters but found {internalNodes.Count} parameters.");


            var actionSelectorNode = internalNodes[1].NodeType == NodeType.Variable
                ? internalNodes[1]
                : throw new Exception($"Expected variable but found {internalNodes[1].NodeType}");


            var rawTitleNode = NodeUtils.GetRawOrVariableValue(internalNodes[2]);

            var selectorTitleNode = rawTitleNode != null && rawTitleNode.NodeType == NodeType.String
                ? internalNodes[2]
                : throw new Exception($"Required string as Selector Title but found {internalNodes[2].NodeType}");

            var actionSelector = new ActionSelector
            {
                Title = selectorTitleNode.Value
            };
            MiscVariable.PutActionSelector(actionSelectorNode.Value, actionSelector);
            return args;
        };

        #endregion

        #region CreateAction

        public static Func<LineNode, Collection<object>> CreateAction = delegate(LineNode node)
        {
            var args = new Collection<object>();

            if (node.GetHeadNodeType() != NodeType.CreateAction)
                throw new Exception($"Called {NodeType.CreateAction} processor but found {node.GetHeadNodeType()}");

            var internalNodes = node.InternalNodes;

            if (internalNodes.Count != 3)
                throw new Exception($"Required 2 parameter but found {internalNodes.Count - 1} parameters.");


            var actionSelectorNode = internalNodes[1].NodeType == NodeType.Variable
                ? internalNodes[1]
                : throw new Exception($"Expected variable but found {internalNodes[1].NodeType}");

            if (actionSelectorNode.Value.Count(x => x == '.') == 0)
                throw new Exception($"Expected $selector.action shape but found {actionSelectorNode.Value}");

            var varArgs = actionSelectorNode.Value.Split('.');

            var selectorName = varArgs[0];
            var actionName = varArgs[1];

            if (!MiscVariable.ActionSelectorExists(selectorName))
                throw new Exception($"Cannot resolve selector {selectorName}");

            var rawTextNode = NodeUtils.GetRawOrVariableValue(internalNodes[2]);

            var actionTextNode = rawTextNode != null && rawTextNode.NodeType == NodeType.String
                ? internalNodes[2]
                : throw new Exception($"Required string as Action Text but found {internalNodes[2].NodeType}");


            var action = new Action
            {
                SucceedingSegments = new List<Segment>(),
                Text = actionTextNode.Value,
                Name = actionName
            };
            MiscVariable.PutAction(selectorName, action);
            return args;
        };

        #endregion

        #region BindCallback

        public static Func<LineNode, Collection<object>> BindCallback = delegate(LineNode node)
        {
            var args = new Collection<object>();

            if (node.GetHeadNodeType() != NodeType.BindCallback)
                throw new Exception($"Called {NodeType.BindCallback} processor but found {node.GetHeadNodeType()}");

            var internalNodes = node.InternalNodes;

            var actionSelectorNode = internalNodes[1].NodeType == NodeType.Variable
                ? internalNodes[1]
                : throw new Exception($"Expected variable but found {internalNodes[1].NodeType}");

            if (actionSelectorNode.Value.Count(x => x == '.') == 0)
                throw new Exception($"Expected $selector.action shape but found {actionSelectorNode.Value}");

            var varArgs = actionSelectorNode.Value.Split('.');

            var selectorName = varArgs[0];
            var actionName = varArgs[1];


            if (!MiscVariable.ActionSelectorExists(selectorName))
                throw new Exception($"Cannot resolve selector {selectorName}");

            var action = MiscVariable.GetAction(selectorName, actionName) != null
                ? MiscVariable.GetAction(selectorName, actionName)
                : throw new Exception($"Cannot resolve action {actionName} in {selectorName}.");

            var startBracketIdx = NodeUtils.FindStartIndex(internalNodes, NodeType.StartBracket);

            if (startBracketIdx == -1)
                throw new Exception("{ expected.");

            var endBracketIdx = NodeUtils.FindStartIndex(internalNodes, NodeType.EndBracket);
            if (endBracketIdx == -1)
                throw new Exception("} expected.");

            var callbackStartIdx = startBracketIdx + 1;
            var callbackEndIdx = endBracketIdx - 1;

            var callbackNodes =
                internalNodes.ToList().GetRange(callbackStartIdx, callbackEndIdx - callbackStartIdx + 1);

            var callbackSegment =
                NodeParser.ParseSegment(new LineNode(NodeUtils.JoinNodeToString(new Collection<Node>(callbackNodes)),
                    callbackNodes));

            action.SucceedingSegments.Add(callbackSegment);
            return args;
        };

        #endregion

        #region ShowActionSelector

        public static Func<LineNode, Collection<object>> ShowActionSelector = delegate(LineNode node)
        {
            var args = new Collection<object>();

            if (node.GetHeadNodeType() != NodeType.ShowActionSelector)
                throw new Exception(
                    $"Called {NodeType.ShowActionSelector} processor but found {node.GetHeadNodeType()}");

            var internalNodes = node.InternalNodes;

            var actionSelectorNode = internalNodes[1].NodeType == NodeType.Variable
                ? internalNodes[1]
                : throw new Exception($"Expected variable but found {internalNodes[1].NodeType}");


            var selectorName = actionSelectorNode.Value;


            if (!MiscVariable.ActionSelectorExists(selectorName))
                throw new Exception($"Cannot resolve selector {selectorName}");

            args.Add(MiscVariable.GetActionSelector(selectorName));
            return args;
        };

        #endregion

        #region Print

        public static Func<LineNode, Collection<object>> Print = delegate(LineNode node)
        {
            var args = new Collection<object>();

            if (node.GetHeadNodeType() != NodeType.Print)
                throw new Exception($"Called {NodeType.Print} but found {node.GetHeadNodeType()}");

            var startBracketIdx = NodeUtils.FindStartIndex(node.InternalNodes, NodeType.StartBracket);
            if (startBracketIdx == -1)
                throw new Exception("{ expected.");

            var endBracketIdx = NodeUtils.FindStartIndex(node.InternalNodes, NodeType.EndBracket);
            if (endBracketIdx == -1)
                throw new Exception("} expected.");

            args.Add(ParseVariableStatement(node.InternalNodes.ToList()
                .GetRange(startBracketIdx, endBracketIdx - startBracketIdx + 1).ToCollection()));

            return args;
        };

        #endregion
    }
}