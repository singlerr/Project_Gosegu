using Handler.FlowContext;
using Handler.Pipelines;
using Handler.Segments;

namespace Handler.Wrappers.Actions
{
    public interface IActionExecutor
    {
        SegmentResponse Execute(Context ctx);
    }
}