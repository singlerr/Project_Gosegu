using System;

namespace ScriptEngine.Elements
{
    public enum NodeType
    {
        /**
         * Basic Node Type
         */
        Line,
        String,
        Int,
        Double,
        [NodeExpression("{")] StartBracket,
        [NodeExpression("}")] EndBracket,
        [NodeExpression(",")] Comma,
        [NodeExpression("+")] Add,
        [NodeExpression("-")] Sub,
        [NodeExpression("*")] Mul,
        [NodeExpression("/")] Div,
        [NodeExpression("==")] Equal,
        [NodeExpression(">")] LeftBig,
        [NodeExpression("<")] RightBig,
        [NodeExpression(">=")] LeftBigEqual,
        [NodeExpression("<=")] RightBigEqual,
        [NodeExpression("!=")] NotEqual,
        [NodeExpression("\"")] Quote,
        [NodeExpression("if")] If,
        [NodeExpression(":")] NameAndVarSeparator,
        [NodeExpression(".")] Dot,
        [NodeExpression("")] Unknown,
        [NodeExpression("@")] NewLine,
        [NodeExpression("$")] Variable,

        /**
         * Reserved words
         */
        [NodeExpression("update_state")] UpdateState,
        [NodeExpression("set_variable")] SetVariable,
        [NodeExpression("print")] Print,
        [NodeExpression("show_actions")] ShowActions
    }

    public class NodeExpression : Attribute
    {
        public NodeExpression(string expression)
        {
            Expression = expression;
        }

        public string Expression { get; }
    }

    public static class NodeTypeExt
    {
        public static string GetExpression(this NodeType nodeType)
        {
            var type = nodeType.GetType();
            var fieldInfo = type.GetField(nodeType.ToString());

            var attribs = fieldInfo.GetCustomAttributes(typeof(NodeExpression), false) as NodeExpression[];
            return attribs?.Length > 0 ? attribs[0].Expression : null;
        }
    }
}