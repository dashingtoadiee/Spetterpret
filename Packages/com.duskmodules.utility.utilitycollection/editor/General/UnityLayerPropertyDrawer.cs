#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DuskModules.DuskEditor {

  /// <summary> Property drawer of Unity Layer </summary>
  [CustomPropertyDrawer(typeof(UnityLayer))]
  public class UnityLayerPropertyDrawer : PropertyDrawer {

    /// <summary> OnGUI </summary>
    public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label) {
      EditorGUI.BeginProperty(_position, GUIContent.none, _property);
      SerializedProperty layerIndex = _property.FindPropertyRelative("_layerIndex");
      _position = EditorGUI.PrefixLabel(_position, GUIUtility.GetControlID(FocusType.Passive), _label);
      if (layerIndex != null) {
        layerIndex.intValue = EditorGUI.LayerField(_position, layerIndex.intValue);
      }
      EditorGUI.EndProperty();
    }

  }
}
#endif