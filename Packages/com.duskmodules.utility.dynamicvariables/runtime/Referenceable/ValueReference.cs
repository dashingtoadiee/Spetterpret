using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules.DynamicVariables {

	/// <summary> Generic version of value reference, which is tied to generic editor property drawer </summary>
	[Serializable]
	public class ValueReference<T> : ValueReferenceBase<T> { }

}