using System.Collections.ObjectModel;
using Handler.FlowContext;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;

namespace Handler.Segments.Action
{
    // 선택지 생성 Segment
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