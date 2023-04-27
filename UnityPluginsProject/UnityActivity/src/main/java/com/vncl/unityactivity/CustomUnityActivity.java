package com.vncl.unityactivity;

import android.content.Context;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.hardware.camera2.CameraManager;
import android.os.Bundle;
import android.util.Log;
import android.view.KeyEvent;

import androidx.annotation.NonNull;

import com.unity3d.player.UnityPlayer;

public class CustomUnityActivity extends UnityPlayerActivity {
    private static final String LOGTAG = "LetThereBeLightActivity";

    private boolean isVolumeButtonLocked = false;

    private CameraManager cameraManager;
    private CameraManager.TorchCallback flashCallback;

    private ScreenshotDetector screenshotDetector;

    @Override
    public boolean onKeyDown(int keyCode, KeyEvent event) {
        Log.i(LOGTAG, isVolumeButtonLocked ? "Volume button is locked" : "Volume button is unlocked");
        if(isVolumeButtonLocked){
            switch (event.getKeyCode()) {
                case KeyEvent.KEYCODE_VOLUME_UP:
                    Log.i(LOGTAG, "Volume up pressed (locked)");
                    UnityPlayer.UnitySendMessage("GameController", "IncrementAlphaUp", "");

                    return true;
                case KeyEvent.KEYCODE_VOLUME_DOWN:
                    Log.i(LOGTAG, "Volume down pressed (locked)");
                    UnityPlayer.UnitySendMessage("GameController", "IncrementAlphaDown", "");

                    return true;
                default:
                    return super.onKeyDown(keyCode, event);
            }
        }

        return super.onKeyDown(keyCode, event);
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        cameraManager = (CameraManager) getSystemService(Context.CAMERA_SERVICE);
    }
    @Override
    public void onPause() {
        super.onPause();

        if(flashCallback != null)
            cameraManager.unregisterTorchCallback(flashCallback);
        if(screenshotDetector != null)
            screenshotDetector.stop();
    }
    @Override
    public void onResume() {
        super.onResume();

        if(flashCallback != null)
            cameraManager.registerTorchCallback(flashCallback, null);
        if(screenshotDetector != null)
            screenshotDetector.start();
    }

    public void lockVolumeButton(){
        this.isVolumeButtonLocked = true;
        Log.i(LOGTAG, "Volume button locked");
    }
    public void unlockVolumeButton(){
        isVolumeButtonLocked = false;
        Log.i(LOGTAG, "Volume button unlocked");
    }

    public void enableFlashlightGuard(){

        flashCallback = new CameraManager.TorchCallback() {
            @Override
            public void onTorchModeChanged(@NonNull String cameraId, boolean enabled) {
                Log.i(LOGTAG, "Flash changed on Camera: " + cameraId + " to: " + enabled);
                if(enabled){
                    Log.i(LOGTAG, "Flash enabled");
                    UnityPlayer.UnitySendMessage("GameController", "FlashOn", "");
                    super.onTorchModeChanged(cameraId, true);
                    disableFlashlightGuard();
                    return;
                }
                super.onTorchModeChanged(cameraId, false);
            }

            @Override
            public void onTorchModeUnavailable(String cameraId){
                Log.i(LOGTAG, "Unavailable on camera: " + cameraId);
            }
        };

        cameraManager.registerTorchCallback(flashCallback, null);

        Log.i(LOGTAG, "Flashlight guard enabled");
    }
    public void disableFlashlightGuard(){
        cameraManager.unregisterTorchCallback(flashCallback);
        flashCallback = null;
        Log.i(LOGTAG, "Flashlight guard disabled");
    }

    public void enableScreenshotDetector(){
        screenshotDetector = new ScreenshotDetector(this);
        screenshotDetector.start();
    }
    public void disableScreenshotDetector(){
        screenshotDetector.stop();
        screenshotDetector = null;
    }

    @Override
    protected void onNewIntent(Intent intent) {
        super.onNewIntent(intent);

        UnityPlayer.UnitySendMessage("GameController", "Win", "");
    }

    public boolean deviceHasFlash(){
        boolean hasFlash = getApplicationContext().getPackageManager().hasSystemFeature(PackageManager.FEATURE_CAMERA_FLASH);
        Log.i(LOGTAG, "Device flash available: " + hasFlash);
        return hasFlash;
    }

    public boolean deviceHasMagnetometer(){
        boolean hasMagnetometer = getApplicationContext().getPackageManager().hasSystemFeature(PackageManager.FEATURE_SENSOR_COMPASS);
        Log.i(LOGTAG, "Device magnetometer available: " + hasMagnetometer);

        return hasMagnetometer;
    }

    public boolean deviceHasTwoTouchSupport(){
        boolean hasTouchSupport = getApplicationContext().getPackageManager().hasSystemFeature(PackageManager.FEATURE_TOUCHSCREEN_MULTITOUCH_DISTINCT);
        Log.i(LOGTAG, "Device two touch support available: " + hasTouchSupport);

        return hasTouchSupport;
    }

    public boolean deviceHasFiveTouchSupport(){
        boolean hasTouchSupport = getApplicationContext().getPackageManager().hasSystemFeature(PackageManager.FEATURE_TOUCHSCREEN_MULTITOUCH_JAZZHAND);
        Log.i(LOGTAG, "Device five touch support available: " + hasTouchSupport);

        return hasTouchSupport;
    }
}

