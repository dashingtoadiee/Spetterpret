using UnityEngine;
using System.Collections.Generic;
using System;

namespace DuskModules.ViewControl {

  /// <summary> Controls camera size to ensure it always keeps visionPoint in view, or always stays within its set bounds, regardless of screen ratio. </summary>
  [ExecuteInEditMode]
  public class ViewCamera : MonoBehaviour {

    /// <summary> Called when the view has been updated </summary>
    public event Action onViewUpdated;

    /// <summary> Camera to affect </summary>
    public Camera useCamera { get; protected set; }
    private float baseFOV;
    private float baseCameraZ;

    [Tooltip("The world point to which the screen edge will be locked to.")]
    public Transform visionPoint;
    private Vector3 edgePoint;

    [Tooltip("If not inverted, the screen will always include the visionPoint. If inverted, the screen will always remain within the bounds set by the visionPoint.")]
    public bool invert;

    [Tooltip("Alignment of view, if any. Camera will anchor itself on that side.")]
    public ViewAlignment alignment;

    [Tooltip("Zoom of the camera.")]
    public float zoom = 1;
    private float lastZoom;

    [Tooltip("Whether to exclude zoom for border offset calculations")]
    public bool excludeZoomInOffsets;

    private Vector3 lastPos;

    /// <summary> Left side of camera offset. </summary>
    public float leftOffset { get; protected set; }
    /// <summary> The world position of the left border of the base vision field. </summary>
    public float left {
      get {
        Setup();
        return transform.position.x + leftOffset;
      }
    }
    /// <summary> The world position of the left border of the camera vision field. </summary>
    public float cameraLeft {
      get {
        Setup();
        return useCamera.transform.position.x + leftOffset;
      }
    }

    /// <summary> Top side of camera offset. </summary>
    public float topOffset { get; protected set; }
    /// <summary> The world position of the top border of the base vision field. </summary>
    public float top {
      get {
        Setup();
        return transform.position.y + topOffset;
      }
    }
    /// <summary> The world position of the top border of the camera vision field. </summary>
    public float cameraTop {
      get {
        Setup();
        return useCamera.transform.position.y + topOffset;
      }
    }

    /// <summary> Right side of camera offset. </summary>
    public float rightOffset { get; protected set; }
    /// <summary> The world position of the right border of the base vision field. </summary>
    public float right {
      get {
        Setup();
        return transform.position.x + rightOffset;
      }
    }
    /// <summary> The world position of the right border of the camera vision field. </summary>
    public float cameraRight {
      get {
        Setup();
        return useCamera.transform.position.x + rightOffset;
      }
    }

    /// <summary> Bottom side of camera offset. </summary>
    public float bottomOffset { get; protected set; }
    /// <summary> The world position of the bottom border of the base vision field. </summary>
    public float bottom {
      get {
        Setup();
        return transform.position.y + bottomOffset;
      }
    }
    /// <summary> The world position of the bottom border of the camera vision field. </summary>
    public float cameraBottom {
      get {
        Setup();
        return useCamera.transform.position.y + bottomOffset;
      }
    }

    /// <summary> Base orthographic size before zoom </summary>
    public float baseOrthographicSize { get; protected set; }

    /// <summary> Offset applied due to alignment </summary>
    public Vector2 alignmentOffset { get; protected set; }

    /// <summary> Base home position to focus on </summary>
    public Vector3 position {
      get => homePosition;
      set {
        homePosition = value;
        transform.position = (homePosition + alignmentOffset).WithZ(value.z);
      }
    }

    [Tooltip("Home position of camera.")]
    [SerializeField]
    private Vector2 homePosition;

    private float lastWidth;
    private float lastHeight;

    private bool isSetup;

    // Awake
    void Awake() {
      Setup();
    }

    /// <summary> Sets up the view controller script </summary>
    public void Setup() {
      if (isSetup) return;
      isSetup = true;

      // Get camera
      useCamera = gameObject.GetComponentInChildren<Camera>();
      baseFOV = useCamera.fieldOfView;
      Vector3 delta = useCamera.transform.position - transform.position;
      baseCameraZ = delta.z;
      lastPos = transform.position;

      // Init fixing resolution
      FixResolution();
    }

