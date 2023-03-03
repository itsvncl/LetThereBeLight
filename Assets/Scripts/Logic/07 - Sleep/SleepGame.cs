using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SleepGame : MonoBehaviour
{
    [SerializeField] private float distanceTarget = 1.0f;
    [SerializeField] private float timeTarget = 5.0f;
    [SerializeField] private Image lightImage;

    private float timeDuration = 0.0f;
    private float beginTime = 0.0f;
    private bool win = false;

    private bool deviceHasProxy;

    void Start() {
        InputSystem.EnableDevice(ProximitySensor.current);
        deviceHasProxy = AndoridActivityManager.Instance.DeviceHasProximitySensor();
    }

    void FixedUpdate() {
        if (win) return;

        if (( !deviceHasProxy || ProximitySensor.current.distance.ReadValue() < distanceTarget) && Input.deviceOrientation == DeviceOrientation.FaceDown) {
            timeDuration = Time.time - beginTime;
        }
        else {
            beginTime = Time.time;
            timeDuration -= 0.05f;
        }

        if(timeDuration < 0) timeDuration = 0;

        var tempColor = lightImage.color;
        tempColor.a = timeDuration / timeTarget;
        lightImage.color = tempColor;

        if(timeDuration >= timeTarget) {
            win = true;
            InputSystem.DisableDevice(ProximitySensor.current);
            LevelManager.Instance.LevelComplete();
        }
    }
}
