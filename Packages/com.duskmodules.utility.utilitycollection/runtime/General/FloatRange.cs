using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules {

  /// <summary> Range of values </summary>
  [System.Serializable]
  public class FloatRange {
    [Tooltip("Minimum value of range")]
    public float min;
    [Tooltip("Maximum value of range")]
    public float max;

    /// <summary> Length of range </summary>
    public float length => max - min;

    /// <summary> Base range constructor </summary>
    public FloatRange() {
      min = 0;
      max = 0;
    }

    /// <summary> Range constructor </summary>
    public FloatRange(float min, float max) {
      this.min = min;
      this.max = max;
    }
  }

}