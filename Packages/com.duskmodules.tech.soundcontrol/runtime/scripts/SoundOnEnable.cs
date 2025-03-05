using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules.SoundControl {

  /// <summary> Script that simply plays a single sound on enable. </summary>
  public class SoundOnEnable : MonoBehaviour {

    /// <summary> Sound to play </summary>
    public Sound playSound;

    // Play
    private void OnEnable() {
			playSound.Play();
    }
  }
}