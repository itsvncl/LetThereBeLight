using UnityEngine;
using UnityEngine.InputSystem;

public class SleepGame : MonoBehaviour
{
    [SerializeField] private float distanceTarget = 1.0f;
    [SerializeField] private float timeTarget = 5.0f;

    private float timeDuration = 0.0f;
    private float beginTime = 0.0f;
    private bool win = false;

    void Start() {
        InputSystem.EnableDevice(ProximitySensor.current);
    }

    void Update() {
        if (win) return;

        if (ProximitySensor.current.distance.ReadValue() < distanceTarget && Input.deviceOrientation == DeviceOrientation.FaceDown) {
            timeDuration = Time.time - beginTime;
        }
        else {
            beginTime = Time.time;
            timeDuration = 0.0f;
        }

        if(timeDuration >= timeTarget) {
            win = true;
            InputSystem.DisableDevice(ProximitySensor.current);
            LevelManager.Instance.LevelComplete();
        }
    }
}
