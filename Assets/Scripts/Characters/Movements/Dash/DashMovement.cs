namespace DashAttack.Characters.Movements.Dash
{
    using System;
    using DashAttack.Characters.Movements.Dash.States;
    using UnityEngine;

    using static DashAttack.Characters.Movements.Dash.DashState;
    using static DashAttack.Utility.StateCallBack;

    public class DashMovement : DashMovementBase<DashMovement, DashState>
    {
        public bool Input { get; set; }
        public bool LastFrameInput { get; private set; }
        public Vector2 Direction
        {
            get => direction;
            set
            {
                if (CurrentState == Casting)
                {
                    direction = value;
                }
            }
        }
        public bool CanDash { get; set; }
        public float DashCastingCounter { get; private set; }
        public float DashCounter { get; private set; }
        public float DashRecoveryCounter { get; private set; }

        public event Action<Vector2> DashStarted;
        public event Action DashEnded;

        private Vector2 direction;

        public void Reset()
        {
            CanDash = true;
        }

        protected override void Update()
        {
            base.Update();
            if (PhysicsComponent.Collisions.Below)
            {
                CanDash = true;
            }
            LastFrameInput = Input;
            LastFrameInput = Input;
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
            Subscribe(Casting, OnStateEnter, () =>
            {
                DashStarted?.Invoke(Direction);
                Cast();
            });
            Subscribe(Casting, OnUpdate, () => DashCastingCounter += Time.deltaTime);

            // --- Dashing ---
            Subscribe(Dashing, OnStateEnter, () => Initiate());
            Subscribe(Dashing, OnUpdate, () =>
            {
                DashCounter += Time.deltaTime;
                Deccelerate();
            });

            // --- Recovering ---
            Subscribe(Recovering, OnStateEnter, () => DashRecoveryCounter = 0);
            Subscribe(Recovering, OnUpdate, () =>
            {
                DashRecoveryCounter += Time.deltaTime;
                Deccelerate();
            });
            Subscribe(Recovering, OnStateExit, () => DashEnded?.Invoke());
        }

        private void Deccelerate()
        {
            CurrentVelocity -= Decceleration * Time.deltaTime;
            PhysicsComponent.AddMovement(Direction * DeltaPosition(CurrentVelocity));
        }

        private void Initiate()
        {
            DashCounter = 0;
            CurrentVelocity = InitialVelocity;
        }

        private void Cast()
        {
            CanDash = false;
            DashCastingCounter = 0;
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