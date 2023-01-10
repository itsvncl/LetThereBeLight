using UnityEngine;

public class FlashlightGame : MonoBehaviour
{
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
