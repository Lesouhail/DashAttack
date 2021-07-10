namespace DashAttack.Camera
{
    using DashAttack.Physics;
    using System;
    using UnityEngine;

    [RequireComponent(typeof(BoxCollider2D))]
    public class CameraCollider : MonoBehaviour, ICollisionChecker
    {
        [SerializeField] private LayerMask cameraLayer;
        [SerializeField] private bool debugRays;

        public DashAttack.Physics.Collision Collisions => collisions;

        public Func<GameObject, bool> ShouldIgnoreCollisions { get; set; }

        public event Action<GameObject> OnCollision;
        private BoxCollider2D Collider { get; set; }
        private const float skinWidth = 0.015f;
        private DashAttack.Physics.Collision collisions;

        private void Start()
        {
            Collider = GetComponent<BoxCollider2D>();
        }

        public Vector2 Check(Vector2 velocity)
        {
            ResetCollisions();
            return new Vector2(
                HorizontalCollisions(velocity),
                VerticalCollisions(velocity));
        }

        private float HorizontalCollisions(Vector2 velocity)
        {
            var direction = Mathf.Sign(velocity.x);
            var distance = Mathf.Abs(velocity.x - skinWidth);

            var x = direction < 0
                  ? Collider.bounds.min.x + skinWidth
                  : Collider.bounds.max.x - skinWidth;

            Vector2 rayOrigin = new Vector2(x, Collider.bounds.center.y);

            if (debugRays)
            {
                Debug.DrawRay(rayOrigin, direction * distance * Vector2.right, Color.green);
            }

            var hit = Physics2D.Raycast(rayOrigin, Vector2.right * direction, distance, cameraLayer);

            if (hit)
            {
                if (ShouldIgnoreCollisions(hit.collider.gameObject))
                {
                    return velocity.x;
                }

                velocity.x = (hit.distance - skinWidth) * direction;

                collisions.Left = direction == -1;
                collisions.Right = direction == 1;

                OnCollision?.Invoke(hit.collider.gameObject);
            }
            return velocity.x;
        }

        private float VerticalCollisions(Vector2 velocity)
        {
            var direction = Mathf.Sign(velocity.y);
            var distance = Mathf.Abs(velocity.y - skinWidth);

            var y = direction < 0
                  ? Collider.bounds.min.y + skinWidth
                  : Collider.bounds.max.y - skinWidth;

            Vector2 rayOrigin = new Vector2(Collider.bounds.center.x, y);

            var hit = Physics2D.Raycast(rayOrigin, Vector2.up * direction, distance, cameraLayer);

            if (debugRays)
            {
                Debug.DrawRay(rayOrigin, direction * distance * Vector2.up, Color.green);
            }

            if (hit)
            {
                if (ShouldIgnoreCollisions(hit.collider.gameObject))
                {
                    return velocity.y;
                }

                velocity.y = (hit.distance - skinWidth) * direction;

                collisions.Below = direction == -1;
                collisions.Above = direction == 1;

                OnCollision?.Invoke(hit.collider.gameObject);
            }
            return velocity.y;
        }

        private void ResetCollisions()
        {
            collisions.Left = collisions.Right = false;
            collisions.Above = collisions.Below = false;
        }
    }
}