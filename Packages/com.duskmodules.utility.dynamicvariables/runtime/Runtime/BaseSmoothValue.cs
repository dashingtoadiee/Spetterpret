using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DuskModules.DynamicVariables {

	/// <summary> Simple base smooth value, used for editor code </summary>
	public abstract class BaseSmoothValue<T, VT> where VT : ValueReference<T>, new() {

		/// <summary> If using constant, don't update. </summary>
		public bool useConstant = true;

		/// <summary> Current value </summary>
		public T value;
		/// <summary> Target value </summary>
		public VT valueTarget;
		[Tooltip("Speed with which to move the smooth value")]
		public LerpMoveValue speed;

		/// <summary> The target value of this smooth value </summary>
		public T target { get => valueTarget.value; set { valueTarget.useVariable = false; valueTarget.constant = value; } }
		/// <summary> Whether the value has reached the target </summary>
		public bool atTarget => ValuesEqual();

		/// <summary> Basic constructor </summary>
		public BaseSmoothValue() { }

		/// <summary> Setup the smooth value </summary>
		public BaseSmoothValue(T value, LerpMoveValue speed) {
			useConstant = false;
			this.value = value;
			if (valueTarget == null) valueTarget = new VT();
			valueTarget.SetConstant(value);
			this.speed = speed;
		}

		/// <summary> Updating of value </summary>
		/// <param name="time"> How much time has passed in seconds </param>
		public virtual void Update(float time = -1) {
			if (time < 0) time = Time.deltaTime;
			if (useConstant || ValuesEqual()) return;
			MoveValue(time);
		}

		/// <summary> Moves the value </summary>
		protected virtual void MoveValue(float time) {

		}

		/// <summary> Checks if values are equal </summary>
		protected virtual bool ValuesEqual() {
			return false;
		}

		/// <summary> Copies the values of the target </summary>
		/// <param name="target"> The target to copy </param>
		public virtual void Copy(BaseSmoothValue<T, VT> target) {
			useConstant = target.useConstant;
			value = target.value;
			valueTarget.Copy(target.valueTarget);
			speed.Copy(target.speed);
		}

	}
}