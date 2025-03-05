#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

// Never an end-point, and thus not in Editor namespace. Can be used by other modules.
namespace DuskModules {

  /// <summary> Basic editor list property, like layermask selection </summary>
  public class EditorListPropertyDrawer : PropertyDrawer {

    /// <summary> Current selected category </summary>
    string name;
    /// <summary> List of possible categories </summary>
    List<string> names;
    /// <summary> Current chosen index </summary>
    int choiceIndex = 0;

    /// <summary> Draws the property </summary>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
      SerializedProperty prop = property.FindPropertyRelative("name");
      names = GetNames();

      EditorGUI.BeginProperty(position, label, property);
      position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

      // Find current index
      int startIndex = names.IndexOf(prop.stringValue);
      if (startIndex == -1)
        startIndex = 0;

      int indent = EditorGUI.indentLevel;
      EditorGUI.indentLevel = 0;
      if (names.Count > 0) {
        choiceIndex = EditorGUI.Popup(position, startIndex, names.ToArray());
        prop.stringValue = names[choiceIndex];
        EditorGUI.indentLevel = indent;
      } else {
        EditorGUI.SelectableLabel(position, "Empty");
      }
    }

    /// <summary> Gets the names for the property drawer </summary>
    /// <returns> The possible names </returns>
    public virtual List<string> GetNames() {
      return new List<string>();
    }

    /// <summary> Gets the height of a certain property </summary>
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
      //Get the base height when not expanded
      float height = base.GetPropertyHeight(property, label);

      // if the property is expanded go thru all its children and get their height
      if (property.isExpanded) {
        IEnumerator propEnum = property.GetEnumerator();
        while (propEnum.MoveNext()) {
          height += EditorGUI.GetPropertyHeight((SerializedProperty)propEnum.Current, GUIContent.none, true);
        }
      }
      return height;
    }
  }
}
#endif