namespace DashAttack.Characters.Movements.Vertical
{
    using System;
    using DashAttack.Physics;
    using UnityEngine;

    public abstract class VerticalMovementBase<TChild, TStateType> : Ability<TChild, TStateType>
        where TChild : VerticalMovementBase<TChild, TStateType>
        where TStateType : Enum
    {
        [SerializeField] private float maxJumpHeight;
        [SerializeField] private float minJumpHeight;
        [SerializeField] private float jumpTime;
        [SerializeField] private float maxFallVelocity;
        [SerializeField] private float fallMultiplier;

        public float MaxJumpHeight => maxJumpHeight;
        public float MinJumpHeight => minJumpHeight;
        public float JumpTime => jumpTime;
        public float MaxFallVelocity => maxFallVelocity;
        public float FallMultiplier => fallMultiplier;

        public PhysicsObjects PhysicsComponent { get; private set; }
        public virtual bool Input { get; set; }
        public float JumpStartPosition { get; protected set; }
        public float CurrentVerticalVelocity { get; protected set; }

        public virtual float Gravity => 2 * MaxJumpHeight / Mathf.Pow(JumpTime, 2);
        public virtual float JumpVelocity => Gravity * JumpTime;

        public virtual float DeltaPosition(float velocity) => (velocity * Time.deltaTime) + (Gravity * Mathf.Pow(Time.deltaTime, 2) / 2);

        protected override void Start()
        {
            base.Start();
            PhysicsComponent = GetComponent<PhysicsObjects>();
        }
    }
}