namespace DashAttack.Characters.Movements.Horizontal.States
{
    using DashAttack.Utility;

    public class WallStickedState : State<HorizontalMovement, HorizontalState>
    {
        public WallStickedState(HorizontalMovement owner, StateMachine<HorizontalMovement, HorizontalState> stateMachine)
            : base(owner, stateMachine)
        {
        }

        public override HorizontalState Type => HorizontalState.WallSticked;

        protected override bool HasTransition()
        {
            if (!owner.PhysicsComponent.Collisions.Left &&
                !owner.PhysicsComponent.Collisions.Right)
            {
                stateMachine.TransitionTo(HorizontalState.Rest);
                return true;
            }

            if (owner.PhysicsComponent.Collisions.Below)
            {
                stateMachine.TransitionTo(HorizontalState.Rest);
                return true;
            }

            if (owner.WallStickCounter >= owner.WallStickTime)
            {
                stateMachine.TransitionTo(HorizontalState.Rest);
                return true;
            }

            if (owner.JumpInput && !owner.LastFrameJumpInput)
            {
                stateMachine.TransitionTo(HorizontalState.Accelerating);
                return true;
            }

            return false;
        }
    }
}