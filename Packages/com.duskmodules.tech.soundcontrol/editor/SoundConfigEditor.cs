#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

namespace DuskModules.SoundControl.DuskEditor {

  [CustomEditor(typeof(SoundConfig))]
  public class SoundConfigEditor : Editor {

    // Cached properties
    private SoundConfig config;
    private SerializedProperty audioMixers;
    private SerializedProperty playerPreferencesKey;
    private SerializedProperty parameters;
    private SerializedProperty conflictParameters;
    private SerializedProperty undeterminedParameters;
    private SerializedProperty undeterminedMixers;

    private bool applyFlag;
    private bool reSync;

    // Editor is enabled, initialize data.
    private void OnEnable() {
      config = (SoundConfig)target;
      audioMixers = serializedObject.FindProperty("_audioMixers");
      playerPreferencesKey = serializedObject.FindProperty("playerPreferencesKey");
      parameters = serializedObject.FindProperty("parameters");
      conflictParameters = serializedObject.FindProperty("conflictParameters");
      undeterminedParameters = serializedObject.FindProperty("undeterminedParameters");
      undeterminedMixers = serializedObject.FindProperty("undeterminedMixers");

      // Run a sync on enable.
      SyncParamObjects();
    }

    /// <summary> Render the inspector GUI </summary>
    public override void OnInspectorGUI() {
      applyFlag = false;

      ExtraEditorUtility.HeaderField("Sound Configuration");
      EditorGUI.BeginChangeCheck();
      EditorGUILayout.PropertyField(audioMixers, true);
      EditorGUILayout.PropertyField(playerPreferencesKey, true);

      // Show Sync Button
      if (GUILayout.Button("Sync", GUILayout.Width(105)) || reSync) {
        reSync = false;
        SyncParamObjects();
      }

      // Show parameter setting buttons
      if (conflictParameters.arraySize > 0) {
        ExtraEditorUtility.HeaderField("Sound Parameter Conflicts");
        List<SoundMixerParameter> conflict = new List<SoundMixerParameter>();
        for (int i = 0; i < conflictParameters.arraySize; i++) {
          SerializedProperty prop = conflictParameters.GetArrayElementAtIndex(i);
          conflict.Add((SoundMixerParameter)prop.objectReferenceValue);
        }

        // Run in new list, as original can be changed during
        for (int i = 0; i < conflict.Count; i++) {
          ShowConflictParameter(i, conflict[i]);
        }
      }

      // Apply
      if (EditorGUI.EndChangeCheck() || applyFlag) {
        serializedObject.ApplyModifiedProperties();
      }
    }

    // Syncs parameter objects
    private void SyncParamObjects() {
      bool change = false;

      // Clear
      parameters.ClearArray();
      conflictParameters.ClearArray();
      undeterminedParameters.ClearArray();
      undeterminedMixers.ClearArray();

      // Collect new params
      List<KeyValue<AudioMixer, string>> newParams = new List<KeyValue<AudioMixer, string>>();
      List<KeyValue<AudioMixer, string>> allParams = new List<KeyValue<AudioMixer, string>>();
      for (int i = 0; i < audioMixers.arraySize; i++) {
        SerializedProperty mixer = audioMixers.GetArrayElementAtIndex(i);
        AudioMixer mixerObj = (AudioMixer)mixer.objectReferenceValue;
        Array mixerParameters = (Array)mixer.objectReferenceValue.GetType().GetProperty("exposedParameters").GetValue(mixer.objectReferenceValue);

        for (int p = 0; p < mixerParameters.Length; p++) {
          var o = mixerParameters.GetValue(p);
          string name = (string)o.GetType().GetField("name").GetValue(o);
          KeyValue<AudioMixer, string> param = new KeyValue<AudioMixer, string>(mixerObj, name);
          newParams.Add(param);
          allParams.Add(param);
        }
      }

      // Collect objects
      List<SoundMixerParameter> paramObjects = new List<SoundMixerParameter>();
      List<SoundMixerParameter> oldObjects = new List<SoundMixerParameter>();
      UnityEngine.Object[] objs = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(config));
      for (int i = 0; i < objs.Length; i++) {
        if (objs[i].IsOfType(typeof(SoundMixerParameter))) {
          SoundMixerParameter paramObj = (SoundMixerParameter)objs[i];
          SerializedObject paramSerialized = new SerializedObject(objs[i]);
          SerializedProperty nameProperty = paramSerialized.FindProperty("m_Name");

          ExtraEditorUtility.InsertArrayElement(parameters, paramObj);

          // Check if is contained
          bool contained = false;
          for (int p = 0; p < newParams.Count; p++) {
            if (paramObj.mixer == newParams[p].key && paramObj.parameterName == newParams[p].value) {
              contained = true;

              string newName = newParams[p].key.name + "_" + newParams[p].value;
              if (audioMixers.arraySize == 1)
                newName = newParams[p].value;

              // Auto rename object if found
              if (nameProperty.stringValue != newName) {
                nameProperty.stringValue = newName;
                paramSerialized.ApplyModifiedProperties();
              }

              newParams.RemoveAt(p);  // Claim param
              break;
            }
          }

          if (!contained) {
            oldObjects.Add(paramObj);
            ExtraEditorUtility.InsertArrayElement(conflictParameters, paramObj);
          } else
            paramObjects.Add(paramObj);
        }
      }

