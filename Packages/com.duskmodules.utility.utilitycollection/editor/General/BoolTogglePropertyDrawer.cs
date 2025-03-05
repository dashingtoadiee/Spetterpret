#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

// Never an end-point, and thus not in Editor namespace. Can be used by other modules.
namespace DuskModules {

  /// <summary> Base for many custom property drawer that have a settings box for 2 variables in front </summary>
  public class BoolTogglePropertyDrawer : PropertyDrawer {

    /// <summary> Called when it needs to draw on the GUI  </summary>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
      ExtraEditorUtility.BeginProperty(position, label, property, HasFoldout(property));

      ExtraEditorUtility.TakeSpace(24);
      SerializedProperty boolProp = property.FindPropertyRelative(GetBoolPropertyName());
      ExtraEditorUtility.DrawSettingsToggleDropdown(ExtraEditorUtility.ap.contentPosition, boolProp, GetBoolTrueName(), GetBoolFalseName(), true);

      // Display
      if (boolProp.boolValue) {
        DrawTrueGUI(property);
      } else {
        DrawFalseGUI(property);
      }

      ExtraEditorUtility.EndProperty();
    }

    /// <summary> Name of boolean property </summary>
    public virtual string GetBoolPropertyName() {
      return "useConstant";
    }

    /// <summary> Name for option to show for boolean true </summary>
    public virtual string GetBoolTrueName() {
      return "Constant";
    }

    /// <summary> Name for option to show for boolean false </summary>
    public virtual string GetBoolFalseName() {
      return "Other";
    }

    /// <summary> Whether it has a foldout or not. </summary>
    public virtual bool HasFoldout(SerializedProperty property) {
      return false;
    }

    /// <summary> Draw GUI if boolean is true </summary>
    public virtual void DrawTrueGUI(SerializedProperty property) {

    }

    /// <summary> Draw GUI if boolean is false </summary>
    public virtual void DrawFalseGUI(SerializedProperty property) {

    }
  }
}
#endif