using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassTap : MonoBehaviour, IClickable {
    [SerializeField] private LightSwitchGlassGame game;

    public void OnClick(TouchData touchData) {
        if (touchData.FingerID != 0) return;
        game.Tap();
    }
}
