﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class DashBase<TChild, TStateType> : Ability<TChild, TStateType>
    where TChild : DashBase<TChild, TStateType>
    where TStateType : Enum
{
    [SerializeField] private float dashDistance;
    [SerializeField] private float dashTime;
    [SerializeField] private float castTime;
    [SerializeField] private float recoveryTime;

    public PhysicsObjects PhysicsComponent { get; private set; }

    public float DashDistance => dashDistance;
    public float DashTime => dashTime;
    public float CastTime => castTime;
    public float RecoveryTime => recoveryTime;
    public float TotalDashingTime => dashTime + recoveryTime;

    public float InitialVelocity => Decceleration * TotalDashingTime;
    public float Decceleration => 2 * DashDistance / Mathf.Pow(TotalDashingTime, 2);

    public virtual float DeltaPosition(float velocity) => velocity * Time.deltaTime + (Decceleration * Mathf.Pow(Time.deltaTime, 2) / 2);

    public float CurrentVelocity { get; protected set; }

    protected override void Start()
    {
        base.Start();
        PhysicsComponent = GetComponent<PhysicsObjects>();
    }
}