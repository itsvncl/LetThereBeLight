package com.vncl.unityactivity;

import android.content.Context;
import android.hardware.camera2.CameraManager;
import android.os.Bundle;
import android.util.Log;
import android.view.KeyEvent;
import android.widget.Toast;

import androidx.annotation.NonNull;

import com.unity3d.player.UnityPlayer;

public class CustomUnityActivity extends UnityPlayerActivity {
    private static final String LOGTAG = "CustomUnityActivity";

    private boolean isVolumeButtonLocked = false;
    private CameraManager cameraManager;

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

    public void lockVolumeButton(){
        this.isVolumeButtonLocked = true;
        Log.i(LOGTAG, "Volume button locked");
    }

    public void unlockVolumeButton(){
        isVolumeButtonLocked = false;
        Log.i(LOGTAG, "Volume button unlocked");
    }

    //TODO: If the flash is already on, then give them the win.
    public void enableFlashlightGuard(){
        cameraManager = (CameraManager) this.getSystemService(Context.CAMERA_SERVICE);

        cameraManager.registerTorchCallback(new CameraManager.TorchCallback() {
            @Override
            public void onTorchModeChanged(@NonNull String cameraId, boolean enabled) {
                if(enabled) UnityPlayer.UnitySendMessage("FlashlightGuard", "FlashOn", "");
                super.onTorchModeChanged(cameraId, enabled);
            }
        }, null);

        Log.i(LOGTAG, "Flashlight guard enabled");
    }
    public void disableFlashlightGuard(){
        cameraManager.registerTorchCallback(new CameraManager.TorchCallback() {
            @Override
            public void onTorchModeChanged(@NonNull String cameraId, boolean enabled) {
                super.onTorchModeChanged(cameraId, enabled);
            }
        }, null);

        cameraManager = null;
        Log.i(LOGTAG, "Flashlight guard disabled");
    }
}

