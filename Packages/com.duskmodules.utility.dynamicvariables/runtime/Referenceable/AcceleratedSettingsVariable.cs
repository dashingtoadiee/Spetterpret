using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules.DynamicVariables {

	[CreateAssetMenu(menuName = "DuskModules/Variable/Accelerated Settings")]
	public class AcceleratedSettingsVariable : ValueVariable<AcceleratedSettings> {
		public override bool Differs(AcceleratedSettings a, AcceleratedSettings b) {
			return a != b;
		}
	}

}