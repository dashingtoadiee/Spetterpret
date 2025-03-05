#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DuskModules.SoundControl.DuskEditor {

  /// <summary> Custom property drawer for the MinMaxFloat field </summary>
  [CustomPropertyDrawer(typeof(SoundVariation))]
  public class SoundVariationProperty : PropertyDrawer {

    /// <summary> Called when it needs to draw on the GUI  </summary>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
      label = EditorGUI.BeginProperty(position, label, property);

      Rect contentPosition = EditorGUI.PrefixLabel(position, label);
      float totalWidth = contentPosition.width;
      EditorGUI.indentLevel = 0;

      // Toggle
      float toggleWidth = 20;
      contentPosition.width = toggleWidth;
      SerializedProperty use = property.FindPropertyRelative("useVariation");
      EditorGUI.PropertyField(contentPosition, use, GUIContent.none);
      contentPosition.x += contentPosition.width;

      // Display
      if (use.boolValue) {
        EditorGUIUtility.labelWidth = 26f;
        contentPosition.width = (totalWidth - toggleWidth) * 0.5f;
        EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("minimum"), new GUIContent("Min"));
        EditorGUIUtility.labelWidth = 30f;
        contentPosition.x += contentPosition.width;
        EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("maximum"), new GUIContent("Max"));
      }
      EditorGUI.EndProperty();
    }
  }
}
#endif