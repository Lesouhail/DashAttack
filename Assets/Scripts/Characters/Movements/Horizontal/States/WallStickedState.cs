namespace DashAttack.Characters.Movements.Horizontal.States
{
    using DashAttack.Utility;
    using static HorizontalState;

    public class WallStickedState : State<HorizontalMovement, HorizontalState>
    {
        public WallStickedState(HorizontalMovement owner, StateMachine<HorizontalMovement, HorizontalState> stateMachine)
            : base(owner, stateMachine)
        {
        }

        public override HorizontalState Type => WallSticked;

        protected override bool HasTransition()
        {
            if (!owner.PhysicsObject.Collisions.Left &&
                !owner.PhysicsObject.Collisions.Right)
            {
                stateMachine.TransitionTo(Rest);
                return true;
            }

            if (owner.PhysicsObject.Collisions.Below)
            {
                stateMachine.TransitionTo(Rest);
                return true;
            }

            UnityEngine.Debug.Log(owner.Inputs.WallStickBuffer);
            if (owner.Inputs.WallStickBuffer >= owner.Player.WallStickTime)
            {
                stateMachine.TransitionTo(Rest);
                return true;
            }

            //if (owner.JumpInput && !owner.Player.LastFrameJumpInput)
            //{
            //    stateMachine.TransitionTo(HorizontalState.Accelerating);
            //    return true;
            //}

            return false;
        }
    }
}