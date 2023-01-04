using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotLogic : MonoBehaviour
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
