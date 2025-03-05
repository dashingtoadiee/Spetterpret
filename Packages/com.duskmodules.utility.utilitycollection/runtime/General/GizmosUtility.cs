using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules {

  /// <summary> Helper functions for Gizmos </summary>
  public static class GizmosUtility {

    /// <summary> Draws a gizmos circle </summary>
    public static void DrawCircle(Vector3 position, Quaternion rotation, float radius, float segmentAngle, float angle = 360) {
      int segments = (int)(angle / segmentAngle);
      segments = Mathf.FloorToInt((float)segments / 2) * 2 + 1;

      Vector3 v = new Vector3(0, 0, 1);
      Vector3 prev = position + rotation * Quaternion.Euler(v * (-angle / 2)) * Vector3.up * radius;
      float start = -(((float)segments) / 2 - 0.5f) * segmentAngle;
      for (int i = 0; i <= segments; i++) {
        Vector3 target = (i == segments) ?
          position + rotation * Quaternion.Euler(v * (angle / 2)) * Vector3.up * radius :
          position + rotation * Quaternion.Euler(v * (start + i * segmentAngle)) * Vector3.up * radius;
        Gizmos.DrawLine(prev, target);
        prev = target;
      }
    }
  }
}