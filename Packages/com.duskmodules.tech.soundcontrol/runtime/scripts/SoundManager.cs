using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using DuskModules.Singletons;
using System;

namespace DuskModules.SoundControl {

  /// <summary> The SoundManager is a logic-only class with references to the game world, handling playing of sound. </summary>
  public class SoundManager : Singleton<SoundManager, SoundConfig> {

    /// <summary> Object acting as the grouping object for all players. </summary>
    private GameObject playerGroupObj;
    /// <summary> All sound players available </summary>
    private List<SoundPlayer> players;

    /// <summary> On Setup. </summary>
    protected override void Setup() {
      base.Setup();
      if (config.parameters != null) {
        for (int i = 0; i < config.parameters.Count; i++) {
          config.parameters[i].ConfirmSetup();
        }
      }
    }

    // Update
    private void Update() {
      if (config.parameters != null) {
        for (int i = 0; i < config.parameters.Count; i++) {
          config.parameters[i].Update();
        }
      }
    }

    /// <summary> Plays the given sound </summary>
    /// <param name="sound"> The sound to play </param>
    /// <param name="position"> At what position to play at </param>
    /// <param name="volumeModifier"> Volume modifier to multiply volume with </param>
    /// <param name="pitchModifier"> Pitch modifier to multiply pitch with </param>
    public SoundPlayer PlaySound(Sound sound, Vector3 position, float volumeModifier, float pitchModifier) {
      SoundPlayer player = GetSoundPlayer(position);
      player.Play(sound, volumeModifier, pitchModifier);
      return player;
    }

    /// <summary> Plays the given sound </summary>
    /// <param name="audioClip"> The audioClip to play </param>
    /// <param name="position"> At what position to play at </param>
    public SoundPlayer PlaySound(AudioClip clip, Vector3 position, AudioMixerGroup mixerGroup, float volumeModifier, float pitchModifier) {
      SoundPlayer player = GetSoundPlayer(position);
      player.Play(clip, mixerGroup, volumeModifier, pitchModifier);
      return player;
    }

    /// <summary> Gets or creates the sound player to play </summary>
    /// <param name="position"> Position to place it at </param>
    /// <returns> The sound player </returns>
    private SoundPlayer GetSoundPlayer(Vector3 position) {
      // Create sound players List
      if (players == null) players = new List<SoundPlayer>();

      // Try to get one that is inactive.
      for (int i = 0; i < players.Count; i++) {
        if (!players[i].gameObject.activeSelf) {
          players[i].gameObject.SetActive(true);
          players[i].transform.position = position;
          return players[i];
        }
      }

      // If there is no parent group, create it
      if (playerGroupObj == null) {
        playerGroupObj = new GameObject("SoundPlayerGroup");
        DontDestroyOnLoad(playerGroupObj);
      }

      // If unable to find one, create one
      GameObject playerObj = new GameObject("SoundPlayer");
      playerObj.transform.parent = playerGroupObj.transform;
      playerObj.transform.position = position;
      SoundPlayer player = playerObj.AddComponent<SoundPlayer>();
      players.Add(player);
      return player;
    }
  }
}