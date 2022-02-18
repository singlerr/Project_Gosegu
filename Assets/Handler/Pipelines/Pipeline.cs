using System.Collections.ObjectModel;
using Handler.Segments;

namespace Handler.Pipelines
{
    /**
     * <summary>
     * 하나의 Segment 뭉치입니다.
     * 이 클래스의 <see cref="InternalFlow"/>는 이 Segment 뭉치에서 실행되어야할 Segment들입니다.
     * 하나의 Segment 뭉치가 실행될 때 모든 것을 handle하는 클래스라고 볼 수 있습니다.
     * 게임 내에서 발생하는 모든 이벤트들이 이 클래스에서 처리됩니다.
     * <see cref="ScriptEngine.ScriptLoader"/>에서 모든 스크립트를 읽어들인 뒤 이 클래스 객체들을 만듭니다.
     * 예를 들어 Segment 뭉치가 2개일 경우, 생성되는 Pipeline은 2개입니다. 
     * </summary>
     */
    public class Pipeline
    {
    
        /**
         * <summary>
         * 이 <see cref="Pipeline"/>에서 실행되어야 할 Segment들입니다. 
         * </summary>
         */
        public Collection<Segment> InternalFlow;

        public Pipeline(string name, Collection<Segment> internalFlow)
        {
            Name = name;
            InternalFlow = internalFlow;
        }

        /**
         * <summary>
         * 이 Segment 뭉치의 이름입니다.
         * 예를 들어 Segment 뭉치의 이름이 Day1 이었다면 <see cref="Name"/>의 값은 Day1 입니다.
         * </summary>
         */
        public string Name { get; }

        /**
         * <summary>
         * 게임 이벤트가 실행되었을 때 call되는 함수입니다. 어떤 종류의 이벤트가 발생했는지 확인하기 위해
         * 파라미터를 추가해야합니다.(아직 미정)
         * </summary>
         */
        public void OnEventFired()
        {
            
        }

        /**
         * <summary>
         * 이 Segment 뭉치가 처음으로 시작되었을 때 call되는 함수입니다. 즉, 게임 실행이든, 다른 Segment 뭉치에서 이 Segment 뭉치를 실행했을 경우든 이 함수가 call 됩니다.
         * </summary>
         */
        public void OnStart()
        {
            
        }
        
        /**
         * <summary>
         * 이 Segment 뭉치가 끝났을 때 call 되는 함수입니다. 
         * </summary>
         */
        public void OnEnd()
        {
            
        }
    }
}