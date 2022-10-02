using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VolumeControl : MonoBehaviour {

    public void OnVolumeUp() {
        Debug.Log("ds");
    }
    public void OnVolumeDown() {
        LevelManager.Instance.LevelComplete();
    }
}



