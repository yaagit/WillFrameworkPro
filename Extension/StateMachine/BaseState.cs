using System;

namespace WillFrameworkPro.Extension.StateMachine
{
    /// <summary>
    /// 状态的基类。用户定义状态，需要继承此基类，并实现状态的逻辑代码。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseState<T> where T : Enum
    {
        //状态所属的状态机对象，状态机对象是唯一的，状态与状态机之间的关系为一对多。
        public BaseStateMachine<T> StateMachine 
        {
            protected get; set;
        }
        //todo 以后可以加入其他状态事件方法，例如 collisionEnter, collisionExit... 等等
        /// <summary>
        /// 切换状态后调用的第一个方法，仅调用一次。
        /// </summary>
        public abstract void Enter();

        /// <summary>
        /// 当前状态在 Unity Update 方法中执行的逻辑
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// 当前状态在 Unity FixedUpdate 方法中执行的逻辑
        /// </summary>
        public abstract void FixedUpdate();

        /// <summary>
        /// 切换下一个状态之前调用的方法，仅调用一次。
        /// </summary>
        public abstract void Exit();
    }
}