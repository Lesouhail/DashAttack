namespace DashAttack.Characters.Movements.Dash.States
{
    using DashAttack.Utility;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using static DashState;

    public class DashHitState : State<DashMovement, DashState>
    {
        public DashHitState(DashMovement owner, StateMachine<DashMovement, DashState> stateMachine)
            : base(owner, stateMachine)
        {
        }

        public override DashState Type => Hit;

        protected override bool HasTransition()
        {
            if (owner.CurrentVelocity < 0)
            {
                stateMachine.TransitionTo(Recovering);
                return true;
            }

            if (owner.DashHitCounter >= owner.Player.DashHitTime)
            {
                stateMachine.TransitionTo(Dashing);
                return true;
            }

            return false;
        }
    }
}