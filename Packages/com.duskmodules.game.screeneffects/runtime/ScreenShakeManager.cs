using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuskModules.Singletons;
using System;

namespace DuskModules.ScreenEffects {

  /// <summary> Can play global shakes. </summary>
  public class ScreenShakeManager : Singleton<ScreenShakeManager> {

    // Key for saving screen shake intensity setting
    private const string intensityPlayerPrefKey = "";

    /// <summary> Event fired every time screenshake updates </summary>
    public event Action onShakeUpdate;

    /// <summary> Pool of shake players </summary>
    protected List<ScreenShakePlayer> players;

    /// <summary> Persistent global screen shake intensity </summary>
    public float intensity {
      get => globalIntensity;
      set {
        if (value < 0) value = 0;
        PlayerPrefs.SetFloat(intensityPlayerPrefKey, value);
        PlayerPrefs.Save();
        globalIntensity = value;
      }
    }
    /// <summary> Global screen shake intensity </summary>
    protected float globalIntensity;

    /// <summary> Sets up the singleton before it is used. Call this base AFTER adding components to the singleton, if any. </summary>
    protected override void Setup() {
      globalIntensity = PlayerPrefs.GetFloat(intensityPlayerPrefKey, 1);
      base.Setup();
    }

    /// <summary> Plays a screenshake. </summary>
    /// <param name="shake"> Shake to apply </param>
    /// <param name="position"> Position to centre it at </param>
    /// <param name="frequencyMultiplier"> Multiplier for how fast the shake should be. </param>
    /// <param name="strengthMultiplier">  Multiplier for how powerful the shake should be. </param>
    /// <param name="angle"> Direction modifier for shake, if it uses direction. </param>
    /// <returns> The shake player </returns>
    public ScreenShakePlayer PlayShake(ScreenShake shake, Vector3 position, float frequencyMultiplier, float strengthMultiplier, float angle) {
      ScreenShakePlayer player = GetEmptyShakeData();
      player.Play(shake, position, frequencyMultiplier, strengthMultiplier, angle);
      return player;
    }

    /// <summary> Gets and returns an empty shake data </summary>
    /// <returns> An empty shake data class </returns>
    protected ScreenShakePlayer GetEmptyShakeData() {
      if (players == null) players = new List<ScreenShakePlayer>();
      for (int i = 0; i < players.Count; i++) {
        if (players[i].life <= 0) {
          return players[i];
        }
      }
      ScreenShakePlayer newPlayer = new ScreenShakePlayer();
      players.Add(newPlayer);
      return newPlayer;
    }

    // Update shakes
    private void LateUpdate() {
      if (players != null) {
        bool active = false;
        for (int i = 0; i < players.Count; i++) {
          if (players[i].life > 0) {
            active = true;
            players[i].Update(Time.deltaTime);
          }
        }
        if (active)
          onShakeUpdate?.Invoke();
      }
    }

    /// <summary> Get view shake offset for a certain view position. </summary>
    /// <param name="position"> Where the view is located </param>
    /// <returns> ScreenShake offset to apply </returns>
    public Vector3 GetLocalShakeOffset(Vector3 position) {
      Vector2 offset = Vector2.zero;
      float tilt = 0;
      for (int i = 0; i < players.Count; i++) {
        if (players[i].life > 0) {
          float s = players[i].GetRelativeStrength(position);
          offset += players[i].offset * s;
          tilt += players[i].tilt * s;
        }
      }
      return offset.WithZ(tilt) * globalIntensity;
    }

  }
}