using UnityEngine;

public class AndroidActivityManager : MonoBehaviour {
    public static AndroidActivityManager Instance;

    private AndroidJavaClass playerClass;
    private AndroidJavaObject activity;

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
            return;
        }

        playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity");

        Instance = this;
    }

    private void Update() {
        if (Input.GetKey(KeyCode.Escape)) {

            if(LevelManager.Instance.GetCurrentLevel() == 0) {
                Application.Quit();
            }
        }
    }

    public static int getAPILevel() {
        using (var version = new AndroidJavaClass("android.os.Build$VERSION")) {
            return version.GetStatic<int>("SDK_INT");
        }
    }

    public void LockVolumeButton() {
        NullCheck();
        activity.Call("lockVolumeButton");
    }

    public void UnlockVolumeButton() {
        NullCheck();
        activity.Call("unlockVolumeButton");
    }

    public void StartFlashlightGuard() {
        NullCheck();
        activity.Call("enableFlashlightGuard");
    }

    public void StopFlashlightGuard() {
        NullCheck();
        activity.Call("disableFlashlightGuard");
    }

    public void StartScreenshotDetector() {
        NullCheck();
        activity.Call("enableScreenshotDetector");
    }
    public void StopScreenshotDetector() {
        NullCheck();
        activity.Call("disableScreenshotDetector");
    }

    public bool DeviceHasFlash() {
        NullCheck();
        return activity.Call<bool>("deviceHasFlash");
    }

    public bool DeviceHasMagnetometer() {
        NullCheck();
        return activity.Call<bool>("deviceHasMagnetometer");
    }

    public bool DeviceHasTwoTouchSupport() {
        NullCheck();
        return activity.Call<bool>("deviceHasTwoTouchSupport");
    }

    public bool DeviceHasFiveTouchSupport() {
        NullCheck();
        return activity.Call<bool>("deviceHasFiveTouchSupport");
    }

    private void NullCheck() {
        if (activity == null) {
            throw new System.Exception("Activity is null!");
        }
    }
}
