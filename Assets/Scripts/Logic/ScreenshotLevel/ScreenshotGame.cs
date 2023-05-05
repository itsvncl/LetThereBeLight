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
        int androidSDK = AndroidActivityManager.getAPILevel();
        yield return new WaitForSeconds(0.5f);

        if (androidSDK >= 33)
        {
            if (!Permission.HasUserAuthorizedPermission("android.permission.READ_MEDIA_IMAGES"))
            {
                Permission.RequestUserPermission("android.permission.READ_MEDIA_IMAGES");
            }
        }
        yield return new WaitForSeconds(0.5f);

        if (!Permission.HasUserAuthorizedPermission("android.permission.READ_EXTERNAL_STORAGE")) {
            if(androidSDK >= 33 && Permission.HasUserAuthorizedPermission("android.permission.READ_MEDIA_IMAGES"))
            {
                AndroidActivityManager.Instance.StartScreenshotDetector();
            }
            else
            {
                unplayableOverlay.SetActive(true);
                gameObject.SetActive(false);
            }
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
