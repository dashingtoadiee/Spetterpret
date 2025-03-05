using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using DuskModules;

/// <summary> Static Class containing extension methods for Unity Monobehaviours and objects </summary>
public static class UnityObjectExtensionMethods {

  /// <summary> Checks to see if given object is within layer mask. </summary>
  /// <param name="obj"> What object to check </param>
  /// <param name="mask"> What layermask </param>
  /// <returns> Whether the object is within the layermask or not </returns>
  public static bool IsInLayerMask(this GameObject obj, LayerMask mask) {
    return (mask.value & (1 << obj.layer)) > 0;
  }

  /// <summary> Copies layer of game object to another </summary>
  /// <param name="obj"> Object to change layer of </param>
  /// <param name="other"> GameObject to copy from </param>
  /// <param name="includeChildren"> Whether to include children </param>
  public static void CopyLayer(this GameObject obj, GameObject other, bool includeChildren = false) {
    obj.layer = other.layer;

    if (includeChildren) {
      for (int i = 0; i < obj.transform.childCount; i++) {
        CopyLayer(obj.transform.GetChild(i).gameObject, other, true);
      }
    }
  }

  /// <summary> Manually searches through the parents for the required component. </summary>
  /// <typeparam name="T"> Type to find </typeparam>
  /// <returns> Found component, if any </returns>
  public static T GetComponentInParentInactive<T>(this Behaviour behaviour) where T : Component {
    return GetComponentInParentInactive<T>(behaviour.gameObject);
  }

  /// <summary> Manually searches through the parents for the required component. </summary>
  /// <typeparam name="T"> Type to find </typeparam>
  /// <param name="obj"> Object to start with </param>
  /// <returns> Found component, if any </returns>
  public static T GetComponentInParentInactive<T>(this GameObject obj) where T : Component {
    Transform trf = obj.transform;
    while (trf != null) {
      T comp = trf.gameObject.GetComponent<T>();
      if (comp != null) return comp;
      trf = trf.parent;
    }
    return null;
  }

  /// <summary> Gets and returns a component, saving it in the set local variable </summary>
  /// <typeparam name="T"> Type of component to get (automatically set by compiler) </typeparam>
  /// <param name="gameObject"> GameObject to work with </param>
  /// <param name="local"> The found object </param>
  /// <returns> The found object </returns>
  public static T GetComponentCached<T>(this GameObject gameObject, ref T local) where T : Component {
    if (local == null) local = gameObject.GetComponent<T>();
    return local;
  }

  /// <summary> Gets and returns a component, saving it in the set local variable </summary>
  /// <typeparam name="T"> Type of component to get (automatically set by compiler) </typeparam>
  /// <param name="behaviour"> Behaviour to work with </param>
  /// <param name="local"> The found object </param>
  /// <returns> The found object </returns>
  public static T GetComponentCached<T>(this Behaviour behaviour, ref T local) where T : Component {
    if (local == null) local = behaviour.GetComponent<T>();
    return local;
  }

  /// <summary> Gets and returns a component, saving it in the set local variable </summary>
  /// <typeparam name="T"> Type of component to get (automatically set by compiler) </typeparam>
  /// <param name="gameObject"> GameObject to work with </param>
  /// <param name="local"> The found object </param>
  /// <returns> The found object </returns>
  public static T GetComponentInChildrenCached<T>(this GameObject gameObject, ref T local) where T : Component {
    if (local == null) local = gameObject.GetComponentInChildren<T>(true);
    return local;
  }

  /// <summary> Gets and returns a component, saving it in the set local variable </summary>
  /// <typeparam name="T"> Type of component to get (automatically set by compiler) </typeparam>
  /// <param name="behaviour"> Behaviour to work with </param>
  /// <param name="local"> The found object </param>
  /// <returns> The found object </returns>
  public static T GetComponentInChildrenCached<T>(this Behaviour behaviour, ref T local) where T : Component {
    if (local == null) local = behaviour.GetComponentInChildren<T>(true);
    return local;
  }

  /// <summary> Gets and returns a array of components, saving it in the set local variable </summary>
  /// <typeparam name="T"> Type of component to get (automatically set by compiler) </typeparam>
  /// <param name="gameObject"> GameObject to work with </param>
  /// <param name="local"> The found object </param>
  /// <returns> The found object </returns>
  public static List<T> GetComponentsInChildrenCached<T>(this GameObject gameObject, ref List<T> local) where T : Component {
    if (local == null) local = new List<T>(gameObject.GetComponentsInChildren<T>(true));
    return local;
  }

  /// <summary> Gets and returns a array of components, saving it in the set local variable </summary>
  /// <typeparam name="T"> Type of component to get (automatically set by compiler) </typeparam>
  /// <param name="behaviour"> Behaviour to work with </param>
  /// <param name="local"> The found object </param>
  /// <returns> The found object </returns>
  public static List<T> GetComponentsInChildrenCached<T>(this Behaviour behaviour, ref List<T> local) where T : Component {
    if (local == null) local = new List<T>(behaviour.GetComponentsInChildren<T>(true));
    return local;
  }

