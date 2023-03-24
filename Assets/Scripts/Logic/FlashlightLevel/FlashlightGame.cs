using UnityEngine;

public class FlashlightGame : MonoBehaviour
{
    [SerializeField] private GameObject unplayableOverlay;

    void Start(){
        AndroidActivityManager.Instance.StartFlashlightGuard();

        if (!AndroidActivityManager.Instance.DeviceHasFlash()) {
            unplayableOverlay.SetActive(true);
        }
    }

    void OnDestroy() {
        AndroidActivityManager.Instance.StopFlashlightGuard();
    }

    public void FlashOn(string s = "") {
        LevelManager.Instance.LevelComplete();
    }
}
