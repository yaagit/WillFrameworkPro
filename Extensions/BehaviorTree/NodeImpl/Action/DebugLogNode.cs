// Copyright 2025 Will Chan
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
using UnityEngine;

namespace WillFrameworkPro.Extensions.BehaviorTree.NodeImpl.Action
{
    public class DebugLogNode : ActionNode
    {

        public string _message;
        protected override void OnStart()
        {
            Debug.Log($"OnStart --- {_message}");
        }

        protected override void OnStop()
        {
            Debug.Log($"OnStop --- {_message}");
        }

        protected override State OnUpdate()
        {
            Debug.Log($"OnUpdate --- {_message}");
            return State.Success;
        }
    }
}