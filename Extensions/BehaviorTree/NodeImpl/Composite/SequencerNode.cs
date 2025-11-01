// Copyright 2025 Will Chan
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0

namespace WillFrameworkPro.Extensions.BehaviorTree.NodeImpl.Composite
{
    public class SequencerNode : CompositeNode
    {
        private int _currentChildIndex;
        /// <summary>
        /// 当所有子节点全部执行完，返回 Success 状态，然后执行 OnStop 方法。这样下次再进入 SequencerNode 时，就会先调用 OnStart 方法。
        /// </summary>
        protected override void OnStart()
        {
            _currentChildIndex = 0;
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            var child = _children[_currentChildIndex];
            switch (child.Update())
            {
                case State.Running:
                    return State.Running;
                case State.Failure:
                    return State.Failure;
                case State.Success:
                    _currentChildIndex++;
                    break;
            }
            //判断所有的子节点是否已经成功执行完？
            return _currentChildIndex == _children.Count ? State.Success : State.Running;
        }
    }
}