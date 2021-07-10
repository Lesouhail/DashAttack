namespace DashAttack.Characters.Movements.Dash.States
{
    using DashAttack.Utility;
    using static DashState;

    public class DashRestState : State<DashMovement, DashState>
    {
        public DashRestState(DashMovement owner, StateMachine<DashMovement, DashState> stateMachine)
            : base(owner, stateMachine)
        {
        }

        public override DashState Type => Rest;

        protected override bool HasTransition()
        {
            bool input = owner.Inputs.DashInput && !owner.Inputs.LastFrameDashInput;
            if (input && owner.Inputs.CanDash)
            {
                stateMachine.TransitionTo(Casting);
                return true;
            }

            return false;
        }
    }
}