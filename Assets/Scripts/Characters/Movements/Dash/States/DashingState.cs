using static DashState;

public class DashingState : State<Dash, DashState>
{
    public DashingState(Dash owner, StateMachine<Dash, DashState> stateMachine) : base(owner, stateMachine)
    {
    }

    public override DashState Type => DashState.Dashing;

    protected override bool HasTransition()
    {
        if (owner.DashCounter >= owner.DashTime)
        {
            stateMachine.TransitionTo(Recovering);
            return true;
        }

        if (owner.PhysicsComponent.Collisions.Left || owner.PhysicsComponent.Collisions.Right)
        {
            stateMachine.TransitionTo(Rest);
            return true;
        }
        return false;
    }
}