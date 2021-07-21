namespace DashAttack.Characters
{
    using DashAttack.Physics;
    using UnityEngine;

    public class Player : MonoBehaviour
    {
        [Header("Run")]
        [SerializeField] private float maxSpeed;
        [SerializeField] private float accelerationTime;
        [SerializeField] private float brakingTime;
        [SerializeField] private float turningTime;
        [SerializeField] private float airControlAmount;
        [SerializeField] private float wallStickTime;

        [Header("Jump")]
        [SerializeField] private float maxJumpHeight;
        [SerializeField] private float minJumpHeight;
        [SerializeField] private float jumpTime;
        [SerializeField] private float hangTime;
        [SerializeField] private float hangingFallMultiplier;
        [SerializeField] private float maxFallVelocity;
        [SerializeField] private float fallMultiplier;
        [SerializeField] private float wallSlideMultiplier;
        [SerializeField] private float wallClimbMultiplier;

        [Header("Dash")]
        [SerializeField] private float dashDistance;
        [SerializeField] private float castTime;
        [SerializeField] private float dashTime;
        [SerializeField] private float dashHitTime;
        [SerializeField] private float recoveryTime;

        [Header("Buffers")]
        [SerializeField] private float earlyJumpBuffer;
        [SerializeField] private float lateJumpBuffer;

        private PhysicsObject PhysicsComponent { get; set; }

        // Run Properties Getters
        public float MaxSpeed => maxSpeed;
        public float AccelerationTime => accelerationTime;
        public float BrakingTime => brakingTime;
        public float TurningTime => turningTime;
        public float AirControlAmount => airControlAmount;
        public float WallStickTime => wallStickTime;

        // Dash Properties Getters
        public float DashDistance => dashDistance;
        public float DashTime => dashTime;
        public float CastTime => castTime;
        public float DashHitTime => dashHitTime;
        public float RecoveryTime => recoveryTime;

        // Jump Properties Getters
        public float MaxJumpHeight => maxJumpHeight;
        public float MinJumpHeight => minJumpHeight;
        public float JumpTime => jumpTime;
        public float HangTime => hangTime;
        public float MaxFallVelocity => maxFallVelocity;
        public float HangingFallMultiplier => hangingFallMultiplier;
        public float FallMultiplier => fallMultiplier;
        public float WallSlideMultiplier => wallSlideMultiplier;
        public float WallClimbMultiplier => wallClimbMultiplier;

        // Buffers Getters
        public float LateJumpBuffer => lateJumpBuffer;
        public float EarlyJumpBuffer => earlyJumpBuffer;

        // Run Helpers
        public float Acceleration => maxSpeed / accelerationTime * AerialModifier * Time.deltaTime;
        public float Deceleration => maxSpeed / brakingTime * AerialModifier * Time.deltaTime;
        public float TurningForce => maxSpeed / turningTime * AerialModifier * Time.deltaTime;
        public float AerialModifier => PhysicsComponent.Collisions.Below ? 1 : airControlAmount;
        public float WallJumpHorizontalVelocity => WallJumpHoritonalDeceleration / Time.deltaTime * JumpTime;
        public bool IsOnWallAirborne
            => (PhysicsComponent.Collisions.Left || PhysicsComponent.Collisions.Right)
            && !PhysicsComponent.Collisions.Below;

        public float WallJumpHoritonalDeceleration
        {
            get
            {
                float timeAccelerating = JumpTime >= AccelerationTime
                    ? AccelerationTime
                    : JumpTime - AccelerationTime;

                float timeAtApex = JumpTime - timeAccelerating;
                timeAtApex = timeAtApex > 0 ? timeAtApex : 0;

                float distanceAccelerating = Acceleration / Time.deltaTime * Mathf.Pow(timeAccelerating, 2) / 2;
                float distanceAtApex = MaxSpeed * timeAtApex;
                float wallJumpDistance = distanceAccelerating + distanceAtApex;

                return 2 * wallJumpDistance / Mathf.Pow(JumpTime, 2) * Time.deltaTime;
            }
        }

        // Jump Helpers
        public float Gravity => 2 * MaxJumpHeight / Mathf.Pow(JumpTime, 2);
        public virtual float JumpVelocity => Gravity * JumpTime;

        // Dash Helpers
        public float TotalDashingTime => dashTime + recoveryTime;
        public float DashDecceleration => 2 * DashDistance / Mathf.Pow(TotalDashingTime, 2);
        public float DashVelocity => DashDecceleration * TotalDashingTime;

        public float GetDashDeltaPosition(float dashVelocity)
            => dashVelocity * Time.deltaTime + (DashDecceleration * Mathf.Pow(Time.deltaTime, 2) / 2);

        private void Start()
        {
            PhysicsComponent = GetComponent<PhysicsObject>();
        }
    }
}