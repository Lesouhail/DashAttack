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
            bool input = owner.Inputs.DashInput && !owner.Inputs.LastFrameDashInput;
            if (input && owner.Inputs.CanDash)
            {
                stateMachine.TransitionTo(DashState.Casting);
                return true;
            }
            if (owner.DashRecoveryCounter >= owner.Player.RecoveryTime)
            {
                stateMachine.TransitionTo(DashState.Rest);
                return true;
            }
            return false;
        }
    }
}