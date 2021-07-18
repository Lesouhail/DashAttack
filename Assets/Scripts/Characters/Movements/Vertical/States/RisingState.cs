namespace DashAttack.Characters.Movements.Vertical.States
{
    using UnityEngine;
    using DashAttack.Utility;
    using static VerticalState;

    public class RisingState : State<VerticalMovement, VerticalState>
    {
        public RisingState(VerticalMovement owner, StateMachine<VerticalMovement, VerticalState> stateMachine)
            : base(owner, stateMachine)
        {
        }

        public override VerticalState Type => Rising;

        protected override bool HasTransition()
        {
            if (owner.PhysicsObject.Collisions.Above)
            {
                stateMachine.TransitionTo(Falling);
                return true;
            }

            if (owner.Player.IsOnWallAirborne && !owner.IsWallJumpingFrame)
            {
                stateMachine.TransitionTo(WallSliding);
                return true;
            }

            if (!owner.Inputs.JumpInput)
            {
                stateMachine.TransitionTo(Falling);
                return true;
            }
            else
            {
                var nextVelocity = owner.CurrentVerticalVelocity - (owner.Player.Gravity * Time.deltaTime);
                if (nextVelocity < 0.000_01f)
                {
                    stateMachine.TransitionTo(Hanging);
                    return true;
                }
            }
            return false;
        }
    }
}