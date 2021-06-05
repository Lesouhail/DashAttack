﻿namespace DashAttack.Characters.Movements.Horizontal.States
{
    using DashAttack.Utility;

    public class RestState : State<HorizontalMovement, HorizontalState>
    {
        public RestState(HorizontalMovement owner, StateMachine<HorizontalMovement, HorizontalState> stateMachine)
            : base(owner, stateMachine)
        {
        }

        public override HorizontalState Type => HorizontalState.Rest;

        protected override bool HasTransition()
        {
            if (owner.Input != 0)
            {
                stateMachine.TransitionTo(HorizontalState.Accelerating);
                return true;
            }

            return false;
        }
    }
}