using System.Collections.Generic;
using UnityEngine;

namespace WillFrameworkPro.Runtime.ToolKit.BehaviorTree.NodeImpl.Composite
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