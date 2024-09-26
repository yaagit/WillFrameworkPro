using UnityEngine;
using UnityEngine.Serialization;

namespace WillFrameworkPro.BehaviorTree.NodeImpl.Decorator
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