#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace DuskModules.DuskEditor {

  /// <summary> Custom property drawer for the TypeReference field </summary>
  [CustomPropertyDrawer(typeof(TypeReference))]
  public class TypeReferencePropertyDrawer : PropertyDrawer {

    /// <summary> Cache of scripts based on type names. </summary>
    private static Dictionary<string, MonoScript> scriptCache;

    /// <summary> Called when it needs to draw on the GUI  </summary>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
      SerializedProperty typeNameProperty = property.FindPropertyRelative("typeName");
      SerializedProperty typeFullNameProperty = property.FindPropertyRelative("typeFullName");

      // Load from property
      MonoScript script = null;
      string typeName = typeNameProperty.stringValue;
      if (!string.IsNullOrEmpty(typeName)) {
        if (scriptCache == null)
          scriptCache = new Dictionary<string, MonoScript>();

        scriptCache.TryGetValue(typeName, out script);
        if (script == null) {
          string[] guid = AssetDatabase.FindAssets(typeName);
          for (int i = 0; i < guid.Length; i++) {
            string path = AssetDatabase.GUIDToAssetPath(guid[i]);
            if (path.Contains(".cs")) {
              script = (MonoScript)AssetDatabase.LoadAssetAtPath(path, typeof(MonoScript));
              if (!scriptCache.ContainsKey(typeName))
                scriptCache.Add(typeName, script);
              else
                scriptCache[typeName] = script;
              break;
            }
          }
        }
      }

      // Wait for change, then apply
      script = (MonoScript)EditorGUI.ObjectField(position, label, script, typeof(MonoScript), false);
      if (GUI.changed) {
        if (script != null) {
          typeNameProperty.stringValue = script.GetClass().Name;
          typeFullNameProperty.stringValue = script.GetClass().FullName;
        } else {
          typeNameProperty.stringValue = "";
          typeFullNameProperty.stringValue = "";
        }
      }
    }

  }
}
#endif