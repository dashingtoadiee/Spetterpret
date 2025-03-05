using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuskModules;

/// <summary> Demo script showcasing that you can reference anything implementing an interface, using the InterfaceReference field. </summary>
public class InterfaceReferenceDemo : MonoBehaviour {

#if UNITY_2020_1_OR_NEWER
	// In Unity 2020, you do not need to declare a class to have the interface reference exposed in the Inspector GUI.
	public InterfaceReference<IDemoInterface> targetSO;
	public InterfaceReference<IDemoInterface> targetBehaviour;
#else
	// In Unity 2019 and lower, you need to create a class implementation of the interface type you need, for it to be exposed in the Inspector GUI.
	public DemoInterfaceReference targetSO;
	public DemoInterfaceReference targetBehaviour;
#endif

	[EditorButton]
	public void Hello() {
		targetSO.content.SayHello();
		targetBehaviour.content.SayHello();
	}
}

#if !UNITY_2020_1_OR_NEWER
public class DemoInterfaceReference : InterfaceReference<IDemoInterface> { }
#endif