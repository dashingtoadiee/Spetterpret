using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace DuskModules {

  /// <summary> Behaviour which sets the transparency sort mode and axis for a camera </summary>
  public class CameraSortingMode : MonoBehaviour {

    /// <summary> Camera to adjust sorting order of </summary>
    public Camera sortingCamera { get; protected set; }

    [Tooltip("Desired mode")]
    public TransparencySortMode sortMode;
    [Tooltip("Sorting axis")]
    public Vector3 sortAxis;

    // Set graphic settings
    private void Awake() {
      sortingCamera = gameObject.GetComponent<Camera>();
      sortingCamera.transparencySortMode = sortMode;
      sortingCamera.transparencySortAxis = sortAxis;
    }
  }

}