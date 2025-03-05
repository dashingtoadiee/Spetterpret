#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DuskModules.SoundControl.DuskEditor {

  [CustomEditor(typeof(Sound))]
  [CanEditMultipleObjects]
  public class SoundEditor : Editor {

    // Cached properties
    private Sound sound;
    private SerializedProperty output;
    private SerializedProperty randomClip;
    private SerializedProperty clip;
    private SerializedProperty clips;
    private SerializedProperty volume;
    private SerializedProperty pitch;
    private SerializedProperty priority;
    private SerializedProperty stereoPan;
    private SerializedProperty spatialBlend;
    private SerializedProperty reverbZoneMix;
    private SerializedProperty dopplerLevel;
    private SerializedProperty rollOffMode;
    private SerializedProperty minDistance;
    private SerializedProperty maxDistance;
    private SerializedProperty rollOff;
    private SerializedProperty volumeVariation;
    private SerializedProperty pitchVariation;

    // Editor is enabled, initialize data.
    private void OnEnable() {
      sound = (Sound)target;

      output = serializedObject.FindProperty("_output");
      randomClip = serializedObject.FindProperty("_randomAudioClip");
      clip = serializedObject.FindProperty("_clip");
      clips = serializedObject.FindProperty("_clips");
      volume = serializedObject.FindProperty("_volume");
      pitch = serializedObject.FindProperty("_pitch");
      priority = serializedObject.FindProperty("_priority");
      stereoPan = serializedObject.FindProperty("_stereoPan");
      spatialBlend = serializedObject.FindProperty("_spatialBlend");
      reverbZoneMix = serializedObject.FindProperty("_reverbZoneMix");
      dopplerLevel = serializedObject.FindProperty("_dopplerLevel");
      rollOffMode = serializedObject.FindProperty("_rollOffMode");
      minDistance = serializedObject.FindProperty("_minDistance");
      maxDistance = serializedObject.FindProperty("_maxDistance");
      rollOff = serializedObject.FindProperty("_volumeRollOff");
      volumeVariation = serializedObject.FindProperty("_volumeVariation");
      pitchVariation = serializedObject.FindProperty("_pitchVariation");
    }

    /// <summary> Render the inspector GUI </summary>
    public override void OnInspectorGUI() {
      // Header
      ExtraEditorUtility.HeaderField("Sound");

      EditorGUI.BeginChangeCheck();

      // Help text
      EditorGUILayout.HelpBox("A one-shot sound asset used for sound effects, providing a single reference point to this sound. Call Play() to play the sound.", MessageType.Info);
      EditorGUILayout.Space();

      // Sound settings
      ExtraEditorUtility.HeaderField("Settings");
      EditorGUILayout.PropertyField(output);
      EditorGUILayout.Space();

      EditorGUILayout.PropertyField(randomClip);
      if (randomClip.boolValue)
        EditorGUILayout.PropertyField(clips, true);
      else {
        EditorGUILayout.PropertyField(clip);
      }
      EditorGUILayout.Space();

      EditorGUILayout.PropertyField(volume);
      EditorGUILayout.PropertyField(pitch);
      EditorGUILayout.PropertyField(priority);
      EditorGUILayout.PropertyField(stereoPan);
      EditorGUILayout.PropertyField(spatialBlend);
      EditorGUILayout.PropertyField(reverbZoneMix);

      if (spatialBlend.floatValue > 0) {
        EditorGUILayout.Space();
        ExtraEditorUtility.HeaderField("3D Settings");
        EditorGUILayout.PropertyField(dopplerLevel);
        EditorGUILayout.PropertyField(rollOffMode);
        if (rollOffMode.enumValueIndex != 2)
          EditorGUILayout.PropertyField(minDistance);
        EditorGUILayout.PropertyField(maxDistance);
        if (rollOffMode.enumValueIndex == 2)
          EditorGUILayout.PropertyField(rollOff);
      }
      EditorGUILayout.Space();

      ExtraEditorUtility.HeaderField("Variation");
      EditorGUILayout.PropertyField(volumeVariation);
      EditorGUILayout.PropertyField(pitchVariation);

      if (EditorGUI.EndChangeCheck()) {
        serializedObject.ApplyModifiedProperties();
        sound.SoundUpdated();
      }

      if (!sound.hasClip) {
        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("No AudioClip has been set, cannot play sound!", MessageType.Error);
      }
    }

  }

}
#endif