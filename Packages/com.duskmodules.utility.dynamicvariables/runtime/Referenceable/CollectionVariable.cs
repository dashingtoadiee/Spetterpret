using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules.DynamicVariables {

	/// <summary> Abstract class for any scriptable object with a list of values </summary>
	public abstract class CollectionVariable<T> : ScriptableObject, IValue<T>, IValue<List<T>> {

		public List<T> collection;

		public T value { get => collection.GetRandom(); set { T v = value; } }

		List<T> IValue<List<T>>.value { get => collection; set => collection = value; }

	}

}