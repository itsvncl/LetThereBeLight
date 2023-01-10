using UnityEngine;

public class LightSensorGame : MonoBehaviour
{
    [SerializeField] private float luxTarget = 3000f;

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
