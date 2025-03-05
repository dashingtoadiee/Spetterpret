using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> A MonoBehaviour implementation of IInterfaceReferenceDemoInterface </summary>
public class InterfaceReferenceBehaviour : MonoBehaviour, IDemoInterface {

	public void SayHello() {
		Debug.Log("Hello, I am a MonoBehaviour!");
	}

}