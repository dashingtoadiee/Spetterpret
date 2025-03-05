#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DuskModules.DuskEditor {

  /// <summary> Custom property drawer for the FloatRange field </summary>
  [CustomPropertyDrawer(typeof(FloatRange))]
  public class FloatRangePropertyDrawer : PropertyDrawer {

    /// <summary> Called when it needs to draw on the GUI  </summary>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
      ExtraEditorUtility.BeginProperty(position, label, property, false, true);
      ExtraEditorUtility.TakeSpace();
      ExtraEditorUtility.AddPropertyStack(property.FindPropertyRelative("min"), GUIContent.none);
      ExtraEditorUtility.AddPropertyStack(property.FindPropertyRelative("max"), GUIContent.none);
      ExtraEditorUtility.DrawPropertyStack(0);
      ExtraEditorUtility.EndProperty();
    }
  }

}
#endif