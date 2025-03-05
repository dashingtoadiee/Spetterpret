using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Demo script showcasing what is possible with angle calculations </summary>
public class AngleExtensionsDemo : MonoBehaviour {

	[Header("Pointer")]
	public Transform pointerPivot;
	public Transform smoothPointerPivot;
	public float smoothPointerSpeed;

	[Header("Offsets")]
	public Transform pointerOrbitor;
	public float pointerOrbitorDistance;
	public TextMesh textDegrees;

	[Header("Following")]
	public List<Transform> homingMissiles;
	public float homingMissileRotateSpeed;
	public float homingMissileForwardSpeed;

	private void Update() {
		Vector2 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		// Pointer
		Vector2 pointerDelta = (mp - pointerPivot.position.XY());               // Delta vector from pointerPivot to mouse position
		float pointAngle = pointerDelta.GetAngle();															// Angle of delta vector
		float smoothPointZ = smoothPointerPivot.eulerAngles.z;

		pointerPivot.eulerAngles = pointerPivot.eulerAngles.WithZ(pointAngle);			// Set rotation to angle to point to mouse.

		float smoothPointTargetZ = pointAngle.ComparisonAngle(smoothPointZ);            // Brings 'pointAngle' value within -180, 180 range relative to smoothPointerPivot angle. Results in shortest rotation distance.
		smoothPointZ = Mathf.MoveTowards(smoothPointZ, smoothPointTargetZ, smoothPointerSpeed * Time.deltaTime);  // Smooth move angle to target angle.
		smoothPointerPivot.eulerAngles = pointerPivot.eulerAngles.WithZ(smoothPointZ);  // Set rotation to smoothed angle


		// Offset
		Vector2 orbitorPos = pointerPivot.position.XY() + Vector2.up.RotateVector(pointAngle) * pointerOrbitorDistance;
		pointerOrbitor.position = orbitorPos;

		textDegrees.text = (int)pointAngle + "";


		// Homing Missiles
		for (int i = 0; i < homingMissiles.Count; i++) {
			Transform missile = homingMissiles[i];
			float speedMult = (1 + ((float)i) / 6);

			Vector2 missileDelta = (mp - missile.position.XY());
			float missileAngle = missileDelta.GetAngle();
			float smoothMissileZ = missile.eulerAngles.z;

			float smoothMissileTargetZ = missileAngle.ComparisonAngle(smoothMissileZ);
			smoothMissileZ = Mathf.MoveTowards(smoothMissileZ, smoothMissileTargetZ, speedMult * homingMissileRotateSpeed * Time.deltaTime);
			missile.eulerAngles = missile.eulerAngles.WithZ(smoothMissileZ);

			Vector2 missileMoveVector = Vector2.up.RotateVector(smoothMissileZ) * homingMissileForwardSpeed * speedMult;
			missile.position = missile.position.XY() + missileMoveVector * Time.deltaTime;   // Move forward in the direction of the missile delta.
		}
	}
}