      // Simple addition
      if (newParams.Count > 0 && oldObjects.Count == 0) {
        for (int i = 0; i < newParams.Count; i++) {
          AddParamObject(newParams[i]);
        }
        change = true;
      } else if (newParams.Count > 0 && oldObjects.Count > 0) {
        // Conflicts
        for (int i = 0; i < newParams.Count; i++) {
          ExtraEditorUtility.InsertArrayElement(undeterminedMixers, newParams[i].key);
          ExtraEditorUtility.InsertArrayElement(undeterminedParameters, newParams[i].value);
        }
      }

      // If anything changed, confirm main asset
      if (change) {
        ConfirmMainAsset();
      }
      applyFlag = true;
    }

    // Shows conflict parameter
    private void ShowConflictParameter(int index, SoundMixerParameter paramObj) {
      EditorGUILayout.BeginHorizontal();
      EditorGUI.indentLevel++;
      EditorGUILayout.LabelField((index + 1) + "", GUILayout.Width(48));
      EditorGUI.indentLevel--;

      EditorGUI.BeginDisabledGroup(true);
      EditorGUILayout.ObjectField(paramObj, typeof(SoundMixerParameter), false);
      EditorGUI.EndDisabledGroup();

      if (EditorGUILayout.DropdownButton(new GUIContent("Action"), FocusType.Keyboard)) {
        GenericMenu menu = new GenericMenu();
        for (int i = 0; i < undeterminedParameters.arraySize; i++) {
          string paramName = undeterminedParameters.GetArrayElementAtIndex(i).stringValue;
          menu.AddItem(new GUIContent(paramName), false, handleItemClicked, i);
        }
        menu.AddItem(new GUIContent("Remove"), false, handleItemClicked, -1);
        menu.ShowAsContext();
      }
      EditorGUILayout.EndHorizontal();

      void handleItemClicked(object parameter) {
        int choice = (int)parameter;
        if (choice < 0) {
          RemoveParamObject(paramObj);
        } else {
          MatchParamObject(paramObj, choice);
        }
        applyFlag = true;
      }
    }

    // Remove the object
    private void RemoveParamObject(SoundMixerParameter soundMixerParameter) {
      AssetDatabase.RemoveObjectFromAsset(soundMixerParameter);
      AssetDatabase.SaveAssets();
      AssetDatabase.Refresh();
      reSync = true;
    }

    // Matches the param object with the given param, removing both.
    private void MatchParamObject(SoundMixerParameter paramObj, int index) {
      SerializedObject paramSerialized = new SerializedObject(paramObj);
      SerializedProperty nameProperty = paramSerialized.FindProperty("m_Name");
      SerializedProperty mixerProperty = paramSerialized.FindProperty("mixer");
      SerializedProperty parameterProperty = paramSerialized.FindProperty("parameterName");

      string n = undeterminedParameters.GetArrayElementAtIndex(index).stringValue;
      AudioMixer m = (AudioMixer)undeterminedMixers.GetArrayElementAtIndex(index).objectReferenceValue;

      if (audioMixers.arraySize == 1)
        nameProperty.stringValue = n;
      else
        nameProperty.stringValue = m.name + "_" + n;
      mixerProperty.objectReferenceValue = m;
      parameterProperty.stringValue = n;
      paramSerialized.ApplyModifiedProperties();

      reSync = true;
    }

    // Adds new ParamObject
    private void AddParamObject(KeyValue<AudioMixer, string> keyValue) {
      SoundMixerParameter keyObj = CreateInstance<SoundMixerParameter>();
      if (audioMixers.arraySize == 1)
        keyObj.name = keyValue.value;
      else
        keyObj.name = keyValue.key.name + "_" + keyValue.value;
      keyObj.mixer = keyValue.key;
      keyObj.parameterName = keyValue.value;
      AssetDatabase.AddObjectToAsset(keyObj, config);
      ExtraEditorUtility.InsertArrayElement(parameters, keyObj);
    }

    // Forces an editor window refresh by adding, refreshing and removing an object.
    private void ConfirmMainAsset() {
      AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(config), ImportAssetOptions.ForceUpdate);

      SoundMixerParameter updateObj = CreateInstance<SoundMixerParameter>();
      AssetDatabase.AddObjectToAsset(updateObj, config);

      AssetDatabase.SaveAssets();

      AssetDatabase.RemoveObjectFromAsset(updateObj);
      DestroyImmediate(updateObj);

      AssetDatabase.SetMainObject(target, AssetDatabase.GetAssetPath(target));
      AssetDatabase.SaveAssets();
      AssetDatabase.Refresh();
    }
  }

}
#endif