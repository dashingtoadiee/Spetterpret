#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DuskModules {

  public class TypeUtility : MonoBehaviour {

    public static object GetObject(SerializedProperty prop) {
      string path = prop.propertyPath.Replace(".Array.data[", "[");
      object obj = prop.serializedObject.targetObject;
      string[] elements = path.Split('.');
      foreach (string element in elements.Take(elements.Length)) {
        if (element.Contains("[")) {
          string elementName = element.Substring(0, element.IndexOf("["));
          int index = Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
          obj = GetValue(obj, elementName, index);
        } else {
          obj = GetValue(obj, element);
        }
      }
      return obj;
    }

    public static object GetValue(object source, string name) {
      if (source == null)
        return null;
      var type = source.GetType();
      var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
      if (f == null) {
        var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
        if (p == null)
          return null;
        return p.GetValue(source, null);
      }
      return f.GetValue(source);
    }

    public static object GetValue(object source, string name, int index) {
      var enumerable = GetValue(source, name) as IEnumerable;
      var enm = enumerable.GetEnumerator();
      while (index-- >= 0)
        enm.MoveNext();
      return enm.Current;
    }

  }

}
#endif