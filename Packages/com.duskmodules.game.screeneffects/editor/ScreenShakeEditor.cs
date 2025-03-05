#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DuskModules.ScreenEffects.DuskEditor {

  [CustomEditor(typeof(ScreenShake))]
  [CanEditMultipleObjects]
  public class ScreenShakeEditor : Editor {

    // Cached properties
    private ScreenShake shake;
    private SerializedProperty frequency;
    private SerializedProperty strength;
    private SerializedProperty time;
    private SerializedProperty strengthMultiplierOverTime;
    private SerializedProperty frequencyVariation;
    private SerializedProperty strengthVariation;
    private SerializedProperty spatialBlend;
    private SerializedProperty minDistance;
    private SerializedProperty maxDistance;
    private SerializedProperty strengthRollOff;
    private SerializedProperty strengthVector;
    private SerializedProperty frequencyVector;
    private SerializedProperty direction;

    // Editor is enabled, initialize data.
    private void OnEnable() {
      shake = (ScreenShake)target;

      frequency = serializedObject.FindProperty("_frequency");
      strength = serializedObject.FindProperty("_strength");
      time = serializedObject.FindProperty("_time");
      strengthMultiplierOverTime = serializedObject.FindProperty("_strengthMultiplierOverTime");
      frequencyVariation = serializedObject.FindProperty("_frequencyVariation");
      strengthVariation = serializedObject.FindProperty("_strengthVariation");
      spatialBlend = serializedObject.FindProperty("_spatialBlend");
      minDistance = serializedObject.FindProperty("_minDistance");
      maxDistance = serializedObject.FindProperty("_maxDistance");
      strengthRollOff = serializedObject.FindProperty("_strengthRollOff");
      strengthVector = serializedObject.FindProperty("_strengthVector");
      frequencyVector = serializedObject.FindProperty("_frequencyVector");
      direction = serializedObject.FindProperty("_direction");
    }

    /// <summary> Render the inspector GUI </summary>
    public override void OnInspectorGUI() {
      EditorGUI.BeginChangeCheck();

      // ScreenShake settings
      ExtraEditorUtility.HeaderField("ScreenShake");
      EditorGUILayout.PropertyField(frequency);
      EditorGUILayout.PropertyField(strength);
      EditorGUILayout.PropertyField(time);
      EditorGUILayout.PropertyField(strengthMultiplierOverTime);
      EditorGUILayout.Space();

      ExtraEditorUtility.HeaderField("Variation");
      EditorGUILayout.PropertyField(frequencyVariation);
      EditorGUILayout.PropertyField(strengthVariation);
      EditorGUILayout.Space();

      EditorGUILayout.PropertyField(spatialBlend);
      if (spatialBlend.floatValue > 0) {
        EditorGUILayout.Space();
        ExtraEditorUtility.HeaderField("3D Settings");
        EditorGUILayout.PropertyField(minDistance);
        EditorGUILayout.PropertyField(maxDistance);
        EditorGUILayout.PropertyField(strengthRollOff);
      }
      EditorGUILayout.Space();

      ExtraEditorUtility.HeaderField("Vector");
      EditorGUILayout.PropertyField(strengthVector);
      EditorGUILayout.PropertyField(frequencyVector);
      EditorGUILayout.PropertyField(direction);

      if (EditorGUI.EndChangeCheck()) {
        serializedObject.ApplyModifiedProperties();
      }

      bool previous = GUI.enabled;
      GUI.enabled = Application.isPlaying;
      if (GUILayout.Button("Play")) {
        shake.Play();
      }
      GUI.enabled = previous;
    }
  }
}
#endif