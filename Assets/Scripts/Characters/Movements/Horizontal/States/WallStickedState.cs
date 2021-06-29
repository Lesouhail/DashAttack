namespace DashAttack.Characters.Movements.Horizontal.States
{
    using DashAttack.Utility;
    using static HorizontalState;

    public class WallStickedState : State<HorizontalMovement, HorizontalState>
    {
        public WallStickedState(HorizontalMovement owner, StateMachine<HorizontalMovement, HorizontalState> stateMachine)
            : base(owner, stateMachine)
        {
        }

        public override HorizontalState Type => WallSticked;

        public override void OnStateEnter()
        {
            base.OnStateEnter();
        }

        protected override bool HasTransition()
        {
            if (!owner.PhysicsObject.Collisions.Left &&
                !owner.PhysicsObject.Collisions.Right)
            {
                stateMachine.TransitionTo(Rest);
                return true;
            }

            if (owner.PhysicsObject.Collisions.Below)
            {
                stateMachine.TransitionTo(Rest);
                return true;
            }

            if (owner.Inputs.WallStickBuffer >= owner.Player.WallStickTime)
            {
                stateMachine.TransitionTo(Rest);
                return true;
            }

            var bufferedJumpInput = owner.Inputs.JumpInput && owner.Inputs.JumpInputBuffer <= owner.Player.EarlyJumpBuffer;
            if (bufferedJumpInput && owner.Inputs.CanWallJump)
            {
                stateMachine.TransitionTo(WallJumping);
                return true;
            }

            return false;
        }
    }
}