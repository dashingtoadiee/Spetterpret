using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules.DynamicVariables {

	/// <summary> Abstract base class for any value reference </summary>
	[System.Serializable]
	public class ValueReferenceBase<T> {

		/// <summary> Whether to use variable value instead of constant </summary>
		public bool useVariable = false;

		/// <summary> Constant value if a constant instead </summary>
		public T constant;

		/// <summary> The interface </summary>
		public IValue<T> content {
			get {
				if (contentObject != null) {
					object baseObj = contentObject;
					return (IValue<T>)baseObj;
				}
				else {
					Debug.LogError("Null contentObject for ValueReference of type " + typeof(T).FullName);
					return null;
				}
			}
		}
		/// <summary> The referenced object </summary>
		public Object contentObject;

		/// <summary> Value of this value reference </summary>
		public T value {
			get {
				return useVariable ? content.value : constant;
			}
			set {
				if (useVariable) content.value = value;
				else constant = value;
			}
		}

		/// <summary> Copies data from another value reference of the same type </summary>
		public void Copy(ValueReferenceBase<T> other) {
			useVariable = other.useVariable;
			constant = other.constant;
			contentObject = other.contentObject;
		}

		/// <summary> Set the constant value </summary>
		public void SetConstant(T v) {
			constant = v;
			useVariable = false;
		}

		/// <summary> Set the variable reference </summary>
		public void SetVariable<V>(V v) where V : UnityEngine.Object, IValue<T> {
			contentObject = v;
			useVariable = true;
		}

		// Casting to value
		public static implicit operator T(ValueReferenceBase<T> vr) => vr.value;

	}
}
