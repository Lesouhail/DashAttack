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
        public bool IsWallJumpingFrame { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            Player = GetComponent<Player>();
            PhysicsObject = GetComponent<PhysicsObject>();
        }

        private void LateUpdate()
        {
            IsWallJumpingFrame = false;
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

            // --- REST STATE ---
            Subscribe(Rest, OnStateEnter, () => CurrentVerticalVelocity = 0);

            // --- GROUNDED STATE ---
            Subscribe(Grounded, OnUpdate, () =>
            {
                CurrentVerticalVelocity = 0;
                Fall(Player.FallMultiplier);
            });

            // --- FALING STATE ---
            Subscribe(Falling, OnStateEnter, () => CurrentVerticalVelocity = 0);
            Subscribe(Falling, OnUpdate, () => Fall(Player.FallMultiplier));

            // --- RISING ---
            Subscribe(Rising, OnStateEnter, () =>
            {
                if (PreviousState == WallSliding)
                {
                    IsWallJumpingFrame = true;
                }
                CurrentVerticalVelocity = Player.JumpVelocity;
            });
            Subscribe(Rising, OnUpdate, () => Fall(1));

            // --- WALL SLIDING STATE ---
            Subscribe(WallSliding, OnUpdate, () => WallSlide());

            // --- HANGING STATE ---
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

        protected override void OnUnlock()
        {
            if (!PhysicsObject.Collisions.Below)
            {
                StateMachine.TransitionTo(Hanging);
            }
        }

        private void Fall(float fallMultiplier)
        {
            CurrentVerticalVelocity -= Player.Gravity * Time.deltaTime * fallMultiplier;

            if (CurrentVerticalVelocity < -Player.MaxFallVelocity)
            {
                CurrentVerticalVelocity = -Player.MaxFallVelocity;
            }

            PhysicsObject.AddMovement(new Vector2(0, CurrentVerticalVelocity * Time.deltaTime));
        }

        private void WallSlide()
        {
            float gravityMod = CurrentVerticalVelocity > 0 ? Player.WallClimbMultiplier : Player.WallSlideMultiplier;
            CurrentVerticalVelocity -= Player.Gravity * gravityMod * Time.deltaTime;
            var wallSlideMaxVelocity = -Player.MaxFallVelocity * Player.WallSlideMultiplier;

            if (CurrentVerticalVelocity < wallSlideMaxVelocity)
            {
                CurrentVerticalVelocity = wallSlideMaxVelocity;
            }

            PhysicsObject.AddMovement(new Vector2(0, CurrentVerticalVelocity * Time.deltaTime));
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