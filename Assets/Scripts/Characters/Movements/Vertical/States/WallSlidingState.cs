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
            if (!owner.PhysicsComponent.Collisions.Left &&
                !owner.PhysicsComponent.Collisions.Right)
            {
                stateMachine.TransitionTo(Falling);
                return true;
            }
            if (owner.PhysicsComponent.Collisions.Below)
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
}