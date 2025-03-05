#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DuskModules.DynamicVariables.DuskEditor {

  /// <summary> Custom property drawer for the any variable reference field </summary>
  [CustomPropertyDrawer(typeof(OptionalValue<>), true)]
  public class OptionalValueProperty : BoolTogglePropertyDrawer {

    /// <summary> Name of boolean property </summary>
    public override string GetBoolPropertyName() {
      return "inUse";
    }

    /// <summary> Name for option to show for boolean true </summary>
    public override string GetBoolTrueName() {
      return "Available";
    }

    /// <summary> Name for option to show for boolean false </summary>
    public override string GetBoolFalseName() {
      return "Ignored";
    }

    /// <summary> Draw GUI if boolean is true </summary>
    public override void DrawTrueGUI(SerializedProperty property) {
      ExtraEditorUtility.TakeSpace();
      SerializedProperty propConstant = property.FindPropertyRelative("content");
      ExtraEditorUtility.PropertyField(propConstant, null, propConstant.hasVisibleChildren);
    }

    /// <summary> Draw GUI if boolean is false </summary>
    public override void DrawFalseGUI(SerializedProperty property) {
      ExtraEditorUtility.TakeSpace();
    }

    /// <summary> Returns property height </summary>
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
      SerializedProperty propUseVariable = property.FindPropertyRelative(GetBoolPropertyName());
      if (propUseVariable.boolValue) {
        return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("content"), GUIContent.none, true);
      } else {
        return EditorGUIUtility.singleLineHeight;
      }
    }

  }
}
#endif