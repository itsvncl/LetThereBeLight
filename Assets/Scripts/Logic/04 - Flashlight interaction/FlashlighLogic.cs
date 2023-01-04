using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlighLogic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start(){
        AndoridActivityManager.Instance.StartFlashlightGuard();
    }

    void OnDestroy() {
        AndoridActivityManager.Instance.StopFlashlightGuard();
    }

    public void FlashOn(string s = "") {
        LevelManager.Instance.LevelComplete();
    }
}
