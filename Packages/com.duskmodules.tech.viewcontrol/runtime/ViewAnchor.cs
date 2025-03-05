using UnityEngine;

namespace DuskModules.ViewControl {

  /// <summary> Aligns this transform to the screen, updating accordingly with resolution and camera changes. It is subscribed to an ViewCamera script. </summary>
  [ExecuteInEditMode]
  public class ViewAnchor : ViewBase {

    /// <summary> To which screen border must this transform align. </summary>
    public ViewAlignment alignTo;

    [Tooltip("Whether to apply to local position instead of world position")]
    public bool local;
    [Tooltip("Whether to align the lateral axis too. For example, wether to center the horizontal position if set to top or bottom.")]
    public bool alignLateral;

    /// <summary> Called when resolution has been updated </summary>
    public override void UpdatedView() {
      // Find the world pos to anchor to
      Vector3 offset = Vector3.zero;
      switch (alignTo) {
        case ViewAlignment.left: offset.x = viewCamera.leftOffset; break;
        case ViewAlignment.top: offset.y = viewCamera.topOffset; break;
        case ViewAlignment.right: offset.x = viewCamera.rightOffset; break;
        case ViewAlignment.bottom: offset.y = viewCamera.bottomOffset; break;
        case ViewAlignment.topLeft: offset.x = viewCamera.leftOffset; offset.y = viewCamera.topOffset; break;
        case ViewAlignment.topRight: offset.x = viewCamera.rightOffset; offset.y = viewCamera.topOffset; break;
        case ViewAlignment.bottomLeft: offset.x = viewCamera.leftOffset; offset.y = viewCamera.bottomOffset; break;
        case ViewAlignment.bottomRight: offset.x = viewCamera.rightOffset; offset.y = viewCamera.bottomOffset; break;
      }
      float sizeMult = viewCamera.SizeMultZ(transform.position.z);
      Vector3 pos = local ? transform.localPosition : transform.position;
      Vector3 newPos = offset * sizeMult;
      if (!local)
        newPos += viewCamera.transform.position;

      newPos.z = pos.z;

      if (!alignLateral) {
        if (alignTo == ViewAlignment.top || alignTo == ViewAlignment.bottom)
          newPos.x = pos.x;
        else if (alignTo == ViewAlignment.left || alignTo == ViewAlignment.right)
          newPos.y = pos.y;
      }

      if (local) {
        transform.localPosition = newPos;
      } else {
        transform.position = newPos;
      }
    }

  }

}