using System;
using System.Collections.Generic;
using WillFrameworkPro.Core.Attributes.Types;
using WillFrameworkPro.Core.Views;

namespace WillFrameworkPro.StateMachine
{
    /// <summary>
    /// 继承了 BaseView 的状态机，可以很好地集成到 WillFramework 中。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseStateMachine : BaseView
    {
        protected BaseState LastState { get; private set; }//上一个 State
        protected BaseState CurrentState { get; private set; }//当前持有的 State 对象（当前状态）
        protected List<ParallelState> ParallelStateList { get; private set; }//所有正在运行的并行 State

        /// <summary>
        /// 添加并行 State
        /// </summary>
        public void AddParallelState(ParallelState state)
        {
            if (state == null)
            {
                return;
            }
            if (ParallelStateList == null)
            {
                ParallelStateList = new();
            }
            if (ParallelStateList.Contains(state))
            {
                return;
            }
            state.StateMachine = this;
            ParallelStateList.Add(state);
            state.Enter(gameObject);
        }
        /// <summary>
        /// 移除并行 State
        /// </summary>
        public void RemoveParallelState(ParallelState state)
        {
            if (state == null || ParallelStateList == null)
            {
                return;
            }
            ParallelStateList.Remove(state);
        }
        
        /// <summary>
        /// ！！！设置当前的状态, 然后执行 State 对象的生命周期方法。
        /// </summary>
        /// <param name="stateEnum">当前的状态枚举</param>
        /// <exception cref="Exception"></exception>
        public void SetCurrentState(BaseState state)
        {
            //如果当前状态相同，没必要重复切换
            if (state == CurrentState)
            {
                return;
            }
            //记录上一个状态
            LastState = CurrentState;
            CurrentState?.Exit(gameObject, state);
            //更新当前状态
            CurrentState = state;
            CurrentState.StateMachine = this;
            CurrentState?.Enter(gameObject);
        }

        /// <summary>
        /// 获取当前状态的 State 对象
        /// </summary>
        /// <returns></returns>
        public BaseState GetCurrentState()
        {
            return CurrentState;
        }
        
        /// <summary>
        /// ！！！子类重写 FixedUpdate 方法时，请手动执行 base.FixedUpdate()。
        /// </summary>
        protected virtual void FixedUpdate()
        {
            CurrentState?.FixedUpdate(gameObject);
            //执行 parallel state
            if (ParallelStateList != null)
            {
                foreach (var state in ParallelStateList)
                {
                    state.FixedUpdate(gameObject);
                }
            }
        }
        
        /// <summary>
        /// ！！！子类重写 Update 方法时，请手动执行 base.Update()。
        /// </summary>
        protected virtual void Update()
        {
            CurrentState?.Update(gameObject);
            //执行 parallel state
            if (ParallelStateList != null)
            {
                foreach (var state in ParallelStateList)
                {
                    state.Update(gameObject);
                }
            }
        }
        /// <summary>
        /// ！！！子类重写 LateUpdate 方法时，请手动执行 base.LateUpdate()。
        /// </summary>
        protected virtual void LateUpdate()
        {
            CurrentState?.LateUpdate(gameObject);
            //执行 parallel state
            if (ParallelStateList != null)
            {
                foreach (var state in ParallelStateList)
                {
                    state.LateUpdate(gameObject);
                }
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            var states = _context.StateContainer.GetStates(GetType());
            foreach (var state in states)
            {
                _context.CommandContainer.UnbindEvents(state);
                _context.IocContainer.Remove(TypeEnum.General, state);
            }
            //所有 State 对象的事件已经注销，清理 StateContainer
            _context.StateContainer.ClearStates(GetType());
        }
        /// <summary>
        /// 返回到上一个状态
        /// </summary>
        public void ReturnLastState()
        {
            if (LastState != null)
            {
                SetCurrentState(LastState);
            }
        }
        /// <summary>
        /// 获取上一个状态的信息
        /// </summary>
        public BaseState GetLastState()
        {
            return LastState;
        }
    }
}