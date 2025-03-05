using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules.DynamicVariables {

	[CreateAssetMenu(menuName = "DuskModules/Variable/Int")]
	public class IntVariable : ValueVariable<int> {
		public override bool Differs(int a, int b) {
			return a != b;
		}
	}

}