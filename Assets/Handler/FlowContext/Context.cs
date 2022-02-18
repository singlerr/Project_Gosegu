using System;
using ScriptEngine.Elements;
using ScriptEngine.Elements.Nodes;
using ScriptEngine.Table;

namespace Handler.FlowContext
{
    /**
     * <summary>
     * 게임 전체를 관통하는 상태 저장고입니다.
     * 주로 게임 내 변수, 캐릭터 상태 등을 저장하고, 다음 Segment에서 사용할 수 있도록 구성되어 있습니다.
     * 이 클래스를 이용해 변수를 추가하거나, 삭제할 수 있습니다.
     * 또, 이 클래스에서는 스크립트 상에서 설정한 변수를 그대로 불러 올 수 있습니다. 따라서 게임 내 시스템과 스크립트가 상호작용할 수 있는 유일한 창구입니다.
     * </summary>
     */
    public class Context
    {
        /**
         *  <summary>
         * 변수를 저장합니다.
         * <param name="valueName">변수의 이름입니다. 각 변수의 이름은 unique 해야합니다.</param>
         * <param name="obj">추가할 변수의 값입니다. 지원되는 자료형은 int, double, string 입니다.</param>
         * </summary>
         */
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
        /**
         * <summary>
         * <returns>해당 이름의 변수를 가져옵니다. 만약 해당 변수가 존재하지 않을 경우 null을 반환합니다.</returns>
         * <param name="valueName">변수 이름</param>
         * </summary>
         */
        public T GetVariable<T>(string valueName) where T:class
        {
            return VariableTable.GetVariable(valueName) as T;
        }
    }
}