namespace DashAttack.Characters.Movements.Vertical.States
{
    using DashAttack.Utility;
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
            if (owner.PhysicsComponent.Collisions.Below)
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

            if (owner.PhysicsComponent.Collisions.Left ||
                owner.PhysicsComponent.Collisions.Right)
            {
                stateMachine.TransitionTo(WallSliding);
                return true;
            }
            return false;
        }
    }
}