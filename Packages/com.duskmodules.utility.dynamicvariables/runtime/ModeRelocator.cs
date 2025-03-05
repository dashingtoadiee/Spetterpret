using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules.DynamicVariables {

  /// <summary> Reads from an Int variable to determine what mode it is in, and moves itself to match target. </summary>
  public class ModeRelocator : MonoBehaviour {

    /// <summary> Event fired when starts moving to new target mode. </summary>
    public event Action<int> onStartMoving;
    /// <summary> Event fired when reached target mode position rotation and scale </summary>
    public event Action<int> onEndMoving;

    /// <summary> List of targets </summary>
    [Tooltip("The list of targets this relocator can move from and to.")]
    public List<Transform> targets;

    /// <summary> Current mode to use </summary>
    [Tooltip("The current mode to move to.")]
    public IntReference mode;

    /// <summary> Motion progress to each target </summary>
    [Serializable]
    public class TargetMotion {
      public Transform target;
      public float x;
    }
    /// <summary> Active motions </summary>
    public List<TargetMotion> motions { get; protected set; }

    [Tooltip("Speed to move between targets with.")]
    public LerpMoveValue speed;

    /// <summary> Whether it should ignore out of bounds indexes and keep last position </summary>
    [Tooltip("If true, any value falling outside of targets array bounds will be ignored. " +
      "Use this to keep the last position for invalid indexes instead of clamping and taking the closest possible target index.")]
    public bool ignoreOutOfBounds;
    /// <summary> Value the mode must be for it to match with target index 0. </summary>
    [Tooltip("The first valid value that can be used. The mode value set here will match with target index 0.")]
    public int modeStartIndex;

    /// <summary> The mode to use </summary>
    protected int useMode;

    /// <summary> Whether not yet reached target </summary>
    public bool isMoving { get; protected set; }

    private Vector3 lastLocalEuler;

    // Awaken and set to target immediatly.
    protected virtual void Awake() {
      useMode = Mathf.Clamp(mode.value, 0, targets.Count - 1);

      motions = new List<TargetMotion>();
      for (int i = 0; i < targets.Count; i++) {
        motions.Add(new TargetMotion { target = targets[i], x = i == useMode ? 1 : 0 });
      }
      UpdatePosition();
    }

    // Move self to target mode
    protected virtual void Update() {
      int modeValue = mode.value - modeStartIndex;
      int newMode = modeValue;
      if (!ignoreOutOfBounds)
        newMode = Mathf.Clamp(modeValue, 0, targets.Count - 1);

      if (useMode != newMode && newMode >= 0 && newMode < targets.Count) {
        useMode = newMode;
        isMoving = true;
        onStartMoving?.Invoke(useMode);
      }

      bool update = false;
      bool isInterface = transform.IsOfType(typeof(RectTransform));

      for (int i = 0; i < motions.Count; i++) {
        float t = i == useMode ? 1 : 0;
        if (motions[i].x != t) {
          motions[i].x = speed.Move(motions[i].x, t, DuskUtility.GetDeltaTime(isInterface ? TimeType.interfaceDeltaTime : TimeType.deltaTime));
          update = true;
        }
      }

      if (update) {
        UpdatePosition();
      }
      else if (isMoving) {
        isMoving = false;
        onEndMoving?.Invoke(useMode);
      }
    }

    // Sets position to the current motions
    private void UpdatePosition() {
      RectTransform rTransform = (RectTransform)transform;

      Vector3 position = Vector3.zero;
      Vector3 euler = Vector3.zero;
      Vector3 scale = Vector3.zero;

      Vector2 anchoredPos = Vector2.zero;
      Vector2 sizeDelta = Vector2.zero;
      Vector2 pivot = Vector2.zero;
      Vector2 anchorMin = Vector2.zero;
      Vector2 anchorMax = Vector2.zero;

      float total = 0;
      for (int i = 0; i < motions.Count; i++) {
        TargetMotion m = motions[i];
        total += m.x;
        position += m.target.localPosition * m.x;
        euler += m.target.localEulerAngles * m.x;
        scale += m.target.localScale * m.x;
        RectTransform rTarget = (RectTransform)m.target;
        if (rTarget != null) {
          anchoredPos += rTarget.anchoredPosition * m.x;
          sizeDelta += rTarget.sizeDelta * m.x;
          pivot += rTarget.pivot * m.x;
          anchorMin += rTarget.anchorMin * m.x;
          anchorMax += rTarget.anchorMax * m.x;
        }
      }

      if (total != 1 && total != 0) {
        position /= total;
        euler /= total;
        scale /= total;
        anchoredPos /= total;
        sizeDelta /= total;
        pivot /= total;
        anchorMin /= total;
        anchorMax /= total;
      }

      transform.localPosition = position;
      if (lastLocalEuler != euler) {
        lastLocalEuler = euler;
        transform.localEulerAngles = euler;
      }
      transform.localScale = scale;
      if (rTransform == null) {
        transform.localPosition = position;
      }
      else {
        rTransform.anchoredPosition = anchoredPos;
        rTransform.sizeDelta = sizeDelta;
        rTransform.pivot = pivot;
        rTransform.anchorMin = anchorMin;
        rTransform.anchorMax = anchorMax;
      }
    }

    /// <summary> Instantly moves to the new mode. </summary>
    public void MoveToMode(int setMode) {
      mode.SetConstant(setMode);
      int modeValue = mode.value - modeStartIndex;
      int newMode = modeValue;
      if (!ignoreOutOfBounds)
        newMode = Mathf.Clamp(modeValue, 0, targets.Count - 1);

      if (useMode != newMode && newMode >= 0 && newMode < targets.Count) {
        useMode = newMode;
      }

      bool update = false;
      for (int i = 0; i < motions.Count; i++) {
        float t = i == useMode ? 1 : 0;
        if (motions[i].x != t) {
          motions[i].x = t;
          update = true;
        }
      }

      if (update)
        UpdatePosition();
    }

  }
}