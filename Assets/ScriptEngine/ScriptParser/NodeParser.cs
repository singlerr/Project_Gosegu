using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Handler.Segments;
using Handler.Segments.System;
using ScriptEngine.Elements;
using ScriptEngine.Elements.Nodes;

namespace ScriptEngine.ScriptParser
{
    public class NodeParser
    {
        private static readonly Dictionary<NodeType, Func<LineNode, Collection<object>>> NodeAnalyzers =
            new Dictionary<NodeType, Func<LineNode, Collection<object>>>();

        private static readonly Dictionary<NodeType, Func<Func<LineNode, Collection<object>>,LineNode, Segment>>
            SegmentInitializers = new Dictionary<NodeType,Func<Func<LineNode, Collection<object>>,LineNode, Segment>>();
        
        
        static NodeParser()
        {
            
            NodeAnalyzers[NodeType.UpdateState] = LineNodeParserUnion.UpdateState;
            SegmentInitializers[NodeType.UpdateState] = SegmentUnion.UpdateState;
            NodeAnalyzers[NodeType.SetVariable] = LineNodeParserUnion.SetVariable;
            SegmentInitializers[NodeType.SetVariable] = SegmentUnion.SetVariable;
            NodeAnalyzers[NodeType.CreateAction] = LineNodeParserUnion.CreateAction;
            SegmentInitializers[NodeType.CreateAction] = SegmentUnion.CreateAction;
            NodeAnalyzers[NodeType.CreateActionSelector] = LineNodeParserUnion.CreateActionSelector;
            SegmentInitializers[NodeType.CreateActionSelector] = SegmentUnion.CreateActionSelector;
            NodeAnalyzers[NodeType.BindCallback] = LineNodeParserUnion.BindCallback;
            SegmentInitializers[NodeType.BindCallback] = SegmentUnion.BindCallback;
            NodeAnalyzers[NodeType.ShowActionSelector] = LineNodeParserUnion.ShowActionSelector;
            SegmentInitializers[NodeType.ShowActionSelector] = SegmentUnion.ShowActionSelector;
            NodeAnalyzers[NodeType.Print] = LineNodeParserUnion.Print;
            SegmentInitializers[NodeType.Print] = SegmentUnion.Print;
            NodeAnalyzers[NodeType.If] = LineNodeParserUnion.If;
            SegmentInitializers[NodeType.If] = SegmentUnion.If;
        }

        public static Segment ParseSegment(LineNode lineNode)
        {
            if (NodeAnalyzers.ContainsKey(lineNode.GetHeadNodeType()) && SegmentInitializers.ContainsKey(lineNode.GetHeadNodeType()))
            {
                return SegmentInitializers[lineNode.GetHeadNodeType()]
                    .Invoke(NodeAnalyzers[lineNode.GetHeadNodeType()], lineNode);
            }

            return null;
        }

        public static Collection<object> Execute(LineNode lineNode)
        {
            if (NodeAnalyzers.ContainsKey(lineNode.GetHeadNodeType()))
                return NodeAnalyzers[lineNode.GetHeadNodeType()].Invoke(lineNode);
            return new Collection<object>();
        }
    }
}