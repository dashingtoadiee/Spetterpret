using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules {

  /// <summary> Can call particle system to play </summary>
  public class AnimationParticles : MonoBehaviour {

    [Tooltip("Particles to play")]
    public ParticleSystem particles;

    /// <summary> Plays the particles </summary>
    public void PlayParticles() {
      particles.Play();
    }

  }
}