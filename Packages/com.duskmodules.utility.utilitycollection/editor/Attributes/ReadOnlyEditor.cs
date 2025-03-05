﻿#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace DuskModules.DuskEditor {

  /// <summary> Drawer for a seperator in editor window </summary>
  [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
  public class ReadOnlyEditor : PropertyDrawer {

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
      return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
      GUI.enabled = false;
      EditorGUI.PropertyField(position, property, label, true);
      GUI.enabled = true;
    }
  }
}
#endif