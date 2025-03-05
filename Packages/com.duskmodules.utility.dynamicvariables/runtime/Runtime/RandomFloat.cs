using DuskModules;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules.DynamicVariables {

  /// <summary> A Minimimum / Maximum value that can generate a random float value. </summary>
  [System.Serializable]
  public class RandomFloat : BaseRandomValue {

    /// <summary> Whether to use the constant (minimum) or not </summary>
    public bool useConstant = true;
    /// <summary> Constant value </summary>
    public FloatReference constant;

    /// <summary> The minimum possible value </summary>
    public FloatReference minimum;
    /// <summary> The maximum possible value </summary>
    public FloatReference maximum;
    /// <summary> The actual current value </summary>
    public float value {
      get {
        if (useConstant) return constant.value;
        else {
					lastValue = Random.Range(minimum.value, maximum.value);
          return lastValue;
        }
      }
      set {
        if (useConstant) constant.value = value;
      }
    }
    /// <summary> Last value generated </summary>
    public float lastValue { get; protected set; }

		/// <summary> Set the constant value </summary>
		public void SetConstant(float v) {
			if (constant == null) constant = new FloatReference();
			constant.SetConstant(v);
			useConstant = true;
		}

		/// <summary> Copies the target values </summary>
		/// <param name="target"> Target to copy </param>
		public void Copy(RandomFloat target) {
			useConstant = target.useConstant;
			constant.Copy(target.constant);
			minimum.Copy(target.minimum);
			maximum.Copy(target.maximum);
    }
  }
}