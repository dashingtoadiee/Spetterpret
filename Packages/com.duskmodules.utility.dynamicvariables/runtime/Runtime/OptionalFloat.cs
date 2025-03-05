using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules.DynamicVariables {

	/// <summary> Float value which can be in use or not </summary>
	[System.Serializable]
	public class OptionalFloat {
		public bool inUse;
		public FloatReference content;

		public float value => inUse ? content.value : 0;

		/// <summary> Copies internal data from one to other </summary>
		public void Copy(OptionalFloat other) {
			inUse = other.inUse;
			if (content == null) content = new FloatReference();
			content.Copy(other.content);
		}

		/// <summary> Sets the float </summary>
		public void SetFloat(float v) {
			inUse = true;
			if (content == null) content = new FloatReference();
			content.SetConstant(v);
		}

		// Casting to value
		public static implicit operator float(OptionalFloat of) => of.value;

	}

}
