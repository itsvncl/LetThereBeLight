using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassTap : MonoBehaviour, IClickable {
    [SerializeField] public LightSwitchGlassGame game;

    public void OnClick(TouchData touchData) {
        if (touchData.FingerID != 0) return;
        game.Tap();
    }
}
