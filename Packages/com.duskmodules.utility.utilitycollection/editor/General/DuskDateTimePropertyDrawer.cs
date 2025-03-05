#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

namespace DuskModules.DuskEditor {

  /// <summary> Custom property drawer for the DuskDateTime field </summary>
  [CustomPropertyDrawer(typeof(DuskDateTime))]
  public class DuskDateTimePropertyDrawer : PropertyDrawer {

    /// <summary> Called when it needs to draw on the GUI  </summary>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
      ExtraEditorUtility.BeginProperty(position, label, property);
      ExtraEditorUtility.TakeSpace(40);
      if (ExtraEditorUtility.DrawButton("Now")) {
        // Show a popup to confirm
        if (EditorUtility.DisplayDialog("Set date to now?", "Are you sure you want to set this date to now?", "Yes", "No")) {
          DateTime now = DateTime.Now;
          property.FindPropertyRelative("year").intValue = now.Year;
          property.FindPropertyRelative("month").intValue = now.Month;
          property.FindPropertyRelative("day").intValue = now.Day;
          property.FindPropertyRelative("hour").intValue = now.Hour;
          property.FindPropertyRelative("minute").intValue = now.Minute;
          property.FindPropertyRelative("second").intValue = now.Second;
          property.FindPropertyRelative("millisecond").intValue = now.Millisecond;
          property.serializedObject.ApplyModifiedProperties();
        }
      }
      ExtraEditorUtility.TakeSpace();
      ExtraEditorUtility.AddPropertyStack(property.FindPropertyRelative("year"), GUIContent.none);
      ExtraEditorUtility.AddPropertyStack(property.FindPropertyRelative("month"), GUIContent.none);
      ExtraEditorUtility.AddPropertyStack(property.FindPropertyRelative("day"), GUIContent.none);
      ExtraEditorUtility.AddPropertyStack(property.FindPropertyRelative("hour"), GUIContent.none);
      ExtraEditorUtility.AddPropertyStack(property.FindPropertyRelative("minute"), GUIContent.none);
      ExtraEditorUtility.AddPropertyStack(property.FindPropertyRelative("second"), GUIContent.none);
      ExtraEditorUtility.AddPropertyStack(property.FindPropertyRelative("millisecond"), GUIContent.none);
      ExtraEditorUtility.DrawPropertyStack(0);
      ExtraEditorUtility.EndProperty();
    }
  }
}
#endif