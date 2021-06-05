using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrakingState : State<HorizontalMovement, HorizontalState>
{
    public BrakingState(HorizontalMovement owner, StateMachine<HorizontalMovement, HorizontalState> stateMachine)
        : base(owner, stateMachine)
    {
    }

    public override HorizontalState Type => HorizontalState.Braking;

    protected override bool HasTransition()
    {
        if (owner.CurrentVelocity == 0)
        {
            stateMachine.TransitionTo(HorizontalState.Rest);
            return true;
        }
        if (owner.Input != 0)
        {
            HorizontalState nextState = Mathf.Sign(owner.CurrentVelocity) == Mathf.Sign(owner.Input)
                                      ? HorizontalState.Accelerating
                                      : HorizontalState.Turning;

            stateMachine.TransitionTo(nextState);
            return true;
        }
        return false;
    }
}