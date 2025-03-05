using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules.DynamicVariables {

  /// <summary> Value that smoothly matches the target value, using acceleration, speed and drag.
  /// This causes it to overshoot its target, resulting in wobbles and pop in movement. </summary>
  [System.Serializable]
  public class AcceleratedValue {

    [Tooltip("Acceleration settings defining speed and drag.")]
    public AcceleratedSettingsReference settings;

    [Tooltip("Optional lower limit, value cannot go below it.")]
    public OptionalFloat minimumLimit;
    [Tooltip("Optional upper limit, value cannot go above it.")]
    public OptionalFloat maximumLimit;

    [Tooltip("Target value to match over time.")]
    public FloatReference target;
    [Tooltip("Current value, which moves to target over time.")]
    public float value;

    public float speed { get; set; }

    /// <summary> Whether the value has reached the target </summary>
    public bool atTarget => value == target.value && speed == 0;

    /// <summary> Basic constructor </summary>
    public AcceleratedValue() { }

    /// <summary> Setup the accelerated value </summary>
    public AcceleratedValue(float v, AcceleratedSettings newSettings) {
      Setup(v);
      settings.SetConstant(newSettings);
    }

    /// <summary> Setup the accelerated value </summary>
    public AcceleratedValue(float v, AcceleratedSettingsReference newSettings) {
      Setup(v);
      settings.Copy(newSettings);
    }

    // Sets up 
    private void Setup(float v) {
      value = v;
      if (target == null) target = new FloatReference();
      target.SetConstant(v);

      minimumLimit = new OptionalFloat();
      maximumLimit = new OptionalFloat();

      if (settings == null) settings = new AcceleratedSettingsReference();
    }

    /// <summary> Sets a hard minimum limit </summary>
    public void SetMinimumLimit(float limit = 0) {
      minimumLimit.SetFloat(limit);
    }

    /// <summary> Sets a hard maximum limit </summary>
    public void SetMaximumLimit(float limit = 1) {
      maximumLimit.SetFloat(limit);
    }

    /// <summary> Sets the value, keeping it within limits. </summary>
    public virtual void SetValue(float value) {
      if (minimumLimit.inUse && value < minimumLimit.value)
        value = minimumLimit.value;
      if (maximumLimit.inUse && value > maximumLimit.value)
        value = maximumLimit.value;
      this.value = value;
    }

    /// <summary> Sets the value directly, resetting speed. Updates target as well. </summary>
    public virtual void ResetValue(float value) {
      SetValue(value);
      if (!target.useVariable)
        target.value = value;
      speed = 0;
    }

    /// <summary> Instantly snap to target and stop speed. </summary>
    public void SnapToTarget() {
      value = target.value;
      speed = 0;
    }

    /// <summary> Sets the target, keeping it within possible limits </summary>
    public virtual void SetTarget(float target) {
      if (minimumLimit.inUse && target < minimumLimit.value)
        target = minimumLimit.value;
      if (maximumLimit.inUse && target > maximumLimit.value)
        target = maximumLimit.value;
      this.target.SetConstant(target);
    }

    /// <summary> Updating of value </summary>
    /// <param name="time"> How much time has passed in seconds </param>
    public virtual void Update(float time = -1) {
      if (time < 0) time = Time.deltaTime;

      if (!atTarget) {
        float delta = target.value - value;
        float acc = delta * settings.value.acceleration.value;

        speed += acc * time;
        speed *= Mathf.Pow(.1f, time * settings.value.drag.value);
        if (settings.value.linearDrag.value > 0)
          speed = Mathf.MoveTowards(speed, 0, settings.value.linearDrag.value * time);

        value += speed * time;
        value = Mathf.MoveTowards(value, target.value, settings.value.linearSpeed.value * time);

        if (minimumLimit.inUse && value < minimumLimit.value) {
          value = minimumLimit.value;
          speed = 0;
        } else if (maximumLimit.inUse && value > maximumLimit.value) {
          value = maximumLimit.value;
          speed = 0;
        }
      }
    }

    /// <summary> Copies the values of the target </summary>
    /// <param name="other"> The target to copy </param>
    public virtual void Copy(AcceleratedValue other) {
      settings.Copy(settings);
      value = other.value;
      speed = other.speed;
      target.Copy(other.target);
      minimumLimit.Copy(other.minimumLimit);
      maximumLimit.Copy(other.maximumLimit);
    }
  }
}
