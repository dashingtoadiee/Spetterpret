using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules {

  /// <summary> Range of values </summary>
  [System.Serializable]
  public class IntRange {
    [Tooltip("Minimum value of range")]
    public int min;
    [Tooltip("Maximum value of range")]
    public int max;

    /// <summary> Length of range </summary>
    public int length => max - min;

    /// <summary> Base range constructor </summary>
    public IntRange() {
      min = 0;
      max = 0;
    }

    /// <summary> Range constructor </summary>
    public IntRange(int min, int max) {
      this.min = min;
      this.max = max;
    }
  }

}