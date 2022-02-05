using System.Collections.Generic;
using System.Linq;
using Handler.Segments;
using Handler.Wrappers;

namespace ScriptEngine.Table
{
    public class MiscVariable
    {
        public static Dictionary<string, object> MiscVariables = new Dictionary<string, object>();

        public static object GetVariable(string varName)
        {
            return VariableExists(varName) ? MiscVariables[varName] : null;
        }

        public static bool VariableExists(string varName)
        {
            return MiscVariables.ContainsKey(varName);
        }

        public static void PutVariable(string varName, object variable)
        {
            MiscVariables[varName] = variable;
        }

        public static void PutActionSelector(string selectorName, ActionSelector selector)
        {
            PutVariable(selectorName, selector);
        }

        public static ActionSelector GetActionSelector(string selectorName)
        {
            return ActionSelectorExists(selectorName)
                ? GetVariable(selectorName) as ActionSelector
                : null;
        }

        public static Action GetAction(string selectorName, string action)
        {
            if (!ActionSelectorExists(selectorName))
                return null;

            var selector = MiscVariables[selectorName] as ActionSelector;
            return selector.Actions.SingleOrDefault(x => x.Name.Equals(action));
        }

        public static void PutAction(string selectorName, Action action)
        {
            var selector = GetActionSelector(selectorName);
            selector.Actions.Add(action);
        }

        public static bool ActionSelectorExists(string selectorName)
        {
            return VariableExists(selectorName) && GetVariable(selectorName) is ActionSelector;
        }
    }
}