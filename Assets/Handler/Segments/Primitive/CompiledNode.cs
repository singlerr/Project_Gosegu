using System;
using System.Collections.ObjectModel;
using Handler.FlowContext;
using Handler.Segments;
using ScriptEngine.Elements.Nodes;

namespace Handler.Segments.Primitive
{
public abstract class CompiledNode
{
    /**
     * <summary>
     * 이 클래스의 상속자는 Segment입니다. 그리고 <see cref="Execute(Handler.FlowContext.Context,ScriptEngine.Elements.Nodes.LineNode)"/>,<see cref="Execute(Handler.FlowContext.Context)"/>가 Call되었을 때, <see cref="NodeProcessor"/>가 이 변수를 input으로 받습니다.
     * 그리고 그 처리 결과를 <see cref="Execute(Handler.FlowContext.Context,Collection)"/> 의 args로 넘겨줍니다.
     * </summary>
     */
    private Collection<LineNode> _internalNodes;

    /**
     * <summary>
     * 이 CompiledNode가 할당된 구문(한 줄)입니다. 이 CompiledNode가 Execute되는 내용은, 이 구문의 명령 내용과 같습니다.
     * </summary>
     */
    public LineNode CurrentLineNode;
    /**
     * <summary>
     * <see cref="CurrentLineNode"/>를 해석하고, Execute하는 처리기입니다. 이 처리기는 Object Collection를 return합니다. 이는 <see cref="Execute(Handler.FlowContext.Context,Collection)"/>의 Object Collection 파라미터로 전달됩니다.
     * </summary>
     */
    public Func<LineNode, Collection<object>> NodeProcessor;

    /**
     * <summary>
     * <param name="ctx">Segment Flow가 진행될 때, Segment Flow의 상태를 포괄하는 클래스입니다. 말그대로 '문맥'이라는 의미와 같다고 볼 수 있습니다.
     * 예를 들어, 게임이 진행될 때, 플레이어의 상태를 저장하는 Container입니다. 자세한 내용은 <seealso cref="Context"/>를 참고하세요. 
     * </param>
     * <param name="lineNode">
     *  
     * </param>
     * 이 함수는 Segment 상속 클래스에서 호출되어서는 안됩니다. 이 함수는 반드시 <see cref="PipelineHandler"/>에서만 호출되어야만 합니다.
     *  <param name="lineNode"/>는 <see cref="NodeProcessor"/>의 input이 됩니다. 그리고 그 결과값인 Object Collection과, <param name="ctx"/>으로 <see cref="Execute(Handler.FlowContext.Context,Collection)"/>을 호출합니다.
     * </summary>
     */
    public void Execute(Context ctx, LineNode lineNode)
    {
        var args = NodeProcessor(lineNode);
        Execute(ctx, args);
    }
    /**
     * <summary>
     * <param name="ctx">Segment Flow가 진행될 때, Segment Flow의 상태를 포괄하는 클래스입니다. 말그대로 '문맥'이라는 의미와 같다고 볼 수 있습니다.
     * 예를 들어, 게임이 진행될 때, 플레이어의 상태를 저장하는 Container입니다. 자세한 내용은 <seealso cref="Context"/>를 참고하세요. 
     * </param>
     * 이 함수는 Segment 상속 클래스에서 호출되어서는 안됩니다. 이 함수는 반드시 <see cref="PipelineHandler"/>에서만 호출되어야만 합니다.
     * 이 함수는 <see cref="CurrentLineNode"/>가 not null임을 가정합니다. 즉, <see cref="CurrentLineNode"/>가 null 일때 이 함수를 호출해서는 안됩니다.
     *  <see cref="CurrentLineNode"/>는 <see cref="NodeProcessor"/>의 input이 됩니다. 그리고 그 결과값인 Object Collection과, <param name="ctx"/>으로 <see cref="Execute(Handler.FlowContext.Context,Collection)"/>을 호출합니다.
     * </summary>
     */
    public void Execute(Context ctx)
    {
        var args = NodeProcessor(CurrentLineNode);
        Execute(ctx, args);
    }
    /**
     * <summary>
     * <param name="ctx">Segment Flow가 진행될 때, Segment Flow의 상태를 포괄하는 클래스입니다. 말그대로 '문맥'이라는 의미와 같다고 볼 수 있습니다.
     * 예를 들어, 게임이 진행될 때, 플레이어의 상태를 저장하는 Container입니다. 자세한 내용은 <seealso cref="Context"/>를 참고하세요. 
     * </param>
     * <param name="args">
     * 각 Segment 상속자에게 필요한 파라미터가 전달되는 Container입니다. 상황에 맞게 args의 배열 요소를 불러와 형 변환을 하십시오. 
     * </param>
     * 이 함수는 Segment 상속 클래스에서 구현되는 추상 함수입니다. Segment 상속자는 이 함수를 구현함으로써, 개별적인 행동을 구성할 수 있습니다.
     * 예를 들어, SegText라는 Segment 상속자는 이 함수에 텍스트를 출력하는 구문을 작성합니다. 그러면 이 Segment가 Execute 되었을 때, 텍스트를 출력합니다.
     * 또, args를 이용해 각 기능에 맞는 파라미터들을 불러 올 수 있습니다. args에 어떤 값이 전달되는지는 각 Segment 상속자를 참고하세요.
     *  <returns>SegmentResponse를 반환합니다. SegmentResposne의 NextSegments는 현재 Segment 후에 바로 실행되어야 할 Segment들을 의미합니다.
     * 만약, 후행되어야할 Segment들이 없다면 빈 컬렉션을 반환하십시오.
     * 또, SegmentResponseType은 이 Segment 실행 후 계속 진행할지, 아니면 잠시 멈추고 새로운 이벤트를 기다릴지 결정합니다.
     * <see cref="SegmentResponseType"/>에서 Continue는 현재 Segment Flow대로 계속 진행합니다.
     * Suspend는 잠시 Segment Flow를 멈추고 새로운 이벤트를 기다립니다. 그리고, 새로운 이벤트가 감지되면 <see cref="OnSuspend"/> 함수를 호출합니다.
     * </returns>
     * </summary>
     */
    public abstract SegmentResponse Execute(Context ctx, Collection<object> args);
    /**
     * <summary>
     * <see cref="Execute(Handler.FlowContext.Context,Object Collection)"/>의 SegmentResposneType이 Suspend이었을 때만 "활성화"되는 함수입니다.
     * ReponseType이 Continue 일 경우 이 함수는 어떠한 경우에도 호출되지 않습니다.
     * 이 함수는 
     * </summary>
     */
    public abstract SegmentResponse OnSuspend(Context ctx);
}
}

