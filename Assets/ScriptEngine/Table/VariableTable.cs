using System.Collections.Generic;
using System.Linq;
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

        public static bool StateExists(string stateName)
        {
            return Variables.Where(x => x.Key.Contains(stateName)).LongCount() > 0;
        }

        public static void PutStateVariable(string stateName, KeyValuePair<string, Node> variable)
        {
            var varName = $"{stateName}.{variable.Key}";
            Variables[varName] = variable.Value;
        }
    }
}