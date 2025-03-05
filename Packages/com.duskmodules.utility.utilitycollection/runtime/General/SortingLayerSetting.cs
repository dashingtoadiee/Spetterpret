using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules {

  /// <summary> Sorting layer, selectable in Unity Editor. </summary>
  [System.Serializable]
  public class SortingLayerSetting {

    public string layerName;
    public int layerID => SortingLayer.NameToID(layerName);

  }
}