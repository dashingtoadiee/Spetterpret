using DuskModules;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules.DynamicVariables {

  /// <summary> A Minimimum / Maximum value that can generate a random int value. </summary>
  [System.Serializable]
  public class RandomInt : BaseRandomValue {

    /// <summary> Whether to use the constant (minimum) or not </summary>
    public bool useConstant = true;
    /// <summary> Constant value </summary>
    public IntReference constant;

    /// <summary> The minimum possible value </summary>
    public IntReference minimum;
    /// <summary> The maximum possible value </summary>
    public IntReference maximum;
    /// <summary> The actual current value </summary>
    public int value {
      get {
        if (useConstant) return constant.value;
        else {
          lastValue = Random.Range(minimum.value, maximum.value + 1);
          return lastValue;
        }
      }
      set {
        if (useConstant) constant.value = value;
      }
    }
    /// <summary> Last value generated </summary>
    public int lastValue { get; protected set; }

    /// <summary> Set the constant value </summary>
    public void SetConstant(int v) {
      constant.SetConstant(v);
      useConstant = true;
    }

    /// <summary> Copies the target values </summary>
    /// <param name="target"> Target to copy </param>
    public void Copy(RandomInt target) {
      useConstant = target.useConstant;
      constant.Copy(target.constant);
      minimum.Copy(target.minimum);
      maximum.Copy(target.maximum);
    }
  }
}