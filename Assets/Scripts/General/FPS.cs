using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS : MonoBehaviour {
    void Start() {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 60;
    }

}
