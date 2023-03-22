using UnityEngine;

public class MorseRetryButton : MonoBehaviour, IClickable {
    public void OnClick(TouchData touchData) {
        MorseGame.Instance.ResetGame();
    }
}
