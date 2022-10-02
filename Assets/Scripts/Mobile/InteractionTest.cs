using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionTest : MonoBehaviour
{
    AndroidJavaObject pluginActivity;

    void Start() {
        pluginActivity = new AndroidJavaObject("com.vncl.unityactivity.CustomUnityActivity");
    }
}
