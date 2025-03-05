using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules.DynamicVariables {

  [CreateAssetMenu(menuName = "DuskModules/Variable/Bool")]
  public class BoolVariable : ValueVariable<bool> {
    public override bool Differs(bool a, bool b) {
      return a != b;
    }
  }

}