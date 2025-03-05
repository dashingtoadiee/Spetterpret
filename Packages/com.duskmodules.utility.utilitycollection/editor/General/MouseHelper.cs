#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;

// Never an end-point, and thus not in Editor namespace. Can be used by other modules.
namespace DuskModules {

  /// <summary> This is used to find the mouse position when it's over a SceneView. </summary>
  /// <summary> Used by tools that are menu invoked. </summary>
  [InitializeOnLoad]
  public class MouseHelper : Editor {
    public static Vector2 position { get; private set; }

    static MouseHelper() {
      SceneView.duringSceneGui += UpdateView;
    }

    private static void UpdateView(SceneView sceneView) {
      if (Event.current != null)
        position = new Vector2(Event.current.mousePosition.x + sceneView.position.x, Event.current.mousePosition.y + sceneView.position.y);
    }
  }
}
#endif