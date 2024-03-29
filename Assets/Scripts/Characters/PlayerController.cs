﻿namespace DashAttack.Characters
{
    using DashAttack.Characters.Movements.Dash;
    using DashAttack.Characters.Movements.Horizontal;
    using DashAttack.Characters.Movements.Vertical;
    using DashAttack.Extensions;
    using DashAttack.Physics;
    using UnityEngine;

    using static Utility.StateCallBack;

    public class PlayerController : PhysicsObject
    {
        [SerializeField] private LayerMask collidablesMask;

        private HorizontalMovement HorizontalMovement { get; set; }
        private VerticalMovement VerticalMovement { get; set; }
        private DashMovement Dash { get; set; }
        private PlayerFeedbacks Feedbacks { get; set; }
        private DashAttackUltimateInputs Inputs { get; set; }
        private PlayerInputs PlayerInputs { get; set; }
        private BoxCollider2D DashCollider { get; set; }

        protected virtual void Awake()
        {
            Inputs = new DashAttackUltimateInputs();
            PlayerInputs = new PlayerInputs();

            HorizontalMovement = GetComponent<HorizontalMovement>();
            VerticalMovement = GetComponent<VerticalMovement>();
            Dash = GetComponent<DashMovement>();

            Feedbacks = GetComponentInChildren<PlayerFeedbacks>();

            HorizontalMovement.Inputs = PlayerInputs;
            VerticalMovement.Inputs = PlayerInputs;
            Dash.Inputs = PlayerInputs;

            Feedbacks.Inputs = PlayerInputs;
        }

        protected override void Start()
        {
            base.Start();
            InitializeInputs();
        }

        private void InitializeInputs()
        {
            Inputs.Player.Jump.performed += _ => PlayerInputs.JumpInput = true;
            Inputs.Player.CancelJump.performed += _ => PlayerInputs.JumpInput = false;

            Inputs.Player.Dash.performed += _ => PlayerInputs.DashInput = true;
            Inputs.Player.Dash.canceled += _ => PlayerInputs.DashInput = false;

            Inputs.Player.CancelJump.performed += (ctx) =>
            {
                PlayerInputs.CanWallJump = true;
                PlayerInputs.JumpInputBuffer = 0;
            };

            HorizontalMovement.Subscribe(HorizontalState.WallSticked, OnStateEnter, () => PlayerInputs.WallStickBuffer = 0);
            HorizontalMovement.Subscribe(HorizontalState.WallSticked, OnUpdate, () =>
            {
                var wallDirection = Collisions.Left ? -1 : 1;

                // Sticks the player to wall for a short amount of time allowing him to perform easier wall jump
                var isLeavingWall = PlayerInputs.RunInput != 0 && Mathf.Sign(PlayerInputs.RunInput) != wallDirection;
                PlayerInputs.WallStickBuffer = isLeavingWall ? PlayerInputs.WallStickBuffer + Time.deltaTime : 0;
            });

            VerticalMovement.Subscribe(VerticalState.Grounded, OnStateEnter, () =>
            {
                PlayerInputs.CanWallJump = false;
                PlayerInputs.CanDash = true;
            });
            VerticalMovement.Subscribe(VerticalState.Rising, OnStateEnter, () =>
            {
                PlayerInputs.CanInAirJump = false;
                PlayerInputs.IsInDashRecovery = false;
            });
            VerticalMovement.Subscribe(VerticalState.WallSliding, OnStateEnter, () => PlayerInputs.CanInAirJump = false);
            VerticalMovement.Subscribe(VerticalState.Falling, OnUpdate, () => PlayerInputs.FallBuffer += Time.deltaTime);
            VerticalMovement.Subscribe(VerticalState.Falling, OnStateExit, () => PlayerInputs.FallBuffer = 0);

            Dash.Subscribe(DashState.Rest, OnStateEnter, () =>
            {
                HorizontalMovement.IsLocked = false;
                VerticalMovement.IsLocked = false;
            });
            Dash.Subscribe(DashState.Rest, OnStateExit, () => PlayerInputs.CanDash = false);
            Dash.Subscribe(DashState.Casting, OnStateEnter, () =>
            {
                HorizontalMovement.IsLocked = true;
                VerticalMovement.IsLocked = true;
            });
            Dash.Subscribe(DashState.Casting, OnUpdate, () => PlayerInputs.DashDirection = Inputs.Player.DashDirection.ReadValue<Vector2>());
            Dash.Subscribe(DashState.Recovering, OnStateEnter, () =>
            {
                VerticalMovement.IsLocked = false;
                HorizontalMovement.IsLocked = false;
                PlayerInputs.IsInDashRecovery = true;
                PlayerInputs.CanInAirJump = true;
            });
            Dash.Subscribe(DashState.Recovering, OnStateExit, () => PlayerInputs.IsInDashRecovery = false);
        }

        protected override void Update()
        {
            base.Update();
            PlayerInputs.RunInput = Inputs.Player.Move.ReadValue<float>();

            PlayerInputs.JumpInputBuffer = PlayerInputs.JumpInput
                ? PlayerInputs.JumpInputBuffer + Time.deltaTime
                : 0;
        }

        protected virtual void OnEnable()
        {
            Inputs.Enable();
        }

        protected virtual void LateUpdate()
        {
            PlayerInputs.LastFrameRunInput = PlayerInputs.RunInput;
            PlayerInputs.LastFrameJumpInput = PlayerInputs.JumpInput;
            PlayerInputs.LastFrameDashInput = PlayerInputs.DashInput;
        }

        protected override bool IgnoreCollisions(GameObject other)
        {
            return collidablesMask.ContainsLayer(other.layer) && Dash.CurrentState == DashState.Dashing;
        }
    }
}