using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using ScriptEngine.Elements;
using ScriptEngine.Elements.Nodes;
using ScriptEngine.Utils;

namespace ScriptEngine.Lexer
{
    public class ScriptLexer
    {
        private readonly string[] _rawLines;

        public ScriptLexer(string[] lines)
        {
            _rawLines = lines;
        }

        public ScriptLexer(string path)
        {
            _rawLines = File.ReadAllLines(path);
        }

        public Collection<LineNode> Lex()
        {
            var parsedLineNodes = new Collection<LineNode>();

            var lines = new Collection<string>(_rawLines);
            for (var lineIdx = 0; lineIdx < lines.Count; lineIdx++)
            {
                var currentLine = lines[lineIdx];

                var lineNode = new LineNode(currentLine);
                var internalNodes = LexInternalNodes(currentLine);

                var headNodeType = LineNode.GetHeadNodeType(internalNodes);

                //Add all elements from bound to end if
                if (headNodeType == NodeType.Else)
                {
                    var upperIf =
                        NodeUtils.FindFirstInBound(parsedLineNodes, parsedLineNodes.Count - 1, NodeType.If, true);
                    if (upperIf.Item1 == -1) throw new Exception("Found ElseIf but If not found.");

                    var temp = parsedLineNodes.ToList();

                    var startIdx = upperIf.Item1 + 1;
                    var endIdx = parsedLineNodes.Count - 1;

                    var trueNodes = temp.GetRange(startIdx, endIdx - startIdx + 1);

                    var headNode = (upperIf.Item2 as LineNode).GetHeadNode() as ConditionNode;
                    //Add nodes to collection that would be executed when condition is true
                    headNode.TrueLineNodes = new Collection<LineNode>(trueNodes);
                    temp.RemoveRange(startIdx, endIdx - startIdx + 1);
                    parsedLineNodes = new Collection<LineNode>(temp);
                }

                if (headNodeType == NodeType.ElseIf)
                    throw new NotImplementedException("Currently ElseIf is not supported.");
                if (headNodeType == NodeType.EndIf)
                {
                    var ifUpperNode =
                        NodeUtils.FindFirstInBound(parsedLineNodes, parsedLineNodes.Count - 1, NodeType.If, true);
                    var elseNode = NodeUtils.FindFirstInBound(parsedLineNodes, parsedLineNodes.Count - 1, NodeType.Else,
                        true);

                    if (ifUpperNode.Item1 == -1)
                        throw new Exception("Expected if but not found.");

                    if (elseNode.Item1 != -1)
                    {
                        var temp = parsedLineNodes.ToList();

                        var startIdx = elseNode.Item1 + 1;
                        var endIdx = parsedLineNodes.Count - 1;

                        var falseNodes = temp.GetRange(startIdx, endIdx - startIdx + 1);

                        var headNode = (ifUpperNode.Item2 as LineNode).GetHeadNode() as ConditionNode;

                        //Add nodes to collection that would be executed when condition is true
                        headNode.FalseLineNodes = new Collection<LineNode>(falseNodes);
                        //Remove else sections including 'else'
                        temp.RemoveRange(startIdx - 1, endIdx - (startIdx - 1) + 1);
                        parsedLineNodes = new Collection<LineNode>(temp);
                        continue;
                    }
                    else
                    {
                        var temp = parsedLineNodes.ToList();

                        var startIdx = ifUpperNode.Item1 + 1;
                        var endIdx = parsedLineNodes.Count - 1;

                        var trueNodes = temp.GetRange(startIdx, endIdx - startIdx + 1);

                        var headNode = (ifUpperNode.Item2 as LineNode).GetHeadNode() as ConditionNode;

                        //Add nodes to collection that would be executed when condition is true
                        headNode.FalseLineNodes = new Collection<LineNode>(trueNodes);
                        temp.RemoveRange(startIdx, endIdx - startIdx + 1);
                        parsedLineNodes = new Collection<LineNode>(temp);
                        continue;
                    }
                }

                foreach (var internalNode in internalNodes) lineNode.InternalNodes.Add(internalNode);
                parsedLineNodes.Add(lineNode);
            }

            return parsedLineNodes;
        }

        public Collection<string> Preprocess()
        {
            return Preprocess(_rawLines);
        }

        public Collection<string> Preprocess(string[] lines)
        {
            var isTail = false;
            var newLines = new Collection<string>();
            var headLine = "";
            for (var i = 0; i < lines.Length; i++)
            {
                var currentLine = lines[i].Trim();
                if (currentLine.StartsWith(NodeType.NewLine.GetExpression()))
                {
                    headLine += currentLine.Remove(0, 1);
                    isTail = true;
                }
                else
                {
                    if (isTail)
                    {
                        newLines.Add(headLine);
                        headLine = currentLine;
                        isTail = false;
                    }
                    else
                    {
                        headLine = currentLine;
                    }
                }

                //Last line
                if (i == lines.Length - 1)
                {
                    if (!isTail)
                    {
                        headLine = currentLine;
                        newLines.Add(headLine);
                    }
                    else
                    {
                        newLines.Add(headLine);
                    }
                }
            }

            return newLines;
        }

