using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules.DynamicVariables {

	/// <summary> Color value which can be in use or not </summary>
	[System.Serializable]
	public class OptionalColor {
		public bool inUse;
		public ColorReference content;

		public Color value => inUse ? content.value : Color.black.WithA(0);

		/// <summary> Copies internal data from one to other </summary>
		public void Copy(OptionalColor other) {
			inUse = other.inUse;
			if (content == null) content = new ColorReference();
			content.Copy(other.content);
		}

		/// <summary> Sets the color </summary>
		public void SetColor(Color v) {
			inUse = true;
			if (content == null) content = new ColorReference();
			content.SetConstant(v);
		}

		// Casting to value
		public static implicit operator Color(OptionalColor oc) => oc.value;

	}
}