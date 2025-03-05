using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary> Extension Methods functions making it easier to deal with default Unity Audio things. </summary>
public static class SoundExtensionMethods {

  /// <summary> Sets a parameter to the correct volume, using a range from 0 to 1. </summary>
  /// <param name="mixer"> Mixer to adjust </param>
  /// <param name="parameter"> Volume parameter name </param>
  /// <param name="volume"> Volume in a range from 0 to 1 </param>
  public static void SetVolumeParameter(this AudioMixer mixer, string parameter, float volume) {
    float value = (volume > 0) ? (20 * Mathf.Log(volume)) : -200;
    mixer.SetFloat(parameter, value);
  }
}