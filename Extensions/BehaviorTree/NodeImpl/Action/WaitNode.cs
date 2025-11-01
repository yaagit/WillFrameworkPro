// Copyright 2025 Will Chan
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
using UnityEngine;

namespace WillFrameworkPro.Extensions.BehaviorTree.NodeImpl.Action
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