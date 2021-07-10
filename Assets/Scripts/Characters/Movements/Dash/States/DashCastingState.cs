namespace DashAttack.Characters.Movements.Dash.States
{
    using DashAttack.Utility;
    using static DashState;

    public class DashCastingState : State<DashMovement, DashState>
    {
        public DashCastingState(DashMovement owner, StateMachine<DashMovement, DashState> stateMachine) : base(owner, stateMachine)
        {
        }

        public override DashState Type => Casting;

        protected override bool HasTransition()
        {
            if (owner.DashCastingCounter >= owner.Player.CastTime)
            {
                stateMachine.TransitionTo(Dashing);
                return true;
            }
            return false;
        }
    }
}