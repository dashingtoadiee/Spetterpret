using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules.DynamicVariables {

	[CreateAssetMenu(menuName = "DuskModules/Variable/Random Color")]
	public class RandomColorVariable : ScriptableObject, IValue<Color> {

		public ColorReference min;
		public ColorReference max;

		public Color value { get => Color.Lerp(min.value, max.value, Random.Range(0f, 1f)); set { Color v = value; } }

	}

}