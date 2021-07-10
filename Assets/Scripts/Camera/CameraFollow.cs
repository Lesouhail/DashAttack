namespace DashAttack.Camera
{
    using DashAttack.Characters.Movements.Dash;
    using DashAttack.Characters.Movements.Horizontal;
    using DashAttack.Characters.Movements.Vertical;
    using DashAttack.Physics;
    using UnityEngine;

    public class CameraFollow : PhysicsObject
    {
        [SerializeField] private PhysicsObject player;
        [SerializeField] private float horizontalSmoothTime = 0.2f;
        [SerializeField] private float verticalSmoothTime = 0.2f;

        [SerializeField] private Vector2 playerFrameScale;
        [SerializeField] private bool drawDebugFrame;

        private float currentXVelocity;
        private float currentYVelocity;

        private void LateUpdate()
        {
            if (drawDebugFrame)
            {
                DebugFrame();
            }

            bool isOutOfFrameOnX = Mathf.Abs(player.transform.position.x - transform.position.x) > playerFrameScale.x;
            bool isOutOfFrameOnY = Mathf.Abs(player.transform.position.y - transform.position.y) > playerFrameScale.y;

            Vector2 deltaPosition = Vector2.zero;

            if (isOutOfFrameOnX)
            {
                var deltaX = player.transform.position.x - transform.position.x;
                var directionX = Mathf.Sign(deltaX);
                deltaPosition.x = deltaX - playerFrameScale.x * directionX;
            }
            else
            {
                var smoothX = Mathf.SmoothDamp(transform.position.x, player.transform.position.x, ref currentXVelocity, horizontalSmoothTime, Mathf.Infinity, Time.smoothDeltaTime);
                deltaPosition.x = smoothX - transform.position.x;
            }

            if (isOutOfFrameOnY)
            {
                var deltaY = player.transform.position.y - transform.position.y;
                var directionY = Mathf.Sign(deltaY);
                deltaPosition.y = deltaY - playerFrameScale.y * directionY;
            }
            else
            {
                var smoothY = Mathf.SmoothDamp(transform.position.y, player.transform.position.y, ref currentYVelocity, verticalSmoothTime, Mathf.Infinity, Time.smoothDeltaTime);
                //var smoothY = Mathf.Lerp(player.transform.position.y, transform.position.y, verticalSmoothTime * Time.deltaTime);
                deltaPosition.y = smoothY - transform.position.y;
            }

            AddMovement(deltaPosition);
        }

        protected override bool IgnoreCollisions(GameObject other)
        {
            return false;
        }

        private void DebugFrame()
        {
            var bottomLeft = new Vector2(transform.position.x - playerFrameScale.x, transform.position.y - playerFrameScale.y);
            var topLeft = new Vector2(transform.position.x - playerFrameScale.x, transform.position.y + playerFrameScale.y);
            var bottomRight = new Vector2(transform.position.x + playerFrameScale.x, transform.position.y - playerFrameScale.y);
            var topRight = new Vector2(transform.position.x + playerFrameScale.x, transform.position.y + playerFrameScale.y);

            Debug.DrawLine(bottomLeft, topLeft, Color.green);
            Debug.DrawLine(topLeft, topRight, Color.green);
            Debug.DrawLine(topRight, bottomRight, Color.green);
            Debug.DrawLine(bottomRight, bottomLeft, Color.green);
        }
    }
}