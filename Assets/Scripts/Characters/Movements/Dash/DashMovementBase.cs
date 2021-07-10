namespace DashAttack.Characters.Movements.Dash
{
    using DashAttack.Physics;
    using System;
    using UnityEngine;

    public abstract class DashMovementBase<TChild, TStateType> : Ability<TChild, TStateType>
        where TChild : DashMovementBase<TChild, TStateType>
        where TStateType : Enum
    {
        [SerializeField] private float dashDistance;
        [SerializeField] private float dashTime;
        [SerializeField] private float castTime;
        [SerializeField] private float recoveryTime;

        public PhysicsObject PhysicsComponent { get; private set; }

        public float DashDistance => dashDistance;
        public float DashTime => dashTime;
        public float CastTime => castTime;
        public float RecoveryTime => recoveryTime;
        public float TotalDashingTime => dashTime + recoveryTime;

        public float InitialVelocity => Decceleration * TotalDashingTime;
        public float Decceleration => 2 * DashDistance / Mathf.Pow(TotalDashingTime, 2);

        public virtual float DeltaPosition(float velocity) => velocity * Time.deltaTime + (Decceleration * Mathf.Pow(Time.deltaTime, 2) / 2);

        public float CurrentVelocity { get; protected set; }
    }
}