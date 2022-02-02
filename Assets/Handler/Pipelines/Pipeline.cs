using System.Collections.ObjectModel;
using Handler.Segments;

namespace Handler.Pipelines
{
    public abstract class Pipeline
    {
        public Collection<Segment> InternalFlow;

        public Pipeline(string name, Collection<Segment> internalFlow)
        {
            Name = name;
            InternalFlow = internalFlow;
        }

        public string Name { get; }

        public abstract void OnEventFired();
        public abstract void OnStart();
        public abstract void OnEnd();
    }
}