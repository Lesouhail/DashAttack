namespace DashAttack.Characters.Movements.Horizontal.States
{
    using DashAttack.Utility;
    using UnityEngine;

    using static HorizontalState;

    public class MaxSpeedState : State<HorizontalMovement, HorizontalState>
    {
        public MaxSpeedState(HorizontalMovement owner, StateMachine<HorizontalMovement, HorizontalState> stateMachine)
            : base(owner, stateMachine)
        {
        }

        public override HorizontalState Type => AtApex;

        protected override bool HasTransition()
        {
            if (owner.Player.IsWallSticked && !owner.IsWallJumping)
            {
                stateMachine.TransitionTo(WallSticked);
                return true;
            }

            if (owner.Inputs.RunInput == 0)
            {
                stateMachine.TransitionTo(Braking);
                return true;
            }
            else if (Mathf.Sign(owner.Inputs.RunInput) != Mathf.Sign(owner.CurrentVelocity))
            {
                stateMachine.TransitionTo(Turning);
                return true;
            }

            return false;
        }
    }
}