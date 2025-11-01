// Copyright 2025 Will Chan
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
using System.Collections.Generic;
using UnityEngine;

namespace WillFrameworkPro.Extensions.BehaviorTree.NodeImpl.Composite
{
    public abstract class CompositeNode : Node
    {
        [HideInInspector] public List<Node> _children = new();
        
        public override Node Clone()
        {
            CompositeNode node = Instantiate(this);
            node._children = _children.ConvertAll(c => c.Clone());
            return node;
        }
    }
}