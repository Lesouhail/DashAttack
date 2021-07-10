using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DashAttack.Utility
{
    public abstract class State<TEntityType, TStateType> : IState where TStateType : Enum
    {
        public Action StateEntered { get; set; }
        public Action StateExited { get; set; }
        public Action StateUpdated { get; set; }

        public abstract TStateType Type { get; }

        protected readonly StateMachine<TEntityType, TStateType> stateMachine;

        protected readonly TEntityType owner;

        public State(TEntityType owner, StateMachine<TEntityType, TStateType> stateMachine)
        {
            this.owner = owner;
            this.stateMachine = stateMachine;
        }

        public virtual void OnStateEnter()
        {
            StateEntered?.Invoke();
            Update();
        }

        public virtual void OnStateExit()
        {
            StateExited?.Invoke();
        }

        public void Update()
        {
            if (!HasTransition())
            {
                StateUpdated?.Invoke();
            }
        }

        protected abstract bool HasTransition();
    }
}