using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules.DynamicVariables {

	[CreateAssetMenu(menuName = "DuskModules/Variable/Float")]
	public class FloatVariable : ValueVariable<float> {
		public override bool Differs(float a, float b) {
			return a != b;
		}
	}

}