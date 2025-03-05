using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules.DynamicVariables {

	[CreateAssetMenu(menuName = "DuskModules/Variable/String")]
	public class StringVariable : ValueVariable<string> {
		public override bool Differs(string a, string b) {
			return a != b;
		}
	}

}