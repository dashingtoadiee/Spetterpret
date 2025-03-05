#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace DuskModules.DynamicVariables.DuskEditor {

  /// <summary> Custom property drawer for any random value that has a constant, minimum and maximum. </summary>
  [CustomPropertyDrawer(typeof(BaseRandomValue), true)]
  public class BaseRandomProperty : BoolTogglePropertyDrawer {

    /// <summary> Name for option to show for boolean false </summary>
    public override string GetBoolFalseName() {
      return "Random";
    }

    /// <summary> Draw GUI if boolean is true </summary>
    public override void DrawTrueGUI(SerializedProperty property) {
      ExtraEditorUtility.TakeSpace();
      ExtraEditorUtility.PropertyField(property.FindPropertyRelative("constant"));
    }

    /// <summary> Draw GUI if boolean is false </summary>
    public override void DrawFalseGUI(SerializedProperty property) {
      ExtraEditorUtility.TakeSpace();
      ExtraEditorUtility.AddPropertyStack(property.FindPropertyRelative("minimum"), new GUIContent("Min"));
      ExtraEditorUtility.AddPropertyStack(property.FindPropertyRelative("maximum"), new GUIContent("Max"));
      ExtraEditorUtility.DrawPropertyStack(30);
    }
  }
}
#endif