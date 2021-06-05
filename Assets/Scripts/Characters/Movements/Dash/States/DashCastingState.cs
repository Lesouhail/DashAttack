using static DashState;

public class DashCastingState : State<Dash, DashState>
{
    public DashCastingState(Dash owner, StateMachine<Dash, DashState> stateMachine) : base(owner, stateMachine)
    {
    }

    public override DashState Type => Casting;

    protected override bool HasTransition()
    {
        if (owner.DashCastingCounter >= owner.CastTime)
        {
            stateMachine.TransitionTo(Dashing);
            return true;
        }
        return false;
    }
}