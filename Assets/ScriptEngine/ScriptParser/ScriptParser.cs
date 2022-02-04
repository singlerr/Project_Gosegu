using System;
using System.Collections.ObjectModel;
using Handler.Segments;
using ScriptEngine.Elements.Nodes;

namespace ScriptEngine.ScriptParser
{
    public class ScriptParser
    {
        private Collection<LineNode> _lineNodes;

        public ScriptParser(Collection<LineNode> lineNodes)
        {
            _lineNodes = lineNodes;
        }

        public static Segment Parse(LineNode node)
        {
            var internalNodes = node.InternalNodes;
            throw new NotImplementedException();
        }
    }
}