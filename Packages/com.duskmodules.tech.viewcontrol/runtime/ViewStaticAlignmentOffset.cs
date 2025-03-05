using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules.ViewControl {

  /// <summary> Positions the ViewCamera local position so that the view is aligned to a side </summary>
  [ExecuteInEditMode]
  public class ViewStaticAlignmentOffset : ViewBase {

    [Tooltip("Alignment to apply")]
    public ViewAlignment alignment;

    /// <summary> Called when resolution has been updated </summary>
    public override void UpdatedView() {
      // Find the world pos to anchor to
      Vector3 offset = Vector3.zero;
      switch (alignment) {
        case ViewAlignment.left: offset.x = viewCamera.leftOffset; break;
        case ViewAlignment.top: offset.y = viewCamera.topOffset; break;
        case ViewAlignment.right: offset.x = viewCamera.rightOffset; break;
        case ViewAlignment.bottom: offset.y = viewCamera.bottomOffset; break;
        case ViewAlignment.topLeft: offset.x = viewCamera.leftOffset; offset.y = viewCamera.topOffset; break;
        case ViewAlignment.topRight: offset.x = viewCamera.rightOffset; offset.y = viewCamera.topOffset; break;
        case ViewAlignment.bottomLeft: offset.x = viewCamera.leftOffset; offset.y = viewCamera.bottomOffset; break;
        case ViewAlignment.bottomRight: offset.x = viewCamera.rightOffset; offset.y = viewCamera.bottomOffset; break;
      }
      viewCamera.transform.localPosition = -offset;
    }


  }
}