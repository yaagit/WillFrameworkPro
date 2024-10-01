using System;

namespace WillFrameworkPro.Runtime.ToolKit.StateMachine
{
    public abstract class BaseState<T> where T : Enum
    {
        private BaseStateMachine<T> _stateMachine;
        public BaseStateMachine<T> StateMachine 
        {
            protected get => _stateMachine;
            set
            {
                _stateMachine = value;
            }
        }
        //todo 以后可以加入其他状态事件方法，例如 collisionEnter, collisionExit... 等等
        public virtual void Enter() {}
        //需要在 Unity MonoBehavior 的 Update 的方法里进行调用，例如： _stateMachine.CurrentState.Update();
        public virtual void Update() {}
        //需要在 Unity MonoBehavior 的 FixedUpdate 的方法里进行调用，例如： _stateMachine.CurrentState.FixedUpdate();
        public virtual void FixedUpdate() {}
        public virtual void Exit() {}
    }
}