using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules {

	/// <summary> A Unity Layer, selectable in the inspector, accepting only one layer. </summary>
	[System.Serializable]
	public class UnityLayer {

		[SerializeField]
		private int _layerIndex;
		/// <summary> Layer Index of this Unity layer </summary>
		public int layerIndex => _layerIndex;

		/// <summary> LayerMask integer </summary>
		public int mask => 1 << _layerIndex;

		/// <summary> Set the new layer by code </summary>
		/// <param name="index"> What layer this layer needs to be set to. </param>
		public void SetLayer(int index) {
			if (index > 0 && index < 32) {
				_layerIndex = index;
			}
		}

		// Casting to value for use in layers
		public static implicit operator int(UnityLayer x) => x._layerIndex;

	}

}