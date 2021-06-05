namespace DashAttack.Physics
{
    using System;
    using UnityEngine;

    [RequireComponent(typeof(BoxCollider2D))]
    public class BoxCheck : MonoBehaviour, ICollisionChecker
    {
        [SerializeField]
        private int horizontalRayCount;
        [SerializeField]
        private LayerMask collisionMask;
        [SerializeField]
        private float verticalRayCount;
        [SerializeField]
        private bool debugRays;

        public readonly float skinWidth = 0.015f;
        public Collision Collisions => collisions;
        public Func<GameObject, bool> ShouldIgnoreCollisions { get; set; }
        public event Action<GameObject> OnCollision;

        private BoxCollider2D BoxCollider;
        private float horizontalRaySpacing;
        private RaycastOrigins raycastOrigins;
        private float verticalRaySpacing;
        private Collision collisions;
        private Collision lastFrameCollisions;

        private void Start()
        {
            BoxCollider = GetComponent<BoxCollider2D>();
            CalculateRaySpacing();
        }

        public Vector2 Check(Vector2 velocity)
        {
            UpdateRaycastOrigins();
            ResetCollisionInfos();
            HorizontalCollisions(ref velocity);
            VerticalCollisions(ref velocity);
            return velocity;
        }

        private void HorizontalCollisions(ref Vector2 velocity)
        {
            int directionX = (int)Mathf.Sign(velocity.x);
            float rayLength = Mathf.Abs(velocity.x) + skinWidth;

            if (velocity.x == 0 && (lastFrameCollisions.Left || lastFrameCollisions.Right))
            {
                directionX = lastFrameCollisions.Left ? -1 : 1;
                rayLength = skinWidth * 2;
            }

            for (int i = 0; i < horizontalRayCount; i++)
            {
                Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
                rayOrigin += Vector2.up * (horizontalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

                if (debugRays) Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

                if (hit)
                {
                    if (ShouldIgnoreCollisions(hit.collider.gameObject))
                    {
                        return;
                    }

                    velocity.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;

                    collisions.Left = directionX == -1;
                    collisions.Right = directionX == 1;

                    OnCollision?.Invoke(hit.collider.gameObject);
                }
            }
        }

        private void VerticalCollisions(ref Vector2 velocity)
        {
            float directionY = Mathf.Sign(velocity.y);
            float rayLength = Mathf.Abs(velocity.y) + skinWidth;

            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
                rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

                if (debugRays) Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

                if (hit)
                {
                    if (ShouldIgnoreCollisions(hit.collider.gameObject))
                    {
                        return;
                    }

                    velocity.y = (hit.distance - skinWidth) * directionY;
                    rayLength = hit.distance;

                    collisions.Below = directionY == -1;
                    collisions.Above = directionY == 1;

                    OnCollision?.Invoke(hit.collider.gameObject);
                }
            }
        }

        private void UpdateRaycastOrigins()
        {
            Bounds bounds = BoxCollider.bounds;
            bounds.Expand(skinWidth * -2);

            raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
            raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
            raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
            raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
        }

        private void CalculateRaySpacing()
        {
            Bounds bounds = BoxCollider.bounds;
            bounds.Expand(skinWidth * -2);

            horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
            verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

            horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
            verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
        }

        private void ResetCollisionInfos()
        {
            lastFrameCollisions = Collisions;
            collisions.Above = collisions.Below = false;
            collisions.Left = collisions.Right = false;
        }

        private struct RaycastOrigins
        {
            public Vector2 topLeft, topRight;
            public Vector2 bottomLeft, bottomRight;
        }
    }
}