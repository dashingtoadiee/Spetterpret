using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Static class containing extension methods for angular calculations </summary>
public static class AngleExtensionMethods {

  /// <summary> Gets the angle of a certain vector delta </summary>
  /// <param name="delta"> The delta vector to get the angle from </param>
  /// <returns> The angle </returns>
  public static float GetAngle(this Vector2 delta) {
    float angle = Vector2.Angle(Vector2.up, delta);
    if (delta.x > 0) {
      angle *= -1;
    }
    return angle.CorrectAngle();
  }

  /// <summary> Rotates given vector2 by given degrees </summary>
  /// <param name="v"> The vector to rotate </param>
  /// <param name="degrees"> The amount of degrees to rotate it with </param>
  /// <returns> The rotated vector2 </returns>
  public static Vector2 RotateVector(this Vector2 v, float degrees) {
    if (degrees == 0) return v;
    float radians = degrees * Mathf.Deg2Rad;
    float ca = Mathf.Cos(radians);
    float sa = Mathf.Sin(radians);
    float x = v.x;
    float y = v.y;
    v.x = x * ca - y * sa;
    v.y = x * sa + y * ca;
    return v;
  }

  /// <summary> Rotates given vector3 by given degrees </summary>
  /// <param name="v"> The vector to rotate </param>
  /// <param name="degrees"> The amount of degrees to rotate it with </param>
  /// <returns> The rotated vector3 </returns>
  public static Vector3 RotateVector(this Vector3 v, float degrees) {
    if (degrees == 0) return v;
    float radians = degrees * Mathf.Deg2Rad;
    float ca = Mathf.Cos(radians);
    float sa = Mathf.Sin(radians);
    float x = v.x;
    float y = v.y;
    v.x = x * ca - y * sa;
    v.y = x * sa + y * ca;
    return v;
  }

  /// <summary> Brings the given angle back to 0 - 360 range, without changing orientation </summary>
  /// <param name="angle"> The angle to get into the correct range </param>
  /// <returns> Angle within a 0 - 360 range </returns>
  public static float CorrectAngle(this float angle) {
    while (angle > 360) { angle -= 360; }
    while (angle < 0) { angle += 360; }
    return angle;
  }

  /// <summary> Brings the given angle to a range able to properly compare to given comparison </summary>
  /// <param name="angle"> The angle to get into the correct range </param>
  /// <param name="comparison"> The comparison to use </param>
  /// <returns> The angle within a -180 - 180 range to comparison </returns>
  public static float ComparisonAngle(this float angle, float comparison) {
    while (angle > comparison + 180) { angle -= 360; };
    while (angle < comparison - 180) { angle += 360; };
    return angle;
  }
}