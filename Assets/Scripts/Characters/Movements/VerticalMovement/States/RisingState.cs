using UnityEngine;
using static VerticalState;

public class RisingState : State<VerticalMovement, VerticalState>
{
    public RisingState(VerticalMovement owner, StateMachine<VerticalMovement, VerticalState> stateMachine)
        : base(owner, stateMachine)
    {
    }

    public override VerticalState Type => Rising;

    protected override bool HasTransition()
    {
        if (owner.physicsComponent.Collisions.Above)
        {
            stateMachine.TransitionTo(Falling);
            return true;
        }

        if (!owner.Input &&
            (owner.transform.position.y - owner.JumpStartPosition >= owner.MinJumpHeight))
        {
            stateMachine.TransitionTo(Falling);
            return true;
        }
        else
        {
            var nextVelocity = owner.CurrentVerticalVelocity - (owner.Gravity * Time.deltaTime);
            if (nextVelocity < 0 ||
                owner.DeltaPosition(nextVelocity) < 0.00001f)
            {
                stateMachine.TransitionTo(Falling);
                return true;
            }
        }
        return false;
    }
}