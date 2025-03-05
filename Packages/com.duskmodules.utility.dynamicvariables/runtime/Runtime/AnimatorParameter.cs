using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules.DynamicVariables {

  /// <summary> Animator parameter setting, which can be applied to an animator </summary>
  [System.Serializable]
  public class AnimatorParameter {

    /// <summary> Parameter name </summary>
    public string parameterName;

    /// <summary> Type of parmeter </summary>
    public AnimatorControllerParameterType type;

    /// <summary> Float value, if used </summary>
    public float valueFloat;
    /// <summary> Integer value, if used </summary>
    public int valueInt;
    /// <summary> Bool value, if used </summary>
    public bool valueBool;

    /// <summary> Applies this animator parameter to the given animator </summary>
    public void ApplyTo(Animator animator) {
      switch (type) {
        case AnimatorControllerParameterType.Float: animator.SetFloat(parameterName, valueFloat); break;
        case AnimatorControllerParameterType.Int: animator.SetInteger(parameterName, valueInt); break;
        case AnimatorControllerParameterType.Bool: animator.SetBool(parameterName, valueBool); break;
        case AnimatorControllerParameterType.Trigger: animator.SetTrigger(parameterName); break;
      }
    }

  }
}