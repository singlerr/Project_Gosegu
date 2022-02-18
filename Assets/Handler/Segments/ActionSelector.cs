using System.Collections.ObjectModel;
using Handler.Wrappers;

namespace Handler.Segments
{
    /**
     * <summary>
     * 게임 내에서 선택지를 선택할 때 사용되는 선택지 Container입니다.
     * 이 Container에서 선택가능한 선택지를 확인할 수 있으며, 선택지 창의 제목도 확인할 수 있습니다.
     * 이 클래스는 <see cref="ScriptEngine.ScriptParser.LineNodeParserUnion"/>의 CreateActionSelector, BindCallback으로 만들어진 데이터를 바탕으로 생성됩니다.
     * </summary>
     */
    public class ActionSelector : Wrapper
    {

        /**
         * <summary>
         * 선택지 창의 선택지들입니다. 각 요소에 대한 설명은 <see cref="Wrappers.Action"/>을 참고하십시오.
         * 이 컬렉션의 오름차순이 선택지의 순서가 됩니다. 예를 들어, 컬렉션의 0번째 요소, 1번째 요소가 각각 Action1, Action2 라면, 이는 선택지에서 각각 첫번째 선택지, 두번째 선택지가 됩니다.
         * </summary>
         */
        public Collection<Wrappers.Action> Actions;
        /**
         * <summary>
         * 선택지 창의 제목입니다. 게임 UI에 따라 제목이 필요없을 수도 있습니다.
         * </summary>
         */
        public string Title;

        public ActionSelector()
        {
            Actions = new Collection<Wrappers.Action>();
        }
    }
}