// Copyright 2025 Will Chan
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
using UnityEngine;

namespace WillFrameworkPro.Extensions.BehaviorTree.NodeImpl.Decorator
{
    public abstract class DecoratorNode : Node
    {
        [HideInInspector] public Node _child;
        
        public override Node Clone()
        {
            DecoratorNode node = Instantiate(this);
            node._child = _child.Clone();
            return node;
        }
    }
}