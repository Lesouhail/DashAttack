namespace DashAttack.Characters.Movements.Dash.States
{
    using DashAttack.Utility;
    using static DashState;

    public class DashingState : State<DashMovement, DashState>
    {
        public DashingState(DashMovement owner, StateMachine<DashMovement, DashState> stateMachine) : base(owner, stateMachine)
        {
        }

        public override DashState Type => DashState.Dashing;

        protected override bool HasTransition()
        {
            if (owner.DashCounter >= owner.Player.DashTime)
            {
                stateMachine.TransitionTo(Recovering);
                return true;
            }

            if (owner.PhysicsObject.Collisions.Left || owner.PhysicsObject.Collisions.Right)
            {
                stateMachine.TransitionTo(Rest);
                return true;
            }
            return false;
        }
    }
}