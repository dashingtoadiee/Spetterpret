using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Extension Methods functions making it easier to deal with default Unity RectTransforms. </summary>
public static class ParticleSystemExtensionMethods {

  /// <summary> Gets the position of this RectTransform relative to the RectTransform root parent </summary>
  public static void Copy(this ParticleSystem particles, ParticleSystem other) {
    ParticleSystem.MainModule main = particles.main;
    main.startColor = other.main.startColor;
  }

}