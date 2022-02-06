using System;
using System.Collections.ObjectModel;
using Handler.FlowContext;
using Handler.Segments;
using ScriptEngine.Elements.Nodes;

namespace Handler.Segments.Primitive
{
public abstract class CompiledNode
{
    private Collection<LineNode> _internalNodes;

    public LineNode CurrentLineNode;
    public Func<LineNode, Collection<object>> NodeProcessor;

    public void Execute(Context ctx, LineNode lineNode)
    {
        var args = NodeProcessor(lineNode);
        Execute(ctx, args);
    }

    public void Execute(Context ctx)
    {
        var args = NodeProcessor(CurrentLineNode);
        Execute(ctx, args);
    }

    public abstract SegmentResponse Execute(Context ctx, Collection<object> args);
    public abstract SegmentResponse OnSuspend(Context ctx);
}
}

