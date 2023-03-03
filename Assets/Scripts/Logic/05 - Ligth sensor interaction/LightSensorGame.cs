using UnityEngine;
using UnityEngine.UI;

public class LightSensorGame : MonoBehaviour
{
    [SerializeField] private float luxTarget = 3000f;
    [SerializeField] private Image lightImage;

    void Start()
    {
        AndoridActivityManager.Instance.StartLightSensorGuard(luxTarget);
    }

    private void OnDestroy() {
        AndoridActivityManager.Instance.StopLightSensorGuard();
    }

    void LightTargetReached(string s = "") {
        LevelManager.Instance.LevelComplete();
    }
}
