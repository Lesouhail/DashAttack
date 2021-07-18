namespace DashAttack.Characters.Movements.Dash
{
    using DashAttack.Characters.Movements.Dash.States;
    using DashAttack.Physics;
    using System.Linq;
    using UnityEngine;

    using static DashAttack.Characters.Movements.Dash.DashState;
    using static DashAttack.Utility.StateCallBack;

    public class DashMovement : Ability<DashMovement, DashState>
    {
        [SerializeField] private int rayCount = 5;
        [SerializeField] private int angleInDegree = 20;
        [SerializeField] private LayerMask collidablesLayer;
        [SerializeField] private bool debugRays = true;

        public float CurrentVelocity { get; private set; }

        public PlayerInputs Inputs { get; set; }
        public Player Player { get; set; }
        public PhysicsObject PhysicsObject { get; set; }

        public float DashCastingCounter { get; private set; }

        internal void Subscribe(DashState dashing, object onStateEnter)
        {
            throw new System.NotImplementedException();
        }

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

            // --- Rest ---
            Subscribe(Rest, OnStateEnter, () => CurrentVelocity = 0);

            // --- Casting ---
            Subscribe(Casting, OnStateEnter, () => DashCastingCounter = 0);
            Subscribe(Casting, OnUpdate, () => DashCastingCounter += Time.deltaTime);

            // --- Dashing ---
            Subscribe(Dashing, OnStateEnter, () =>
            {
                CorrectPosition();
                Initiate();
            });
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

        private void CorrectPosition()
        {
            var angleBetweenRays = angleInDegree / (rayCount - 1);
            var baseAngle = -Vector2.SignedAngle(Vector2.up, Inputs.DashDirection);

            RaycastHit2D[] hits = new RaycastHit2D[rayCount];

            for (int i = 0; i < rayCount; i++)
            {
                var rayPosition = i - ((rayCount - 1) / 2);
                var angle = baseAngle + (rayPosition * angleBetweenRays);

                var x = Mathf.Sin(angle * Mathf.Deg2Rad);
                var y = Mathf.Cos(angle * Mathf.Deg2Rad);

                var direction = new Vector2(x, y).normalized;

                hits[i] = Physics2D.Raycast(transform.position, direction, Player.DashDistance, collidablesLayer);

                if (debugRays)
                {
                    var target = transform.position + (Vector3)direction.normalized * Player.DashDistance;
                    Debug.DrawLine(transform.position, target, Color.yellow, 1);
                }
            }

            var other = hits.FirstOrDefault(r => r.collider != null).collider;
            if (other != null)
            {
                var distance = Vector2.Distance(other.transform.position, transform.position);
                var correctedPosition = (Vector2)other.transform.position + (distance * -Inputs.DashDirection);
                transform.position = correctedPosition;
            }
        }
    }

    public enum DashState
    {
        Rest,
        Casting,
        Dashing,
        Recovering, ur
    }
}