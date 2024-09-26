using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using WillFrameworkPro.BehaviorTree.NodeImpl;
using WillFrameworkPro.BehaviorTree.NodeImpl.Action;
using WillFrameworkPro.BehaviorTree.NodeImpl.Composite;
using WillFrameworkPro.BehaviorTree.NodeImpl.Decorator;

namespace WillFrameworkPro.BehaviorTree.Editor.GraphView
{
    // [Serializable]
    /// <summary>
    /// 提示：在画布上找不到节点的位置时，按下 F 快捷键可以迅速定位。
    /// </summary>
    public class BehaviorTreeView : UnityEditor.Experimental.GraphView.GraphView
    {
        public Action<NodeView> OnNodeSelected;
        
        public new class UxmlFactory : UxmlFactory<BehaviorTreeView, UxmlTraits> {}

        private BehaviorTree _behaviorTree;
        public BehaviorTreeView()
        {
            Insert(0, new GridBackground()); 
            
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            
            StyleSheet uss = AssetDatabase.LoadAssetAtPath<StyleSheet>(FilePathConstant.USS_PATH);
            styleSheets.Add(uss);
        }
        
        public void PopulateView(BehaviorTree tree)
        {
            _behaviorTree = tree;
            graphViewChanged -= OnGraphViewChanged; 
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;

            if (tree._rootNode == null)
            {
                tree._rootNode = tree.CreateNode(typeof(RootNode)) as RootNode;
                EditorUtility.SetDirty(tree);
                AssetDatabase.SaveAssets();
            }

            _behaviorTree._nodeList.ForEach(n => CreateNodeView(n));
            
            _behaviorTree._nodeList.ForEach(n =>
            {
                NodeView parentView = FindNodeView(n);
                var children = _behaviorTree.GetChildren(n);
                children.ForEach(c =>
                {
                    NodeView childView = FindNodeView(c);
                    Edge edge = parentView._output.ConnectTo(childView._input);
                    AddElement(edge);
                });
            });
        }

        private NodeView FindNodeView(Node node)
        {
            return GetNodeByGuid(node.GUID) as NodeView;
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.elementsToRemove != null)
            {
                graphViewChange.elementsToRemove.ForEach(elem =>
                {
                    NodeView nodeView = elem as NodeView;
                    if (nodeView != null)
                    {
                        _behaviorTree.DeleteNode(nodeView._node);
                    }

                    Edge edge = elem as Edge;
                    if (edge != null)
                    {
                        NodeView parentView = edge.output.node as NodeView;
                        NodeView childView = edge.input.node as NodeView;
                        _behaviorTree.RemoveChild(parentView._node, childView._node);
                    }
                });
            }

            if (graphViewChange.edgesToCreate != null)
            {
                graphViewChange.edgesToCreate.ForEach(edge =>
                {
                    NodeView parentView = edge.output.node as NodeView;
                    NodeView childView = edge.input.node as NodeView;
                    _behaviorTree.AddChild(parentView._node, childView._node);
                });
            }
            return graphViewChange;
        }
        
        

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            {
                var types = TypeCache.GetTypesDerivedFrom<ActionNode>();
                foreach (var type in types)
                {
                    evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
                }
            }
            {
                var types = TypeCache.GetTypesDerivedFrom<CompositeNode>();
                foreach (var type in types)
                {
                    evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
                }
            }
            {
                var types = TypeCache.GetTypesDerivedFrom<DecoratorNode>();
                foreach (var type in types)
                {
                    evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
                }
            }
        }

        private void CreateNode(Type type)
        {
            Node node = _behaviorTree.CreateNode(type);
            CreateNodeView(node);
        }
        
        private void CreateNodeView(Node node)
        {
            NodeView nodeView = new NodeView(node);
            nodeView.OnNodeSelected = OnNodeSelected;
            AddElement(nodeView);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(endPort => endPort.direction != startPort.direction && endPort.node != startPort.node).ToList();
        }
        
    }
}