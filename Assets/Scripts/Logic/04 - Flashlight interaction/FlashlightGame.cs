using UnityEngine;

public class FlashlightGame : MonoBehaviour
{
    void Start(){
        AndoridActivityManager.Instance.StartFlashlightGuard();

        if (!AndoridActivityManager.Instance.DeviceHasFlash()) {
            //TODO: Level cant be played popup.
        }
    }

    void OnDestroy() {
        AndoridActivityManager.Instance.StopFlashlightGuard();
    }

    public void FlashOn(string s = "") {
        LevelManager.Instance.LevelComplete();
    }
}
