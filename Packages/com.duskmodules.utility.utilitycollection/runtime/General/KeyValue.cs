using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules {

	/// <summary> KeyValue in serializable class format. </summary>
	[System.Serializable]
	public class KeyValue<K, V> {
		public K key;
		public V value;

		public KeyValue() { }

		public KeyValue(K key, V value) {
			this.key = key;
			this.value = value;
		}
	}
}