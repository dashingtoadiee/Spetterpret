using UnityEngine;

namespace DuskModules {

  /// <summary> Place this above a field to prevent it being adjusted </summary>
  [System.AttributeUsage(System.AttributeTargets.Field)]
  public class ReadOnlyAttribute : PropertyAttribute {

  }

}
