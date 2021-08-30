namespace DashAttack.Characters.Movements.Dash
{
    using System.Linq;
    using DashAttack.Characters.Movements.Dash.States;
    using DashAttack.Physics;
    using UnityEngine;
    using static DashAttack.Characters.Movements.Dash.DashState;
    using static DashAttack.Utility.StateCallBack;

    public class DashMovement : Ability<DashMovement, DashState>
    {
        [SerializeField] private int rayCount = 5;
        [SerializeField] private int angleInDegree = 20;
        [SerializeField] private float maxCorrectionDistance;
        [SerializeField] private LayerMask collidablesLayer;
        [SerializeField] private bool debugRays = true;
        [SerializeField] private float boxRadius = 1;

        public float CurrentVelocity { get; private set; }

        public PlayerInputs Inputs { get; set; }
        public Player Player { get; set; }
        public PhysicsObject PhysicsObject { get; set; }

        public float DashCastingCounter { get; private set; }
        public float DashCounter { get; private set; }
        public float DashRecoveryCounter { get; private set; }
        public float DashHitCounter { get; private set; }

        private Vector2 CorrectedDashDirection { get; set; }
        private Collider2D HittedObject { get; set; }

        public bool HasHit => HittedObject != null;

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
                new DashRecoveryState(this, StateMachine),
                new DashHitState(this, StateMachine));

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
                HittedObject = GetHittedObject();
                Decelerate();
            });

            // --- Hit ---
            Subscribe(Hit, OnStateEnter, () =>
            {
                DashHitCounter = 0;
                bool hasHit = HittedObject.gameObject.TryGetComponent<ICollidable>(out var collidable);
                collidable?.Collide(gameObject);
                if (hasHit)
                {
                    Inputs.CanDash = true;
                }
            });
            Subscribe(Hit, OnUpdate, () =>
            {
                DashHitCounter += Time.deltaTime;
                Decelerate();
            });
            Subscribe(Hit, OnStateExit, () => HittedObject = null);

            // --- Recovering ---
            Subscribe(Recovering, OnStateEnter, () => DashRecoveryCounter = 0);
            Subscribe(Recovering, OnUpdate, () =>
            {
                DashRecoveryCounter += Time.deltaTime;
                Decelerate();
            });
        }

        private Collider2D GetHittedObject()
            => Physics2D.OverlapBox(
                point: transform.position,
                size: new Vector2(boxRadius, boxRadius),
                angle: 0f,
                layerMask: collidablesLayer);

        private void Decelerate()
        {
            CurrentVelocity -= Player.DashDecceleration * Time.deltaTime;
            PhysicsObject.AddMovement(Inputs.DashDirection * Player.GetDashDeltaPosition(CurrentVelocity));
        }

        private void Initiate()
        {
            if (PreviousState != Hit)
            {
                DashCounter = 0;
                CurrentVelocity = Player.DashVelocity;
            }
        }

        private void CorrectPosition()
        {
            Debug.Log(Inputs.DashDirection);

            var other = Physics2D.BoxCast(
                origin: transform.position,
                size: new Vector2(boxRadius, boxRadius),
                angle: 0,
                direction: Inputs.DashDirection,
                distance: Player.DashDistance,
                layerMask: collidablesLayer);

            if (other)
            {
                var distance = Vector2.Distance(other.collider.transform.position, transform.position);
                var perfectPosition = (Vector2)other.collider.transform.position + (distance * -Inputs.DashDirection);

                var correctedPosition = Vector2.MoveTowards(transform.position, perfectPosition, maxCorrectionDistance);

                if (debugRays)
                {
                    Debug.DrawLine(other.collider.transform.position, correctedPosition, Color.red, 1);
                    Debug.DrawLine(other.collider.transform.position, perfectPosition, Color.blue, 1);
                }

                CorrectedDashDirection = (other.collider.transform.position - transform.position).normalized;
                transform.position = correctedPosition;
            }
        }

        private void OnDrawGizmosSelected()
        {
            var bottomLeft = new Vector2(transform.position.x - boxRadius / 2, transform.position.y - boxRadius / 2);
            var topLeft = new Vector2(transform.position.x - boxRadius / 2, transform.position.y + boxRadius / 2);
            var bottomRight = new Vector2(transform.position.x + boxRadius / 2, transform.position.y - boxRadius / 2);
            var topRight = new Vector2(transform.position.x + boxRadius / 2, transform.position.y + boxRadius / 2);

            Debug.DrawLine(bottomLeft, topLeft, Color.cyan);
            Debug.DrawLine(topLeft, topRight, Color.cyan);
            Debug.DrawLine(topRight, bottomRight, Color.cyan);
            Debug.DrawLine(bottomRight, bottomLeft, Color.cyan);
        }
    }

    public enum DashState
    {
        Rest,
        Casting,
        Dashing,
        Recovering,
        Hit
    }
}