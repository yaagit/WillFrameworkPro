using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using WillFrameworkPro.BehaviorTree.NodeImpl;
using WillFrameworkPro.BehaviorTree.NodeImpl.Action;
using WillFrameworkPro.BehaviorTree.NodeImpl.Composite;
using WillFrameworkPro.BehaviorTree.NodeImpl.Decorator;

namespace WillFrameworkPro.BehaviorTree.Editor.GraphView
{
    public class NodeView : UnityEditor.Experimental.GraphView.Node
    {
        public Action<NodeView> OnNodeSelected;
        
        public Node _node;
        public Port _input;
        public Port _output;
        
        public NodeView(Node node) : base(FilePathConstant.NODE_UXML_TEMPLATE)
        {
            _node = node;
            title = node.name;
            //后面可以通过 GetNodeByGuid(node.GUID) 来获取 NodeView
            viewDataKey = node.GUID;
            
            style.left = node.position.x;
            style.top = node.position.y;
            
            CreateInputPorts();
            CreateOutputPorts();
        }
        /// <summary>
        /// 创建节点的连接口 - 出口点
        /// </summary>
        private void CreateInputPorts()
        {
            if (_node is ActionNode)
            {
                _input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
            } else if (_node is CompositeNode)
            {
                _input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
                
            } else if (_node is DecoratorNode)
            {
                _input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
            } else if (_node is RootNode)
            {
                
            }

            if (_input != null)
            {
                _input.portName = "";
                inputContainer.Add(_input);
            }
        }
        /// <summary>
        /// 创建节点的连接口 - 入口点
        /// </summary>
        private void CreateOutputPorts()
        {
            if (_node is ActionNode)
            {
                
            } else if (_node is CompositeNode)
            {
                _output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool));
            } else if (_node is DecoratorNode)
            {
                _output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
            } else if (_node is RootNode)
            {
                _output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
            }
            
            if (_output != null)
            {
                _output.portName = "";
                outputContainer.Add(_output);
            }
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            _node.position = new Vector2(newPos.xMin, newPos.yMin);
        }

        public override void OnSelected()
        {
            base.OnSelected();
            if (OnNodeSelected != null)
            {
                OnNodeSelected.Invoke(this);
            }
        }
    }
    
}