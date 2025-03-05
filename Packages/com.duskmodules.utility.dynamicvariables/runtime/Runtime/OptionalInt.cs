using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules.DynamicVariables {

	/// <summary> Float value which can be in use or not </summary>
	[System.Serializable]
	public class OptionalInt {
		public bool inUse;
		public IntReference content;

		public int value => inUse ? content.value : 0;

		/// <summary> Copies internal data from one to other </summary>
		public void Copy(OptionalInt other) {
			inUse = other.inUse;
			if (content == null) content = new IntReference();
			content.Copy(other.content);
		}

		/// <summary> Sets the int </summary>
		public void SetInt(int v) {
			inUse = true;
			if (content == null) content = new IntReference();
			content.SetConstant(v);
		}

		// Casting to value
		public static implicit operator int(OptionalInt oi) => oi.value;

	}

}
