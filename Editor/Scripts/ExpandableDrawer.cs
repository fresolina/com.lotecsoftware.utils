using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace Lotec.Utils.Attributes {
    [CustomPropertyDrawer(typeof(ExpandableAttribute))]
    public class ExpandableDrawer : PropertyDrawer {
        UnityEditor.Editor _editor = null;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.PropertyField(position, property, label, true);
            if (property.objectReferenceValue == null)
                return;

            if (property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, GUIContent.none, true)) {
                if (!_editor)
                    UnityEditor.Editor.CreateCachedEditor(property.objectReferenceValue, null, ref _editor);

                // EditorGUI.indentLevel++;
                EditorGUILayout.BeginVertical(GUI.skin.box);
                _editor.OnInspectorGUI();
                EditorGUILayout.EndVertical();
                // EditorGUI.indentLevel--;
            }
        }
    }
}
