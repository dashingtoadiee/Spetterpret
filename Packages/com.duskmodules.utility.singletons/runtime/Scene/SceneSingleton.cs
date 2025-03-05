using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DuskModules.Singletons {

  /// <summary> Behaviour of which there is only one within a given scene, able to be referenced by any other object in that same scene. </summary>
  public class SceneSingleton<T> : MonoBehaviour, ISingleton where T : MonoBehaviour, ISingleton {

    /// <summary> All scene singletons of this type </summary>
    protected static Dictionary<Scene, T> sceneSingletons;

    /// <summary> Gets the instance for the active scene. </summary>
    public static T instance => GetInstance(SceneManager.GetActiveScene());
    /// <summary> Get whether it exists for the active scene. </summary>
    public static bool exists => Exists(SceneManager.GetActiveScene());

    /// <summary> Whether it is setup </summary>
    public bool isSetup { get; protected set; }

    /// <summary> Gets whether a singleton exists for the given component </summary>
    public static bool Exists(Component component) {
      return Exists(component.gameObject.scene);
    }
    /// <summary> Gets whether a singleton exists for the given scene </summary>
    public static bool Exists(Scene scene) {
      return sceneSingletons != null && sceneSingletons.ContainsKey(scene);
    }

    /// <summary> Gets instance of this singleton for the scene of this component </summary>
    public static T GetInstance(Component component) {
      return GetInstance(component.gameObject.scene);
    }
    /// <summary> Gets the instance of this singleton type based on the component's scene </summary>
    public static T GetInstance(Scene scene) {
      if (sceneSingletons == null)
        sceneSingletons = new Dictionary<Scene, T>();

      T found = null;
      if (sceneSingletons.TryGetValue(scene, out found)) {
        return found;
      }

      GameObject[] objects = scene.GetRootGameObjects();
      for (int i = 0; i < objects.Length; i++) {
        found = objects[i].GetComponentInChildren<T>(true);
        if (found != null)
          break;
      }

      if (found != null) {
        found.CheckSetup();
        return found;
      }

      Debug.LogWarning($"WARNING: Scene Singleton of type {typeof(T).Name} could not be found! Add it to the scene, or remove the script looking for it.");
      return null;
    }

    /// <summary> Awaken, setup as instance in this scene </summary>
    protected virtual void Awake() {
      CheckSetup();
    }

    /// <summary> Checks setup of this singleton </summary>
    public void CheckSetup() {
      if (!isSetup) {
        if (sceneSingletons == null) sceneSingletons = new Dictionary<Scene, T>();
        if (sceneSingletons.ContainsKey(gameObject.scene)) {
          Debug.LogWarning($"WARNING: Duplicate Scene Singleton found of type {typeof(T).Name} on object {gameObject.name}! The duplicate component is deleted.");
          Destroy(this);
          return;
        } else {
          sceneSingletons.Add(gameObject.scene, gameObject.GetComponent<T>());
        }

        isSetup = true;
        Setup();
      }
    }

    /// <summary> Called when the singleton must setup properties. Called before instance is returned, and only once. </summary>
    protected virtual void Setup() {

    }

    /// <summary> When destroyed, remove from index </summary>
    protected virtual void OnDestroy() {
      sceneSingletons.Remove(gameObject.scene);
    }

  }
}