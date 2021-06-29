namespace DashAttack.Characters.Movements.Horizontal.States
{
    using DashAttack.Utility;
    using UnityEngine;
    using static HorizontalState;

    public class TurningState : State<HorizontalMovement, HorizontalState>
    {
        public TurningState(HorizontalMovement owner, StateMachine<HorizontalMovement, HorizontalState> stateMachine)
            : base(owner, stateMachine)
        {
        }

        public override HorizontalState Type => Turning;

        protected override bool HasTransition()
        {
            if (owner.Player.IsOnWallAirborne && !owner.InWallJumpFrame)
            {
                stateMachine.TransitionTo(WallSticked);
                return true;
            }

            if (owner.Inputs.RunInput == 0)
            {
                stateMachine.TransitionTo(Braking);
                return true;
            }
            else if (Mathf.Sign(owner.Inputs.RunInput) == Mathf.Sign(owner.CurrentVelocity))
            {
                stateMachine.TransitionTo(Accelerating);
                return true;
            }
            return false;
        }
    }
}