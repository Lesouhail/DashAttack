namespace DashAttack.Characters.Movements.Horizontal.States
{
    using DashAttack.Utility;
    using UnityEngine;

    public class TurningState : State<HorizontalMovement, HorizontalState>
    {
        public TurningState(HorizontalMovement owner, StateMachine<HorizontalMovement, HorizontalState> stateMachine)
            : base(owner, stateMachine)
        {
        }

        public override HorizontalState Type => HorizontalState.Turning;

        protected override bool HasTransition()
        {
            if (owner.Input == 0)
            {
                stateMachine.TransitionTo(HorizontalState.Braking);
                return true;
            }
            else if (Mathf.Sign(owner.Input) == Mathf.Sign(owner.CurrentVelocity))
            {
                stateMachine.TransitionTo(HorizontalState.Accelerating);
                return true;
            }
            return false;
        }
    }
}