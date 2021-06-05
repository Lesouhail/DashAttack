using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DashAttack.Utility{
	
	public interface IStateMachine<TStateType>
	{
	    public TStateType CurrentState { get; }
	    public TStateType PreviousState { get; }
	
	    public void TransitionTo(TStateType nextState);
	}
}