#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DuskModules.DynamicVariables.DuskEditor {

  /// <summary> Custom property drawer for the MinMaxFloat field </summary>
  [CustomPropertyDrawer(typeof(MorphingValue))]
  public class MorphingValueProperty : PropertyDrawer {

    /// <summary> Called when it needs to draw on the GUI  </summary>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
      ExtraEditorUtility.BeginProperty(position, GUIContent.none, property);

      property.isExpanded = EditorGUI.Foldout(new Rect(position.x, position.y, 40, EditorGUIUtility.singleLineHeight), property.isExpanded, label, true);
      if (property.isExpanded) {
        ExtraEditorUtility.PropertyFieldLine(property.FindPropertyRelative("randomTarget"), new GUIContent("Target", "Random value to choose target from."));
        ExtraEditorUtility.PropertyFieldLine(property.FindPropertyRelative("randomTime"), new GUIContent("Time", "Randomized interval for when to choose a new target."));
        ExtraEditorUtility.PropertyFieldLine(property.FindPropertyRelative("speed"), new GUIContent("Speed", "Speed with which to move value to target value."));
        ExtraEditorUtility.PropertyFieldLine(property.FindPropertyRelative("value"), new GUIContent("V", "Current value, which moves to match target value."));
      }

      EditorGUI.EndProperty();
    }

    /// <summary> Height of this property. </summary>
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
      return EditorGUIUtility.singleLineHeight * (property.isExpanded ? 5 : 1);
    }
  }

}
#endif