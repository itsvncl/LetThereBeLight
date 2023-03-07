using UnityEngine;

public class AndoridActivityManager : MonoBehaviour {
    public static AndoridActivityManager Instance;
    
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

    public void StartLightSensorGuard(float targetValue) {
        NullCheck();
        activity.Call("enableLightSensorGuard", targetValue);
    }

    public void StopLightSensorGuard() {
        NullCheck();
        activity.Call("disableLightSensorGuard");
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
