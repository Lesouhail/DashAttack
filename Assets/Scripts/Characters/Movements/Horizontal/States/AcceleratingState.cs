﻿namespace DashAttack.Characters.Movements.Horizontal.States
{
    using DashAttack.Utility;
    using UnityEngine;

    using static HorizontalState;

    public class AcceleratingState : State<HorizontalMovement, HorizontalState>
    {
        public AcceleratingState(HorizontalMovement owner, StateMachine<HorizontalMovement, HorizontalState> stateMachine)
            : base(owner, stateMachine)
        {
        }

        public override HorizontalState Type => Accelerating;

        protected override bool HasTransition()
        {
            bool isWallStickTimeOver = owner.Inputs.WallStickBuffer >= owner.Player.WallStickTime;
            if (owner.Player.IsOnWallAirborne && !owner.IsWallJumpFrame && !isWallStickTimeOver)
            {
                stateMachine.TransitionTo(WallSticked);
                return true;
            }

            if (owner.Inputs.RunInput == 0)
            {
                stateMachine.TransitionTo(Braking);
                return true;
            }
            else if (Mathf.Sign(owner.Inputs.RunInput) != Mathf.Sign(owner.CurrentVelocity) &&
                     owner.CurrentVelocity != 0)
            {
                stateMachine.TransitionTo(Turning);
                return true;
            }

            if (Mathf.Abs(owner.CurrentVelocity) == owner.Player.MaxSpeed / Mathf.Abs(owner.Inputs.RunInput))
            {
                stateMachine.TransitionTo(AtApex);
                return true;
            }

            return false;
        }
    }
}