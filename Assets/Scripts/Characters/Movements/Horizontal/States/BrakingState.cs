namespace DashAttack.Characters.Movements.Horizontal.States
{
    using DashAttack.Utility;
    using UnityEngine;

    using static HorizontalState;

    public class BrakingState : State<HorizontalMovement, HorizontalState>
    {
        public BrakingState(HorizontalMovement owner, StateMachine<HorizontalMovement, HorizontalState> stateMachine)
            : base(owner, stateMachine)
        {
        }

        public override HorizontalState Type => Braking;

        protected override bool HasTransition()
        {
            if (owner.Player.IsOnWallAirborne && !owner.InWallJumpFrame)
            {
                stateMachine.TransitionTo(WallSticked);
                return true;
            }

            if (owner.CurrentVelocity == 0)
            {
                stateMachine.TransitionTo(Rest);
                return true;
            }

            if (owner.Inputs.RunInput != 0)
            {
                HorizontalState nextState = Mathf.Sign(owner.CurrentVelocity) == Mathf.Sign(owner.Inputs.RunInput)
                                          ? Accelerating
                                          : Turning;

                stateMachine.TransitionTo(nextState);
                return true;
            }
            return false;
        }
    }
}