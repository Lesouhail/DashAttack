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
            if (!owner.PhysicsComponent.Collisions.Below)
            {
                stateMachine.TransitionTo(Falling);
                return true;
            }

            if (owner.Input && owner.EarlyJumpCounter <= owner.JumpEarlyBuffer)
            {
                stateMachine.TransitionTo(Rising);
                return true;
            }

            return false;
        }
    }
}