using static VerticalState;

public class WallSlidingState : State<VerticalMovement, VerticalState>
{
    public WallSlidingState(VerticalMovement owner, StateMachine<VerticalMovement, VerticalState> stateMachine)
        : base(owner, stateMachine)
    {
    }

    public override VerticalState Type => WallSliding;

    protected override bool HasTransition()
    {
        if (!owner.physicsComponent.Collisions.Left &&
            !owner.physicsComponent.Collisions.Right)
        {
            stateMachine.TransitionTo(Falling);
            return true;
        }
        if (owner.physicsComponent.Collisions.Below)
        {
            stateMachine.TransitionTo(Grounded);
            return true;
        }
        if (owner.Input && !owner.LastFrameInput)
        {
            stateMachine.TransitionTo(Rising);
            return true;
        }
        return false;
    }
}