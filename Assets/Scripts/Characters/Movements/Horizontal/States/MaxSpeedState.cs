namespace DashAttack.Characters.Movements.Horizontal.States
{
    using DashAttack.Utility;
    using UnityEngine;

    public class MaxSpeedState : State<HorizontalMovement, HorizontalState>
    {
        public MaxSpeedState(HorizontalMovement owner, StateMachine<HorizontalMovement, HorizontalState> stateMachine)
            : base(owner, stateMachine)
        {
        }

        public override HorizontalState Type => HorizontalState.AtApex;

        protected override bool HasTransition()
        {
            if (owner.Input == 0)
            {
                stateMachine.TransitionTo(HorizontalState.Braking);
                return true;
            }
            else if (Mathf.Sign(owner.Input) != Mathf.Sign(owner.CurrentVelocity))
            {
                stateMachine.TransitionTo(HorizontalState.Turning);
                return true;
            }

            return false;
        }
    }
}