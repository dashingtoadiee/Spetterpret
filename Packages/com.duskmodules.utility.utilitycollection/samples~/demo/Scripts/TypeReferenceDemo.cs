using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuskModules;

/// <summary> Demo script showcasing that you can reference a type, and use that type to get the System.Type. </summary>
public class TypeReferenceDemo : MonoBehaviour {

	public TypeReference type;

	public MonoBehaviour target;

	[EditorButton]
	public void PrintTypeName() {
		Debug.Log(type.type.Name + " : " + target.IsOfType(type.type));
	}

}