#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering;
using static UnityEditor.EditorGUI;

namespace DuskModules.DuskEditor {

  /// <summary> Custom property exposing the sorting layer setting. </summary>
  [CustomPropertyDrawer(typeof(SortingLayerSetting))]
  public class SortingLayerSettingPropertyDrawer : PropertyDrawer {

    /// <summary> Called when it needs to draw on the GUI  </summary>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
      SerializedProperty layerNameProp = property.FindPropertyRelative("layerName");

      // Find options and index
      string layerName = layerNameProp.stringValue;
      SortingLayer[] options = SortingLayer.layers;
      int index = -1;
      for (int i = 0; i < options.Length; i++) {
        if (layerName == options[i].name) {
          index = i;
          break;
        }
      }

      if (index == -1) {
        for (int i = 0; i < options.Length; i++) {
          if (options[i].name == "Default") {
            index = i;
            break;
          }
        }
      }

      GUIContent[] contentOptions = new GUIContent[options.Length];
      for (int i = 0; i < options.Length; i++) {
        contentOptions[i] = new GUIContent(options[i].name);
      }

      // Sorting Layer
      index = EditorGUI.Popup(position, new GUIContent(property.displayName, property.tooltip), index, contentOptions);
      if (options[index].name != layerName) {
        layerNameProp.stringValue = options[index].name;
      }
    }
  }
}
#endif