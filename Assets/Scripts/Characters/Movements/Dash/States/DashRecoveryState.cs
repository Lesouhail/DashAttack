namespace DashAttack.Characters.Movements.Dash.States
{
    using DashAttack.Utility;

    public class DashRecoveryState : State<DashMovement, DashState>
    {
        public DashRecoveryState(DashMovement owner, StateMachine<DashMovement, DashState> stateMachine) : base(owner, stateMachine)
        {
        }

        public override DashState Type => DashState.Recovering;

        protected override bool HasTransition()
        {
            if (owner.Input && !owner.LastFrameInput && owner.CanDash)
            {
                stateMachine.TransitionTo(DashState.Casting);
                return true;
            }
            if (owner.DashRecoveryCounter >= owner.RecoveryTime)
            {
                stateMachine.TransitionTo(DashState.Rest);
                return true;
            }
            return false;
        }
    }
}