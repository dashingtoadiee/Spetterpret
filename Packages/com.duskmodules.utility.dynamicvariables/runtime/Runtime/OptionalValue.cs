using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules.DynamicVariables {

  /// <summary> Generic optional value </summary>
  [System.Serializable]
  public class OptionalValue<T> {
    public bool inUse;
    public ValueReference<T> content;

    public T value => inUse ? content.value : default(T);

    /// <summary> Copies internal data from one to other </summary>
    public void Copy(OptionalValue<T> other) {
      inUse = other.inUse;
      if (content == null) content = new ValueReference<T>();
      content.Copy(other.content);
    }

    /// <summary> Sets optional value as in use, and sets the value </summary>
    public void SetValue(T v) {
      inUse = true;
      if (content == null) content = new ValueReference<T>();
      content.SetConstant(v);
    }

    // Casting to value
    public static implicit operator T(OptionalValue<T> oi) => oi.value;

  }

}