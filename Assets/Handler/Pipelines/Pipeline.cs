namespace Handler.Pipelines
{
    public abstract class Pipeline
    {
        public string Name { get; private set; }

        public Pipeline(string name)
        {
            Name = name;
        }
    
        public abstract void OnEventFired();
        public abstract void OnStart();
        public abstract void OnEnd();

    } 
}

