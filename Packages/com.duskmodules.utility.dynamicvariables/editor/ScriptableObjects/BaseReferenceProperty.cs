#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace DuskModules.DynamicVariables.DuskEditor {

  /// <summary> Custom property drawer for the any variable reference field </summary>
  [CustomPropertyDrawer(typeof(ValueReference<>), true)]
  public class BaseReferenceProperty : BoolTogglePropertyDrawer {

    /// <summary> Name of boolean property </summary>
    public override string GetBoolPropertyName() {
      return "useVariable";
    }

    /// <summary> Name for option to show for boolean true </summary>
    public override string GetBoolTrueName() {
      return "Variable";
    }

    /// <summary> Name for option to show for boolean false </summary>
    public override string GetBoolFalseName() {
      return "Constant";
    }

    /// <summary> Draw GUI if boolean is true </summary>
    public override void DrawTrueGUI(SerializedProperty property) {
      ExtraEditorUtility.TakeSpace();

      SerializedProperty propContentObj = property.FindPropertyRelative("contentObject");

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
    }

    /// <summary> Draw GUI if boolean is false </summary>
    public override void DrawFalseGUI(SerializedProperty property) {
      ExtraEditorUtility.TakeSpace();
      SerializedProperty propConstant = property.FindPropertyRelative("constant");
      ExtraEditorUtility.PropertyField(propConstant, null, propConstant.hasVisibleChildren);
    }

    /// <summary> Height of this property. </summary>
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
      SerializedProperty propUseVariable = property.FindPropertyRelative(GetBoolPropertyName());
      SerializedProperty propConstant = property.FindPropertyRelative("constant");
      SerializedProperty propContentObj = property.FindPropertyRelative("contentObject");

      float height = 0;
      float lines = 1;
      if (!propUseVariable.boolValue) {
        if (propConstant.isArray && propConstant.propertyType != SerializedPropertyType.String) {
          if (propConstant.isExpanded) {
            lines += 1.11111111f;

            for (int i = 0; i < propConstant.arraySize; i++) {
              height += EditorGUI.GetPropertyHeight(propConstant.GetArrayElementAtIndex(i));
              height += EditorGUIUtility.singleLineHeight * 0.11111111f;
            }
          }
        } else {
          lines -= 1;
#if !UNITY_2020_1_OR_NEWER
					if (property.Copy().CountInProperty() > 1) lines -= 1;
#endif
          height += EditorGUI.GetPropertyHeight(propConstant);
        }
        return height + EditorGUIUtility.singleLineHeight * lines;
      }
      return EditorGUI.GetPropertyHeight(propContentObj);
    }
  }
}
#endif