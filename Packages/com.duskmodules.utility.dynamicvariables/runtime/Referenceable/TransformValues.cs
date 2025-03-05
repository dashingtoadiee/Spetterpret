using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules.DynamicVariables {

	/// <summary> Exposes transform values for use by IValue </summary>
	public class TransformValues : MonoBehaviour, IValue<Vector3>, IValue<Quaternion> {

		/// <summary> Types of transform values </summary>
		public enum TransformValueType {
			position,
			rotation,
			scale
		}
		/// <summary> Value type to expose </summary>
		public TransformValueType valueType;

		/// <summary> Whether to use local values where possible. </summary>
		public bool local;

		/// <summary> Value of this transform </summary>
		public Vector3 value {
			get {
				switch (valueType) {
					case TransformValueType.position: return local ? transform.localPosition : transform.position;
					case TransformValueType.rotation: return local ? transform.localEulerAngles : transform.eulerAngles;
					case TransformValueType.scale: return local ? transform.localScale : transform.lossyScale;
				}
				return Vector3.zero;
			}
			set {
				if (local) {
					switch (valueType) {
						case TransformValueType.position: transform.localPosition = value; break;
						case TransformValueType.rotation: transform.localEulerAngles = value; break;
						case TransformValueType.scale: transform.localScale = value; break;
					}
				}
				else {
					switch (valueType) {
						case TransformValueType.position: transform.position = value; break;
						case TransformValueType.rotation: transform.eulerAngles = value; break;
					}
				}
			}
		}

		/// <summary> Always returns rotation </summary>
		Quaternion IValue<Quaternion>.value { 
			get {
				return local ? transform.localRotation : transform.rotation;
			}
			set {
				if (local) transform.localRotation = value;
				else transform.rotation = value;
			}
		}

	}
}