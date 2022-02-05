using System.Collections.ObjectModel;
using Handler.Segments;
using ScriptEngine.Elements.Nodes;

namespace ScriptEngine.ScriptParser
{
    public class ScriptParser
    {
        private readonly Collection<LineNode> _lineNodes;

        public ScriptParser(Collection<LineNode> lineNodes)
        {
            _lineNodes = lineNodes;
        }

        public Collection<Segment> Parse()
        {
            var segments = new Collection<Segment>();
            foreach (var lineNode in _lineNodes) segments.Add(NodeParser.ParseSegment(lineNode));
            return segments;
        }
    }
}