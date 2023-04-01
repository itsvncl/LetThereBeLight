using System.Collections;
using UnityEngine;
using UnityEngine.Android;

public class ScreenshotGame : MonoBehaviour {
    [SerializeField] private GameObject unplayableOverlay;

    void Start() {
        if (!Permission.HasUserAuthorizedPermission("android.permission.READ_EXTERNAL_STORAGE")) {
            Permission.RequestUserPermission("android.permission.READ_EXTERNAL_STORAGE");
        }

        StartCoroutine(CheckPermissionAccess());
    }

    IEnumerator CheckPermissionAccess() {
        yield return new WaitForSeconds(0.5f);

        if (!Permission.HasUserAuthorizedPermission("android.permission.READ_EXTERNAL_STORAGE")) {
            unplayableOverlay.SetActive(true);
            gameObject.SetActive(false);
        }
        else {
            AndroidActivityManager.Instance.StartScreenshotDetector();
        }
    }

    void OnDestroy() {
        AndroidActivityManager.Instance.StopScreenshotDetector();
    }

    void ScreenshotTaken(string s = "") {
        LevelManager.Instance.LevelComplete();
        AndroidActivityManager.Instance.StopScreenshotDetector();
    }
}
