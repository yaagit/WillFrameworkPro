using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using WillFrameworkPro.Core.Attributes.Types;
using WillFrameworkPro.Core.Views;

namespace WillFrameworkPro.Core.StateMachine
{
    /// <summary>
    /// 继承了 BaseView 的状态机，可以很好地集成到 WillFramework 中。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseStateMachine : BaseView
    {
        protected BaseState LastState { get; private set; }//上一个 State
        protected BaseState CurrentState { get; private set; }//当前持有的 State 对象（当前状态）
        
        /// <summary>
        /// 状态机会监听各种各样的事件，这些事件会随时改变状态机的当前状态，这样就会引入一个竞争条件（类似多线程）。如果当前状态中的一个动画没播放完，另外一边就切换到其他的状态，就会导致动画卡在最后一帧，无法再继续执行。
        ///     为了消除这个竞争条件，引入了“执行链”的概念：
        ///         1.每次在调用 SetCurrentState 的时候，会先检查调用者，如果调用者是当前状态对象，就把这个状态对象从执行列表中移除，然后把新的状态添加在列表的最后面。如果调用者是状态机对象，就直接把最新的状态添加在最后面。
        ///         2.执行列表会始终执行列表的第一个 State 对象的 Update 方法，直到它被移除。
        /// </summary>
        // 执行链，始终执行第一个 State，避免竞争条件
        // private readonly Queue<BaseState> _executionQueue = new Queue<BaseState>();

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
            CurrentState?.Exit(gameObject);
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
        }
        
        /// <summary>
        /// ！！！子类重写 Update 方法时，请手动执行 base.Update()。
        /// </summary>
        protected virtual void Update()
        {
            CurrentState?.Update(gameObject);
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
    }
}