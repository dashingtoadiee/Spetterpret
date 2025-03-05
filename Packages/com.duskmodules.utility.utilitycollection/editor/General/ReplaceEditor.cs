#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DuskModules.DuskEditor {

  /// <summary> Editor window that replaces objects. </summary>
  public class ReplaceEditor : EditorWindow {

    [Tooltip("The objects to replace")]
    public List<GameObject> replace;
    [Tooltip("The prefab to replace all objects with")]
    public GameObject prefab;
    [Tooltip("Whether to keep the name of the original object.")]
    public bool keepName;

    private SerializedObject serializedObject;
    private SerializedProperty replaceProp;
    private SerializedProperty prefabProp;
    private SerializedProperty keepNameProp;

    [MenuItem("DuskModules/Tools/ReplaceObjects")]
    public static void OpenWindow() {
      ReplaceEditor replaceEditor = (ReplaceEditor)GetWindow(typeof(ReplaceEditor), false, "Replace Objects", true);
    }

    private void OnEnable() {
      if (replace == null)
        replace = new List<GameObject>();

      serializedObject = new SerializedObject(this);
      replaceProp = serializedObject.FindProperty("replace");
      prefabProp = serializedObject.FindProperty("prefab");
      keepNameProp = serializedObject.FindProperty("keepName");
    }

    void OnGUI() {
      EditorGUILayout.HelpBox("Enter objects in the list below and set a prefab to replace all objects with. " +
        "All objects will be removed, and prefabs will be spawned with their position, rotation and scale.", MessageType.Info);

      EditorGUI.BeginChangeCheck();

      EditorGUILayout.Space();
      GUILayout.Label("Replace Object", EditorStyles.boldLabel);
      EditorGUILayout.PropertyField(replaceProp, true);
      EditorGUILayout.PropertyField(prefabProp);
      EditorGUILayout.PropertyField(keepNameProp);

      EditorGUILayout.Space();
      if (GUILayout.Button("Replace")) {
        AttemptReplace();
      }

      if (EditorGUI.EndChangeCheck()) {
        serializedObject.ApplyModifiedProperties();
      }
    }

    private void AttemptReplace() {
      if (replace.Count == 0) {
        EditorUtility.DisplayDialog("Empty list", "There aren't any objects to replace!", "Oops.");
        return;
      }
      if (prefab == null) {
        EditorUtility.DisplayDialog("No Prefab", "There is no prefab set to replace objects with!", "Oops.");
        return;
      }

      if (EditorUtility.DisplayDialog("Replace all?", "Are you sure you want to replace all objects with the prefab? This cannot be undone!", "Proceed", "Cancel")) {
        Replace();
      }
    }

    private void Replace() {
      for (int i = 0; i < replace.Count; i++) {
        Transform point = replace[i].transform;
        GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        obj.transform.SetParent(point.parent);
        obj.transform.position = point.position;
        obj.transform.rotation = point.rotation;
        obj.transform.localScale = point.localScale;
        obj.name = keepName ? point.gameObject.name : prefab.name;
        DestroyImmediate(replace[i]);
      }

      replaceProp.ClearArray();
    }

  }
}
#endif