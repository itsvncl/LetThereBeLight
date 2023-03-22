using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class LightSensorGame : MonoBehaviour
{
    [SerializeField] private float luxTarget = 3000f;
    [SerializeField] private Image lightImage;

    private bool win = false;
    private float initial;
    private bool lockGame = false;

    private float startTime;
    private float waitTime = 2.0f;

    [SerializeField] private GameObject unplayableOverlay;

    void Awake()
    {
        if (LightSensor.current == null) {
            unplayableOverlay.SetActive(true);
            gameObject.SetActive(false);
            return;
        }

        InputSystem.EnableDevice(LightSensor.current);
        initial = LightSensor.current.lightLevel.ReadValue();
    }

    void Start() {
        if(initial >= luxTarget) {
            lockGame = true;
            startTime = Time.time;
        }
    }

    void Update() {
        if (win) return;
        if (lockGame) {
            if (waitTime < Time.time - startTime) {
                lockGame = false;
            }

            return;
        }

        var tempColor = lightImage.color;
        float lux = LightSensor.current.lightLevel.ReadValue();

        tempColor.a = lux / luxTarget;
        lightImage.color = tempColor;

        Debug.Log(lux);

        if(lux >= luxTarget) {
            win = true;
            LevelManager.Instance.LevelComplete();
        }
    }

    void OnDestroy() {
        InputSystem.DisableDevice(LightSensor.current);
    }
}
