#if UNITY_EDITOR
using System;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;

namespace DuskModules.DuskEditor {

  /// <summary> Custom editor for button </summary>
  [CanEditMultipleObjects]
  [CustomEditor(typeof(MonoBehaviour), true)]
  public class EditorButton : Editor {

    public override void OnInspectorGUI() {
      base.OnInspectorGUI();

      MonoBehaviour mono = (MonoBehaviour)target;
      IEnumerable<MemberInfo> methods = mono.GetType().
          GetMembers(BindingFlags.Instance | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).
          Where(o => Attribute.IsDefined(o, typeof(EditorButtonAttribute)));

      if (methods.Count() > 0) {
        GUILayout.Space(12);
      }

      foreach (MemberInfo memberInfo in methods) {
        object[] attributes = memberInfo.GetCustomAttributes(typeof(EditorButtonAttribute), true);
        EditorButtonAttribute attribute = (EditorButtonAttribute)attributes[0];

        bool preEnabled = GUI.enabled;
        if (attribute.runTimeOnly && !Application.isPlaying)
          GUI.enabled = false;

        if (GUILayout.Button(memberInfo.Name)) {
          MethodInfo method = (MethodInfo)memberInfo;
          method.Invoke(mono, null);
        }
        GUI.enabled = preEnabled;
      }
    }

  }
}
#endif