namespace DashAttack.Characters.Movements.Dash.States
{
    using DashAttack.Utility;

    public class DashRestState : State<DashMovement, DashState>
    {
        public DashRestState(DashMovement owner, StateMachine<DashMovement, DashState> stateMachine)
            : base(owner, stateMachine)
        {
        }

        public override DashState Type => DashState.Rest;

        protected override bool HasTransition()
        {
            bool input = owner.Inputs.DashInput && !owner.Inputs.LastFrameDashInput;
            if (input && owner.Inputs.CanDash)
            {
                stateMachine.TransitionTo(DashState.Casting);
                return true;
            }

            return false;
        }
    }
}