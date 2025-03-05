#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DuskModules.DuskEditor {

  /// <summary> Custom property drawer for the MinMaxFloat field </summary>
  [CustomPropertyDrawer(typeof(Vector2Bool))]
  public class Vector2BoolPropertyDrawer : PropertyDrawer {

    /// <summary> Called when it needs to draw on the GUI  </summary>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
      ExtraEditorUtility.BeginProperty(position, label, property);
      ExtraEditorUtility.TakeSpace();
      ExtraEditorUtility.AddPropertyStack(property.FindPropertyRelative("x"), new GUIContent("X"));
      ExtraEditorUtility.AddPropertyStack(property.FindPropertyRelative("y"), new GUIContent("Y"));
      ExtraEditorUtility.DrawPropertyStack(10);
      ExtraEditorUtility.EndProperty();
    }
  }
}
#endif