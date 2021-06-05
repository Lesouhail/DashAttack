using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceleratingState : State<HorizontalMovement, HorizontalState>
{
    public AcceleratingState(HorizontalMovement owner, StateMachine<HorizontalMovement, HorizontalState> stateMachine)
        : base(owner, stateMachine)
    {
    }

    public override HorizontalState Type => HorizontalState.Accelerating;

    protected override bool HasTransition()
    {
        if (owner.Input == 0)
        {
            stateMachine.TransitionTo(HorizontalState.Braking);
            return true;
        }
        else if (Mathf.Sign(owner.Input) != Mathf.Sign(owner.CurrentVelocity) &&
                 owner.CurrentVelocity != 0)
        {
            stateMachine.TransitionTo(HorizontalState.Turning);
            return true;
        }

        if (Mathf.Abs(owner.CurrentVelocity) == owner.MaxSpeed)
        {
            stateMachine.TransitionTo(HorizontalState.AtApex);
            return true;
        }
        return false;
    }
}