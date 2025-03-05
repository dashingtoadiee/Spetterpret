using UnityEngine;

namespace DuskModules.ViewControl {

  /// <summary> Base class for any behaviour relying on a ViewCamera. </summary>
  public class ViewBase : MonoBehaviour {

    /// <summary> The view Camera controller </summary>
    protected ViewCamera viewCamera;

    private bool isSetup;

    /// <summary> Awakens and setup view camera </summary>
    protected virtual void Awake() {
      CheckSetup();
    }

    /// <summary> Attempts setup </summary>
    protected void CheckSetup() {
      if (isSetup) return;
      isSetup = true;

      Setup();
    }

    /// <summary> Finds and listens to component </summary>
    protected virtual void Setup() {
      viewCamera = FindViewCamera(transform);
      viewCamera.Setup();
      viewCamera.onViewUpdated += UpdatedView;
      UpdatedView();
    }

    // Unhook event
    protected virtual void OnDestroy() {
      if (viewCamera != null)
        viewCamera.onViewUpdated -= UpdatedView;
    }

    /// <summary> Locates the viewCamera script this base belongs to. It checks itself first, before checking each parent for it. </summary>
    /// <param name="trf"> The starting transform </param>
    /// <returns> The view camera script, if found </returns>
    protected ViewCamera FindViewCamera(Transform trf) {
      if (viewCamera != null) return viewCamera;
      ViewCamera found = trf.GetComponentInChildren<ViewCamera>();
      if (found != null) return found;
      else if (trf.parent != null) return FindViewCamera(trf.parent);
      else return FindFirstObjectByType<ViewCamera>();
    }

    /// <summary> Called when resolution has been updated </summary>
    public virtual void UpdatedView() {

    }

    // Update to find camera while in editor.
    protected virtual void Update() {
      if (Application.isEditor && !Application.isPlaying) {
        CheckSetup();
        UpdatedView();
      }
    }

  }

}