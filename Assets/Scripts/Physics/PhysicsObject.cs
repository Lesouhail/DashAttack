using UnityEngine;

namespace DashAttack.Physics
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class PhysicsObject : MonoBehaviour
    {
        [SerializeField]
        private float maxHorizontalVelocity = 10;
        [SerializeField]
        private float maxVerticalVelocity = 10;
        [SerializeField]
        private ICollisionChecker checker;

        public Collision Collisions => checker.Collisions;
        public Collision LastFrameCollisions { get; private set; }
        public Vector2 Velocity { get; private set; }

        private Vector2 deltaPosition;
        // Make sure velocity does not exceeds maximum values
        private Vector2 DeltaPosition
        {
            get => deltaPosition;
            set
            {
                deltaPosition.x = Mathf.Clamp(value.x, -maxHorizontalVelocity, maxHorizontalVelocity);
                deltaPosition.y = Mathf.Clamp(value.y, -maxVerticalVelocity, maxVerticalVelocity);
            }
        }

        protected virtual void Start()
        {
            checker = GetComponent<ICollisionChecker>();
            checker.OnCollision += (other) => CollisionEntered(other);
            checker.ShouldIgnoreCollisions = (other) => IgnoreCollisions(other);
        }

        // Gatter all the movement request and add them to velocity
        public void AddMovement(Vector2 movement) => DeltaPosition += movement;

        private void CollisionCheck()
        {
            if (DeltaPosition != Vector2.zero)
            {
                DeltaPosition = checker.Check(DeltaPosition);
            }
        }

        // Move the body and reset movementrequest and velocity
        private void Move()
        {
            transform.Translate(deltaPosition);
            //rb.MovePosition(rb.position + deltaPosition);
            Velocity = DeltaPosition;
            DeltaPosition = Vector2.zero;
        }

        protected virtual void Update()
        {
            LastFrameCollisions = Collisions.Clone();
            CollisionCheck();
            Move();
        }

        protected virtual void CollisionEntered(GameObject other)
        {
        }

        protected virtual bool IgnoreCollisions(GameObject other) => false;
    }
}