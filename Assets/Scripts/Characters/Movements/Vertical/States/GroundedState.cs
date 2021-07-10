namespace DashAttack.Characters.Movements.Vertical.States
{
    using DashAttack.Utility;
    using static VerticalState;

    public class GroundedState : State<VerticalMovement, VerticalState>
    {
        public GroundedState(VerticalMovement owner, StateMachine<VerticalMovement, VerticalState> stateMachine)
            : base(owner, stateMachine)
        {
        }

        public override VerticalState Type => Grounded;

        protected override bool HasTransition()
        {
            if (!owner.PhysicsObject.Collisions.Below)
            {
                stateMachine.TransitionTo(Falling);
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