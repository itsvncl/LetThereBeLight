using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevTools : MonoBehaviour
{
    public static DevTools Instance;

    public float prevTime = 0;


    private void Awake() {
        if(Instance != null && Instance != this) {
            Destroy(this);
        }else{
            Instance = this;
            DontDestroyOnLoad(this.gameObject);

        }
    }

    void Update()
    {
        if(Input.touchCount == 1 && Time.time - prevTime > 0.5f) {
            if(Input.GetTouch(0).tapCount >= 5 && Input.GetTouch(0).phase == TouchPhase.Ended && Time.time - prevTime > 0.5f) {
                prevTime = Time.time;
                LevelManager.Instance.DebugNextLevel();
            }
        }
        if (Input.touchCount == 10 && Time.time - prevTime > 0.5f) {
            prevTime = Time.time;
            LevelManager.Instance.DebugPreviousLevel();
        }
    }
}
