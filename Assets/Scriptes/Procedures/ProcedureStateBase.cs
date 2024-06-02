public abstract class ProcedureStateBase : State
{
    protected StateMachine _stateMachine;

    public ProcedureStateBase(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }
}