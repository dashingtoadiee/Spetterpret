#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace DuskModules.DuskEditor {

  /// <summary> Contains useful functions for editor windows </summary>
  public class EditorListWindow {

    /// <summary> Shows a category list. </summary>
    /// <param name="selection"> The selection list </param>
    public static void EditorList(string header, ref List<string> names, ref List<bool> selection) {
      if (names == null) names = new List<string>();
      if (selection == null) selection = new List<bool>();
      while (selection.Count < names.Count) selection.Add(false);
      while (selection.Count > names.Count) selection.RemoveAt(selection.Count - 1);

      EditorGUILayout.LabelField(header, EditorStyles.boldLabel);

      for (int i = 0; i < names.Count; i++) {
        EditorGUILayout.BeginHorizontal();
        names[i] = EditorGUILayout.TextField(names[i]);
        selection[i] = EditorGUILayout.Toggle(selection[i], GUILayout.Width(50));
        EditorGUILayout.EndHorizontal();
      }
      EditorGUILayout.BeginHorizontal();
      if (GUILayout.Button("Add")) {
        names.Add("new");
      }
      if (GUILayout.Button("Delete Selected")) {
        for (int i = selection.Count - 1; i >= 0; i--) {
          if (selection[i]) {
            selection.RemoveAt(i);
            names.RemoveAt(i);
          }
        }
      }
      EditorGUILayout.EndHorizontal();
      EditorGUILayout.Space();
    }
  }
}
#endif