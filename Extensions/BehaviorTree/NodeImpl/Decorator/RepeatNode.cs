// Copyright 2025 Will Chan
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
using UnityEngine;

namespace WillFrameworkPro.Extensions.BehaviorTree.NodeImpl.Decorator
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