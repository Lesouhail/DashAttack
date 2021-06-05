namespace DashAttack.Characters
{
    using System;
    using DashAttack.Utility;
    using UnityEngine;

    public abstract class Ability<TAbilityType, TStateType> : MonoBehaviour
        where TAbilityType : Ability<TAbilityType, TStateType>
        where TStateType : Enum
    {
        public TStateType CurrentState => StateMachine.CurrentState;

        public TStateType PreviousState => StateMachine.PreviousState;

        protected StateMachine<TAbilityType, TStateType> StateMachine { get; set; }

        protected virtual void Start()
        {
            StateMachine = new StateMachine<TAbilityType, TStateType>();
            InitStateMachine();
        }

        protected virtual void Update()
        {
            StateMachine.Update();
        }

        protected abstract void InitStateMachine();

        protected void Subscribe(TStateType state, StateCallBack callBack, Action method)
            => StateMachine.Subscribe(state, callBack, method);
    }
}