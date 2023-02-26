using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    bool win = false;
    [SerializeField] GameObject[] winObjects;

    void OnTriggerEnter2D(Collider2D col) {
        WinLogic(col.gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        WinLogic(collision.gameObject);
    }

    void WinLogic(GameObject gameObject) {
        if (win) return;

        foreach (GameObject go in winObjects) {
            if (go.Equals(gameObject)) {
                win = true;
                Debug.Log("Level completed by WinTrigger");
                LevelManager.Instance.LevelComplete();
            }
        }
    }

    public void SetWinObjects(GameObject[] winObjects) {
        this.winObjects = winObjects;
    }
}
