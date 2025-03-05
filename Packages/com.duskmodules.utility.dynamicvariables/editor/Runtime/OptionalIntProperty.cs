#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DuskModules.DynamicVariables.DuskEditor {

  /// <summary> Custom property drawer for the any variable reference field </summary>
  [CustomPropertyDrawer(typeof(OptionalInt))]
  public class OptionalIntProperty : BoolTogglePropertyDrawer {

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
      ExtraEditorUtility.PropertyField(property.FindPropertyRelative("content"));
    }

    /// <summary> Draw GUI if boolean is false </summary>
    public override void DrawFalseGUI(SerializedProperty property) {
      ExtraEditorUtility.TakeSpace();
    }
  }
}
#endif