using System.Collections.Generic;
using Handler.Segments;

namespace Handler.Wrappers
{
    public class Action : Wrapper
    {
        public string Name;
        public List<Segment> SucceedingSegments;
        public string Text;
    }
}