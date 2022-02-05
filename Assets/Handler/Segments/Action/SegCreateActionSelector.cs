using System.Collections.ObjectModel;
using Handler.FlowContext;

namespace Handler.Segments.Action
{
    public class SegCreateActionSelector : Segment
    {
        public override SegmentResponse Execute(Context ctx, Collection<object> args)
        {
            return new SegmentResponse(null, SegmentResponseType.Continue);
        }

        public override SegmentResponse OnSuspend(Context ctx)
        {
            return new SegmentResponse(null, SegmentResponseType.Continue);
        }
    }
}