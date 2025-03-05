#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DuskModules.DynamicVariables.DuskEditor {

  /// <summary> Custom property drawer for the MinMaxFloat field </summary>
  [CustomPropertyDrawer(typeof(TimerValue))]
  public class TimerProperty : PropertyDrawer {

    /// <summary> Called when it needs to draw on the GUI  </summary>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

      label = EditorGUI.BeginProperty(position, label, property);
      Rect contentPosition = EditorGUI.PrefixLabel(position, label);
      EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("time"), GUIContent.none);
      EditorGUI.EndProperty();

    }

  }
}
#endif