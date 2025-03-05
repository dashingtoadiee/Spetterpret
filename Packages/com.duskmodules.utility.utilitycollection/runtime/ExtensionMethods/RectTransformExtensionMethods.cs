using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Extension Methods functions making it easier to deal with default Unity RectTransforms. </summary>
public static class RectTransformExtensionMethods {

  /// <summary> Gets the position of this RectTransform relative to the RectTransform root parent </summary>
  public static Vector3 GetRootPosition(this RectTransform rectTransform) {
    RectTransform rootParent = rectTransform.GetRootRectTransform();
    return rootParent.InverseTransformPoint(rectTransform.position);
  }

  // Gets the topmost RectTransform
  private static RectTransform GetRootRectTransform(this RectTransform rectTransform) {
    if (rectTransform.parent != null && rectTransform.parent.IsOfType(typeof(RectTransform)))
      return ((RectTransform)rectTransform.parent).GetRootRectTransform();
    else
      return rectTransform;
  }

}