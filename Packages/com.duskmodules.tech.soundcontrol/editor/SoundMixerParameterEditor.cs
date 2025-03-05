#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

namespace DuskModules.SoundControl.DuskEditor {

  /// <summary> Custom editor of sound mixer parameter </summary>
  [CustomEditor(typeof(SoundMixerParameter))]
  public class SoundMixerParameterEditor : Editor {

    private SoundMixerParameter parameter;
    private SerializedProperty mixer;
    private SerializedProperty parameterName;
    private SerializedProperty isVolume;
    private SerializedProperty persisted;
    private SerializedProperty instant;
    private SerializedProperty speed;
    private SerializedProperty minimum;
    private SerializedProperty maximum;
    private SerializedProperty multiTargetMode;

    // Editor is enabled, initialize data.
    private void OnEnable() {
      parameter = (SoundMixerParameter)target;
      mixer = serializedObject.FindProperty("mixer");
      parameterName = serializedObject.FindProperty("parameterName");
      isVolume = serializedObject.FindProperty("isVolume");
      persisted = serializedObject.FindProperty("persisted");
      instant = serializedObject.FindProperty("instant");
      speed = serializedObject.FindProperty("speed");
      minimum = serializedObject.FindProperty("minimum");
      maximum = serializedObject.FindProperty("maximum");
      multiTargetMode = serializedObject.FindProperty("multiTargetMode");
    }

    /// <summary> Render the inspector GUI </summary>
    public override void OnInspectorGUI() {

      EditorGUI.BeginChangeCheck();
      EditorGUILayout.PropertyField(mixer);
      EditorGUILayout.PropertyField(parameterName);
      EditorGUILayout.PropertyField(isVolume);
      EditorGUILayout.PropertyField(persisted);
      EditorGUILayout.PropertyField(instant);
      if (!instant.boolValue)
        EditorGUILayout.PropertyField(speed);
      if (!isVolume.boolValue) {
        EditorGUILayout.PropertyField(minimum);
        EditorGUILayout.PropertyField(maximum);
      }
      if (!persisted.boolValue)
        EditorGUILayout.PropertyField(multiTargetMode);

      // Apply
      if (EditorGUI.EndChangeCheck()) {
        serializedObject.ApplyModifiedProperties();
      }
    }

  }
}

#endif