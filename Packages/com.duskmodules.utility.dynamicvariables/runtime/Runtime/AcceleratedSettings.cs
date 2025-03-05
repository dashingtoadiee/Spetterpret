using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace DuskModules.DynamicVariables {

	/// <summary> Settings for an accelerated value </summary>
	[System.Serializable]
	[Preserve]
	public class AcceleratedSettings {

		[Tooltip("Acceleration value, increases and decreases speed value towards target value")]
		[Preserve]
		public FloatReference acceleration;
		[Tooltip("Drag value, decreases velocity at a higher rate when velocity is high")]
		[Preserve]
		public FloatReference drag;
		[Tooltip("Linear Drag value, decreases velocity over time at a constant rate.")]
		[Preserve]
		public FloatReference linearDrag;
		[Tooltip("Linear Speed value, moves value to target at a constant rate.")]
		[Preserve]
		public FloatReference linearSpeed;

	}
}