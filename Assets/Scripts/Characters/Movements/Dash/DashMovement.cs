namespace DashAttack.Characters.Movements.Dash
{
    using DashAttack.Characters.Movements.Dash.States;
    using DashAttack.Physics;
    using UnityEngine;

    using static DashAttack.Characters.Movements.Dash.DashState;
    using static DashAttack.Utility.StateCallBack;

    public class DashMovement : Ability<DashMovement, DashState>
    {
        public float CurrentVelocity { get; private set; }

        public PlayerInputs Inputs { get; set; }
        public Player Player { get; set; }
        public PhysicsObject PhysicsObject { get; set; }

        public float DashCastingCounter { get; private set; }
        public float DashCounter { get; private set; }
        public float DashRecoveryCounter { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            Player = GetComponent<Player>();
            PhysicsObject = GetComponent<PhysicsObject>();
        }

        protected override void InitStateMachine()
        {
            StateMachine.Init(
                Rest,
                new DashRestState(this, StateMachine),
                new DashCastingState(this, StateMachine),
                new DashingState(this, StateMachine),
                new DashRecoveryState(this, StateMachine));

            // --- Casting ---
            Subscribe(Casting, OnStateEnter, () => DashCastingCounter = 0);
            Subscribe(Casting, OnUpdate, () => DashCastingCounter += Time.deltaTime);

            // --- Dashing ---
            Subscribe(Dashing, OnStateEnter, () => Initiate());
            Subscribe(Dashing, OnUpdate, () =>
            {
                DashCounter += Time.deltaTime;
                Decelerate();
            });

            // --- Recovering ---
            Subscribe(Recovering, OnStateEnter, () => DashRecoveryCounter = 0);
            Subscribe(Recovering, OnUpdate, () =>
            {
                DashRecoveryCounter += Time.deltaTime;
                Decelerate();
            });
        }

        private void Decelerate()
        {
            CurrentVelocity -= Player.DashDecceleration * Time.deltaTime;
            PhysicsObject.AddMovement(Inputs.DashDirection * Player.GetDashDeltaPosition(CurrentVelocity));
        }

        private void Initiate()
        {
            DashCounter = 0;
            CurrentVelocity = Player.DashVelocity;
        }
    }

    public enum DashState
    {
        Rest,
        Casting,
        Dashing,
        Recovering,
    }
}