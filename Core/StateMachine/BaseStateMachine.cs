using System;
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
        private BaseState _currentState;//当前持有的 State 对象（当前状态）

        /// <summary>
        /// ！！！设置当前的状态, 然后执行 State 对象的生命周期方法。
        /// </summary>
        /// <param name="stateEnum">当前的状态枚举</param>
        /// <exception cref="Exception"></exception>
        public void SetCurrentState(BaseState state)
        {
            //如果当前状态相同，没必要重复切换
            if (state == _currentState)
            {
                return;
            }
            _currentState?.Exit(gameObject);
            _currentState = state;
            _currentState.StateMachine = this;
            _currentState?.Enter(gameObject);

        }
        /// <summary>
        /// 获取当前状态的 State 对象
        /// </summary>
        /// <returns></returns>
        public BaseState GetCurrentState()
        {
            return _currentState;
        }
        
        /// <summary>
        /// ！！！子类重写 FixedUpdate 方法时，请手动执行 base.FixedUpdate()。
        /// </summary>
        protected virtual void FixedUpdate()
        {
            _currentState?.FixedUpdate(gameObject);
        }
        
        /// <summary>
        /// ！！！子类重写 Update 方法时，请手动执行 base.Update()。
        /// </summary>
        protected virtual void Update()
        {
            _currentState?.Update(gameObject);
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
    }
}