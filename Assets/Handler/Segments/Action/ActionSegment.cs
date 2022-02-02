using Handler.FlowContext;
using Handler.Pipelines;
using Handler.Wrappers;

namespace Handler.Segments.Action
{
    public class ActionSegment : Segment
    {
        public Wrappers.Action ExecutableAction;
        
        public override SegmentResponse Execute(Context ctx)
        {
            return SegmentResponse.Continue;
        }

        public override SegmentResponse OnSuspend(Context ctx)
        {
            return SegmentResponse.Continue;
        }
    }
}