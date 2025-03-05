using UnityEngine;

namespace DuskModules.ViewControl {

  /// <summary> Automatic border of the viewControl </summary>
  [ExecuteInEditMode]
  public class ViewBorder : ViewBase {

    /// <summary> The current width </summary>
    public float width;
    /// <summary> Width in use </summary>
    protected float useWidth;

    /// <summary> Governing anchor </summary>
    protected ViewAnchor anchor;
    /// <summary> All other borders matching this one </summary>
    protected ViewBorder[] otherBorders;

    /// <summary> Sprite of this border </summary>
    protected SpriteRenderer borderSprite;
    /// <summary> Whether to hide this border on play </summary>
    public bool hideOnPlay;
    /// <summary> Hide it on play </summary>
    protected bool hide;

    // Starten
    void Start() {
      borderSprite = transform.GetComponent<SpriteRenderer>();
      if (Application.isPlaying) {
        if (hideOnPlay) borderSprite.enabled = false;
      }
    }

    /// <summary> Sets up the component </summary>
    protected override void Setup() {
      base.Setup();
      FindFellowBorders(transform.parent);
      anchor = transform.GetComponentInParent<ViewAnchor>();
      borderSprite = transform.GetComponent<SpriteRenderer>();
    }

    /// <summary> Finds all fellow borders </summary>
    /// <param name="trf"> From where to search </param>
    protected void FindFellowBorders(Transform trf) {
      ViewBorder[] others = trf.GetComponentsInChildren<ViewBorder>();
      if (others.Length > 1) otherBorders = others;
      else if (trf.parent != null) FindFellowBorders(trf.parent);
    }

    /// <summary> Moves and scales itself according to the anchor it's attached to. </summary>
    void LateUpdate() {
      if (otherBorders == null) FindFellowBorders(transform.parent);
      if (useWidth != width) {
        for (int i = 0; i < otherBorders.Length; i++) {
          otherBorders[i].width = width;
          otherBorders[i].useWidth = width;
          otherBorders[i].UpdatedView();
        }
      }
      if (hide != hideOnPlay) {
        for (int i = 0; i < otherBorders.Length; i++) {
          otherBorders[i].hideOnPlay = hideOnPlay;
          otherBorders[i].hide = hideOnPlay;
        }
      }
    }

    /// <summary> Called when resolution has been updated </summary>
    public override void UpdatedView() {
      if (anchor == null) return;

      Vector2 offset = Vector2.zero;
      Vector3 scale = Vector3.one;

      switch (anchor.alignTo) {
        case ViewAlignment.left:
          offset.x = -useWidth / 2;
          scale.x = useWidth;
          scale.y = viewCamera.topOffset - viewCamera.bottomOffset + useWidth * 2;
          break;
        case ViewAlignment.top:
          offset.y = useWidth / 2;
          scale.x = viewCamera.leftOffset - viewCamera.rightOffset;
          scale.y = useWidth;
          break;
        case ViewAlignment.right:
          offset.x = useWidth / 2;
          scale.x = useWidth;
          scale.y = viewCamera.topOffset - viewCamera.bottomOffset + useWidth * 2;
          break;
        case ViewAlignment.bottom:
          offset.y = -useWidth / 2;
          scale.x = viewCamera.leftOffset - viewCamera.rightOffset;
          scale.y = useWidth;
          break;
      }

      transform.localPosition = offset;
      transform.localScale = scale;
    }
  }

}