using System.Collections.ObjectModel;
using Handler.FlowContext;
using NotImplementedException = System.NotImplementedException;

namespace Handler.Segments.Action
{
    public class SegShowActionSelector : Segment
    {
        public override SegmentResponse Execute(Context ctx, Collection<object> args)
        {
            throw new NotImplementedException();
        }

        public override SegmentResponse OnSuspend(Context ctx)
        {
            throw new NotImplementedException();
        }
    }
}