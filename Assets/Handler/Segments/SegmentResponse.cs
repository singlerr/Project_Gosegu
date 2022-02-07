using System.Collections.ObjectModel;

namespace Handler.Segments
{
    public class SegmentResponse
    {
        public readonly Collection<Segment> NextSegments;
        public readonly SegmentResponseType SegmentResponseType;

        public SegmentResponse(Collection<Segment> nextSegment, SegmentResponseType responseType)
        {
            NextSegments = nextSegment;
            SegmentResponseType = responseType;
        }
    }

    public enum SegmentResponseType
    {
        Continue,
        Suspend
    }
}