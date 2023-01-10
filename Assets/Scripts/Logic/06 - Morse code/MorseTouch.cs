using UnityEngine;

public class MorseTouch : MonoBehaviour, ITouchBeginEvent, ITouchEndEvent
{
    float touchTime = 0.0f;

    public void OnTouchBegin(TouchData touchData) {
        touchTime = Time.time;
    }

    public void OnTouchEnd(TouchData touchData) {
        if(Time.time - touchTime >= MorseGame.Instance.LongPressTime) {
            MorseGame.Instance.AddMorse(MorseGame.MorseType.DASH);
        }
        else {
            MorseGame.Instance.AddMorse(MorseGame.MorseType.DOT);
        }
    }
}
