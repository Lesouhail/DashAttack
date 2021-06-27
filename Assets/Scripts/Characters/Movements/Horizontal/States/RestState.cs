namespace DashAttack.Characters.Movements.Horizontal.States
{
    using UnityEngine;
    using DashAttack.Utility;
    using static HorizontalState;

    public class RestState : State<HorizontalMovement, HorizontalState>
    {
        public RestState(HorizontalMovement owner, StateMachine<HorizontalMovement, HorizontalState> stateMachine)
            : base(owner, stateMachine)
        {
        }

        public override HorizontalState Type => Rest;

        protected override bool HasTransition()
        {
            if (owner.Player.IsWallSticked && !owner.IsWallJumping)
            {
                stateMachine.TransitionTo(WallSticked);
                return true;
            }

            if (owner.Inputs.RunInput != 0)
            {
                stateMachine.TransitionTo(Accelerating);
                return true;
            }
            return false;
        }
    }
}