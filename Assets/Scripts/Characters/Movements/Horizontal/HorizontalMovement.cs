namespace DashAttack.Characters.Movements.Horizontal
{
    using UnityEngine;
    using DashAttack.Physics;
    using DashAttack.Characters.Movements.Horizontal.States;

    using static HorizontalState;
    using static Utility.StateCallBack;

    [RequireComponent(typeof(PhysicsObject))]
    public class HorizontalMovement : Ability<HorizontalMovement, HorizontalState>
    {
        public float CurrentVelocity { get; private set; }

        public Player Player { get; private set; }
        public PlayerInputs Inputs { get; set; }
        public PhysicsObject PhysicsObject { get; private set; }

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
                new RestState(this, StateMachine),
                new AcceleratingState(this, StateMachine),
                new BrakingState(this, StateMachine),
                new TurningState(this, StateMachine),
                new MaxSpeedState(this, StateMachine),
                new WallStickedState(this, StateMachine));

            // --- REST STATE ---
            Subscribe(Rest, OnStateEnter, () => CurrentVelocity = 0);

            // --- ACCELERATION STATE ---
            Subscribe(Accelerating, OnStateEnter, () =>
            {
                if (PreviousState == WallSticked)
                {
                    WallJump();
                }
            });
            Subscribe(Accelerating, OnUpdate, () => Accelerate());

            // --- TURNING STATE ---
            Subscribe(Turning, OnUpdate, () => Turn());

            // --- BRAKING STATE ---
            Subscribe(Braking, OnUpdate, () => Brake());

            // --- AT APEX STATE
            Subscribe(AtApex, OnUpdate, () => Accelerate());
        }

        protected override void OnLock() => CurrentVelocity = 0;

        private void WallJump()
        {
            var direction = PhysicsObject.Collisions.Left ? 1 : -1;
            CurrentVelocity = direction * Player.TurningForce / Time.deltaTime * Player.AerialModifier * Player.JumpTime;
        }

        private void Brake()
        {
            bool isStopping = Mathf.Abs(CurrentVelocity) - Player.Decceleration < 0;

            CurrentVelocity = !isStopping
                ? CurrentVelocity - Player.Decceleration * Mathf.Sign(CurrentVelocity)
                : 0;

            PhysicsObject.AddMovement(new Vector2(CurrentVelocity * Time.deltaTime, 0));
        }

        private void Turn()
        {
            CurrentVelocity -= Player.TurningForce * Mathf.Sign(CurrentVelocity);
            PhysicsObject.AddMovement(new Vector2(CurrentVelocity * Time.deltaTime, 0));
        }

        private void Accelerate()
        {
            CurrentVelocity += Player.Acceleration * Inputs.RunInput;
            CurrentVelocity = Mathf.Clamp(CurrentVelocity, -Player.MaxSpeed, Player.MaxSpeed);
            PhysicsObject.AddMovement(new Vector2(CurrentVelocity * Time.deltaTime, 0));
        }
    }

    public enum HorizontalState
    {
        Rest,
        Accelerating,
        Braking,
        AtApex,
        Turning,
        WallSticked
    }
}