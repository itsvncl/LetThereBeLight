using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabyrinthGame : MonoBehaviour
{
    [SerializeField] private GameObject maze;
    [SerializeField] private GameObject unplayableOverlay;

    void Awake() {
        if (!SystemInfo.supportsGyroscope) {
            unplayableOverlay.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    void Start()
    {
        float width = ScreenManager.SM.ScreenWidth;
        float scale = Mathf.Clamp(width / 5.63f, 0.1f, 1.0f);

        maze.transform.localScale = new Vector3(scale, scale, 1.0f);
    }
}
