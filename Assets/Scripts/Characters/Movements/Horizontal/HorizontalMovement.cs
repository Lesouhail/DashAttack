namespace DashAttack.Characters.Movements.Horizontal
{
    using System;
    using UnityEngine;
    using DashAttack.Physics;
    using DashAttack.Utility;
    using DashAttack.Characters.Movements.Vertical;
    using DashAttack.Characters.Movements.Horizontal.States;

    using static HorizontalState;
    using static Utility.StateCallBack;

    [RequireComponent(typeof(PhysicsObjects))]
    public class HorizontalMovement : HorizontalMovementBase<HorizontalMovement, HorizontalState>
    {
        [SerializeField] private float wallStickTime;

        public float WallStickTime => wallStickTime;
        public float WallStickCounter { get; private set; }

        public bool JumpInput { get; set; }
        public bool LastFrameJumpInput { get; private set; }

        public bool IsLocked
        {
            get => StateMachine.IsLocked;
            set
            {
                if (value)
                {
                    CurrentVelocity = 0;
                }
                StateMachine.IsLocked = value;
            }
        }

        public event Action RunStarted;
        public event Action RunEnded;

        private VerticalMovement VerticalMovement { get; set; }

        private bool IsWallSticked
            => (PhysicsComponent.Collisions.Left || PhysicsComponent.Collisions.Right)
            && !PhysicsComponent.Collisions.Below;

        private bool IsWalkingOnWall
            => (PhysicsComponent.Collisions.Left && CurrentVelocity < 0)
            || (PhysicsComponent.Collisions.Right && CurrentVelocity > 1);

        public void WallJump(int direction)
        {
            //set velocity
        }

        protected override void Start()
        {
            base.Start();
            VerticalMovement = GetComponent<VerticalMovement>();
        }

        protected override void Update()
        {
            if (StateMachine.CurrentState != WallSticked && IsWallSticked)
            {
                StateMachine.TransitionTo(WallSticked);
            }

            base.Update();

            if (IsWalkingOnWall)
            {
                CurrentVelocity = 0;
            }

            LastFrameInput = Input;
            LastFrameJumpInput = JumpInput;
        }

        protected override void InitStateMachine()
        {
            StateMachine = new StateMachine<HorizontalMovement, HorizontalState>();
            StateMachine.Init(
                Rest,
                new RestState(this, StateMachine),
                new AcceleratingState(this, StateMachine),
                new BrakingState(this, StateMachine),
                new TurningState(this, StateMachine),
                new MaxSpeedState(this, StateMachine),
                new WallStickedState(this, StateMachine));

            // --- REST STATE ---
            Subscribe(Rest, OnStateEnter, () =>
            {
                CurrentVelocity = 0;
                RunEnded?.Invoke();
            });
            Subscribe(Rest, OnStateExit, () => RunStarted?.Invoke());

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

            // --- WALL STICKED STATE
            Subscribe(WallSticked, OnStateEnter, () => WallStickCounter = 0);
            Subscribe(WallSticked, OnUpdate, () => StickToWall());
        }

        private void WallJump()
        {
            var direction = PhysicsComponent.Collisions.Left ? 1 : -1;
            CurrentVelocity = direction * Decceleration / Time.deltaTime * AerialModifier * VerticalMovement.JumpTime;
        }

        private void StickToWall()
        {
            var wallDirection = PhysicsComponent.Collisions.Left ? -1 : 1;

            // Sticks the player to wall for a short amount of time allowing to him perform easier wall jump
            var isLeavingWall = Input != 0 && Mathf.Sign(Input) != wallDirection;
            WallStickCounter = isLeavingWall ? WallStickCounter + Time.deltaTime : 0;
        }

        private void Brake()
        {
            if (Mathf.Abs(CurrentVelocity) - Decceleration < 0)
            {
                CurrentVelocity = 0;
            }
            else
            {
                CurrentVelocity -= Decceleration * Mathf.Sign(CurrentVelocity);
            }
            PhysicsComponent.AddMovement(new Vector2(CurrentVelocity * Time.deltaTime, 0));
        }

        private void Turn()
        {
            CurrentVelocity -= TurningForce * Mathf.Sign(CurrentVelocity);
            PhysicsComponent.AddMovement(new Vector2(CurrentVelocity * Time.deltaTime, 0));
        }

        private void Accelerate()
        {
            CurrentVelocity += Acceleration * Input;
            if (Mathf.Abs(CurrentVelocity) > MaxSpeed)
            {
                CurrentVelocity = MaxSpeed * Mathf.Sign(Input);
            }
            PhysicsComponent.AddMovement(new Vector2(CurrentVelocity * Time.deltaTime, 0));
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