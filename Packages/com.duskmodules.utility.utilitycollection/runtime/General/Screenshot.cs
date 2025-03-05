using UnityEngine;
using System.Collections;
using System.IO;

namespace DuskModules {

  /// <summary> Allows player to take a screenshot </summary>
  public class Screenshot : MonoBehaviour {

    /// <summary> Key to use for screenshot </summary>
    public KeyCode screenshotKey;

    private void Awake() {
      DontDestroyOnLoad(gameObject);
    }

    // Allows taking screenshots while in editor by checking input.
    void Update() {
      if (Application.isEditor && Input.GetKeyDown(screenshotKey)) {
        Debug.Log("Screenshot taken! Check project folder 'Screenshots' (outside of assets)");

        string path = "Screenshots";

        DirectoryInfo info = new DirectoryInfo("Screenshots");
        if (!info.Exists) {
          Directory.CreateDirectory(path);
        }

        int fileIndex = 1;
        bool captured = false;
        while (!captured) {
          string useNumber = "" + fileIndex;
          if (fileIndex < 10) useNumber = "0" + useNumber;

          string usePath = path + "/Screenshot" + useNumber + ".png";
          if (!File.Exists(usePath)) {
            ScreenCapture.CaptureScreenshot(usePath);
            captured = true;
          }
          fileIndex++;
        }
      }
    }
  }
}