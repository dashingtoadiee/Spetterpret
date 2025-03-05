#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace DuskModules.DynamicVariables.DuskEditor {

  /// <summary> Custom property drawer for the ColorSetting field </summary>
  [CustomPropertyDrawer(typeof(ColorSettingMoving))]
  public class ColorSettingMovingEditor : PropertyDrawer {

    /// <summary> Called when it needs to draw on the GUI  </summary>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
      float labelWidth = EditorGUIUtility.labelWidth;

      ExtraEditorUtility.BeginProperty(position, label, property);

      ExtraEditorUtility.TakeSpace(80);
      SerializedProperty enumProp = property.FindPropertyRelative("colorUse");
      ExtraEditorUtility.PropertyField(enumProp);

      if (enumProp.enumValueIndex != 1) {
        ExtraEditorUtility.TakeSpace(4);
        ExtraEditorUtility.TakeSpace();
        ExtraEditorUtility.PropertyField(property.FindPropertyRelative("color"));

        ExtraEditorUtility.IncreaseIndent();
        ExtraEditorUtility.NextLine();
        EditorGUIUtility.labelWidth = labelWidth;
        ExtraEditorUtility.PropertyField(property.FindPropertyRelative("speed"), new GUIContent("Speed"));
        ExtraEditorUtility.DecreaseIndent();
      }

      ExtraEditorUtility.EndProperty();
    }

    /// <summary> Height of this property. </summary>
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
      SerializedProperty enumProp = property.FindPropertyRelative("colorUse");
      return EditorGUIUtility.singleLineHeight * (enumProp.enumValueIndex == 1 ? 1 : 2);
    }
  }

}
#endif