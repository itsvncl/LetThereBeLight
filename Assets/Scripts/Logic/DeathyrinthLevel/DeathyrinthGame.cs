using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathyrinthGame : MonoBehaviour
{
    [SerializeField] private MazeGenerator generator;
    [SerializeField] private GameObject unplayableOverlay;

    bool reset = false;


    void Awake() {
        if (!AndroidActivityManager.Instance.DeviceHasGyroscope() || !SystemInfo.supportsGyroscope) {
            unplayableOverlay.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    void Start()
    {
        generator.Generate();
    }

    public void ResetGame() {
        if (reset) return;

        reset = true;
        generator.ResetGame();
        generator.Generate();


        reset = false;
    }
}
