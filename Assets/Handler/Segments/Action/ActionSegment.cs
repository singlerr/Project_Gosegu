using System.Collections.ObjectModel;
using Handler.FlowContext;

namespace Handler.Segments.Action
{
    public class ActionSegment : Segment
    {
        public Collection<Wrappers.Action> ExecutableActions;

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