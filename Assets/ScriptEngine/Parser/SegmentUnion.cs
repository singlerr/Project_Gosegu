using System;
using System.Collections.ObjectModel;
using Handler.Segments;
using Handler.Segments.Action;
using Handler.Segments.Effect;
using Handler.Segments.System;
using ScriptEngine.Elements.Nodes;

namespace ScriptEngine.ScriptParser
{
    public static class SegmentUnion
    {
        
        public static Func<Func<LineNode, Collection<object>>,LineNode, Segment> UpdateState = (func, node) =>  new SegUpdateState
        {
            CurrentLineNode = node,
            NodeProcessor = func
        };

        public static Func<Func<LineNode, Collection<object>>, LineNode, Segment> SetVariable = (func, node) =>
            new SegSetVariable
            {
                CurrentLineNode = node,
                NodeProcessor = func
            };
        public static Func<Func<LineNode, Collection<object>>,LineNode, Segment> Print = (func, node) =>  new SegText
        {
            CurrentLineNode = node,
            NodeProcessor = func
        };
        public static Func<Func<LineNode, Collection<object>>,LineNode, Segment> CreateActionSelector = (func, node) =>  new SegCreateActionSelector
        {
            CurrentLineNode = node,
            NodeProcessor = func
        };
        
        public static Func<Func<LineNode, Collection<object>>,LineNode, Segment> CreateAction = (func, node) =>  new SegCreateAction
        {
            CurrentLineNode = node,
            NodeProcessor = func
        };
        public static Func<Func<LineNode, Collection<object>>,LineNode, Segment> BindCallback = (func, node) =>  new SegBindCallback
        {
            CurrentLineNode = node,
            NodeProcessor = func
        };
        public static Func<Func<LineNode, Collection<object>>,LineNode, Segment> ShowActionSelector = (func, node) =>  new SegBindCallback
        {
            CurrentLineNode = node,
            NodeProcessor = func
        };
        public static Func<Func<LineNode, Collection<object>>,LineNode, Segment> If = (func, node) =>  new SegIf
        {
            CurrentLineNode = node,
            NodeProcessor = func
        };

    }
}