using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules {

  /// <summary> The frame rate limiter script that simply sets a target frame rate on awake </summary>
  public class FrameRateLimiter : MonoBehaviour {

    /// <summary> Frame rate to apply </summary>
    public int targetFrameRate = 30;

    // Awaken and set framerate
    private void Awake() {
      Application.targetFrameRate = targetFrameRate;
    }
  }
}