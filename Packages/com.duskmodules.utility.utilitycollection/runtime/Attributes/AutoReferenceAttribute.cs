using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules {

	/// <summary> Attribute for AutoReference. </summary>
	public class AutoReferenceAttribute : PropertyAttribute {
		
		public bool autoCreate;
		public bool disabled;

		public AutoReferenceAttribute() {
			autoCreate = false;
			disabled = false;
		}
		
		/// <summary> Constructor with param </summary>
		/// <param name="autoCreate">If true, it will add component automatically if it is missing.</param>
		public AutoReferenceAttribute(bool autoCreate) {
			this.autoCreate = autoCreate;
			disabled = false;
		}

		/// <summary> Constructor with params </summary>
		/// <param name="autoCreate">If true, it will add component automatically if it is missing.</param>
		/// <param name="disabled">If true, it will not permit editing in the inspector</param>
		public AutoReferenceAttribute(bool autoCreate, bool disabled) {
			this.autoCreate = autoCreate;
			this.disabled = disabled;
		}

	}
}