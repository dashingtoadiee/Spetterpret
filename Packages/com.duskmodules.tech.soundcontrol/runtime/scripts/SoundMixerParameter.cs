using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using DuskModules.CacheUtility;
using System;

namespace DuskModules.SoundControl {

  /// <summary> Cache object </summary>
  public class ParameterCache : ICache {
    public float value;
    public float target;
    public List<SoundMixerParameterTarget> targetValues;
  }

  /// <summary> Controller for an audio group mixer </summary>
  public class SoundMixerParameter : CacheObject<ParameterCache> {

    /// <summary> Called when parameter changed </summary>
    public event Action<float> onValueChanged;

    [ReadOnly]
    public AudioMixer mixer;

    [ReadOnly]
    public string parameterName;

    [Header("Parameter Settings")]
    [Tooltip("Whether this parameter is a volume parameter")]
    public bool isVolume;
    [Tooltip("Whether the value is saved in player preferences")]
    public bool persisted;

    [Tooltip("Whether changing the parameter is normally instantanious")]
    public bool instant = true;
    [Tooltip("Linear speed of changing the parameter value to the target")]
    public float speed;

    [Tooltip("Minimum")]
    public float minimum;
    [Tooltip("Maximum")]
    public float maximum;

    /// <summary> In what way to use multi target values </summary>
    public enum MultiTargetMode {
      multiplied,
      lowest,
      highest,
      average
    }
    [Tooltip("When multiple target values are assigned to this parameter, how to determine the actual target value?")]
    public MultiTargetMode multiTargetMode;

    /// <summary> Current value of this parameter </summary>
    public float value {
      get {
        return cache.value;
      }
      set {
        SetFloat(value);
      }
    }

    /// <summary> Get the current value target </summary>
    public float target => cache.target;

    /// <summary> Sets the value of this parameter </summary>
    public void SetFloat(float v, bool isInstant = false) {
      if (instant) isInstant = true;

      float useMinimum = isVolume ? 0 : minimum;
      float useMaximum = isVolume ? 200 : maximum;
      if (v < useMinimum) v = useMinimum;
      if (v > useMaximum) v = useMaximum;

      cache.target = v;
      if (isInstant) {
        cache.value = v;
        onValueChanged?.Invoke(cache.value);
        SetMixerParameter();
      }

      if (persisted) {
        string key = SoundConfig.instance.playerPreferencesKey + "-" + parameterName;
        PlayerPrefs.SetFloat(key, v);
        PlayerPrefs.Save();
      }
    }

    /// <summary> Sets up the cache </summary>
    protected override void CacheSetup() {
      cache.targetValues = new List<SoundMixerParameterTarget>();

      float v = 1;
      if (persisted) {
        string key = SoundConfig.instance.playerPreferencesKey + "-" + parameterName;
        if (PlayerPrefs.HasKey(key)) {
          v = PlayerPrefs.GetFloat(key);
          cache.value = v;
          cache.target = v;
          onValueChanged?.Invoke(cache.value);
          SetMixerParameter();
          return;
        }
      }

      if (!isVolume) {
        mixer.GetFloat(parameterName, out v);
      }
      cache.value = v;
      cache.target = v;
      onValueChanged?.Invoke(cache.value);
      if (isVolume) {
        SetMixerParameter();
      }
    }

    /// <summary> Updates the parameter </summary>
    internal void Update() {
      if (instant) return;

      // Determine target value
      float useTarget = DetermineTarget();

      if (cache.value != useTarget) {
        cache.value = Mathf.MoveTowards(cache.value, useTarget, speed * DuskUtility.interfaceDeltaTime);
        onValueChanged?.Invoke(cache.value);
        SetMixerParameter();
      }
    }

    // Determines and returns target
    private float DetermineTarget() {
      float useTarget = cache.target;
      if (cache.targetValues.Count > 0 && !persisted) {
        for (int i = 0; i < cache.targetValues.Count; i++) {
          float t;
          switch (multiTargetMode) {
            case MultiTargetMode.multiplied: useTarget *= cache.targetValues[i].value; break;
            case MultiTargetMode.lowest:
              t = cache.targetValues[i].value;
              if (t < useTarget) useTarget = t;
              break;
            case MultiTargetMode.highest:
              t = cache.targetValues[i].value;
              if (t > useTarget) useTarget = t;
              break;
            case MultiTargetMode.average: useTarget += cache.targetValues[i].value; break;
          }
        }

        if (multiTargetMode == MultiTargetMode.average)
          useTarget /= cache.targetValues.Count + 1;
      }
      return useTarget;
    }

    // Sets the mixer parameter
    private void SetMixerParameter() {
      if (isVolume)
        mixer.SetVolumeParameter(parameterName, cache.value);
      else
        mixer.SetFloat(parameterName, cache.value);
    }

    /// <summary> Adds a target value </summary>
    public SoundMixerParameterTarget AddTarget(float target, bool isInstant = false) {
      if (persisted) Debug.LogWarning($"WARNING: MultiTargetValue added to {name}, which is persisted in player preferences. Value is added but ignored, you cannot use multi values when the parameter is saved and loaded.");
      SoundMixerParameterTarget t = new SoundMixerParameterTarget { value = target };
      cache.targetValues.Add(t);
      CheckUpdateTargets(isInstant);
      return t;
    }

    /// <summary> Removes target value </summary>
    public void RemoveTarget(SoundMixerParameterTarget target, bool isInstant = false) {
      if (cache.targetValues.Contains(target)) {
        cache.targetValues.Remove(target);
        CheckUpdateTargets(isInstant);
      }
    }

    // Checks if by addition or removal of multitarget, the value must change
    private void CheckUpdateTargets(bool isInstant) {
      if (instant || isInstant) {
        float useTarget = DetermineTarget();
        cache.value = useTarget;
        onValueChanged?.Invoke(cache.value);
        SetMixerParameter();
      }
    }

  }
}