using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuskModules;

/// <summary> Showcases an implementation of the Attributes Utility of the DuskModules. </summary>
public class EditorColorShifter : MonoBehaviour {

	[Header("Editor Color Shifter")]
	public SpriteRenderer spriteRenderer;

	[Space]
	public Color colorA = Color.white;
	public Color colorB = Color.red;

	[Space]
	public float minSize = 0.9f;
	public float maxSize = 1.1f;

	public float resizeSpeed;

	[ReadOnly]
	public float target;
	[ReadOnly]
	public float size;

	private void Awake() {
		target = spriteRenderer.transform.localScale.x;
		size = target;
	}

	[EditorButton]
	public void ChangeColor() {
		spriteRenderer.color = (spriteRenderer.color == colorA) ? colorB : colorA;
	}

	[EditorButton]
	public void Resize() {
		target = Random.Range(minSize, maxSize);
		size = target;
		spriteRenderer.transform.localScale = Vector3.one * size;
	}

	[EditorButton(true)]
	public void ResizeOverTime() {
		target = Random.Range(minSize, maxSize);
	}
	
	// Resize over time
	private void Update() {
		size = Mathf.MoveTowards(size, target, resizeSpeed * Time.deltaTime);
		spriteRenderer.transform.localScale = Vector3.one * size;
	}


}