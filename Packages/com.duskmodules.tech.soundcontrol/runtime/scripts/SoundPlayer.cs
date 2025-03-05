using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace DuskModules.SoundControl {

  /// <summary> Handles the actual playing of an audio clip with the specified parameters. </summary>
  public class SoundPlayer : MonoBehaviour {

    /// <summary> Event called when sound stops playing. </summary>
    public event Action<SoundPlayer> onPlayEnd;

    /// <summary> The actual AudioSource used </summary>
    public AudioSource source { get; private set; }

    /// <summary> Sound to play </summary>
    public Sound sound { get; private set; }

    /// <summary> Applied clip </summary>
    protected AudioClip clip;
    /// <summary> Mixer group of this sound player </summary>
    protected AudioMixerGroup mixerGroup;
    /// <summary> Applied modifier for volume </summary>
    protected float volumeModifier;
    /// <summary> Applied modifier for pitch </summary>
    protected float pitchModifier;

    /// <summary> Starts playing this sound </summary>
    /// <param name="sound"> The sound to play </param>
    /// <param name="volumeModifier"> Volume modifier to multiply volume with </param>
    /// <param name="pitchModifier"> Pitch modifier to multiply pitch with </param>
    /// <param name="stereo"> Stereo value to apply </param>
    public void Play(Sound sound, float volumeModifier, float pitchModifier) {
      // If the sound player has no audio source, create it.
      if (source == null)
        source = gameObject.AddComponent<AudioSource>();

      this.sound = sound;
      sound.onSoundChange += SetAudioSource;
      clip = sound.clip;
      mixerGroup = sound.output;

      this.volumeModifier = volumeModifier * sound.volumeVariation;
      this.pitchModifier = pitchModifier * sound.pitchVariation;
      SetAudioSource();

      source.Play();
    }

    /// <summary> Play only an audio clip </summary>
    public void Play(AudioClip clip, AudioMixerGroup mixerGroup, float volumeModifier, float pitchModifier) {
      if (source == null)
        source = gameObject.AddComponent<AudioSource>();
      this.clip = clip;
      sound = null;

      this.volumeModifier = volumeModifier;
      this.pitchModifier = pitchModifier;
      SetAudioSource();
      source.Play();
    }

    /// <summary> Copies AudioSource data from sound to source </summary>
    protected void SetAudioSource() {
      source.loop = false;
      source.clip = clip;
      source.outputAudioMixerGroup = mixerGroup;
      source.playOnAwake = false;

      if (sound != null) {
        source.volume = sound.volume * volumeModifier;
        source.pitch = sound.pitch * pitchModifier;
        source.priority = sound.priority;
        source.panStereo = sound.stereoPan;
        source.spatialBlend = sound.spatialBlend;
        source.reverbZoneMix = sound.reverbZoneMix;
        if (sound.spatialBlend > 0) {
          source.dopplerLevel = sound.dopplerLevel;
          source.minDistance = sound.minDistance;
          source.maxDistance = sound.maxDistance;
          source.rolloffMode = sound.rollOffMode;
          if (source.rolloffMode == AudioRolloffMode.Custom)
            source.SetCustomCurve(AudioSourceCurveType.CustomRolloff, sound.volumeRollOff);
        } else {
          source.dopplerLevel = 0;
          source.minDistance = 1;
          source.maxDistance = 500;
          source.rolloffMode = AudioRolloffMode.Logarithmic;
        }
      } else {
        source.volume = 1;
        source.pitch = 1;
        source.priority = 128;
        source.panStereo = 0;
        source.spatialBlend = 0;
        source.reverbZoneMix = 1;
        source.minDistance = 1;
        source.maxDistance = 500;
        source.rolloffMode = AudioRolloffMode.Logarithmic;
      }
    }

    // If player stopped playing, remove pool object.
    void Update() {
      if (!source.isPlaying) {
        Stop();
      }
    }

    /// <summary> Stops playing audio and resolve self </summary>
    public void Stop() {
      source.Stop();
      gameObject.SetActive(false);
      onPlayEnd?.Invoke(this);
      onPlayEnd = null;
      if (sound != null) {
        sound.onSoundChange -= SetAudioSource;
        sound = null;
      }
    }

    /// <summary> Set the volume of the sound player, multiplied by the modifier provided at play. </summary>
    public void SetVolume(float volume) {
      source.volume = volume * volumeModifier;
    }

    /// <summary> Set the pitch of the sound player, multiplied by the modifier provided at play. </summary>
    public void SetPitch(float pitch) {
      source.pitch = pitch * pitchModifier;
    }

    /// <summary> Sets the stereo of the sound player </summary>
    public void SetStereo(float stereo) {
      source.panStereo = stereo;
    }

    /// <summary> Set whether audio should loop or not </summary>
    public void SetLoop(bool loop) {
      source.loop = loop;
    }

    // If destroyed, stop.
    private void OnDestroy() {
      Stop();
    }
  }
}