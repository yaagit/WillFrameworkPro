using System;
using System.Collections.Generic;
using UnityEngine;

namespace WillFrameworkPro.Runtime.ToolKit.StateMachine
{
    public abstract class BaseStateMachine<T> : MonoBehaviour where T : Enum
    {
        private BaseState<T> _currentState;
        private T _currentStateEnum;

        protected Dictionary<T, BaseState<T>> _enumStateMapping = new();
        
        protected abstract void InitialEnumStateMapping(Dictionary<T, BaseState<T>> enumStateMapping);

        public BaseStateMachine()
        {
            InitialEnumStateMapping(_enumStateMapping);
        }
        
        public GameObject GameObject
        {
            get => gameObject;
        }

        public void SetCurrentState(T stateEnum)
        {
            if (_enumStateMapping.Count == 0)
            {
                throw new Exception("EnumStateDictionary 必须至少拥有一个元素。");
            }
            if (_enumStateMapping.TryGetValue(stateEnum, out BaseState<T> state))
            {
                _currentState?.Exit();
                _currentState = state;
                _currentStateEnum = stateEnum;
                _currentState.StateMachine = this;
                _currentState?.Enter();
            }
        }

        public BaseState<T> GetCurrentState()
        {
            return _currentState;
        }

        public T GetCurrentStateEnum()
        {
            return _currentStateEnum;
        }

        protected void AddToEnumStateDictionary(T stateEnum, BaseState<T> state)
        {
            state.StateMachine = this;
            _enumStateMapping.Add(stateEnum, state);
        }
        
        protected virtual void FixedUpdate()
        {
            _currentState.FixedUpdate();
        }
        

        protected virtual void Update()
        {
            _currentState.Update();
        }

    }
}