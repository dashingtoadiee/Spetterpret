using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuskModules;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary> Example config used to showcase how a config works. </summary>
public class ConfigExample : Config<ConfigExample> {

	[Header("Example values")]
	public Color targetColor;
	public float targetMorphColorSpeed;
	public float targetRotateSpeed;

#if UNITY_EDITOR
	[MenuItem("DuskModules/Settings/ConfigExample")]
	public static void OpenConfig() { OpenConfigFile(); }
#endif

}