using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightbulbGame : MonoBehaviour
{
    public bool win = false;
    public void onClick() {
        if (win) return;

        win = true;

        try {
            LevelManager.Instance.LevelComplete();
        } catch {
            Debug.Log("LevelManager is not inited");
        }
    }
}
