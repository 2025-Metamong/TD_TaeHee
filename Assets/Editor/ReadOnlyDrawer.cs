using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(pos, prop, label, true);
        GUI.enabled = true;
    }

    public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(prop, label, true);
    }
}
