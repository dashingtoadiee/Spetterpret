using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules.DynamicVariables {

	[CreateAssetMenu(menuName = "DuskModules/Variable/Random Int")]
	public class RandomIntVariable : ScriptableObject, IValue<int> {

		public IntReference min;
		public IntReference max;

		public int value { get => Random.Range(min.value, max.value); set { int v = value; } }

	}

}