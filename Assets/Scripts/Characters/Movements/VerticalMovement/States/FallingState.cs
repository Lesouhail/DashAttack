using static VerticalState;

public class FallingState : State<VerticalMovement, VerticalState>
{
    public FallingState(VerticalMovement owner, StateMachine<VerticalMovement, VerticalState> stateMachine)
        : base(owner, stateMachine)
    {
    }

    public override VerticalState Type => Falling;

    protected override bool HasTransition()
    {
        if (owner.physicsComponent.Collisions.Below)
        {
            stateMachine.TransitionTo(Grounded);
            return true;
        }

        if (owner.Input && !owner.LastFrameInput &&
            owner.LateJumpCounter <= owner.JumpLateBuffer &&
            (stateMachine.PreviousState == Grounded ||
             stateMachine.PreviousState == WallSliding))
        {
            stateMachine.TransitionTo(Rising);
            return true;
        }

        if (owner.physicsComponent.Collisions.Left ||
            owner.physicsComponent.Collisions.Right)
        {
            stateMachine.TransitionTo(WallSliding);
            return true;
        }
        return false;
    }
}