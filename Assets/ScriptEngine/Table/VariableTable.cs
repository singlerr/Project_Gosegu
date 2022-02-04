using System.Collections.Generic;
using ScriptEngine.Elements;

namespace ScriptEngine.Table
{
    public class VariableTable
    {
        public static Dictionary<string, Node> Variables = new Dictionary<string, Node>();

        public static Node GetVariable(string varName)
        {
            return VariableExists(varName) ? Variables[varName] : null;
        }

        public static bool VariableExists(string varName)
        {
            return Variables.ContainsKey(varName);
        }

        public static void PutVariable(string varName, Node variable)
        {
            Variables[varName] = variable;
        }
    }
}