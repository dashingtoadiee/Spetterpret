using UnityEngine;

namespace DuskModules.ViewControl {

  /// <summary> Rescales object based on screen size, and distance to camera (for perspective views) </summary>
  [ExecuteInEditMode]
  public class ViewScaler : ViewBase {

    [Tooltip("Screen ratio this object is based on.")]
    public Vector2 baseRatio;

    [Tooltip("Base scale to apply")]
    public Vector3 baseScale;

    /// <summary> Mode of scaling of the view scaler. </summary>
    public enum ScaleMode {
      none,
      scale,
      scaleUniformly
    }
    [Tooltip("Wh")]
    public ScaleMode scaleMode;

    private float z;

    /// <summary> Called when view has been updated </summary>
    public override void UpdatedView() {
      float desiredScale = viewCamera.SizeMultZ(transform.position.z);
      Vector3 scale = baseScale * desiredScale;

      float screenRatio = (float)Screen.width / (float)Screen.height;
      float ratio = baseRatio.x / baseRatio.y;

      float m = screenRatio / ratio;

      switch (scaleMode) {
        case ScaleMode.scale:
          if (m > 1) scale.x *= m;
          else if (m < 1) scale.y *= 1 / m;
          break;
        case ScaleMode.scaleUniformly:
          if (m < 1) m = 1 / m;
          scale.x *= m;
          scale.y *= m;
          break;
      }

      if (transform.localScale != scale) {
        transform.localScale = scale;
      }
      z = transform.position.z;
    }

    // Scales itself
    protected override void Update() {
      base.Update();
      if (z != transform.position.z) {
        UpdatedView();
      }
    }
  }

}