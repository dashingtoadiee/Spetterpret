using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules.CacheUtility {

	/// <summary> An AssetObject is a base ScriptableObject with a cache. </summary>
	public class CacheObject<C> : ScriptableObject, ICacheCarrier where C : ICache, new() {

		/// <summary> Data cache kept during runtime </summary>
		public C cache {
			get => this.GetCache<C>(CacheSetup);
			set => this.SetCache(CacheSetup, value);
		}

		/// <summary> Makes sure the cache of this object is setup, without doing anything else. </summary>
		public void ConfirmSetup() {
			this.GetCache<C>(CacheSetup);
		}

		/// <summary> Called the first time the cache has been initialized. </summary>
		protected virtual void CacheSetup() {

		}

	}
}