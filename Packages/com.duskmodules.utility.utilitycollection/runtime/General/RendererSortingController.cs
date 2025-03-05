using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules {

  /// <summary> Controls the 2D Sorting settings of the Renderer. </summary>
  [ExecuteAlways]
  public class RendererSortingController : MonoBehaviour {

    /// <summary> Renderer to change sorting of </summary>
    public new Renderer renderer { get; protected set; }

    [Tooltip("Name of the Renderer's sorting layer.")]
    public SortingLayerSetting sortingLayer;
    [Tooltip("Renderer's order within a sorting layer.")]
    public int sortingOrder;

    // Update mesh rendere
    private void LateUpdate() {
      if (renderer == null) {
        renderer = gameObject.GetComponent<Renderer>();
        sortingLayer.layerName = renderer.sortingLayerName;
      }

      if (renderer.sortingLayerName != sortingLayer.layerName)
        renderer.sortingLayerID = sortingLayer.layerID;
      if (renderer.sortingOrder != sortingOrder)
        renderer.sortingOrder = sortingOrder;
    }

  }

}