using System;
using System.Collections.Generic;

namespace DashAttack.Utility
{
    public class StateMachine<TAbilityType, TStateType> where TStateType : Enum
    {
        public TStateType CurrentState { get; protected set; }
        public TStateType PreviousState { get; protected set; }

        public bool IsLocked
        {
            get => isLocked;
            set
            {
                isLocked = value;
                if (isLocked)
                {
                    PreviousState = CurrentState;
                    CurrentState = EntryState;
                }
            }
        }

        private TStateType EntryState { get; set; }
        private bool isLocked;

        private Dictionary<TStateType, State<TAbilityType, TStateType>> States { get; set; }
            = new Dictionary<TStateType, State<TAbilityType, TStateType>>();

        public void Init(TStateType entryState, params State<TAbilityType, TStateType>[] states)
        {
            foreach (var state in states)
            {
                States.Add(state.Type, state);
            }
            EntryState = entryState;
            CurrentState = EntryState;
        }

        public void TransitionTo(TStateType nextState)
        {
            PreviousState = CurrentState;
            States[CurrentState].OnStateExit();
            CurrentState = nextState;
            States[CurrentState].OnStateEnter();
        }

        public void Update()
        {
            if (IsLocked)
            {
                return;
            }
            States[CurrentState].Update();
        }

        public void Subscribe(TStateType state, StateCallBack callBack, Action method)
        {
            switch (callBack)
            {
                case StateCallBack.OnStateEnter:
                    States[state].StateEntered += method;
                    break;

                case StateCallBack.OnUpdate:
                    States[state].StateUpdated += method;
                    break;

                case StateCallBack.OnStateExit:
                    States[state].StateExited += method;
                    break;
            }
        }
    }

    public enum StateCallBack
    {
        OnStateEnter,
        OnUpdate,
        OnStateExit
    }
}