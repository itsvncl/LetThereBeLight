using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LightLogic : MonoBehaviour
{
    [SerializeField] private Image image;
    private float alpha = 0f;
    private float alphaTarget = 0f;

    void Start() {
        AndoridActivityManager.Instance.LockVolumeButton();
    }
    void OnDestroy() {
        AndoridActivityManager.Instance.UnlockVolumeButton();
    }

    void FixedUpdate() {
        if(alphaTarget > alpha) {
            alpha += 0.01f;

            var tempColor = image.color;
            tempColor.a = alpha;

            image.color = tempColor;
        }else if(alphaTarget + 0.02f < alpha) {
            alpha -= 0.01f;

            var tempColor = image.color;
            tempColor.a = alpha;

            image.color = tempColor;
        }
    }

    public void IncrementAlphaUp(string s = "") {
        if (alphaTarget >= 1f) {
            onWin();
            return;
        }

        alphaTarget += 0.1f;
    }

    public void IncrementAlphaDown(string s = "") {
        if (alphaTarget <= 0f) return;

        alphaTarget -= 0.1f;
    }

    private void onWin() {
        Debug.Log("Level 03 is completed");

        AndoridActivityManager.Instance.UnlockVolumeButton();
        LevelManager.Instance.LevelComplete();
    }
}
