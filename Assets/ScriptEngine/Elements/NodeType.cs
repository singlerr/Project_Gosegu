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
        [NodeExpression("{")]
        StartBracket,
        [NodeExpression("}")]
        EndBracket,
        [NodeExpression(",")]
        Comma,
        [NodeExpression("+")]
        Add,
        [NodeExpression("-")]
        Sub,
        [NodeExpression("*")]
        Mul,
        [NodeExpression("/")]
        Div,
        [NodeExpression("\"")]
        Quote,
        [NodeExpression(":")]
        NameAndVarSeparator,
        [NodeExpression(".")]
        Dot,
        /**
         * Reserved words
         */
        [NodeExpression("update_state")]
        UpdateState,
        [NodeExpression("set_variable")]
        SetVariable
    }

    public class NodeExpression : Attribute
    {
        public string Expression { get; private set; }

        public NodeExpression(string expression)
        {
            Expression = expression;
        }
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

        public static string ValueOf(this NodeType nodeType, string value)
        {
            var type = nodeType.GetType();
            //Value of
            throw new NotImplementedException();
        }
    }
}