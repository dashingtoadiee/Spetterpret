#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace DuskModules.DynamicVariables.DuskEditor {

  /// <summary> Custom property drawer for the accelerated settings reference field </summary>
  [CustomPropertyDrawer(typeof(AcceleratedSettingsReference))]
  public class AcceleratedSettingsReferenceProperty : BaseReferenceProperty {

    /// <summary> Draw GUI if boolean is true </summary>
    public override void DrawFalseGUI(SerializedProperty property) {
      float labelWidth = EditorGUIUtility.labelWidth;

      EditorGUIUtility.labelWidth = labelWidth;

      // Next line
      EditorGUI.indentLevel++;
      Rect usePosition = new Rect(ExtraEditorUtility.ap.totalPosition);

      SerializedProperty constant = property.FindPropertyRelative("constant");
      usePosition.height = EditorGUIUtility.singleLineHeight;

      usePosition.y += EditorGUIUtility.singleLineHeight;
      EditorGUI.PropertyField(usePosition, constant.FindPropertyRelative("acceleration"), new GUIContent("Acceleration"));
      usePosition.y += EditorGUIUtility.singleLineHeight;
      EditorGUI.PropertyField(usePosition, constant.FindPropertyRelative("drag"), new GUIContent("Drag"));
      usePosition.y += EditorGUIUtility.singleLineHeight;
      EditorGUI.PropertyField(usePosition, constant.FindPropertyRelative("linearDrag"), new GUIContent("Linear Drag"));
      usePosition.y += EditorGUIUtility.singleLineHeight;
      EditorGUI.PropertyField(usePosition, constant.FindPropertyRelative("linearSpeed"), new GUIContent("Linear Speed"));

      EditorGUI.indentLevel--;
    }

    /// <summary> Draw GUI if boolean is false </summary>
    public override void DrawTrueGUI(SerializedProperty property) {
      ExtraEditorUtility.TakeSpace();
      SerializedProperty propContentObj = property.FindPropertyRelative("contentObject");

      EditorGUI.indentLevel--;
      System.Type valueType;
      if (fieldInfo.FieldType.BaseType.GetGenericArguments().Length > 0)
        valueType = fieldInfo.FieldType.BaseType.GetGenericArguments()[0];
      else
        valueType = fieldInfo.FieldType.GetGenericArguments()[0];

      System.Type interfaceType = typeof(IValue<>).MakeGenericType(valueType);

      GUIContent label = EditorGUI.BeginProperty(ExtraEditorUtility.ap.contentPosition, GUIContent.none, property);

      EditorGUI.BeginChangeCheck();
      Object selectedObject = EditorGUI.ObjectField(ExtraEditorUtility.ap.contentPosition, label, propContentObj.objectReferenceValue, interfaceType, true);
      if (EditorGUI.EndChangeCheck()) {
        propContentObj.objectReferenceValue = selectedObject;
      }

      EditorGUI.EndProperty();
      EditorGUI.indentLevel++;
    }

    /// <summary> Height of this property. </summary>
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
      SerializedProperty boolProp = property.FindPropertyRelative(GetBoolPropertyName());
      int lines = (boolProp.boolValue) ? 1 : 5;
      return EditorGUIUtility.singleLineHeight * lines;
    }

  }
}
#endif