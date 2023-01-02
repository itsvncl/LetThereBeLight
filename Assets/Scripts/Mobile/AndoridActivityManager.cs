using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndoridActivityManager : MonoBehaviour {
    public static AndoridActivityManager Instance;
    
    private AndroidJavaClass playerClass;
    private AndroidJavaObject activity;

    void Awake() {
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

    public void StartProximitySensorGuard(float targetValue) {
        NullCheck();
        activity.Call("enableProximitySensorGuard", targetValue);
    }

    public void StopProximitySensorGuard() {
        NullCheck();
        activity.Call("disableProximitySensorGuard");
    }

    private void NullCheck() {
        if (activity == null) {
            throw new System.Exception("Activity is null!");
        }
    }
}
