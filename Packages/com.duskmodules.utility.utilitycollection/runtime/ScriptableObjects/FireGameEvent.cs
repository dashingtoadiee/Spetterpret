using UnityEngine;

namespace DuskModules {

	/// <summary> Behaviour that simply fires a game event on awake. </summary>
	public class FireGameEvent : MonoBehaviour {

		/// <summary> Event to fire </summary>
		public GameEvent fireEvent;

		private void Awake() {
			fireEvent.FireEvent();
		}
	}
}