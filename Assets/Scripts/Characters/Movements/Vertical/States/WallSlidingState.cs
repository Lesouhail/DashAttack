namespace DashAttack.Characters.Movements.Vertical.States
{
    using DashAttack.Utility;
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
            if (!owner.PhysicsObject.Collisions.Left &&
                !owner.PhysicsObject.Collisions.Right)
            {
                stateMachine.TransitionTo(Falling);
                return true;
            }
            if (owner.PhysicsObject.Collisions.Below)
            {
                stateMachine.TransitionTo(Grounded);
                return true;
            }
            if (owner.Inputs.JumpInput && owner.Inputs.JumpInputBuffer <= owner.Player.EarlyJumpBuffer)
            {
                stateMachine.TransitionTo(Rising);
                return true;
            }
            return false;
        }
    }
}