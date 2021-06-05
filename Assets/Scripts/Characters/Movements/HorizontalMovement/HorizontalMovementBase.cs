using System;
using UnityEngine;

public abstract class HorizontalMovementBase<TChild, TStateType> : Ability<TChild, TStateType>
    where TChild : HorizontalMovementBase<TChild, TStateType>
    where TStateType : Enum
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private float accelerationTime;
    [SerializeField] private float brakingTime;
    [SerializeField] private float turningTime;
    [SerializeField] private float airControlAmount;

    public float MaxSpeed => maxSpeed;
    public float AccelerationTime => accelerationTime;
    public float BrakingTime => brakingTime;
    public float TurningTime => turningTime;
    public float AirControlAmount => airControlAmount;

    public float Input { get; set; }
    public float LastFrameInput { get; protected set; }
    public float CurrentVelocity
    {
        get => currentVelocity;
        protected set
        {
            LastFrameVelocity = currentVelocity;
            currentVelocity = value;
        }
    }

    public float LastFrameVelocity { get; protected set; }
    public PhysicsObjects PhysicsComponent { get; private set; }

    protected float Acceleration => maxSpeed / accelerationTime * AerialModifier * Time.deltaTime;
    protected float Decceleration => maxSpeed / brakingTime * AerialModifier * Time.deltaTime;
    protected float TurningForce => maxSpeed / turningTime * AerialModifier * Time.deltaTime;
    protected float AerialModifier => PhysicsComponent.Collisions.Below ? 1 : airControlAmount;
    private float currentVelocity;

    protected override void Start()
    {
        base.Start();
        PhysicsComponent = GetComponent<PhysicsObjects>();
    }
}