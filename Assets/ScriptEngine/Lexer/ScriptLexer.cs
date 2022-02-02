using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using ScriptEngine.Elements;
using ScriptEngine.Elements.Nodes;

namespace ScriptEngine.Lexer
{
    public class ScriptLexer
    {
        private string[] _lines;
        public ScriptLexer(string[] lines)
        {
            _lines = lines;
        }

        public ScriptLexer(string path)
        {
            _lines = File.ReadAllLines(path);
        }

        public LineNode[] Lex()
        {
            var parsedLineNodes = new LineNode[_lines.Length];

            for (var lineIdx = 0; lineIdx < _lines.Length; lineIdx++)
            {
                var currentLine = _lines[lineIdx];
                
            }
            
            return parsedLineNodes;
        }

        public Collection<Node> LexInternalNodes(string line)
        {
            var internalNodes = new Collection<Node>();
            var buffer = new StringBuilder();

            var lexerState = LexerState.None;
            for (var charIdx = 0; charIdx < line.Length; charIdx++)
            {
                var currentChar = line[charIdx];
                buffer.Append(currentChar);

                var currentValue = buffer.ToString();
                //Meet string quote
                if (NodeType.Quote.GetExpression().Equals(currentValue))
                {
                    //End string quote(current in string)
                    if ((lexerState & LexerState.InString) != 0)
                    {
                        //Remove first "
                        buffer.Remove(0, 0);
                        //Remove last "
                        buffer.Remove(buffer.Length - 2, buffer.Length - 1);

                        var stringNode = new ElementNode(NodeType.String, currentValue);
                        
                        internalNodes.Add(stringNode);
                        
                        lexerState &= ~LexerState.InString;
                    }
                    else
                    {
                        //Not in string
                        lexerState |= LexerState.InString;
                    }
                    continue;
                }
                else
                {
                    if (char.IsWhiteSpace(currentChar))
                    {
                        buffer.Clear();
                        continue;
                    }
                }
                
                if (NodeType.StartBracket.GetExpression().Equals(currentValue))
                {
                    //Current in bracket
                    if ((lexerState & LexerState.InBracket) != 0)
                    {
                        //Currently start bracket in bracket not supported;
                        buffer.Clear();
                        throw new ArgumentOutOfRangeException(nameof(currentValue),"Currently start bracket in bracket is not supported.");
                    }
                    else
                    {
                        var bracketNode = new ElementNode(NodeType.StartBracket,currentValue);
                        internalNodes.Add(bracketNode);
                    }
                    continue;
                }

                if (NodeType.EndBracket.GetExpression().Equals(currentValue))
                {
                    //Current in bracket
                    if ((lexerState & LexerState.InBracket) != 0)
                    {
                        var bracketNode = new ElementNode(NodeType.EndBracket,currentValue);
                        internalNodes.Add(bracketNode);
                    }
                    else
                    {
                        //Currently start bracket in bracket not supported;
                        buffer.Clear();
                        throw new ArgumentOutOfRangeException(nameof(currentValue),"Start bracket must be stated first.");
                    }
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

                var operators = new string[] {NodeType.Add.GetExpression(),NodeType.Div.GetExpression(),NodeType.Sub.GetExpression(),NodeType.Mul.GetExpression()};
                foreach (var op in operators)
                {
                    if (op.Equals(currentValue))
                    {
                        var opNode = new ElementNode()
                    }
                }
            }

            return internalNodes;
        }
        
    }
}