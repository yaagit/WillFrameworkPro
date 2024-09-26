using System.ComponentModel;
using UnityEngine.UIElements;

namespace WillFrameworkPro.BehaviorTree.Editor.GraphView
{
    public class InspectorView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<InspectorView, UxmlTraits> {}

        private UnityEditor.Editor _editor;
        public void UpdateSelection(NodeView nodeView)
        {
            Clear();
            UnityEngine.Object.DestroyImmediate(_editor);
            
            _editor = UnityEditor.Editor.CreateEditor(nodeView._node);
            IMGUIContainer container = new IMGUIContainer(() =>
            {
                _editor.OnInspectorGUI();
            });
            Add(container);
        }
    }
}