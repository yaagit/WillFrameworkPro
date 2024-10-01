using System;
using System.Collections.Generic;
using UnityEngine;
using WillFrameworkPro.Runtime.ToolKit.BehaviorTree.NodeImpl;
using WillFrameworkPro.Runtime.ToolKit.BehaviorTree.NodeImpl.Composite;
using WillFrameworkPro.Runtime.ToolKit.BehaviorTree.NodeImpl.Decorator;

namespace WillFrameworkPro.Runtime.ToolKit.BehaviorTree
{
    public class BehaviorTree : ScriptableObject
    {
        public Node _rootNode;
        public List<Node> _nodeList = new();
        //行为树的当前状态
        public State _currentState = State.Running;

        public State Update()
        {
            if (_rootNode.currentState == State.Running)
            {
                //根节点返回了 Success 或者 Failure 就不会再执行根节点的 Update 方法
                _currentState = _rootNode.Update();
            }
            return _currentState;
        }

        public Node CreateNode(Type type)
        {
            Node node = CreateInstance(type) as Node;
            node.name = type.Name;
            _nodeList.Add(node);
            
            return node;
        }

        public void DeleteNode(Node node)
        {
            _nodeList.Remove(node);
        }

        public void AddChild(Node parent, Node child)
        {
            DecoratorNode decorator = parent as DecoratorNode;
            if (decorator)
            {
                decorator._child = child;
            }
            
            RootNode rootNode = parent as RootNode;
            if (rootNode)
            {
                rootNode._child = child;
            }
            
            CompositeNode composite = parent as CompositeNode;
            if (composite)
            {
                composite._children.Add(child);
            }
        }

        public void RemoveChild(Node parent, Node child)
        {
            DecoratorNode decorator = parent as DecoratorNode;
            if (decorator && decorator._child != null)
            {
                decorator._child = null;
            }
            RootNode rootNode = parent as RootNode;
            if (rootNode)
            {
                rootNode._child = null;
            }
            CompositeNode composite = parent as CompositeNode;
            if (composite)
            {
                composite._children.Remove(child);
            }
        }
        public List<Node> GetChildren(Node parent)
        {
            List<Node> children = new();
            
            DecoratorNode decorator = parent as DecoratorNode;
            if (decorator && decorator._child != null)
            {
                children.Add(decorator._child);
            }
            
            RootNode rootNode = parent as RootNode;
            if (rootNode && rootNode._child != null)
            {
                children.Add(rootNode._child);
            }
            
            CompositeNode composite = parent as CompositeNode;
            if (composite)
            {
                return composite._children;
            }

            return children;
        }

        public BehaviorTree Clone()
        {
            BehaviorTree tree = Instantiate(this);
            tree._rootNode = tree._rootNode.Clone();
            return tree;
        }
    }
}