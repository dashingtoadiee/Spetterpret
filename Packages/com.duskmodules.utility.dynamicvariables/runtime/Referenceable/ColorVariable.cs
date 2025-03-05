using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules.DynamicVariables {

	[CreateAssetMenu(menuName = "DuskModules/Variable/Color")]
	public class ColorVariable : ValueVariable<Color> {
		public override bool Differs(Color a, Color b) {
			return a != b;
		}
	}

}