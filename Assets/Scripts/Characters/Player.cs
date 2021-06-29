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
        [SerializeField] private float maxFallVelocity;
        [SerializeField] private float fallMultiplier;
        [SerializeField] private float wallSlideMultiplier;

        [Header("Dash")]
        [SerializeField] private float dashDistance;
        [SerializeField] private float castTime;
        [SerializeField] private float dashTime;
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
        public float RecoveryTime => recoveryTime;

        // Jump Properties Getters
        public float MaxJumpHeight => maxJumpHeight;
        public float MinJumpHeight => minJumpHeight;
        public float JumpTime => jumpTime;
        public float MaxFallVelocity => maxFallVelocity;
        public float FallMultiplier => fallMultiplier;
        public float WallSlideMultiplier => wallSlideMultiplier;

        // Buffers Getters
        public float LateJumpBuffer => lateJumpBuffer;
        public float EarlyJumpBuffer => earlyJumpBuffer;

        // Run Helpers
        public float Acceleration => maxSpeed / accelerationTime * AerialModifier * Time.deltaTime;
        public float Decceleration => maxSpeed / brakingTime * AerialModifier * Time.deltaTime;
        public float TurningForce => maxSpeed / turningTime * AerialModifier * Time.deltaTime;
        public float AerialModifier => PhysicsComponent.Collisions.Below ? 1 : airControlAmount;
        public bool IsOnWallAirborne
            => (PhysicsComponent.Collisions.Left || PhysicsComponent.Collisions.Right)
            && !PhysicsComponent.Collisions.Below;

        public float GetWallJumpHorizontalVelocity()
        {
            float acceleration = MaxSpeed / AccelerationTime * AirControlAmount;
            float accelerationTime = MaxSpeed / acceleration;

            float timeAccelerating = JumpTime >= accelerationTime
                ? accelerationTime
                : JumpTime;

            float timeAtApex = JumpTime - timeAccelerating >= 0
                ? JumpTime - timeAccelerating
                : 0;

            float distanceAccelerating = acceleration * Mathf.Pow(timeAccelerating, 2) / 2;
            float distanceAtApex = timeAtApex * MaxSpeed;

            float distance = distanceAccelerating + distanceAtApex;

            float decceleration = 2 * distance / Mathf.Pow(JumpTime, 2);
            return decceleration * JumpTime;
        }

        public float GetWallJumpHorizontalDecceleration()
        {
            float velocity = GetWallJumpHorizontalVelocity();
            float turningForce = MaxSpeed / TurningTime * AerialModifier;
            float wallJumpDecceleration = (velocity / JumpTime) - turningForce;
            return wallJumpDecceleration * Time.deltaTime;
        }

        // Jump Helpers
        public float Gravity => 2 * MaxJumpHeight / Mathf.Pow(JumpTime, 2);
        public virtual float JumpVelocity => Gravity * JumpTime;

        public float GetVerticalDeltaPosition(float verticalVelocity) => (verticalVelocity * Time.deltaTime) + (Gravity * Mathf.Pow(Time.deltaTime, 2) / 2);

        // Dash Helpers
        public float TotalDashingTime => dashTime + recoveryTime;
        public float DashVelocity => DashDecceleration * TotalDashingTime;
        public float DashDecceleration => 2 * DashDistance / Mathf.Pow(TotalDashingTime, 2);

        private void Start()
        {
            PhysicsComponent = GetComponent<PhysicsObject>();
        }
    }
}