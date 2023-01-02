using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityLogic : MonoBehaviour
{
    [SerializeField] private float distanceTarget = 1f;

    void Start() {
        AndoridActivityManager.Instance.StartProximitySensorGuard(distanceTarget);
    }

    void ProximityTargetReached(string s = "") {
        LevelManager.Instance.LevelComplete();
    }
}
