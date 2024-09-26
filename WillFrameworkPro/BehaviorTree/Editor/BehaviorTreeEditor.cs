using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;
using WillFrameworkPro.BehaviorTree.Editor.GraphView;

namespace WillFrameworkPro.BehaviorTree.Editor
{
    public class BehaviorTreeEditor : EditorWindow
    {
        private BehaviorTreeView _behaviorTreeView;
        private InspectorView _inspectorView;
    
        [MenuItem("BehaviorTreeEditor/Editor...")]
        public static void OpenWindow()
        {
            BehaviorTreeEditor wnd = GetWindow<BehaviorTreeEditor>();
            wnd.titleContent = new GUIContent("BehaviorTreeEditor");
        }
        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceId, int line)
        {
            if (Selection.activeObject is BehaviorTree)
            {
                OpenWindow();
                return true;
            }

            return false;
        }
        public void CreateGUI()
        {
            VisualTreeAsset uxml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(FilePathConstant.UXML_PATH);
            StyleSheet uss = AssetDatabase.LoadAssetAtPath<StyleSheet>(FilePathConstant.USS_PATH);

            VisualElement root = rootVisualElement;
            root.styleSheets.Add(uss);
            uxml.CloneTree(root);

            _behaviorTreeView = root.Q<BehaviorTreeView>();
            _inspectorView = root.Q<InspectorView>();
            _behaviorTreeView.OnNodeSelected = OnNodeSelectionChanged;
            OnSelectionChange();
        }

        private void OnSelectionChange()
        {
            BehaviorTree tree = Selection.activeObject as BehaviorTree;
            if (tree && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
            {
                _behaviorTreeView.PopulateView(tree);
            }
        }

        void OnNodeSelectionChanged(NodeView nodeView)
        {
            _inspectorView.UpdateSelection(nodeView);
        }
    }
}
