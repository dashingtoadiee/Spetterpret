using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace DuskModules {

  /// <summary> Reference wrapper to a type, set by monoscript editor object field </summary>
  [System.Serializable]
  public class TypeReference {
    public string typeName;
    public string typeFullName;

    private Type _type;
    public Type type {
      get {
        if (_type != null) return _type;
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) {
          _type = assembly.GetType(typeFullName);
          if (_type != null) return _type;
        }
        return null;
      }
    }
  }

}