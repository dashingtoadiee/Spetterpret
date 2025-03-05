using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules.ScreenEffects {

  /// <summary> Moves the layer based on camera offset </summary>
  public class ParallaxLayerController : MonoBehaviour {

    /// <summary> ViewController singleton </summary>
    public ParallaxViewController viewController => this.GetSingleton<ParallaxViewController>();

    [Tooltip("By how much to move along with camera.")]
    [Range(-1, 1)]
    public float multiplier;

    /// <summary> This layer's original position </summary>
    public Vector3 originalPosition { get; protected set; }

    // Setup
    private void Awake() {
      originalPosition = transform.localPosition;
    }

    // Late update, move
    private void LateUpdate() {
      if (ParallaxViewController.Exists(this)) {
        Vector3 useCenter = transform.parent != null ? transform.parent.position : originalPosition;
        Vector3 delta = viewController.transform.position - useCenter;

        transform.localPosition = originalPosition + (delta * multiplier).WithZ(0);
      }
    }

  }
}