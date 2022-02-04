namespace Handler.Segments
{
    public class SegmentResponse
    {
        public readonly Segment NextSegment;
        public readonly SegmentResponseType SegmentResponseType;

        public SegmentResponse(Segment nextSegment, SegmentResponseType responseType)
        {
            NextSegment = nextSegment;
            SegmentResponseType = responseType;
        }
    }

    public enum SegmentResponseType
    {
        Continue,
        Suspend
    }
}