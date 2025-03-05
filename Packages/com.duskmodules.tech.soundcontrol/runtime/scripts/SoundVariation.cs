using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules.SoundControl {

  /// <summary> Allows for variation in a float value </summary>
  [System.Serializable]
  public class SoundVariation {

    /// <summary> Whether to use variation </summary>
    public bool useVariation;

    /// <summary> Minimum variation </summary>
    public float minimum = 0.9f;
    /// <summary> Maximum variation </summary>
    public float maximum = 1.1f;
    /// <summary> The actual current value </summary>
    public float value {
      get {
        if (useVariation) return Random.Range(minimum, maximum);
        else return 1;
      }
    }
  }
}