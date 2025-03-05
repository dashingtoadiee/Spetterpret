#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DuskModules.DynamicVariables.DuskEditor {

  /// <summary> Custom property drawer for the MinMaxFloat field </summary>
  [CustomPropertyDrawer(typeof(SmoothQuaternion))]
  public class SmoothQuaternionProperty : BaseSmoothValueProperty {

    /// <summary> Whether to show the actual values. </summary>
    protected override bool ShowValues() {
      return false;
    }
  }

}
#endif