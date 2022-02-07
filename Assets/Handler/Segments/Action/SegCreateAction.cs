using System.Collections.ObjectModel;
using Handler.FlowContext;
using NotImplementedException = System.NotImplementedException;

namespace Handler.Segments.Action
{
    // 선택지 선택 action segment
    public class SegCreateAction : Segment
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