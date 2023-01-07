using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour {
    public static ScreenManager SM;

    private void Awake() {
        SM = this;
    }

    public float ScreenHeight {
       get{ return Camera.main.orthographicSize * 2.0f;}

    }
    public float ScreenWidth {
        get { return ScreenHeight * Screen.width / Screen.height; }
    }

}
