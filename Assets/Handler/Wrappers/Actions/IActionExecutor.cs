using Handler.FlowContext;
using Handler.Segments;

namespace Handler.Wrappers.Actions
{
    public interface IActionExecutor
    {
        SegmentResponse Execute(Context ctx);
    }
}