  /// <summary> Gets and returns a component in parent, saving it in the set local variable </summary>
  /// <typeparam name="T"> Type of component to get (automatically set by compiler) </typeparam>
  /// <param name="gameObject"> GameObject to work with </param>
  /// <param name="local"> The found object </param>
  /// <returns> The found object </returns>
  public static T GetComponentInParentCached<T>(this GameObject gameObject, ref T local) where T : Component {
    if (local == null) local = GetComponentInParentInactive<T>(gameObject);
    return local;
  }

  /// <summary> Gets and returns a component in parent, saving it in the set local variable </summary>
  /// <typeparam name="T"> Type of component to get (automatically set by compiler) </typeparam>
  /// <param name="behaviour"> Behaviour to work with </param>
  /// <param name="local"> The found object </param>
  /// <returns> The found object </returns>
  public static T GetComponentInParentCached<T>(this Behaviour behaviour, ref T local) where T : Component {
    if (local == null) local = GetComponentInParentInactive<T>(behaviour);
    return local;
  }

  /// <summary> Finds the component within the scene, checking each root object, then caches it in ref local. </summary>
  /// <typeparam name="T">Type of component to get</typeparam>
  /// <param name="behaviour"> Behaviour to work with </param>
  /// <param name="local"> The found object </param>
  /// <returns> The found object </returns>
  public static T GetComponentInSceneCached<T>(this Behaviour behaviour, ref T local) where T : Component {
    return behaviour.gameObject.GetComponentInSceneCached<T>(ref local);
  }

  /// <summary> Finds the component within the scene, checking each root object, then caches it in ref local. </summary>
  /// <typeparam name="T">Type of component to get</typeparam>
  /// <param name="gameObject"> GameObject to work with </param>
  /// <param name="local"> The found object </param>
  /// <returns> The found object </returns>
  public static T GetComponentInSceneCached<T>(this GameObject gameObject, ref T local) where T : Component {
    if (local == null) {
      GameObject[] objects = gameObject.scene.GetRootGameObjects();
      // First check each root object, because this kind of function is usually used to search for a main controller script
      for (int i = 0; i < objects.Length; i++) {
        local = objects[i].GetComponent<T>();
        if (local != null) return local;
      }
      // Then search deeper.
      for (int i = 0; i < objects.Length; i++) {
        for (int c = 0; c < objects[i].transform.childCount; c++) {
          local = objects[i].transform.GetChild(c).GetComponentInChildren<T>(true);
          if (local != null) return local;
        }
      }
    }
    return local;
  }

  /// <summary> Gets the correct time type for the behaviour, based on what type of transform it contains. </summary>
  /// <typeparam name="T"> Type to find </typeparam>
  /// <returns> The correct delta time </returns>
  public static TimeType GetTimeType(this Behaviour behaviour) {
    if (behaviour.transform.IsOfType(typeof(RectTransform)))
      return TimeType.interfaceDeltaTime;
    else
      return TimeType.deltaTime;
  }

  /// <summary> Gets the correct deltaTime for the behaviour, based on what type of transform it contains. Uses a cast and type check. </summary>
  /// <typeparam name="T"> Type to find </typeparam>
  /// <returns> The correct delta time </returns>
  public static float GetDeltaTime(this TimeType type) {
    switch (type) {
      case TimeType.deltaTime: return Time.deltaTime;
      case TimeType.interfaceDeltaTime: return DuskUtility.interfaceDeltaTime;
    }
    return Time.deltaTime;
  }

  /// <summary> Checks if given object is of the given type, or a subclass of </summary>
  /// <param name="obj"> What object to check </param>
  /// <param name="type"> What type to check for </param>
  /// <returns> True if it matches </returns>
  public static bool IsOfType(this object obj, System.Type type) {
    if (obj == null) return false;
    System.Type objType = obj.GetType();
    if (objType == type || objType.IsSubclassOf(type)) {
      return true;
    }
    return false;
  }

  /// <summary> Gets component only if it doesn't exist. </summary>
  /// <typeparam name="T"> Type of component to get </typeparam>
  /// <param name="gameObject"> GameObject to search </param>
  /// <param name="comp"> component to set </param>
  /// <param name="searchChildren"> whether to search for children </param>
  public static void ConfirmComponent<T>(this GameObject gameObject, ref T comp, bool create = false, bool searchChildren = true) where T : Component {
    if (comp == null) comp = searchChildren ? gameObject.GetComponentInChildren<T>(true) : gameObject.GetComponent<T>();
    if (comp == null && create) comp = gameObject.AddComponent<T>();
  }

  /// <summary> Creates an exact duplicate of any serializable object, do not us on non-serializable objects. </summary>
  /// <typeparam name="T"> The object type </typeparam>
  /// <param name="obj"> The actual object </param>
  /// <returns> The copy of the object </returns>
  public static T DeepClone<T>(this T obj) {
    if (!typeof(T).IsSerializable) {
      Debug.LogWarning("The type must be serializable, " + obj + " is not.");
      return default;
    }
    using (MemoryStream ms = new MemoryStream()) {
      BinaryFormatter formatter = new BinaryFormatter();
      formatter.Serialize(ms, obj);
      ms.Position = 0;
      return (T)formatter.Deserialize(ms);
    }
  }
}