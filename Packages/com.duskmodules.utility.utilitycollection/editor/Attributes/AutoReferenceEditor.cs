#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;

namespace DuskModules {

  /// <summary> Property drawer of auto reference. </summary>
  [CustomPropertyDrawer(typeof(AutoReferenceAttribute))]
  public class AutoReferenceEditor : PropertyDrawer {

    // OnGUI
    public override void OnGUI(Rect position, SerializedProperty serializedProperty, GUIContent label) {

      AutoReferenceAttribute autoRef = (AutoReferenceAttribute)attribute;

      GameObject parentGameObject = ((MonoBehaviour)serializedProperty.serializedObject.targetObject).gameObject;
      Component comp = (Component)serializedProperty.objectReferenceValue;

      // Automatically set the property if it is not set, using GetComponentInChildren.
      if (Selection.objects.Length <= 1 && serializedProperty.objectReferenceValue == null
        || (autoRef.disabled && comp.gameObject != parentGameObject)
        || (!autoRef.disabled && IsParentOf(comp.transform, parentGameObject.transform))) {

        Type parentType = serializedProperty.serializedObject.targetObject.GetType();
        Type type = parentType.GetField(serializedProperty.propertyPath).FieldType;

        UnityEngine.Object o = autoRef.disabled ? parentGameObject.GetComponent(type) : parentGameObject.GetComponentInChildren(type);
        serializedProperty.objectReferenceValue = o;

        // Or create it, if still missing.
        if (autoRef.autoCreate && serializedProperty.objectReferenceValue == null) {
          serializedProperty.objectReferenceValue = parentGameObject.AddComponent(type);
        }
      }

      // Allow custom editing or not
      EditorGUI.BeginDisabledGroup(autoRef.disabled);
      EditorGUI.PropertyField(position, serializedProperty, label, true);
      EditorGUI.EndDisabledGroup();
    }

    private bool IsParentOf(Transform c, Transform p) {
      if (c == p) return true;
      if (c.parent != null) return IsParentOf(c.parent, p);
      return false;
    }

  }
}
#endif