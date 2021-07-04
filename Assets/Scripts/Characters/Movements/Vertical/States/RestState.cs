namespace DashAttack.Characters.Movements.Vertical.States
{
    using DashAttack.Utility;
    using static VerticalState;

    public class RestState : State<VerticalMovement, VerticalState>
    {
        public RestState(VerticalMovement owner, StateMachine<VerticalMovement, VerticalState> stateMachine)
            : base(owner, stateMachine)
        {
        }

        public override VerticalState Type => Rest;

        protected override bool HasTransition()
        {
            if (owner.PhysicsObject.Collisions.Below)
            {
                stateMachine.TransitionTo(Grounded);
                return true;
            }

            stateMachine.TransitionTo(Falling);
            return true;
        }
    }
}