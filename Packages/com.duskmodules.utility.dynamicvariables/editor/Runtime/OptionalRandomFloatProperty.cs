#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DuskModules.DynamicVariables.DuskEditor {

  /// <summary> Custom property drawer for the any variable reference field </summary>
  [CustomPropertyDrawer(typeof(OptionalRandomFloat))]
  public class OptionalRandomFloatProperty : OptionalFloatProperty {

  }
}
#endif