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
            if (owner.PhysicsObject.Collisions.Below)
            {
                stateMachine.TransitionTo(Grounded);
                return true;
            }

            bool input = owner.Inputs.JumpInput &&
                        !owner.Inputs.LastFrameJumpInput &&
                         owner.Inputs.FallBuffer <= owner.Player.LateJumpBuffer;

            bool grounded = stateMachine.PreviousState == WallSliding ||
                            stateMachine.PreviousState == Grounded;

            if (input && grounded)
            {
                stateMachine.TransitionTo(Rising);
                return true;
            }

            if (owner.PhysicsObject.Collisions.Left ||
                owner.PhysicsObject.Collisions.Right)
            {
                stateMachine.TransitionTo(WallSliding);
                return true;
            }
            return false;
        }
    }
}