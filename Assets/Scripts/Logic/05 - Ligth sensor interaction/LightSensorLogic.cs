using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSensorLogic : MonoBehaviour
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
