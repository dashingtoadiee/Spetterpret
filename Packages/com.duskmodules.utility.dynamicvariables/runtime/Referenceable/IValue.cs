using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace DuskModules.DynamicVariables {

	/// <summary> Interface for anything which can return a value of a certain type. </summary>
	//[Preserve]
	public interface IValue<T> : IValue {
		T value { get; set; }
	}

	/// <summary> Base IValue interface, for referencing in 2019 and lower. </summary>
	//[Preserve]
	public interface IValue {

	}

}