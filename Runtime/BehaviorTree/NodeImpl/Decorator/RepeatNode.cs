using UnityEngine;

namespace WillFrameworkPro.Runtime.BehaviorTree.NodeImpl.Decorator
{
    /// <summary>
    /// 重复执行一个子节点 N 次。
    /// </summary>
    public class RepeatNode : DecoratorNode
    {
        protected override void OnStart()
        {
            Debug.Log("RepeatNode 的 OnStart()");
        }

        protected override void OnStop()
        {
            Debug.Log("RepeatNode 的 OnStop()");
        }

        protected override State OnUpdate()
        {
            _child.Update();
            return State.Running;
        }
    }
}