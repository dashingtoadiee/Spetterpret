using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> A ScriptableObject implementation of IInterfaceReferenceDemoInterface </summary>
[CreateAssetMenu(menuName = "DuskModules/Demo/UtilityCollection/InterfaceReferenceSO")]
public class InterfaceReferenceSO : ScriptableObject, IDemoInterface {

	public void SayHello() {
		Debug.Log("Hello, I am a Scriptable Object!");
	}

}