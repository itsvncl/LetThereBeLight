using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLogic : MonoBehaviour {
    private int connectedWires = 0;

    public void wireConnected() {
        connectedWires++;

        if (connectedWires >= 4) {
            LevelManager.Instance.LevelComplete();
        }
    }
}
