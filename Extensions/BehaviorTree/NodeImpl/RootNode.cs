// Copyright 2025 Will Chan
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0

namespace WillFrameworkPro.Extensions.BehaviorTree.NodeImpl
{
    public class RootNode : Node
    {
        public Node _child;
        protected override void OnStart()
        {
            
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            return _child.Update();
        }

        public override Node Clone()
        {
            RootNode node = Instantiate(this);
            node._child = _child.Clone();
            return node;
        }
    }
}