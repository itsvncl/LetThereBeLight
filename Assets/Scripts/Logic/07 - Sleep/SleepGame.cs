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
        deviceHasProxy = ProximitySensor.current != null;

        if (deviceHasProxy) {
            InputSystem.EnableDevice(ProximitySensor.current);
        }
    }

    void FixedUpdate() {
        if (win) return;

        if (Input.deviceOrientation == DeviceOrientation.FaceDown) {
            if(!deviceHasProxy || (deviceHasProxy && ProximitySensor.current.distance.ReadValue() < distanceTarget)) {
                timeDuration = Time.time - beginTime;
            }
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

            if (deviceHasProxy) {
                InputSystem.DisableDevice(ProximitySensor.current);
            }

            LevelManager.Instance.LevelComplete();
        }
    }
}
