using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateMachine<TStateType>
{
    public TStateType CurrentState { get; }
    public TStateType PreviousState { get; }

    public void TransitionTo(TStateType nextState);
}