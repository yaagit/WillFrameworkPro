using System;
using System.Collections.Generic;
using UnityEngine;
using WillFrameworkPro.Core.Views;

namespace WillFrameworkPro.Extension.StateMachine
{
    /// <summary>
    /// 继承了 BaseView 的状态机，可以很好地集成到 WillFramework 中。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseStateMachine<T> : BaseView where T : Enum
    {
        private BaseState<T> _currentState;//当前持有的 State 对象（当前状态）
        private T _currentStateEnum;//当前持有的 State 枚举（当前状态）

        protected Dictionary<T, BaseState<T>> _enumStateMapping = new();//枚举与相应 State 对象的映射容器
        
        /// <summary>
        /// ！！！强制要求子类手动实现枚举与相应 State 对象的对照映射。
        /// </summary>
        /// <param name="enumStateMapping"></param>
        protected abstract void InitialEnumStateMapping();

        /// <summary>
        /// 无参构造方法会被 Unity 隐式调用。因此也会同时调用强制要求子类实现的 InitialEnumStateMapping 方法。
        /// </summary>
        public BaseStateMachine()
        {
            InitialEnumStateMapping();
        }
        /// <summary>
        /// ！！！设置当前的状态。入参使用枚举即可，该方法会从“状态枚举映射容器”中找到 State 对象，然后执行 State 对象的生命周期方法。
        /// </summary>
        /// <param name="stateEnum">当前的状态枚举</param>
        /// <exception cref="Exception"></exception>
        public void SetCurrentState(T stateEnum)
        {
            if (_enumStateMapping.Count == 0)
            {
                throw new Exception("EnumStateDictionary（状态枚举映射容器）必须至少拥有一个元素。");
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
        /// <summary>
        /// 获取当前状态的 State 对象
        /// </summary>
        /// <returns></returns>
        public BaseState<T> GetCurrentState()
        {
            return _currentState;
        }
        /// <summary>
        /// 获取当前状态的 State 枚举
        /// </summary>
        /// <returns></returns>
        public T GetCurrentStateEnum()
        {
            return _currentStateEnum;
        }
        /// <summary>
        /// ！！！构建枚举和 State 对象映射的工具方法。
        /// </summary>
        /// <param name="stateEnum"></param>
        /// <param name="state"></param>
        protected void AddToEnumStateDictionary(T stateEnum, BaseState<T> state)
        {
            state.StateMachine = this;
            _enumStateMapping.Add(stateEnum, state);
        }
        /// <summary>
        /// ！！！子类重写 FixedUpdate 方法时，请手动执行 base.FixedUpdate()。
        /// </summary>
        protected virtual void FixedUpdate()
        {
            _currentState.FixedUpdate();
        }
        
        /// <summary>
        /// ！！！子类重写 Update 方法时，请手动执行 base.Update()。
        /// </summary>
        protected virtual void Update()
        {
            _currentState.Update();
        }

    }
}