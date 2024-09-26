using UnityEngine;

namespace WillFrameworkPro.Runtime.BehaviorTree.NodeImpl.Action
{
    /// <summary>
    /// 什么也不做，只是停留 N 秒后返回 Success
    /// </summary>
    public class WaitNode : ActionNode
    {
        //单位：秒
        public float _duration = 1;

        private float _startTime;
        
        protected override void OnStart()
        {
            _startTime = Time.time;
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            if (Time.time - _startTime > _duration )
            {
                return State.Success;
            }

            return State.Running;
        }
    }
}