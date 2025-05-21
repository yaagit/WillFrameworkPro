using System;
using UnityEngine;

namespace WillFrameworkPro.Extension.StateMachine
{
    /// <summary>
    /// 状态的基类。用户定义状态，需要继承此基类，并实现状态的逻辑代码。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseState
    {
        //状态机对象是唯一的，状态与状态机之间的关系为多对一。
        public BaseStateMachine StateMachine 
        {
            protected get; set;
        }
        
        //todo 以后可以加入其他状态事件方法，例如 collisionEnter, collisionExit... 等等
        /// <summary>
        /// 切换状态后调用的第一个方法，仅调用一次。
        /// </summary>
        public abstract void Enter(GameObject gameObject);

        /// <summary>
        /// 当前状态在 Unity Update 方法中执行的逻辑
        /// </summary>
        public abstract void Update(GameObject gameObject);

        /// <summary>
        /// 当前状态在 Unity FixedUpdate 方法中执行的逻辑
        /// </summary>
        public abstract void FixedUpdate(GameObject gameObject);

        /// <summary>
        /// 切换下一个状态之前调用的方法，仅调用一次。
        /// </summary>
        public abstract void Exit(GameObject gameObject);
    }
}