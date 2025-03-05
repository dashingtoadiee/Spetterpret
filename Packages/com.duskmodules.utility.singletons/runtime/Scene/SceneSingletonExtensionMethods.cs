using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuskModules.Singletons;

/// <summary> Contains extension methods to more easily get scene singletons. </summary>
public static class SceneSingletonExtensionMethods {

  /// <summary> Finds the singleton within the scene. </summary>
  /// <typeparam name="T">Type of singleton to get</typeparam>
  /// <param name="behaviour"> Behaviour to work with </param>
  /// <returns> The found singleton </returns>
  public static T GetSingleton<T>(this Behaviour behaviour) where T : MonoBehaviour, ISingleton {
    return SceneSingleton<T>.GetInstance(behaviour.gameObject.scene);
  }

  /// <summary> Finds the singleton within the scene. </summary>
  /// <typeparam name="T">Type of singleton to get</typeparam>
  /// <param name="gameObject"> GameObject to work with </param>
  /// <returns> The found singleton </returns>
  public static T GetSingleton<T>(this GameObject gameObject) where T : MonoBehaviour, ISingleton {
    return SceneSingleton<T>.GetInstance(gameObject.scene);
  }

}
