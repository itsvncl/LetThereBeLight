using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    bool win = false;
    [SerializeField] GameObject[] winObjects;

    void OnTriggerEnter2D(Collider2D col) {
        Debug.Log("Hali");
        if (win) return;

        foreach (GameObject go in winObjects) {
            if (go.Equals(col.gameObject)) {
                win = true;
                Debug.Log("Level completed by WinTrigger");
                LevelManager.Instance.LevelComplete();
            }
        }
    }
}
