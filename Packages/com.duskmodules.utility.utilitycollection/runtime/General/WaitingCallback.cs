using System;
using UnityEngine;

namespace DuskModules {

	/// <summary> Callback waiting for all things to complete before firing. </summary>
	public class WaitingCallback {

		public Action callback;

		public int waitCount { get; private set; }
		public bool waitSet { get; private set; }
		public int hitCount { get; private set; }
		public bool done { get; private set; }

		/// <summary> Creates the callback. </summary>
		public WaitingCallback(Action callback) {
			this.callback = callback;
			done = false;
		}

		/// <summary> Resets the waiting callback </summary>
		public void Reset(Action callback) {
			this.callback = callback;
			done = false;
			waitSet = false;
			waitCount = 0;
			hitCount = 0;
		}

		/// <summary> Sets the count it waits for. Returns whether completed. </summary>
		public void SetWait(int wait) {
			waitCount = wait;
			waitSet = true;
			CheckHit();
		}

		/// <summary> Fired when something completes. Returns whether completed. </summary>
		public void CallbackHit() {
			hitCount++;
			CheckHit();
		}

		/// <summary> Forces the waiting callback to complete. </summary>
		public void Complete() {
			if (!done) {
				callback();
				done = true;
			}
		}

		private void CheckHit() {
			if (!done && waitSet && hitCount >= waitCount) {
				callback();
				done = true;
			}
		}
	}
}