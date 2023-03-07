using UnityEngine;
using UnityEngine.UI;

public class VolumeButtonGame : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private GameObject progress;
    private float alpha = 0f;
    private float alphaTarget = 0f;

    private float progressScaleMin = 0f;
    private float progressScaleMax = 2.7f;
    private float progressPosMin = -1.962f;
    private float progressPosMax = -0.6134f;

    private float progressScaleStep;
    private float progressPosStep;

    void Start() {
        AndoridActivityManager.Instance.LockVolumeButton();
        progressScaleStep = (progressScaleMax - progressScaleMin) / 11;
        progressPosStep = Mathf.Abs((progressPosMax - progressPosMin) / 11);
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
            OnWin();
            return;
        }

        alphaTarget += 0.1f;

        progress.transform.position = new Vector3(progress.transform.position.x, progress.transform.position.y + progressPosStep, progress.transform.position.z);
        progress.transform.localScale = new Vector3(progress.transform.localScale.x, progress.transform.localScale.y + progressScaleStep, progress.transform.localScale.z);
    }

    public void IncrementAlphaDown(string s = "") {
        if (alphaTarget <= 0f) return;

        alphaTarget -= 0.1f;

        progress.transform.position = new Vector3(progress.transform.position.x, progress.transform.position.y - progressPosStep, progress.transform.position.z);
        progress.transform.localScale = new Vector3(progress.transform.localScale.x, progress.transform.localScale.y - progressScaleStep, progress.transform.localScale.z);
    }

    private void OnWin() {
        AndoridActivityManager.Instance.UnlockVolumeButton();
        LevelManager.Instance.LevelComplete();
    }
}
