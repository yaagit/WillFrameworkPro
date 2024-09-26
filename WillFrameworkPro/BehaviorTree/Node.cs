using UnityEngine;
using UnityEngine.Serialization;
using State = WillFrameworkPro.BehaviorTree.State;

namespace WillFrameworkPro.BehaviorTree
{
    /// <summary>
    /// 基本节点，所有的节点都派生自基本节点。
    /// 每个节点都有三个生命周期方法：
    ///     OnStart(); OnUpdate(); OnStop();
    /// 即使是包含子节点的父节点，其自身也拥有这三个生命周期方法。
    /// </summary>
    public abstract class Node : ScriptableObject
    {
        //节点的唯一标识
        [HideInInspector] public string GUID;

        [HideInInspector] public bool started = false;
        //保存每个节点的位置
        [HideInInspector] public Vector2 position;
        
        [HideInInspector] public State currentState = State.Running;

        public State Update()
        {
            if (!started)
            {
                OnStart();
                started = true;
            }

            currentState = OnUpdate();

            if (currentState == State.Failure || currentState == State.Success)
            {
                OnStop();
                started = false;
            }

            return currentState;
        }

        public virtual Node Clone()
        {
            return Instantiate(this);
        }
        
        protected abstract void OnStart();
        protected abstract void OnStop();
        protected abstract State OnUpdate();
    }
}