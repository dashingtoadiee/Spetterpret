using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules.ScreenEffects {

	/// <summary> Acts as a source for a ScreenShake. </summary>
	public class ScreenShakeSource : MonoBehaviour {

		[Tooltip("Shake to play.")]
		public ScreenShake shake;

		[Tooltip("Strength multiplier to apply to shake.")]
		public float strengthMultiplier = 1;
		[Tooltip("Frequency multiplier to apply to shake.")]
		public float frequencyMultiplier = 1;
		[Tooltip("Base direction to apply to shake.")]
		public float angle;

		[Tooltip("Whether to use the Z euler as angle for the shake. Only recommended for 2D objects.")]
		public bool addEulerZAsAngle;

		[Tooltip("Whether to play the shake on awake.")]
		public bool playOnAwake;
		
		/// <summary> Player of active shake </summary>
		public ScreenShakePlayer player { get; protected set; }
		/// <summary> Whether a shake is currently playing </summary>
		public bool isPlaying { get; protected set; }

		// Awaken
		private void Awake() {
			if (playOnAwake) {
				Play(strengthMultiplier, frequencyMultiplier, angle + (addEulerZAsAngle ? transform.eulerAngles.z : 0));
			}
		}
		// Stop shake on destroy
		private void OnDestroy() {
			if (isPlaying) {
				Stop();
			}
		}

		/// <summary> Play the shake, or update existing shake if already playing. </summary>
		/// <param name="frequencyMultiplier">  Multiplier for how fast the shake should be. </param>
		/// <param name="strengthMultiplier">  Multiplier for how powerful the shake should be. </param>
		/// <param name="angle"> Direction modifier for shake, if it uses direction. </param>
		public void Play(float frequencyMultiplier = 1, float strengthMultiplier = 1, float angle = 0) {
			this.frequencyMultiplier = frequencyMultiplier;
			this.strengthMultiplier = strengthMultiplier;
			this.angle = angle;

			if (!isPlaying) {
				player = shake.Play(frequencyMultiplier, strengthMultiplier, angle + (addEulerZAsAngle ? transform.eulerAngles.z : 0));
				player.onShakeEnd += OnShakeEnd;
				isPlaying = true;
			}
			else {
				UpdatePlayer();
			}
		}

		/// <summary> Updates the existing shake if already playing, otherwise does nothing. </summary>
		/// <param name="frequencyMultiplier">  Multiplier for how fast the shake should be. </param>
		/// <param name="strengthMultiplier">  Multiplier for how powerful the shake should be. </param>
		/// <param name="angle"> Direction modifier for shake, if it uses direction. </param>
		public void UpdateShake(float frequencyMultiplier = 1, float strengthMultiplier = 1, float angle = 0) {
			this.frequencyMultiplier = frequencyMultiplier;
			this.strengthMultiplier = strengthMultiplier;
			this.angle = angle;
			UpdatePlayer();
		}

		// If playing, update shake with values
		private void Update() {
			UpdatePlayer();
		}

		/// <summary> Updates player values </summary>
		protected void UpdatePlayer() {
			if (isPlaying) {
				player.position = transform.position;
				player.strengthMultiplier = strengthMultiplier;
				player.frequencyMultiplier = frequencyMultiplier;
				player.angle = angle + (addEulerZAsAngle ? transform.eulerAngles.z : 0);
			}
		}

		/// <summary> Stops playing the active shake </summary>
		public void Stop(float decayTime = 0) {
			if (isPlaying) {
				player.EndShake(decayTime);
			}
		}

		// On shake end
		private void OnShakeEnd() {
			isPlaying = false;
		}

	}
}