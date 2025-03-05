using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuskModules;

/// <summary> Demo script showcasing use of a config. </summary>
public class ConfigDemo : MonoBehaviour {

	public Transform targetPivot;
	public SpriteRenderer targetSpriteRenderer;
	
	private void Update() {
		// Rotate using config settings
		Vector3 euler = targetPivot.eulerAngles;
		euler.z += ConfigExample.instance.targetRotateSpeed * Time.deltaTime;
		targetPivot.eulerAngles = euler;

		// Update color using config settings
		targetSpriteRenderer.color = Color.Lerp(targetSpriteRenderer.color, ConfigExample.instance.targetColor, ConfigExample.instance.targetMorphColorSpeed * Time.deltaTime);
	}
	
}