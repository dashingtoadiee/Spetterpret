using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules.DynamicVariables {

  /// <summary> Value that smoothly matches the target value </summary>
  [System.Serializable]
  public class SmoothVector2 : BaseSmoothValue<Vector2, Vector2Reference> {

		public SmoothVector2() { }
		public SmoothVector2(Vector2 value, LerpMoveValue speed) : base(value, speed) { }

		protected override void MoveValue(float time) {
			value = speed.Move(value, target, time);
		}

		protected override bool ValuesEqual() {
			return value == target;
		}

  }
}