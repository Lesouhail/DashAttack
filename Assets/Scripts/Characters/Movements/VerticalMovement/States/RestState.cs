using static VerticalState;

public class VerticalRestState : State<VerticalMovement, VerticalState>
{
    public VerticalRestState(VerticalMovement owner, StateMachine<VerticalMovement, VerticalState> stateMachine)
        : base(owner, stateMachine)
    {
    }

    public override VerticalState Type => Rest;

    protected override bool HasTransition()
    {
        stateMachine.TransitionTo(Grounded);
        return true;
    }
}