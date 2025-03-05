using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuskModules.Singletons;

namespace DuskModules.ScreenEffects {

  /// <summary> Controller for the parallax view </summary>
  public class ParallaxViewController : SceneSingleton<ParallaxViewController> {

    [Tooltip("Offset between the component's transform and this transform determines parallax.")]
    public Transform referenceTransform;

    /// <summary> Original position of the view </summary>
    public Vector3 originalPosition { get; protected set; }
    /// <summary> Active center of the view </summary>
    public Vector3 center => referenceTransform != null ? referenceTransform.position : originalPosition;

    /// <summary> Delta from original position, used by parallax </summary>
    public Vector2 delta => transform.position - center;

    /// <summary> On Setup </summary>
    protected override void Setup() {
      base.Setup();
      originalPosition = transform.position;
    }

  }

}