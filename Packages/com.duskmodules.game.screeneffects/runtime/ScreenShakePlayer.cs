using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules.ScreenEffects {

  /// <summary> A single shake player </summary>
  [System.Serializable]
  public class ScreenShakePlayer {

    /// <summary> Action called on shake end, if set. </summary>
    public event Action onShakeEnd;

    /// <summary> Position of the shake </summary>
    public Vector3 position;
    /// <summary> Frequency of the shake </summary>
    public float frequency;
    /// <summary> Strength of the shake </summary>
    public float strength;
    /// <summary> Strength multiplier over time </summary>
    public AnimationCurve strengthMultiplierOverTime;
    /// <summary> Multiplier for strength </summary>
    public float strengthMultiplier;
    /// <summary> Multiplier for frequency </summary>
    public float frequencyMultiplier;

    /// <summary> Strength multiplier of the shake </summary>
    public Vector3 strengthVector;
    /// <summary> Frequency multiplier of the shake </summary>
    public Vector3 frequencyVector;
    /// <summary> Angular direction of shake axis, if directional, from ScreenShake data </summary>
    public float direction;
    /// <summary> Any extra angle, per rotation of objects or whatever is given </summary>
    public float angle;

    /// <summary> How much offset to award frequency per axis </summary>
    public Vector3 frequencyOffset;

    /// <summary> Maximum time of shake. If zero, it is infinite and must be ended manually. </summary>
    public float maxTime;
    /// <summary> Time running </summary>
    public float time { get; protected set; }
    /// <summary> The life power of this shake, drains from 1 to 0. If 0 or lower, it can be recycled. </summary>
    public float life => ended ? 0 : (maxTime > 0) ? 1 - (time / maxTime) : 1;

    /// <summary> How much 2D or 3D is this shake. </summary>
    public float spatialBlend;
    /// <summary> Curve for the strength rolloff, determining the strength based on the distance of the screenshake to a shake view controller. </summary>
    public AnimationCurve strengthRollOff;
    /// <summary> Minimum distance of strength roll off. </summary>
    public float minDistance;
    /// <summary> Maximum distance of strength roll off. </summary>
    public float maxDistance;

    /// <summary> Progress of shake </summary>
    public Vector3 progress;
    /// <summary> Offset of shake </summary>
    public Vector2 offset;
    /// <summary> Tilt of shake </summary>
    public float tilt;

    private bool ended;

    /// <summary> Resets screenshake with the given settings. </summary>
    public void Play(ScreenShake shake, Vector3 position, float frequencyMultiplier, float strengthMultiplier, float angle) {
      this.position = position;

      frequency = shake.frequency.value;
      this.frequencyMultiplier = frequencyMultiplier;

      strength = shake.strength.value;
      if (shake.strengthVariation.inUse) strength *= shake.strengthVariation.value;
      this.strengthMultiplier = strengthMultiplier;

      strengthMultiplierOverTime = shake.strengthMultiplierOverTime;

      strengthVector = shake.strengthVector;
      frequencyVector = shake.frequencyVector;
      direction = shake.direction.inUse ? shake.direction.value : 0;
      this.angle = angle;

      maxTime = shake.time.value;
      time = 0;

      spatialBlend = shake.spatialBlend;
      strengthRollOff = shake.strengthRollOff;
      minDistance = shake.minDistance.value;
      maxDistance = shake.maxDistance.value;

      progress = new Vector3();
      progress.x = UnityEngine.Random.Range(0, Mathf.PI * 2);
      progress.y = UnityEngine.Random.Range(0, Mathf.PI * 2);
      progress.z = UnityEngine.Random.Range(0, Mathf.PI * 2);

      if (shake.frequencyVariation.inUse) {
        frequencyVector.x *= shake.frequencyVariation.value;
        frequencyVector.y *= shake.frequencyVariation.value;
        frequencyVector.z *= shake.frequencyVariation.value;
      }

      ended = false;
    }

    /// <summary> Updates the shake and returns offset by shake </summary>
    /// <param name="runTime"> How long to run it </param>
    /// <returns> The offset by this shake </returns>
    public void Update(float runTime) {
      if (maxTime > 0) time += runTime;
      float fp = frequency * frequencyMultiplier * runTime;
      progress.x += fp * frequencyVector.x;
      progress.y += fp * frequencyVector.y;
      progress.z += fp * frequencyVector.z;

      float m = ended ? 0 : maxTime > 0 ? strengthMultiplierOverTime.Evaluate(time / maxTime) : 1;
      float s = m * strength * strengthMultiplier;

      offset.x = Mathf.Sin(progress.x) * s * strengthVector.x;
      offset.y = Mathf.Cos(progress.y) * s * strengthVector.y;
      tilt = Mathf.Sin(progress.z) * s * strengthVector.z;

      offset = offset.RotateVector(direction + angle);

      CheckEndShake();
    }

    /// <summary> Gets the spatial multiplier by location </summary>
    public float GetRelativeStrength(Vector3 target) {
      if (spatialBlend == 0) return 1;
      if (minDistance <= 0) return 0;
      float d = (target - position).magnitude;
      float m = strengthRollOff.Evaluate(d) * minDistance;
      return (1 - spatialBlend) + spatialBlend * m;
    }

    /// <summary> Ends this shake, weakening over the given time. </summary>
    public void EndShake(float decayTime = 0) {
      if (decayTime == 0) {
        EndShakeDone();
        return;
      }

      if (maxTime == 0) {
        maxTime = decayTime;
      } else if (time > decayTime) {
        float m = decayTime / time;
        time *= m;
        maxTime *= m;
      }
      CheckEndShake();
    }

    // Checks end of shake
    private void CheckEndShake() {
      if (maxTime > 0 && time > maxTime) {
        EndShakeDone();
      }
    }

    // Called when end shake is done
    private void EndShakeDone() {
      onShakeEnd?.Invoke();
      onShakeEnd = null;
      ended = true;
    }

  }
}