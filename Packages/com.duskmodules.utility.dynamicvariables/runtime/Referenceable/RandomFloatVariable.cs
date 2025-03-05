using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules.DynamicVariables {

	[CreateAssetMenu(menuName = "DuskModules/Variable/Random Float")]
	public class RandomFloatVariable : ScriptableObject, IValue<float> {

		public FloatReference min;
		public FloatReference max;

		public float value { get => Random.Range(min.value, max.value) ; set { float v = value; } }

	}

}