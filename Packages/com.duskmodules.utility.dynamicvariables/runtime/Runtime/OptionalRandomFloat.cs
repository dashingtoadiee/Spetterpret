using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules.DynamicVariables {

	/// <summary> Float value which can be in use or not </summary>
	[System.Serializable]
	public class OptionalRandomFloat {
		public bool inUse;
		public RandomFloat content;

		public float value => inUse ? content.value : 0;

		/// <summary> Copies internal data from one to other </summary>
		public void Copy(OptionalRandomFloat other) {
			inUse = other.inUse;
			if (content == null) content = new RandomFloat();
			content.Copy(other.content);
		}

		/// <summary> Sets the float </summary>
		public void SetFloat(float v) {
			inUse = true;
			if (content == null) content = new RandomFloat();
			content.SetConstant(v);
		}

	}

}
