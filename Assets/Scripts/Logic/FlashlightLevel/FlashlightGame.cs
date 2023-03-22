using UnityEngine;

public class FlashlightGame : MonoBehaviour
{
    [SerializeField] private GameObject unplayableOverlay;

    void Start(){
        AndoridActivityManager.Instance.StartFlashlightGuard();

        if (!AndoridActivityManager.Instance.DeviceHasFlash()) {
            unplayableOverlay.SetActive(true);
        }
    }

    void OnDestroy() {
        AndoridActivityManager.Instance.StopFlashlightGuard();
    }

    public void FlashOn(string s = "") {
        LevelManager.Instance.LevelComplete();
    }
}
