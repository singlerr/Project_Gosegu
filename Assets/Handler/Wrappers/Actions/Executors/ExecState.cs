using Handler.FlowContext;
using Handler.FlowContext.States;
using Handler.Pipelines;
using Handler.Segments;

namespace Handler.Wrappers.Actions.Executors
{
    public class ExecState : IActionExecutor
    {
        private State _newState;
        public ExecState(State newState)
        {
            _newState = newState;
        }
        public SegmentResponse Execute(Context ctx)
        {
            var newStateType = _newState.GetType();

            if (newStateType == typeof(GoseguState))
            {
                
            }else if (newStateType == typeof(StockState))
            {
                
            }
            
            throw new System.NotImplementedException();
        }
    }
}