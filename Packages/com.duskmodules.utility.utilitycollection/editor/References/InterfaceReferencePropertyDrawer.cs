#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace DuskModules {

  /// <summary> Display a Interface Reference object in the editor. </summary>
  [CustomPropertyDrawer(typeof(InterfaceReference<>), true)]
  public class InterfaceReferencePropertyDrawer : PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
      SerializedProperty contentProperty = property.FindPropertyRelative("contentObject");

      System.Type interfaceType = null;
      if (fieldInfo.FieldType.BaseType.GetGenericArguments().Length > 0)
        interfaceType = fieldInfo.FieldType.BaseType.GetGenericArguments()[0];
      else
        interfaceType = fieldInfo.FieldType.GetGenericArguments()[0];

      label = EditorGUI.BeginProperty(position, label, property);
      EditorGUI.BeginChangeCheck();

      Object selectedObject = EditorGUI.ObjectField(position, label, contentProperty.objectReferenceValue, interfaceType, true);

      if (EditorGUI.EndChangeCheck()) {
        contentProperty.objectReferenceValue = selectedObject;
      }

      EditorGUI.EndProperty();
    }
  }
}
#endif