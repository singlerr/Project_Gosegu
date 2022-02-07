using System;
using System.Collections.ObjectModel;
using Handler.FlowContext;
using NotImplementedException = System.NotImplementedException;

namespace Handler.Segments.System
{
    public class SegIf : Segment
    {
        public override SegmentResponse Execute(Context ctx, Collection<object> args)
        {
            if (args[0] ! is Collection<Segment>)
            {
                throw new Exception($"Expected {typeof(Collection<Segment>)} but found {args[0].GetType()}");
            }

            return new SegmentResponse(args[0] as Collection<Segment>, SegmentResponseType.Continue);
        }

        public override SegmentResponse OnSuspend(Context ctx)
        {
            return new SegmentResponse(new Collection<Segment>(), SegmentResponseType.Continue);
        }
    }
}