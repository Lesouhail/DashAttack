namespace DashAttack.Characters.Movements.Dash.States
{
    using DashAttack.Utility;
    using static DashState;

    public class DashRecoveryState : State<DashMovement, DashState>
    {
        public DashRecoveryState(DashMovement owner, StateMachine<DashMovement, DashState> stateMachine) : base(owner, stateMachine)
        {
        }

        public override DashState Type => Recovering;

        protected override bool HasTransition()
        {
            bool input = owner.Inputs.DashInput && !owner.Inputs.LastFrameDashInput;
            if (input && owner.Inputs.CanDash)
            {
                stateMachine.TransitionTo(Casting);
                return true;
            }
            if (owner.DashRecoveryCounter >= owner.Player.RecoveryTime)
            {
                stateMachine.TransitionTo(Rest);
                return true;
            }
            if (owner.PhysicsObject.Collisions.Above ||
                owner.PhysicsObject.Collisions.Below ||
                owner.PhysicsObject.Collisions.Left ||
                owner.PhysicsObject.Collisions.Right)
            {
                stateMachine.TransitionTo(Rest);
                return true;
            }
            return false;
        }
    }
}