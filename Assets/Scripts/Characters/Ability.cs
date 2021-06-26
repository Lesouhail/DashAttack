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
        public bool IsLocked
        {
            get => StateMachine.IsLocked;
            set
            {
                StateMachine.IsLocked = value;
                if (IsLocked)
                {
                    OnLock();
                }
                else
                {
                    OnUnlock();
                }
            }
        }

        protected StateMachine<TAbilityType, TStateType> StateMachine { get; set; }

        protected virtual void Awake()
        {
            StateMachine = new StateMachine<TAbilityType, TStateType>();
            InitStateMachine();
        }

        protected virtual void Update()
        {
            StateMachine.Update();
        }

        public void Subscribe(TStateType state, StateCallBack callBack, Action method)
            => StateMachine.Subscribe(state, callBack, method);

        protected abstract void InitStateMachine();

        protected virtual void OnLock()
        {
        }

        protected virtual void OnUnlock()
        {
        }
    }
}