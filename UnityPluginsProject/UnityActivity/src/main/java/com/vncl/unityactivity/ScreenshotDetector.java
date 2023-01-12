package com.vncl.unityactivity;

import android.content.Context;
import android.database.ContentObserver;
import android.database.Cursor;
import android.net.Uri;
import android.os.Handler;
import android.os.Looper;
import android.provider.MediaStore;
import android.util.Log;

import androidx.annotation.Nullable;

import com.unity3d.player.UnityPlayer;

public class ScreenshotDetector {
    private static final String LOGTAG = "LetThereBeLightScreenshotDetector";

    private Context ctx;
    private ContentObserver contentObserver = null;

    public ScreenshotDetector(Context ctx){
        this.ctx = ctx;
    }

    public void start(){
        if (contentObserver == null) {
            initObserver();
            ctx.getContentResolver().registerContentObserver(
                    MediaStore.Images.Media.EXTERNAL_CONTENT_URI,
                    true,
                    contentObserver
            );
            Log.i(LOGTAG, "Screenshot detector started");
        }
    }
    public void stop(){
        if(contentObserver != null){
            ctx.getContentResolver().unregisterContentObserver(contentObserver);
            contentObserver = null;
            Log.i(LOGTAG, "Screenshot detector stopped");
        }
    }

    private void queryScreenshots(Uri uri){
        Cursor cursor = null;
        try {
            String[] projection = new String[] {
                    MediaStore.Images.Media.DISPLAY_NAME,
                    MediaStore.Images.Media.DATA
            };

            cursor = ctx.getContentResolver().query(uri, projection, null, null, null);
            if (cursor != null && cursor.moveToFirst()) {
                int displayNameColIndex = cursor.getColumnIndex(MediaStore.Images.Media.DISPLAY_NAME);
                final String fileName = cursor.getString(displayNameColIndex);

                if(fileName.toLowerCase().contains("screenshot")){
                    UnityPlayer.UnitySendMessage("GameController", "ScreenshotTaken", "");
                    Log.i(LOGTAG, "Screenshot taken");
                }
            }
        } catch (Exception e){
            Log.e(LOGTAG, "Query error");
        } finally {
            if (cursor != null)  {
                cursor.close();
            }
        }
    }

    private void initObserver(){
        contentObserver = new ContentObserver(new Handler(Looper.getMainLooper())) {
            @Override
            public void onChange(boolean selfChange, @Nullable Uri uri) {
                if(uri != null){
                    queryScreenshots(uri);
                }

                super.onChange(selfChange, uri);
            }
        };
    }
}
