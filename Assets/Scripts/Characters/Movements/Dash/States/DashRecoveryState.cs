using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DashRecoveryState : State<Dash, DashState>
{
    public DashRecoveryState(Dash owner, StateMachine<Dash, DashState> stateMachine) : base(owner, stateMachine)
    {
    }

    public override DashState Type => DashState.Recovering;

    protected override bool HasTransition()
    {
        if (owner.Input && !owner.LastFrameInput && owner.CanDash)
        {
            stateMachine.TransitionTo(DashState.Casting);
            return true;
        }
        if (owner.DashRecoveryCounter >= owner.RecoveryTime)
        {
            stateMachine.TransitionTo(DashState.Rest);
            return true;
        }
        return false;
    }
}