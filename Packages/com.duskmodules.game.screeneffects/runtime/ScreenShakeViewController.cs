using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuskModules;

namespace DuskModules.ScreenEffects {

  /// <summary> View controller listening for screenshake, applying it to its local position. </summary>
  public class ScreenShakeViewController : MonoBehaviour {

    [Tooltip("Transform used for spatial positioning of shake. Use its own transform if attached to a camera," +
      "but use the transform of a camera if this is placed on something within an Canvas.")]
    [AutoReference]
    public Transform spatialTarget;

    [Tooltip("How much offset to apply per shake strength.")]
    public float offsetPerStrength;
    [Tooltip("How much tilt to apply per shake strength.")]
    public float tiltPerStrength;

    [Tooltip("Whether to invert the shake offset.")]
    public bool inverted;
    [Tooltip("Whether to apply the 2D direction of this view to the shakes.")]
    public bool useViewDirection;

    protected Vector3 startPos;

    // Connect
    private void Awake() {
      gameObject.ConfirmComponent(ref spatialTarget);
      startPos = transform.localPosition;
      ScreenShakeManager.instance.onShakeUpdate += OnShakeUpdate;
    }
    private void OnDestroy() {
      if (ScreenShakeManager.exists)
        ScreenShakeManager.instance.onShakeUpdate -= OnShakeUpdate;
    }

    // Shake update
    private void OnShakeUpdate() {
      Vector3 offset = ScreenShakeManager.instance.GetLocalShakeOffset(spatialTarget.position);
      float tilt = offset.z * tiltPerStrength;
      offset = offset.WithZ(0) * offsetPerStrength;

      if (inverted) offset *= -1;
      if (useViewDirection) offset = offset.RotateVector(-spatialTarget.eulerAngles.z);
      transform.localPosition = startPos + offset.WithZ(0);
      transform.localEulerAngles = Vector3.zero.WithZ(tilt);
    }

  }
}