using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules.DynamicVariables {

  /// <summary> Value that smoothly matches the target value </summary>
  [System.Serializable]
	public class SmoothQuaternion : BaseSmoothValue<Quaternion, QuaternionReference> {

		public SmoothQuaternion() { }
		public SmoothQuaternion(Quaternion value, LerpMoveValue speed) : base(value, speed) { }

		protected override void MoveValue(float time) {
			value = speed.Move(value, target, time);
		}

		protected override bool ValuesEqual() {
			return value == target;
		}

	}
}