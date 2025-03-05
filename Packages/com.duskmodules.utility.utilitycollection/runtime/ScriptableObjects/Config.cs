using UnityEngine;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif

namespace DuskModules {

  /// <summary> Config file which automatically instantiates itself in the Resources folder if it doesn't exist yet. </summary>
  /// <typeparam name="C"> Type of config </typeparam>
  public abstract class Config<C> : ScriptableObject where C : Config<C> {

    /// <summary> Finds and gets the .asset in the resources of this config type </summary>
    public static C instance {
      get {
        if (_instance != null && Application.isPlaying)
          return _instance;

        string cName = typeof(C).Name;
        C config = Resources.Load<C>("Config/" + cName);

        // If config cannot be found, create it.
        if (config == null) {
          config = CreateInstance<C>();

#if UNITY_EDITOR
          // Create asset in default resources folder. It can be moved to a different one if need be.
          string properPath = Path.Combine(Application.dataPath, "Resources", "Config");
          if (!Directory.Exists(properPath)) {
            if (!AssetDatabase.IsValidFolder(Path.Combine("Assets", "Resources")))
              AssetDatabase.CreateFolder("Assets", "Resources");
            AssetDatabase.CreateFolder("Assets/Resources", "Config");
          }
          string fullPath = Path.Combine(Path.Combine("Assets", "Resources", "Config"), cName + ".asset");
          AssetDatabase.CreateAsset(config, fullPath);
#endif
        }
        _instance = config;
        return config;
      }
    }

    // Cached instance
    private static C _instance;

    /// <summary> Opens this configuration. </summary>
    protected static void OpenConfigFile() {
#if UNITY_EDITOR
      Selection.activeObject = instance;
#endif
    }

#if UNITY_EDITOR
    //[MenuItem("DuskModules/Settings/Config")]
    //public static void OpenConfig() { OpenConfigFile(); }
#endif


  }

}