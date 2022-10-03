using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionCounter : MonoBehaviour {
    private int connectedWires = 0;

    public void ConnectWire() {
        connectedWires++;

        if (connectedWires >= 4) {
            LevelManager.Instance.LevelComplete();
        }
    }

    public int ConnectedWires {
        get { return connectedWires; }
    }
}
