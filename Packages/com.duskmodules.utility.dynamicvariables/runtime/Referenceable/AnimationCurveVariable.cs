using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules.DynamicVariables {

  [CreateAssetMenu(menuName = "DuskModules/Variable/Animation Curve")]
  public class AnimationCurveVariable : ValueVariable<AnimationCurve> {
    public override bool Differs(AnimationCurve a, AnimationCurve b) {
      return a != b;
    }
  }

}