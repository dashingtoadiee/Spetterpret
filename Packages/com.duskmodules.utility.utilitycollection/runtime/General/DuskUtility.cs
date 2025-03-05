using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using DuskModules;
using System;

/// <summary> Class with generic utility methods </summary>
public static class DuskUtility {

  /// <summary> Default date time formatting, usually for data persistence </summary>
  public static string dateTimeFormat => "yyyy-MM-ddTHH:mm:sszzz";

  /// <summary> Returns unscaled delta time, but it cannot exceed the maximum particle deltaTime. </summary>
  public static float interfaceDeltaTime => Mathf.Min(Time.unscaledDeltaTime, Time.maximumParticleDeltaTime);

  /// <summary> Converts a value to a vector, with the specific axis </summary>
  public static Vector3 AxisVector(Axis3D axis, float value) {
    switch (axis) {
      case Axis3D.x: return new Vector3(value, 0, 0);
      case Axis3D.y: return new Vector3(0, value, 0);
      case Axis3D.z: return new Vector3(0, 0, value);
    }
    return Vector3.zero;
  }

  /// <summary> Returns 50/50 true or false </summary>
  public static bool CoinFlip() {
    return UnityEngine.Random.Range(0, 2) == 0;
  }

  /// <summary> Gets the correct deltaTime for a certain time type. </summary>
  /// <typeparam name="T"> Type to find </typeparam>
  /// <param name="timeType"> Type of time to get </param>
  /// <returns> The correct delta time </returns>
  public static float GetDeltaTime(TimeType timeType) {
    switch (timeType) {
      case TimeType.deltaTime: return Time.deltaTime;
      case TimeType.interfaceDeltaTime: return interfaceDeltaTime;
    }
    return Time.deltaTime;
  }

  /// <summary> Mathf.Sign, but with returning 0 as 0, as is expected of Sign </summary>
  public static float Sign(float v) {
    return v > 0 ? 1 : v < 0 ? -1 : 0;
  }

  /// <summary> Return position between a and b equal to t, but allow exceeding beyond either. </summary>
  public static float UnboundLerp(float a, float b, float t) {
    return a + (b - a) * t;
  }

}