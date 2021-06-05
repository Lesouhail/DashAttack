using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DashRestState : State<Dash, DashState>
{
    public DashRestState(Dash owner, StateMachine<Dash, DashState> stateMachine) : base(owner, stateMachine)
    {
    }

    public override DashState Type => DashState.Rest;

    protected override bool HasTransition()
    {
        if (owner.Input && !owner.LastFrameInput && owner.CanDash)
        {
            stateMachine.TransitionTo(DashState.Casting);
            return true;
        }
        return false;
    }
}