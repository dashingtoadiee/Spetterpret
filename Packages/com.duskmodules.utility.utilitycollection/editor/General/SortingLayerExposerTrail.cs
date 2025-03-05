#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;

namespace DuskModules.DuskEditor {

  /// <summary> Editor script for the sorting layer exposer </summary>
  [CanEditMultipleObjects()]
  [CustomEditor(typeof(TrailRenderer), true)]
  public class SortingLayerExposerTrail : Editor {

    /// <summary> List of layer names </summary>
    protected string[] layerNameOptions;
    /// <summary> Chosen layer name index </summary>
    protected int layerNameIndex = -1;

    /// <summary> Inspector GUI for this script </summary>
    public override void OnInspectorGUI() {
      base.OnInspectorGUI();

      TrailRenderer renderer = (TrailRenderer)target;

      // Find options and index
      string layerName = renderer.sortingLayerName;
      if (layerNameOptions == null) layerNameOptions = ExtraEditorUtility.GetSortingLayerNames();
      if (layerNameIndex == -1) {
        for (int i = 0; i < layerNameOptions.Length; i++) {
          if (layerName == layerNameOptions[i]) {
            layerNameIndex = i;
            break;
          }
        }
      }

      // Sorting Layer
      layerNameIndex = EditorGUILayout.Popup("Sorting Layer", layerNameIndex, layerNameOptions);
      if (layerNameOptions[layerNameIndex] != layerName) {
        renderer.sortingLayerName = layerNameOptions[layerNameIndex];
      }

      // Sorting order
      int sortingOrder = EditorGUILayout.IntField("Sorting Order", renderer.sortingOrder);
      if (renderer.sortingOrder != sortingOrder) {
        renderer.sortingOrder = sortingOrder;
      }
    }
  }
}
#endif