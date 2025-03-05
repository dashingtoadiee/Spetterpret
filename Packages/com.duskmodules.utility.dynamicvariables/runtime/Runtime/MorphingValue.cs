using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules.DynamicVariables {

  /// <summary> A value that morphs randomly by itself over time. </summary>
  [System.Serializable]
  public class MorphingValue {

    /// <summary> Target value is randomized </summary>
    public RandomFloat randomTarget;
    /// <summary> Time for target value reset </summary>
    public RandomFloat randomTime;
    /// <summary> Running timer </summary>
    public float timer;

    /// <summary> Current value </summary>
    public float value;
    /// <summary> Target value </summary>
    public float valueTarget;
    /// <summary> Speed to move with. </summary>
    [Tooltip("Speed with which to move the smooth value")]
    public LerpMoveValue speed;

    /// <summary> Resets target value </summary>
    public void ResetTarget() {
			valueTarget = randomTarget.value;
			timer = randomTime.value;
			value = Mathf.Clamp(value, randomTarget.minimum.value, randomTarget.maximum.value);
    }

    /// <summary> Updating of value </summary>
    /// <param name="time"> How much time has passed in seconds </param>
    public virtual void Update(float time = -1) {
			if (time < 0) time = Time.deltaTime;
			
			timer -= time;
      if (timer <= 0)
				ResetTarget();

      value = speed.Move(value, valueTarget, time);
    }

    /// <summary> Copies the values of the target </summary>
    /// <param name="target"> The target to copy </param>
    public virtual void Copy(MorphingValue target) {
			randomTarget.Copy(target.randomTarget);
			randomTime.Copy(target.randomTime);
      value = target.value;
      valueTarget = target.valueTarget;
      speed.Copy(target.speed);
    }
  }

}