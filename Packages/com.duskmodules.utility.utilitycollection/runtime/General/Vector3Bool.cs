using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules {

  /// <summary> Boolean for application in vector calculations. </summary>
  [System.Serializable]
  public class Vector3Bool {

    /// <summary> Vector3Bool where all is set to true by default. </summary>
    public static Vector3Bool allTrue => new Vector3Bool(true, true, true);

    public bool x;
    public bool y;
    public bool z;

    public Vector3Bool(bool x, bool y, bool z) {
      this.x = x;
      this.y = y;
      this.z = z;
    }

    /// <summary> Applies the Vector3Bool to a Vector. </summary>
		/// <param name="v"> Vector3 to pass through the bool checks. </param>
		/// <param name="type"> Whether the passed vector is an additive or multiplier </param>
		/// <returns> The modified vector </returns>
    public Vector3 Apply(Vector3 v, VectorBoolType type = VectorBoolType.multiply) {
      float nV = type == VectorBoolType.add ? 0 : 1;
      if (!x) v.x = nV;
      if (!y) v.y = nV;
      if (!z) v.z = nV;
      return v;
    }
  }

}