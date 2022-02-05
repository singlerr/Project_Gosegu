using System.Collections.ObjectModel;
using Handler.Wrappers;

namespace Handler.Segments
{
    public class ActionSelector : Wrapper
    {
        public Collection<Wrappers.Action> Actions;
        public string Title;

        public ActionSelector()
        {
            Actions = new Collection<Wrappers.Action>();
        }
    }
}