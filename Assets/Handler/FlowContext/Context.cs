using System;
using ScriptEngine.Elements;
using ScriptEngine.Elements.Nodes;
using ScriptEngine.Table;

namespace Handler.FlowContext
{
    public class Context
    {
        public bool PutVariable(string valueName, object obj)
        {
            if (obj is int)
            {
                VariableTable.PutVariable(valueName, new ElementNode(NodeType.Int, obj.ToString()));
                return true;
            }if (obj is double)
            {
                VariableTable.PutVariable(valueName, new ElementNode(NodeType.Double, obj.ToString()));
                return true;
            }if (obj is string)
            {
                VariableTable.PutVariable(valueName, new ElementNode(NodeType.String, obj.ToString()));
                return true;
            }

            return false;
        }
        public T GetVariable<T>(string valueName) where T:class
        {
            return VariableTable.GetVariable(valueName) as T;
        }
    }
}