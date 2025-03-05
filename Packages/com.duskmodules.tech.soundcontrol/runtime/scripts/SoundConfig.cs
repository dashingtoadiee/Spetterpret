using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace DuskModules.SoundControl {

  /// <summary> Config file for all sound settings. </summary>
  public class SoundConfig : Config<SoundConfig> {

    /// <summary> Audio Mixer Groups to track parameters of. </summary>
    [Tooltip("Audio Mixer Groups that are in use in the game, of which parameters need to be tracked.")]
    [SerializeField]
    protected List<AudioMixer> _audioMixers;
    /// <summary> Audio Mixer Groups to track parameters of. </summary>
    public List<AudioMixer> audioMixers => _audioMixers;

    /// <summary> Player preferences key </summary>
    public string playerPreferencesKey;

    /// <summary> All parameters </summary>
    public List<SoundMixerParameter> parameters;
    /// <summary> Parameters that have no matching parameter and need to be updated. </summary>
    public List<SoundMixerParameter> conflictParameters;
    /// <summary> AudioMixers of new parameters that have yet to be utilized </summary>
    public List<AudioMixer> undeterminedMixers;
    /// <summary> New parameters that have yet to be utilized </summary>
    public List<string> undeterminedParameters;


#if UNITY_EDITOR
    [MenuItem("DuskModules/Settings/Sound")]
    public static void OpenConfig() { OpenConfigFile(); }
#endif

  }
}