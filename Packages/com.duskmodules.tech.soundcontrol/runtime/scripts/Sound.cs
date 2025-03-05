using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace DuskModules.SoundControl {

  /// <summary> Playable sound </summary>
  [CreateAssetMenu(menuName = "DuskModules/Audio/Sound", order = 0)]
  public class Sound : ScriptableObject {

    /// <summary> Called when sound has changed </summary>
    public event Action onSoundChange;

    [Tooltip("Set whether the sound should play through an Audio Mixer first or directly to the Audio Listener.")]
    [SerializeField]
    protected AudioMixerGroup _output;
    /// <summary> Output audio mixer group for this sound. </summary>
    public AudioMixerGroup output => _output;

    [Tooltip("Set to true to enable random selection from multiple audio clips.")]
    [SerializeField]
    protected bool _randomAudioClip;

    [Tooltip("The AudioClip asset played by the sound.")]
    [SerializeField]
    protected AudioClip _clip;

    [Tooltip("List of AudioClip assets, of which one is randomly picked to be played by the sound.")]
    [SerializeField]
    protected List<AudioClip> _clips;
    /// <summary> The AudioClip to be played </summary>
    public AudioClip clip {
      get {
        if (!_randomAudioClip) return _clip;
        else {
          return _clips[UnityEngine.Random.Range(0, _clips.Count)];
        }
      }
    }

    [Tooltip("Sets the overal volume of the sound.")]
    [Range(0, 1)]
    [SerializeField]
    protected float _volume = 1;
    /// <summary> Volume of sound </summary>
    public float volume => _volume;

    [Tooltip("Sets the frequency of the sound. Use this to slow down or speed up the sound.")]
    [Range(-3, 3)]
    [SerializeField]
    protected float _pitch = 1;
    /// <summary> Pitch of sound </summary>
    public float pitch => _pitch;

    [Tooltip("Sets the priority of the sound. Note that a sound with a larger priority value will more likely be stolen by sounds with smaller priority values.")]
    [Range(0, 256)]
    [SerializeField]
    protected int _priority = 128;
    /// <summary> Priority of sound </summary>
    public int priority => _priority;

    [Tooltip("Pans a playing sound in a stereo way (left or right). This only applies to sounds that are Mono or Stereo.")]
    [Range(-1, 1)]
    [SerializeField]
    protected float _stereoPan = 0;
    /// <summary> Stereopan of sound </summary>
    public float stereoPan => _stereoPan;

    [Tooltip("Sets how much this sound is treated as a 3D source. 3D sources are affected by spatial position and spread. " +
      "If 3D Pan Level is 0, all spatial attenuation is ignored.")]
    [Range(0, 1)]
    [SerializeField]
    protected float _spatialBlend = 0;
    /// <summary> SpatialBlend of sound </summary>
    public float spatialBlend => _spatialBlend;

    [Tooltip("Sets how much of the signal this sound is mixing into the global reverb associated with the zones. " +
      "[0,1] is a linear range (like volume) while [1, 1.1] lets you boost the reverb mix by 10 dB..")]
    [Range(0, 1.1f)]
    [SerializeField]
    protected float _reverbZoneMix = 1;
    /// <summary> Reverb Zone Mix of sound </summary>
    public float reverbZoneMix => _reverbZoneMix;

    [Tooltip("Doppler scale of the sound.")]
    [SerializeField]
    [Range(0, 1)]
    protected float _dopplerLevel = 1;
    /// <summary> Doppler scale of the sound </summary>
    public float dopplerLevel => _dopplerLevel;

    [Tooltip("How the AudioSource attenuates over distance.")]
    [SerializeField]
    protected AudioRolloffMode _rollOffMode;
    /// <summary> How the AudioSource attenuates over distance. </summary>
    public AudioRolloffMode rollOffMode => _rollOffMode;

    [Tooltip("Minimum distance utilized for volume rolloff curve.")]
    [SerializeField]
    protected float _minDistance = 1;
    /// <summary> Minimum distance for volume roll off </summary>
    public float minDistance => _minDistance;

    [Tooltip("Maximum distance utilized for volume rolloff curve.")]
    [SerializeField]
    protected float _maxDistance = 500;
    /// <summary> Maximum distance for volume roll off </summary>
    public float maxDistance => _maxDistance;

    [Tooltip("Curve for the custom volume rolloff, determining the volume based on the distance of this source to the AudioListener.")]
    [SerializeField]
    protected AnimationCurve _volumeRollOff;
    /// <summary> Roll off for distance to volume </summary>
    public AnimationCurve volumeRollOff => _volumeRollOff;

    [Tooltip("Random variation in volume of sound.")]
    [SerializeField]
    protected SoundVariation _volumeVariation;
    /// <summary> Volume variation </summary>
    public float volumeVariation => _volumeVariation.value;

    [Tooltip("Random variation in pitch of sound.")]
    [SerializeField]
    protected SoundVariation _pitchVariation;
    /// <summary> Pitch variation </summary>
    public float pitchVariation => _pitchVariation.value;

    /// <summary> Returns true if it has at least 1 audioclip set. </summary>
    public bool hasClip {
      get {
        if (_randomAudioClip) {
          if (_clips.Count > 0) {
            for (int i = 0; i < _clips.Count; i++) {
              if (_clips[i] != null)
                return true;
            }
          }
          return false;
        } else return (_clip != null);
      }
    }

    /// <summary> Called by editor when sound has been updated </summary>
    public void SoundUpdated() {
      onSoundChange?.Invoke();
    }

    /// <summary> Plays the sound. </summary>
    public SoundPlayer Play() {
      return Play(Vector3.zero);
    }
    /// <summary> Plays the sound at the given position. </summary>
    /// <param name="position"> Position to play it at. </param>
    public SoundPlayer Play(Vector3 position) {
      return Play(position, 1, 1);
    }
    /// <summary> Plays the sound. </summary>
    /// <param name="volumeModifier"> Volume modifier to multiply volume with </param>
    /// <param name="pitchModifier"> Pitch modifier to multiply pitch with </param>
    public SoundPlayer Play(float volumeModifier, float pitchModifier) {
      return Play(Vector3.zero, volumeModifier, pitchModifier);
    }

    /// <summary> Plays the sound at the given position. </summary>
    /// <param name="position"> Position to play it at. </param>
    /// <param name="volumeModifier"> Volume modifier to multiply volume with </param>
    /// <param name="pitchModifier"> Pitch modifier to multiply pitch with </param>
    public SoundPlayer Play(Vector3 position, float volumeModifier, float pitchModifier) {
      return SoundManager.instance.PlaySound(this, position, volumeModifier, pitchModifier);
    }
  }
}