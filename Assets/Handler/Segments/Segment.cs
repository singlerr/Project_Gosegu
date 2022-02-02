using Handler.FlowContext;
using Handler.Pipelines;

namespace Handler.Segments
{
    public abstract class Segment
    {
        public abstract SegmentResponse Execute(Context ctx);

        public abstract SegmentResponse OnSuspend(Context ctx);
    }
}