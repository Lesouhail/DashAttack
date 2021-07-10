namespace DashAttack.Characters.Movements.Vertical.States
{
    using DashAttack.Utility;
    using static VerticalState;

    public class HangingState : State<VerticalMovement, VerticalState>
    {
        public HangingState(VerticalMovement owner, StateMachine<VerticalMovement, VerticalState> stateMachine)
            : base(owner, stateMachine)
        {
        }

        public override VerticalState Type => Hanging;

        protected override bool HasTransition()
        {
            if (owner.HangTimeCounter >= owner.Player.HangTime && !owner.Inputs.IsInDashRecovery)
            {
                stateMachine.TransitionTo(Falling);
                return true;
            }
            return false;
        }
    }
}