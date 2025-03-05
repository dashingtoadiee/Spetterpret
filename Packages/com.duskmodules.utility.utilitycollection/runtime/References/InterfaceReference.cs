using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace DuskModules {
	
	/// <summary> Reference wrapper for any interface. </summary>
	[System.Serializable]
	public class InterfaceReference<T> {

		/// <summary> The interface </summary>
		public T content => (T)(object)contentObject;
		
		/// <summary> The referenced object </summary>
		public Object contentObject;

	}

}