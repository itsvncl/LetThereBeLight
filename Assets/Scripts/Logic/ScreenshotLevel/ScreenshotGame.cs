using UnityEngine;

public class ScreenshotGame : MonoBehaviour
{
    void Start() {
        AndoridActivityManager.Instance.StartScreenshotDetector();
    }

    void OnDestroy() {
        AndoridActivityManager.Instance.StopScreenshotDetector();
    }

    void ScreenshotTaken(string s = "") {
        LevelManager.Instance.LevelComplete();
        AndoridActivityManager.Instance.StopScreenshotDetector();
    }
}
