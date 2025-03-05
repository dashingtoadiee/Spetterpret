using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules.DynamicVariables {

  /// <summary> Color setting, which moves fill to a target over time. </summary>
  [System.Serializable]
  public class ColorSettingMoving : ColorSetting {

    /// <summary> Target value to move to. </summary>
    public float target { get; set; }

    /// <summary> Whether the color is at target. </summary>
    public bool atTarget => fill == target;

    [Tooltip("Speed to move color fillwith.")]
    public LerpMoveValue speed;

    /// <summary> When used for first time </summary>
    public ColorSettingMoving() : base() {
      speed = new LerpMoveValue(0, 0);
    }

    /// <summary> Updating of value </summary>
    /// <param name="time"> How much time has passed in seconds </param>
    public virtual void Update(float time = -1) {
      if (time < 0) time = Time.deltaTime;
      fill = speed.Move(fill, target, time);
    }

  }
}