using System;
using System.Collections.ObjectModel;
using Handler.FlowContext;

namespace Handler.Segments.System
{
    public class SegSetVariable : Segment
    {
        public override SegmentResponse Execute(Context ctx, Collection<object> args)
        {
            return new SegmentResponse(new Collection<Segment>(), SegmentResponseType.Continue);
        }

        public override SegmentResponse OnSuspend(Context ctx)
        {
            return new SegmentResponse(new Collection<Segment>(), SegmentResponseType.Continue);
        }
    }
}