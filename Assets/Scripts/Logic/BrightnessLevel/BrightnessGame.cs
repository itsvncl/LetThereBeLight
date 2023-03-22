using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrightnessGame : MonoBehaviour
{
    [SerializeField] private Image lightImage;
    [SerializeField] private GameObject progress;

    private bool win = false;
    private float xiaomiMax = 16.0f;
    private float androidMax = 1.0f;

    private bool isXiaomi;

    private float progressScaleMin = 0f;
    private float progressScaleMax = 4.2687f;
    private float progressPosMax = -0.253f;
    private float progressPosMin = -2.378f;

    private float progressScaleRange;
    private float progressPosRange;

    private bool lockGame = false;
    private float startTime;
    private float waitTime = 2.0f;

    void Start() {
        isXiaomi = Screen.brightness > androidMax;
        progressScaleRange = Mathf.Abs(progressScaleMax - progressScaleMin);
        progressPosRange = Mathf.Abs(progressPosMax - progressPosMin);

        float level = GetLevel();
        if (level >= 1.0f) {
            lockGame = true;
            startTime = Time.time;
        }
    }

    void Update()
    {
        if (win) return;
        if (lockGame) {
            if (waitTime < Time.time - startTime) {
                lockGame = false;
            }

            return;
        }

        AdjustProgress();

        if (isXiaomi) {
            if (Screen.brightness >= xiaomiMax) {
                win = true;
                LevelManager.Instance.LevelComplete();
            }

        }
        else {
            if (Screen.brightness >= 1.0) {
                win = true;
                LevelManager.Instance.LevelComplete();
            }
        }
    }

    private float GetLevel() {
        return isXiaomi ? Mathf.Log(Screen.brightness+1.0f, xiaomiMax) : Screen.brightness;
    }

    private void AdjustProgress() {
        float level = GetLevel();

        if(level < 0) level = 0;

        var tempColor = lightImage.color;
        tempColor.a = level;

        if(!lockGame) lightImage.color = tempColor;

        Vector3 newPos = progress.transform.localPosition;
        newPos.y = progressPosMin + progressPosRange * level;
        progress.transform.localPosition = newPos;

        Vector3 newScale = progress.transform.localScale;
        newScale.y = progressScaleMin + progressScaleRange * level;
        progress.transform.localScale = newScale;
    }
}
