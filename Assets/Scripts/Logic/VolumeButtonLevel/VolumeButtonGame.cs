using UnityEngine;
using UnityEngine.UI;

public class VolumeButtonGame : MonoBehaviour
{
    [SerializeField] public Image image;
    [SerializeField] public GameObject progress;
    private float alpha = 0f;
    public float alphaTarget = 0f;
    public float incrementValue = 0.1f;
    public float winTarget = 1.0f;
    public bool win = false;

    private float progressScaleMin = 0f;
    private float progressScaleMax = 2.7f;
    private float progressPosMin = -1.962f;
    private float progressPosMax = -0.6134f;

    private float progressScaleStep;
    private float progressPosStep;

    void Start() {
        try {
            AndoridActivityManager.Instance.LockVolumeButton();
        } catch {
            Debug.Log("AndroidActivityManager not inited");
        }
        progressScaleStep = (progressScaleMax - progressScaleMin) / 11;
        progressPosStep = Mathf.Abs((progressPosMax - progressPosMin) / 11);
    }
    void OnDestroy() {
        try {
            AndoridActivityManager.Instance.UnlockVolumeButton();
        } catch {
            Debug.Log("AndroidActivityManager not inited");
        }
    }

    void FixedUpdate() {
        if(alphaTarget > alpha) {
            alpha += 0.01f;

            AdjustImageAlpha();
        }
        else if(alphaTarget + 0.02f < alpha) {
            alpha -= 0.01f;

            AdjustImageAlpha();
        }
    }

    private void AdjustImageAlpha() {
        try {
            var tempColor = image.color;
            tempColor.a = alpha;

            image.color = tempColor;
        } catch {
            Debug.Log("Image not inited");
        }
    }


    public void IncrementAlphaUp(string s = "") {
        if (alphaTarget >= winTarget) {
            OnWin();
            return;
        }

        alphaTarget += incrementValue;

        progress.transform.position = new Vector3(progress.transform.position.x, progress.transform.position.y + progressPosStep, progress.transform.position.z);
        progress.transform.localScale = new Vector3(progress.transform.localScale.x, progress.transform.localScale.y + progressScaleStep, progress.transform.localScale.z);
    }


    public void IncrementAlphaDown(string s = "") {
        if (alphaTarget <= 0f) return;

        alphaTarget -= incrementValue;

        progress.transform.position = new Vector3(progress.transform.position.x, progress.transform.position.y - progressPosStep, progress.transform.position.z);
        progress.transform.localScale = new Vector3(progress.transform.localScale.x, progress.transform.localScale.y - progressScaleStep, progress.transform.localScale.z);
    }

    private void OnWin() {
        win = true;
        try {
            LevelManager.Instance.LevelComplete();
            AndoridActivityManager.Instance.UnlockVolumeButton();
        } catch {
            Debug.Log("LevelManager and AndoridActivityManager are not inited");
        }
    }
}
