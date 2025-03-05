using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules.SoundControl {

  /// <summary> Initializes the sound control instance  </summary>
  public class SoundInitializer {

    [RuntimeInitializeOnLoadMethod]
    private static void OnRuntime() {
      // Gets the instance, which is then created and handles the rest.
      SoundManager instance = SoundManager.instance;
    }

  }
}