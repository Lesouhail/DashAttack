namespace DashAttack.Characters
{
    using DashAttack.Characters.Movements.Dash;
    using DashAttack.Characters.Movements.Horizontal;
    using DashAttack.Characters.Movements.Vertical;
    using DashAttack.Extensions;
    using DashAttack.Physics;
    using System;
    using UnityEngine;

    using static DashAttack.Characters.Movements.Dash.DashState;
    using static Utility.StateCallBack;

    public class PlayerController : PhysicsObject
    {
        [SerializeField] private LayerMask collidablesMask;

        private HorizontalMovement HorizontalMovement { get; set; }
        private VerticalMovement VerticalMovement { get; set; }
        private DashMovement Dash { get; set; }
        private DashAttackUltimateInputs Inputs { get; set; }
        private PlayerInputs PlayerInputs { get; set; }

        protected virtual void Awake()
        {
            Inputs = new DashAttackUltimateInputs();
            PlayerInputs = new PlayerInputs();

            HorizontalMovement = GetComponent<HorizontalMovement>();
            VerticalMovement = GetComponent<VerticalMovement>();
            Dash = GetComponent<DashMovement>();

            HorizontalMovement.Inputs = PlayerInputs;
            VerticalMovement.Inputs = PlayerInputs;
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

            HorizontalMovement.Subscribe(HorizontalState.WallSticked, OnStateEnter, () => PlayerInputs.WallStickBuffer = 0);
            HorizontalMovement.Subscribe(HorizontalState.WallSticked, OnUpdate, () =>
            {
                var wallDirection = Collisions.Left ? -1 : 1;

                // Sticks the player to wall for a short amount of time allowing him to perform easier wall jump
                var isLeavingWall = PlayerInputs.RunInput != 0 && Mathf.Sign(PlayerInputs.RunInput) != wallDirection;
                PlayerInputs.WallStickBuffer = isLeavingWall ? PlayerInputs.WallStickBuffer + Time.deltaTime : 0;
            });

            VerticalMovement.Subscribe(VerticalState.Falling, OnUpdate, () => PlayerInputs.FallBuffer += Time.deltaTime);
            VerticalMovement.Subscribe(VerticalState.Falling, OnStateExit, () => PlayerInputs.FallBuffer = 0);

            Inputs.Player.CancelJump.performed += (ctx) => PlayerInputs.JumpInputBuffer = 0;
        }

        protected override void Update()
        {
            base.Update();
            PlayerInputs.RunInput = Inputs.Player.Move.ReadValue<float>();

            PlayerInputs.JumpInputBuffer = PlayerInputs.JumpInput
                ? PlayerInputs.JumpInputBuffer + Time.deltaTime
                : 0;

            PlayerInputs.DashInput = new DashInput(
                Inputs.Player.Dash.ReadValue<bool>(),
                Inputs.Player.DashDirection.ReadValue<Vector2>());
        }

        protected virtual void OnEnable()
        {
            Inputs.Enable();
        }

        protected virtual void LateUpdate()
        {
            PlayerInputs.LastFrameRunInput = PlayerInputs.RunInput;
            PlayerInputs.LastFrameJumpInput = PlayerInputs.JumpInput;
        }

        protected override bool IgnoreCollisions(GameObject other)
        {
            //if (Dash.CurrentState == Dashing && collidablesMask.ContainsLayer(other.layer))
            //{
            //    OnDashHit(other);
            //    return true;
            //}
            return false;
        }

        private void OnDashHit(GameObject other)
        {
            if (other.TryGetComponent<ICollidable>(out var collidable))
            {
                collidable.Collide(gameObject);
                Dash.Reset();
            }
        }
    }
}