        public Collection<Node> LexInternalNodes(string line)
        {
            var bracketCount = new AtomicPositiveInteger();
            var internalNodes = new Collection<Node>();
            var buffer = new StringBuilder();

            var lexerState = LexerState.None;
            for (var charIdx = 0; charIdx < line.Length; charIdx++)
            {
                var currentChar = line[charIdx];
                buffer.Append(currentChar);

                var currentValue = buffer.ToString();
                //Meet string quote
                if (NodeType.Quote.GetExpression()[0] == currentChar)
                {
                    //End string quote(current in string)
                    if ((lexerState & LexerState.InString) != 0)
                    {
                        //Remove first "
                        buffer.Remove(0, 1);
                        //Remove last "
                        buffer.Remove(buffer.Length - 1, 1);

                        var stringNode = new ElementNode(NodeType.String, buffer.ToString());

                        internalNodes.Add(stringNode);
                        lexerState &= ~LexerState.InString;
                        buffer.Clear();
                    }
                    else
                    {
                        //Not in string
                        lexerState |= LexerState.InString;
                    }

                    continue;
                }

                //expression of var
                if (NodeType.Variable.GetExpression()[0] == currentChar)
                {
                    //check if next char is whitespace
                    var nextChar = charIdx + 1 < line.Length ? line[charIdx + 1] : ' ';
                    if (char.IsWhiteSpace(nextChar))
                        throw new ArgumentOutOfRangeException(nameof(currentValue), "Variable name must be declared.");

                    buffer.Remove(buffer.Length - 1, 1);
                    lexerState |= LexerState.DefVariable;
                }

                if ((lexerState & LexerState.DefVariable) != 0)
                {
                    if (char.IsWhiteSpace(currentChar))
                    {
                        //End of variable declaration
                        buffer.Remove(buffer.Length - 1, 1);
                        var varNode = new ElementNode(NodeType.Variable, buffer.ToString());
                        internalNodes.Add(varNode);
                        lexerState &= ~ LexerState.DefVariable;
                        buffer.Clear();
                        continue;
                    }

                    if (NodeUtils.IsOperator(currentChar))
                    {
                        buffer.Remove(buffer.Length - 1, 1);
                        var varNode = new ElementNode(NodeType.Variable, buffer.ToString());
                        internalNodes.Add(varNode);
                        lexerState &= ~ LexerState.DefVariable;
                        buffer.Clear();
                        buffer.Append(currentChar);
                        currentValue = buffer.ToString();
                    }
                }

                if ((lexerState & LexerState.InString) != 0)
                    continue;

                if (char.IsWhiteSpace(currentChar))
                {
                    buffer.Remove(buffer.Length - 1, 1);
                    continue;
                }

                if (NodeType.StartBracket.GetExpression()[0] == currentChar)
                {
                    //Current in bracket
                    if ((lexerState & LexerState.InBracket) != 0 || bracketCount.Number > 0)
                    {
                        var bracketNode = new ElementNode(NodeType.StartBracket, currentValue);
                        internalNodes.Add(bracketNode);
                        buffer.Clear();
                    }
                    else
                    {
                        lexerState |= LexerState.InBracket;
                        var bracketNode = new ElementNode(NodeType.StartBracket, currentValue);
                        internalNodes.Add(bracketNode);
                        buffer.Clear();
                    }

                    bracketCount.Increment();
                    continue;
                }

                if (NodeType.EndBracket.GetExpression()[0] == currentChar)
                {
                    //Current in bracket
                    if ((lexerState & LexerState.InBracket) != 0 || bracketCount.Number > 0)
                    {
                        lexerState &= ~LexerState.InBracket;
                        var bracketNode = new ElementNode(NodeType.EndBracket, currentValue);
                        internalNodes.Add(bracketNode);
                        buffer.Clear();
                    }
                    else
                    {
                        //Currently start bracket in bracket not supported;
                        buffer.Clear();
                        throw new ArgumentOutOfRangeException(nameof(currentValue),
                            "Start bracket must be stated first.");
                    }

                    bracketCount.Decrement();
                    continue;
                }

                //Current char is digit
                if (char.IsDigit(currentChar))
                {
                    //Check next char is digit too -> then record to buffer and continue to next char
                    //And when current char becomes next char, then it will process it again and check if digit shows up
                    var nextChar = charIdx + 1 < line.Length ? line[charIdx + 1] : ' ';

                    //If next char is still digit or dot(dot for double value)
                    if (char.IsDigit(nextChar) || NodeType.Dot.GetExpression()[0] == nextChar)
                    {
                    }
                    else
                    {
                        //Next char is not digit. So digit show is end!
                        //Before create node, we need to check {currentValue} contains . for double
                        if (currentValue.Contains(NodeType.Dot.GetExpression()))
                        {
                            var doubleNode = new ElementNode(NodeType.Double, currentValue);
                            internalNodes.Add(doubleNode);
                        }
                        else
                        {
                            //Create node as integer
                            var intNode = new ElementNode(NodeType.Int, currentValue);
                            internalNodes.Add(intNode);
                        }

                        buffer.Clear();
                    }
                }

                if (NodeType.Dot.GetExpression()[0] == currentChar)
                {
                    var prevChar = charIdx - 1 >= 0 ? line[charIdx - 1] : ' ';
                    var nextChar = charIdx + 1 < line.Length ? line[charIdx + 1] : ' ';
                    //We need to check if shape is (NOT_DIGIT).(NOT_DIGIT). It is invalid!
                    if (!char.IsDigit(prevChar) && !char.IsDigit(nextChar))
                    {
                        buffer.Clear();
                        throw new ArgumentOutOfRangeException(nameof(currentValue), ". is not valid number type.");
                    }
                }

                var operators = new[] { NodeType.Add, NodeType.Div, NodeType.Sub, NodeType.Mul };
                var isOkayToContinue = false;
                foreach (var op in operators)
                    if (op.GetExpression().Equals(currentValue))
                    {
                        var opNode = new ElementNode(op, currentValue);
                        internalNodes.Add(opNode);
                        buffer.Clear();
                        isOkayToContinue = true;
                    }

                if (isOkayToContinue)
                    continue;

                var condOps = new[]
                {
                    NodeType.LeftBig, NodeType.RightBig, NodeType.LeftBigEqual, NodeType.RightBigEqual, NodeType.Equal,
                    NodeType.NotEqual
                };
                isOkayToContinue = false;
                foreach (var condOp in condOps)
                    if (condOp.GetExpression().Equals(currentValue))
                    {
                        var condOpNode = new ElementNode(condOp, currentValue);
                        internalNodes.Add(condOpNode);
                        buffer.Clear();
                        isOkayToContinue = true;
                    }

                if (isOkayToContinue)
                    continue;
                if (NodeType.Comma.GetExpression()[0] == currentChar)
                {
                    var commaNode = new ElementNode(NodeType.Comma, currentValue);
                    internalNodes.Add(commaNode);
                    buffer.Clear();
                    continue;
                }

                if (NodeType.NameAndVarSeparator.GetExpression().Equals(currentValue))
                {
                    var sepNode = new ElementNode(NodeType.NameAndVarSeparator, currentValue);
                    internalNodes.Add(sepNode);
                    buffer.Clear();
                    continue;
                }

                if (NodeType.If.GetExpression().Equals(currentValue, StringComparison.InvariantCultureIgnoreCase))
                {
                    var ifNode = new ConditionNode(NodeType.If, currentValue);
                    internalNodes.Add(ifNode);
                    buffer.Clear();
                    continue;
                }

                if (NodeType.Else.GetExpression().Equals(currentValue, StringComparison.InvariantCultureIgnoreCase))
                {
                    var elseNode = new ElementNode(NodeType.Else, currentValue);
                    internalNodes.Add(elseNode);
                    buffer.Clear();
                    continue;
                }

                if (NodeType.EndIf.GetExpression().Equals(currentValue, StringComparison.InvariantCultureIgnoreCase))
                {
                    var endIfNode = new ElementNode(NodeType.EndIf, currentValue);
                    internalNodes.Add(endIfNode);
                    buffer.Clear();
                    continue;
                }

                //TODO("Else if support ")
                //Reserved words
                if (NodeType.UpdateState.GetExpression()
                    .Equals(currentValue, StringComparison.InvariantCultureIgnoreCase))
                {
                    var updateNode = new ElementNode(NodeType.UpdateState, currentValue);
                    internalNodes.Add(updateNode);
                    buffer.Clear();
                    continue;
                }

                if (NodeType.SetVariable.GetExpression()
                    .Equals(currentValue, StringComparison.InvariantCultureIgnoreCase))
                {
                    var setNode = new ElementNode(NodeType.SetVariable, currentValue);
                    internalNodes.Add(setNode);
                    buffer.Clear();
                    continue;
                }

                if (NodeType.Print.GetExpression().Equals(currentValue, StringComparison.InvariantCultureIgnoreCase))
                {
                    var printNode = new ElementNode(NodeType.Print, currentValue);
                    internalNodes.Add(printNode);
                    buffer.Clear();
                    continue;
                }

                if (NodeType.CreateActionSelector.GetExpression()
                    .Equals(currentValue, StringComparison.InvariantCultureIgnoreCase))
                {
                    var showNode = new ElementNode(NodeType.CreateActionSelector, currentValue);
                    internalNodes.Add(showNode);
                    buffer.Clear();
                }
            }

            if (bracketCount.Number != 0)
                throw new Exception("{가 있으먄 }가 있어야됩니다.");
            return internalNodes;
        }
    }
}