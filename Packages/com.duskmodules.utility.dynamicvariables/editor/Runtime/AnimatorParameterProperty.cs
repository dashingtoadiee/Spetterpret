#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace DuskModules.DynamicVariables.DuskEditor {

  /// <summary> Property drawer of the AnimatorParameter </summary>
  [CustomPropertyDrawer(typeof(AnimatorParameter))]
  public class AnimatorParameterProperty : PropertyDrawer {

    /// <summary> Called when it needs to draw on the GUI  </summary>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
      ExtraEditorUtility.BeginProperty(position, label, property);

      ExtraEditorUtility.AddPropertyStack(property.FindPropertyRelative("parameterName"));
      SerializedProperty typeProp = property.FindPropertyRelative("type");
      ExtraEditorUtility.AddPropertyStack(typeProp, 60);

      if (typeProp.enumValueIndex < 5) {
        ExtraEditorUtility.TakeSpace(4);
        ExtraEditorUtility.TakeSpace();
        switch (typeProp.enumValueIndex) {
          case 0: ExtraEditorUtility.AddPropertyStack(property.FindPropertyRelative("valueFloat"), 60); break;
          case 1: ExtraEditorUtility.AddPropertyStack(property.FindPropertyRelative("valueInt"), 60); break;
          case 2: ExtraEditorUtility.AddPropertyStack(property.FindPropertyRelative("valueBool"), 60); break;
        }
      }
      ExtraEditorUtility.DrawPropertyStack(0);

      ExtraEditorUtility.EndProperty();
    }

  }
}
#endif