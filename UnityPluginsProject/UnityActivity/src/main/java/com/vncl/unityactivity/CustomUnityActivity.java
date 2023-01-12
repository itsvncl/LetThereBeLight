package com.vncl.unityactivity;

import android.content.Context;
import android.hardware.Sensor;
import android.hardware.SensorEvent;
import android.hardware.SensorEventListener;
import android.hardware.SensorManager;
import android.hardware.camera2.CameraManager;
import android.os.Bundle;
import android.util.Log;
import android.view.KeyEvent;

import androidx.annotation.NonNull;

import com.unity3d.player.UnityPlayer;

//TODO: onPause remove event listener
//TODO: onResume re add event listener
public class CustomUnityActivity extends UnityPlayerActivity {
    private static final String LOGTAG = "LetThereBeLightActivity";

    private boolean isVolumeButtonLocked = false;

    private CameraManager cameraManager;
    private CameraManager.TorchCallback flashCallback;

    private ScreenshotDetector screenshotDetector;

    private SensorManager sensorManager;
    private Sensor lightSensor;
    private SensorEventListener lightSensorEventListener;


    @Override
    public boolean onKeyDown(int keyCode, KeyEvent event) {
        Log.i(LOGTAG, isVolumeButtonLocked ? "Volume button is locked" : "Volume button is unlocked");
        if(isVolumeButtonLocked){
            switch (event.getKeyCode()) {
                case KeyEvent.KEYCODE_VOLUME_UP:
                    Log.i(LOGTAG, "Volume up pressed (locked)");
                    UnityPlayer.UnitySendMessage("LightImage", "IncrementAlphaUp", "");

                    return true;
                case KeyEvent.KEYCODE_VOLUME_DOWN:
                    Log.i(LOGTAG, "Volume down pressed (locked)");
                    UnityPlayer.UnitySendMessage("LightImage", "IncrementAlphaDown", "");

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
        sensorManager = (SensorManager) getSystemService(Context.SENSOR_SERVICE);
    }
    @Override
    public void onPause() {
        super.onPause();

        if(lightSensorEventListener != null)
            sensorManager.unregisterListener(lightSensorEventListener);
        if(flashCallback != null)
            cameraManager.unregisterTorchCallback(flashCallback);
        if(screenshotDetector != null)
            screenshotDetector.stop();
    }
    @Override
    public void onResume() {
        super.onResume();

        if(lightSensorEventListener != null)
            sensorManager.registerListener(lightSensorEventListener, lightSensor, 1000);
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
                if(enabled){
                    Log.i(LOGTAG, "Flash enabled");
                    UnityPlayer.UnitySendMessage("GameController", "FlashOn", "");
                    super.onTorchModeChanged(cameraId, true);
                    disableFlashlightGuard();
                    return;
                }
                super.onTorchModeChanged(cameraId, false);
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

    public void enableLightSensorGuard(float targetValue){
        lightSensor = sensorManager.getDefaultSensor(Sensor.TYPE_LIGHT);

        lightSensorEventListener = new SensorEventListener() {
            @Override
            public void onSensorChanged(SensorEvent sensorEvent) {

                if(sensorEvent.sensor.getType() == Sensor.TYPE_LIGHT){
                    if(sensorEvent.values[0] >= targetValue){
                        UnityPlayer.UnitySendMessage("GameController", "LightTargetReached", "");
                        disableLightSensorGuard();
                    }
                }
            }

            @Override
            public void onAccuracyChanged(Sensor sensor, int i) {}
        };

        sensorManager.registerListener(lightSensorEventListener, lightSensor, 1000);
        Log.i(LOGTAG, "Light sensor listener enabled with target value: " + targetValue);
    }
    public void disableLightSensorGuard(){
        sensorManager.unregisterListener(lightSensorEventListener);
        lightSensorEventListener = null;
        Log.i(LOGTAG, "Light sensor listener disabled");
    }

    public void enableScreenshotDetector(){
        screenshotDetector = new ScreenshotDetector(this);
        screenshotDetector.start();
    }
    public void disableScreenshotDetector(){
        screenshotDetector.stop();
        screenshotDetector = null;
    }
}

