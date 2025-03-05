using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuskModules;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PilloPlay.Core {

  /// <summary> Config asset for all Pillo input settings </summary>
  [CreateAssetMenu(menuName = "PilloPlay/PilloConfig")]
  public class PilloConfig : ScriptableObject {

    /// <summary> Finds and gets the .asset in the resources of this config type </summary>
    public static PilloConfig instance {
      get {
        if (_instance == null)
          _instance = Resources.Load<PilloConfig>("Config/PilloConfig");
        return _instance;
      }
    }
    private static PilloConfig _instance;

    [Header("Pillo Input")]
    [Tooltip("Sensitivity ranges of each sensitivity setting")]
    public List<KeyValue<PilloSensitivity, IntRange>> ranges;
    [Tooltip("Maximum battery level of Pillo")]
    public int maximumBattery;

    [Tooltip("Pillo pressure value that triggers a button press event.")]
    [Range(0, 1)]
    public float buttonInputMax;
    [Tooltip("Pillo pressure from which 'button pressure' is calculated for button effects.")]
    [Range(0, 1)]
    public float buttonInputMin;
    [Tooltip("Pillo pressure deadzone below buttonInputMax which prevents quick button-press-release chains by wobbles.")]
    [Range(0, 1)]
    public float buttonDeadzone;
    [Tooltip("What triggers interaction events for Pillo button behaviour.")]
    public InteractionType buttonTriggerMode;
    [Tooltip("Button target show pressure delay.")]
    public float buttonShowPressureDelay;
    [Tooltip("Detach delay for auto-detach targets.")]
    public float autoDetachTime;
    [Tooltip("Threshold for zero pressure trigger")]
    [Range(0, 1)]
    public float zeroPressureThreshold;

    [Header("Key Input")]
    [Tooltip("The keys that trigger button press and release while in editor, per pillo.")]
    public List<PilloKeySettings> buttonKeys;
    [Tooltip("Speed with which editor button input is simulated as pressure.")]
    public float buttonKeyInputSpeed;
    [Tooltip("Whether key input for Pillos may only function in editor.")]
    public bool keyInputEditorOnly;

    [Tooltip("Key to press to reset all key bound Pillos.")]
    public KeyCode resetKeyPillosKey;

    /// <summary> Settings to simulate a Pillo using keys. </summary>
    [Serializable]
    public class PilloKeySettings {
      public List<KeyCode> pressureKeys;
    }

    /// <summary> Whether key input is allowed </summary>
    public bool allowKeyInput => !keyInputEditorOnly || Application.isEditor;

#if UNITY_EDITOR
    /// <summary> Opens this configuration. </summary>
    [MenuItem("PilloPlay/Pillo")]
    protected static void OpenConfigFile() {
      Selection.activeObject = instance;
    }
#endif
  }
}