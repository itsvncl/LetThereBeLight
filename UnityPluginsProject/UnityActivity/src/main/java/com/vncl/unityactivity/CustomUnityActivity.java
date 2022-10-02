package com.vncl.unityactivity;

import android.util.Log;
import android.view.KeyEvent;
import android.widget.Toast;

public class CustomUnityActivity extends UnityPlayerActivity {
    private static final String LOGTAG = "VolumeManagerActivity";

    @Override
    public boolean onKeyDown(int keyCode, KeyEvent event) {
        switch (event.getKeyCode()) {
            case KeyEvent.KEYCODE_VOLUME_UP:
                Log.i(LOGTAG, "Volume up pressed");
                Toast.makeText(this, "Volume up pressed", Toast.LENGTH_SHORT).show();
                return true;
            case KeyEvent.KEYCODE_VOLUME_DOWN:
                Log.i(LOGTAG, "Volume down pressed");
                Toast.makeText(this, "Volume down pressed", Toast.LENGTH_SHORT).show();
                return true;
            default:
                return super.onKeyDown(keyCode, event);
        }
    }
}

