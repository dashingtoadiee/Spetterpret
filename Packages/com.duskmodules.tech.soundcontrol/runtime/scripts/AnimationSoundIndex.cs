using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules.SoundControl {

  /// <summary> Provides an index of sounds to play for animations </summary>
  public class AnimationSoundIndex : MonoBehaviour {

    /// <summary> Plays a sound </summary>
    public void PlaySound(Sound sound) {
      sound.Play();
    }

  }

}