    /// <summary> Gets the size mult for the given z world position </summary>
    /// <param name="z"> The used Z position </param>
    /// <returns> The size mult </returns>
    public float SizeMultZ(float z) {
      if (useCamera == null || useCamera.orthographic) return 1;
      float deltaZ = z - useCamera.transform.position.z;
      float one = transform.position.z - useCamera.transform.position.z;
      if (one > 0) return deltaZ / one;
      else return 1;
    }

    /// <summary> Execute a forced refresh </summary>
    public void ForcedRefresh() {
      FixResolution();
    }

    /// <summary> Changes the camera size to match the required limits. </summary>
    protected void FixResolution() {
      if (visionPoint == null) return;
      edgePoint = visionPoint.position - transform.position;
      edgePoint.x = Mathf.Abs(edgePoint.x);
      edgePoint.y = Mathf.Abs(edgePoint.y);

      float pointRatio = edgePoint.x / edgePoint.y;

      float w = Screen.width;
      float h = Screen.height;
      if (w == 0 || h == 0) {
        w = 1;
        h = 1;
        return;
      }
      float screenRatio = w / h;
      float ratioRatio = screenRatio / pointRatio;

      // Orthographic
      float orthoMult = invert ? Mathf.Min(1, 1 / ratioRatio) : Mathf.Max(1, 1 / ratioRatio);
      baseOrthographicSize = edgePoint.y * orthoMult;
      useCamera.orthographicSize = baseOrthographicSize / zoom;

      // Perspective
      Transform baseTrf = (useCamera.transform != transform) ? transform : transform.parent;
      Vector3 basePos = (baseTrf != null) ? baseTrf.position : Vector3.zero;
      baseCameraZ = (useCamera.transform.position - basePos).z;
      float d = Mathf.Atan(topOffset / Mathf.Abs(baseCameraZ)) * 180 / Mathf.PI * 2;
      useCamera.fieldOfView = d / zoom;

      lastWidth = Screen.width;
      lastHeight = Screen.height;
      lastZoom = zoom;
      lastPos = transform.position;

      // Refresh borders
      BorderRefresh(screenRatio);
    }

    // Checks if resolution has changed.
    void LateUpdate() {
      if (visionPoint == null) return;

      Vector3 pos = transform.position;
      Vector2 edgeDelta = visionPoint.position - transform.position;
      float camDeltaZ = (useCamera.transform.position - transform.position).z;
      if (Screen.width != lastWidth || Screen.height != lastHeight ||
        edgePoint.x != Mathf.Abs(edgeDelta.x) || edgePoint.y != Mathf.Abs(edgeDelta.y) ||
        baseCameraZ != camDeltaZ || lastZoom != zoom ||
        pos.x != lastPos.x || pos.y != lastPos.y || !Application.isPlaying) {
        FixResolution();
      }
    }

    /// <summary> Refreshes the borders </summary>
    protected void BorderRefresh(float screenRatio) {
      float zoomMult = excludeZoomInOffsets ? zoom : 1;
      leftOffset = -useCamera.orthographicSize * zoomMult * screenRatio;
      topOffset = useCamera.orthographicSize * zoomMult;
      rightOffset = useCamera.orthographicSize * zoomMult * screenRatio;
      bottomOffset = -useCamera.orthographicSize * zoomMult;

      CheckAlignment();

      onViewUpdated?.Invoke();
    }

    /// <summary> Checks alignment </summary>
    protected void CheckAlignment() {
      if (alignment != ViewAlignment.none) {
        // Find the world pos to anchor to
        Vector3 vp = visionPoint.position - transform.position;
        Vector2 offset = Vector3.zero;
        switch (alignment) {
          case ViewAlignment.left: offset.x = rightOffset; break;
          case ViewAlignment.top: offset.y = bottomOffset; break;
          case ViewAlignment.right: offset.x = leftOffset; break;
          case ViewAlignment.bottom: offset.y = topOffset; break;
          case ViewAlignment.topLeft: offset.x = rightOffset; offset.y = bottomOffset; break;
          case ViewAlignment.topRight: offset.x = leftOffset; offset.y = bottomOffset; break;
          case ViewAlignment.bottomLeft: offset.x = rightOffset; offset.y = topOffset; break;
          case ViewAlignment.bottomRight: offset.x = leftOffset; offset.y = topOffset; break;
        }
        alignmentOffset = offset;
        transform.localPosition = (homePosition + offset).WithZ(transform.localPosition.z);
      }
      else {
        alignmentOffset = Vector2.zero;
        transform.localPosition = homePosition.WithZ(transform.localPosition.z);
      }
    }
  }
}