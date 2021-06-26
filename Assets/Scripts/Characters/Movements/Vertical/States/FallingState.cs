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

            if (owner.Inputs.JumpInput && !owner.Inputs.LastFrameJumpInput &&
                owner.Inputs.JumpInputBuffer <= owner.Player.LateJumpBuffer &&
                (stateMachine.PreviousState == Grounded ||
                 stateMachine.PreviousState == WallSliding))
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