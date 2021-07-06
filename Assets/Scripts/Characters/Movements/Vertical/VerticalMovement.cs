namespace DashAttack.Characters.Movements.Vertical
{
    using System;
    using UnityEngine;
    using DashAttack.Physics;

    using static DashAttack.Characters.Movements.Vertical.VerticalState;
    using static DashAttack.Utility.StateCallBack;
    using DashAttack.Characters.Movements.Vertical.States;

    [RequireComponent(typeof(PhysicsObject))]
    public sealed class VerticalMovement : Ability<VerticalMovement, VerticalState>
    {
        public Player Player { get; private set; }
        public PhysicsObject PhysicsObject { get; private set; }
        public PlayerInputs Inputs { get; set; }

        public float HangTimeCounter { get; private set; }
        public float CurrentVerticalVelocity { get; private set; }

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
                new GroundedState(this, StateMachine),
                new RisingState(this, StateMachine),
                new FallingState(this, StateMachine),
                new WallSlidingState(this, StateMachine),
                new HangingState(this, StateMachine));

            // --- GROUNDED STATE ---
            Subscribe(Grounded, OnUpdate, () =>
            {
                CurrentVerticalVelocity = 0;
                Fall(Player.FallMultiplier);
            });

            // --- FALING STATE ---
            Subscribe(Falling, OnUpdate, () => Fall(Player.FallMultiplier));

            // --- RISING ---
            Subscribe(Rising, OnStateEnter, () => CurrentVerticalVelocity = Player.JumpVelocity);
            Subscribe(Rising, OnUpdate, () => Fall(1));
            Subscribe(Rising, OnStateExit, () => CurrentVerticalVelocity = 0);

            // --- WALL SLIDING STATE ---
            Subscribe(WallSliding, OnUpdate, () => WallSlide());
            Subscribe(WallSliding, OnStateExit, () => CurrentVerticalVelocity = 0);

            // --- HANGING STATE ---
            Subscribe(Hanging, OnStateEnter, () => CurrentVerticalVelocity = 0);
            Subscribe(Hanging, OnUpdate, () =>
            {
                HangTimeCounter += Time.deltaTime;
                Fall(Player.HangingFallMultiplier);
            });
            Subscribe(Hanging, OnStateExit, () => HangTimeCounter = 0);
        }

        protected override void OnLock()
        {
            CurrentVerticalVelocity = 0;
        }

        private void Fall(float fallMultiplier)
        {
            CurrentVerticalVelocity -= Player.Gravity * Time.deltaTime * fallMultiplier;

            if (CurrentVerticalVelocity < -Player.MaxFallVelocity)
            {
                CurrentVerticalVelocity = -Player.MaxFallVelocity;
            }
            PhysicsObject.AddMovement(new Vector2(0, Player.GetVerticalDeltaPosition(CurrentVerticalVelocity)));
        }

        private void WallSlide()
        {
            CurrentVerticalVelocity -= Player.Gravity * Player.WallSlideMultiplier * Time.deltaTime;
            var wallSlideMaxVelocity = -Player.MaxFallVelocity * Player.WallSlideMultiplier;
            if (CurrentVerticalVelocity < wallSlideMaxVelocity)
            {
                CurrentVerticalVelocity = wallSlideMaxVelocity;
            }
            PhysicsObject.AddMovement(new Vector2(0, Player.GetVerticalDeltaPosition(CurrentVerticalVelocity)));
        }
    }

    public enum VerticalState
    {
        Rest,
        Falling,
        Grounded,
        Rising,
        WallSliding,
        Hanging
    }
}