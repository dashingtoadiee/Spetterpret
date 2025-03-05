#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DuskModules.DynamicVariables.DuskEditor {

  /// <summary> Base for any smooth value property </summary>
  [CustomPropertyDrawer(typeof(BaseSmoothValue<,>), true)]
  public class BaseSmoothValueProperty : BoolTogglePropertyDrawer {

    /// <summary> Name for option to show for boolean false </summary>
    public override string GetBoolFalseName() {
      return "Moving";
    }

    /// <summary> Whether it has a foldout or not. </summary>
    public override bool HasFoldout(SerializedProperty property) {
      return !property.FindPropertyRelative(GetBoolPropertyName()).boolValue;
    }

    /// <summary> Draw GUI if boolean is true </summary>
    public override void DrawTrueGUI(SerializedProperty property) {
      if (ShowValues()) {
        ExtraEditorUtility.TakeSpace();
        ExtraEditorUtility.PropertyField(property.FindPropertyRelative("value"));
      }
    }

    /// <summary> Draw GUI if boolean is false </summary>
    public override void DrawFalseGUI(SerializedProperty property) {
      float labelWidth = EditorGUIUtility.labelWidth;

      if (ShowValues()) {
        ExtraEditorUtility.TakeSpace();
        ExtraEditorUtility.AddPropertyStack(property.FindPropertyRelative("value"), new GUIContent("V", "Current value, which moves to match target value."));
        ExtraEditorUtility.AddPropertyStack(property.FindPropertyRelative("valueTarget"), new GUIContent("T", "Target value to which value is moved to match."));
        ExtraEditorUtility.DrawPropertyStack(16);
      }

      EditorGUIUtility.labelWidth = labelWidth;

      if (property.isExpanded) {
        // Next line
        EditorGUI.indentLevel++;
        Rect usePosition = new Rect(ExtraEditorUtility.ap.totalPosition);

        usePosition.y += EditorGUIUtility.singleLineHeight;
        usePosition.height = EditorGUIUtility.singleLineHeight;

        EditorGUI.PropertyField(usePosition, property.FindPropertyRelative("speed"), new GUIContent("Speed"));
        EditorGUI.indentLevel--;
      }
    }

    /// <summary> Height of this property. </summary>
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
      SerializedProperty boolProp = property.FindPropertyRelative(GetBoolPropertyName());
      int lines = (boolProp.boolValue || !property.isExpanded) ? 1 : 2;
      return EditorGUIUtility.singleLineHeight * lines;
    }

    /// <summary> Whether to show the actual values. </summary>
    protected virtual bool ShowValues() {
      return true;
    }
  }
}
#endif