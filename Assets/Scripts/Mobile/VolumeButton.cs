using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeButton : MonoBehaviour {

    [SerializeField] private bool m_bGetVolumeFromPhone = true;

    //for gameobject listener (easier to edit in scene mode)
    [SerializeField] private VolumeControl m_vVolumeListenerGO = null;

    private float m_fPrevVolume = -1;
    private bool m_bShutDown = false;

    //quick access to reference
    //public static VolumeButton s_instance = null;


    //Get phone volume if running or android or application volume if running on pc
    //(or wanted by user)
    public float GetVolume() {
        if (m_bGetVolumeFromPhone && AndroidAudioManager.IsRunningOnAndroid()) {
            AndroidJavaObject audioManager = AndroidAudioManager.GetAndroidAudioManager();
            Debug.Log(audioManager);
            return audioManager.Call<int>("getStreamVolume", 3);
        }
        else {
            return AudioListener.volume;
        }

    }

    //set phone or application volume (according if running on android or if user want application volume)
    public void SetVolume(float a_fVolume) {
        if (m_bGetVolumeFromPhone && AndroidAudioManager.IsRunningOnAndroid()) {
            AndroidJavaObject audioManager = AndroidAudioManager.GetAndroidAudioManager();
            audioManager.Call("setStreamVolume", 3, (int)a_fVolume, 0);
        }
        else {
            AudioListener.volume = a_fVolume;
        }
    }

    private void ResetVolume() {
        SetVolume(m_fPrevVolume);
    }

    void Start() {
        //s_instance = this;
        PowerOn();
    }

    void OnVolumeDown() {
        if (m_vVolumeListenerGO != null) {
            m_vVolumeListenerGO.OnVolumeDown();
        }
    }

    void OnVolumeUp() {
        if (m_vVolumeListenerGO != null) {
            m_vVolumeListenerGO.OnVolumeUp();
        }
    }

    //If user want to change volume, he has to mute this script first
    //else the script will interpret this has a user input and resetvolume
    public void ShutDown() {
        m_bShutDown = true;
    }

    //to unmute the script
    public void PowerOn() {
        m_bShutDown = false;
        //get the volume to avoid interpretating previous change (when script was muted) as user input
        m_fPrevVolume = GetVolume();
    }

    // Update is called once per frame
    void Update() {
        if (m_bShutDown)
            return;

        float fCurrentVolume = GetVolume();
        float fDiff = fCurrentVolume - m_fPrevVolume;

        //if volume change, compute the difference and call listener according to
        if (fDiff < 0) {
            ResetVolume();
            OnVolumeDown();
        }
        else if (fDiff > 0) {
            ResetVolume();
            OnVolumeUp();
        }
    }

}
