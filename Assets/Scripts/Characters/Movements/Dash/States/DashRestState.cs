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
            if (owner.Input && !owner.LastFrameInput && owner.CanDash)
            {
                stateMachine.TransitionTo(DashState.Casting);
                return true;
            }

            return false;
        }
    }
}