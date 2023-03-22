using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightbulbGame : MonoBehaviour
{
    private bool win = false;
    public void onClick() {
        if (win) return;

        win = true;
        LevelManager.Instance.LevelComplete();
    }
}
