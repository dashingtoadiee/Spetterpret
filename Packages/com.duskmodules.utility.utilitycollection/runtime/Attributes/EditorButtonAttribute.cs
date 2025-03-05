using UnityEngine;

namespace DuskModules {

	/// <summary> Place this above a method to show a button for it. </summary>
	[System.AttributeUsage(System.AttributeTargets.Method)]
	public class EditorButtonAttribute : PropertyAttribute {

		/// <summary> Whether to work only while application is playing </summary>
		public bool runTimeOnly;

		public EditorButtonAttribute() {
			this.runTimeOnly = false;
		}

		public EditorButtonAttribute(bool runtimeOnly) {
			this.runTimeOnly = runtimeOnly;
		}

	}

}