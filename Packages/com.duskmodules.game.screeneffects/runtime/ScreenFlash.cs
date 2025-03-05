using UnityEngine;
using System.Collections.Generic;

namespace DuskModules.GameEffects {

  /// <summary> Creates a flash in front of the screen. </summary>
  public class ScreenFlash : MonoBehaviour {

    /// <summary> A static list of all screen flashers, to which they auto subscribe. </summary>
    protected static List<ScreenFlash> screenFlashers;

    /// <summary> The flash renderer </summary>
    public SpriteRenderer flashRenderer;
    /// <summary> Faded flash </summary>
    public bool fadedFlash;

    /// <summary> Time of flash </summary>
    public float flashTime;
    /// <summary> Flash time as used </summary>
    protected float usedFlashTime;
    /// <summary> Timer of flash </summary>
    protected float flashTimer;
    /// <summary> Starting opacity </summary>
    protected float startAlpha;

    /// <summary> Flashes the screen </summary>
    public static void FlashAll() {
      FlashAll(Color.white, -1);
    }
    /// <summary> Flashes the screen </summary>
    /// <param name="c"> Color to use </param>
    public static void FlashAll(Color c) {
      FlashAll(c, -1);
    }
    /// <summary> Flashes the screen </summary>
    /// <param name="useFlashTime"> Time to flash </param>
    public static void FlashAll(float useFlashTime) {
      FlashAll(Color.white, useFlashTime);
    }
    /// <summary> Flashes the screen </summary>
    /// <param name="c"> Color to use </param>
    /// <param name="useFlashTime"> Time to flash </param>
    public static void FlashAll(Color c, float useFlashTime) {
			if (screenFlashers != null) {
				for (int i = 0; i < screenFlashers.Count; i++) {
					screenFlashers[i].Flash(c, useFlashTime);
				}
			}
    }

    /// <summary> Flashes the screen </summary>
    public void Flash() {
			Flash(Color.white, flashTime);
    }
    /// <summary> Flashes the screen </summary>
    /// <param name="c"> What color to use </param>
    public void Flash(Color c) {
			Flash(c, flashTime);
    }
    /// <summary> Flashes the screen </summary>
    /// <param name="useFlashTime"> Time to use </param>
    public void Flash(float useFlashTime) {
			Flash(Color.white, useFlashTime);
    }
    /// <summary> Flashes the screen </summary>
    /// <param name="c"> What color to use </param>
    /// <param name="useFlashTime"> Time to use </param>
    public void Flash(Color c, float useFlashTime) {
			Color current = flashRenderer.color;
			if (c.WithA(current.a) == current && c.a < current.a) return;

			if (useFlashTime < 0) useFlashTime = flashTime;
			usedFlashTime = useFlashTime;
			flashTimer = usedFlashTime;
			flashRenderer.color = c;
			flashRenderer.enabled = true;
			startAlpha = c.a;
    }

    // Awaken
    void Awake() {
      if (screenFlashers == null) screenFlashers = new List<ScreenFlash>();
      screenFlashers.Add(this);
    }
    // On Destroy
    void OnDestroy() {
      screenFlashers.Remove(this);
    }

    // Update
    void Update() {
			UpdateFlash();
    }
    /// <summary> Updates the flash </summary>
    protected void UpdateFlash() {
      if (flashTimer > 0) {
				flashTimer -= Time.deltaTime;
        if (flashTimer <= 0) {
					flashRenderer.enabled = false;
        }
        else if (fadedFlash) {
          Color c = flashRenderer.color;
          c.a = (flashTimer / usedFlashTime) * startAlpha;
					flashRenderer.color = c;
        }
      }
    }
  }

}