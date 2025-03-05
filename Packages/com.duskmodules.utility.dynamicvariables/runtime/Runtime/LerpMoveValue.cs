using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules.DynamicVariables {

  /// <summary> A value with a move and lerp speed to move stuff smoothly with. </summary>
  [System.Serializable]
  public class LerpMoveValue {

    /// <summary> Types of modes for LerpMove </summary>
    public enum LerpMoveMode {
      both,
      lerp,
      move
    }
    /// <summary> Set mode </summary>
    public LerpMoveMode mode;

    [Tooltip("Speed to lerp over time with.")]
    public FloatReference lerpSpeed;
    [Tooltip("Speed to move lineary over time with.")]
    public FloatReference moveSpeed;

    /// <summary> Whether this LerpMove uses Lerp </summary>
    public bool useLerp => (lerpSpeed.value > 0) && (mode == LerpMoveMode.lerp || mode == LerpMoveMode.both);
    /// <summary> Whether this LerpMove uses MoveTowards </summary>
    public bool useMove => (moveSpeed.value > 0) && (mode == LerpMoveMode.both || mode == LerpMoveMode.both);

    /// <summary> Creates lerpMove object </summary>
    public LerpMoveValue(float lSpeed, float mSpeed) {
      mode = LerpMoveMode.both;
      lerpSpeed = new FloatReference();
      lerpSpeed.SetConstant(lSpeed);
      moveSpeed = new FloatReference();
      moveSpeed.SetConstant(mSpeed);
    }

    /// <summary> Copies the target values </summary>
    /// <param name="target"> Target to copy </param>
    public void Copy(LerpMoveValue target) {
      lerpSpeed.Copy(target.lerpSpeed);
      moveSpeed.Copy(target.moveSpeed);
    }

    /// <summary> Moves the float v to float t smoothly over a time of dt </summary>
    /// <param name="v"> Value to move </param>
    /// <param name="t"> Where to move </param>
    /// <param name="dt"> Time to move </param>
    /// <returns> Moved float </returns>
    public float Move(float v, float t, float dt = -1) {
      if (v == t) return v;
      if (dt < 0) dt = Time.deltaTime;
      if (useLerp) v = Mathf.Lerp(v, t, lerpSpeed.value * dt);
      if (useMove) v = Mathf.MoveTowards(v, t, moveSpeed.value * dt);
      return v;
    }

    /// <summary> Moves the Vector3 v to Vector3 t smoothly over a time of dt </summary>
    /// <param name="v"> Value to move </param>
    /// <param name="t"> Where to move </param>
    /// <param name="dt"> Time to move </param>
    /// <returns> Moved Vector3 </returns>
    public Vector3 Move(Vector3 v, Vector3 t, float dt = -1) {
      if (v == t) return v;
      if (dt < 0) dt = Time.deltaTime;
      if (useLerp) v = Vector3.Lerp(v, t, lerpSpeed.value * dt);
      if (useMove) v = Vector3.MoveTowards(v, t, moveSpeed.value * dt);
      return v;
    }

    /// <summary> Moves the Vector2 v to Vector2 t smoothly over a time of dt </summary>
    /// <param name="v"> Value to move </param>
    /// <param name="t"> Where to move </param>
    /// <param name="dt"> Time to move </param>
    /// <returns> Moved Vector2 </returns>
    public Vector2 Move(Vector2 v, Vector2 t, float dt = -1) {
      if (v == t) return v;
      if (dt < 0) dt = Time.deltaTime;
      if (useLerp) v = Vector2.Lerp(v, t, lerpSpeed.value * dt);
      if (useMove) v = Vector2.MoveTowards(v, t, moveSpeed.value * dt);
      return v;
    }

    /// <summary> Rotates the Quaternion v to Quaternion t smoothly over a time of dt </summary>
    /// <param name="v"> Value to move </param>
    /// <param name="t"> Where to move </param>
    /// <param name="dt"> Time to move </param>
    /// <returns> Rotated Quaternion </returns>
    public Quaternion Move(Quaternion v, Quaternion t, float dt = -1) {
      if (v == t) return v;
      if (dt < 0) dt = Time.deltaTime;
      if (useLerp) v = Quaternion.Lerp(v, t, lerpSpeed.value * dt);
      if (useMove) v = Quaternion.RotateTowards(v, t, moveSpeed.value * dt);
      return v;
    }
  }
}