using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor;

namespace DigitalMedia
{
    public class InspectorView : VisualElement
    {
        public new class UxmlFacotry : UxmlFactory<InspectorView, VisualElement.UxmlTraits> {}

        private Editor editor;
        public InspectorView()
        {
            
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BehaviorTreeEditor.uss");
            styleSheets.Add(styleSheet);
        }

        public void UpdateSelection(NodeView nodeView)
        {
            Clear();

            UnityEngine.Object.DestroyImmediate(editor);
            editor = Editor.CreateEditor(nodeView.node);
            IMGUIContainer container = new IMGUIContainer(() => { editor.OnInspectorGUI(); });
            Add(container);
        }
    }
}
