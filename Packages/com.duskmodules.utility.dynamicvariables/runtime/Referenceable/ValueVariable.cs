using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules.DynamicVariables {

	/// <summary> Abstract base class for all variable scriptableObjects </summary>
	public abstract class ValueVariable<T> : ScriptableObject, IValue<T> {

		/// <summary> Event fired when variable changes </summary>
		public event Action onChanged;

		[Header("Variable")]
		[SerializeField]
		private T _value;
		/// <summary> Value of this variable </summary>
		public T value {
			get => _value;
			set {
				if (Differs(_value, value)) {
					_value = value;
					onChanged?.Invoke();
				}
			}
		}

		/// <summary> Checks if the two values are different </summary>
		public abstract bool Differs(T a, T b);
	}

}