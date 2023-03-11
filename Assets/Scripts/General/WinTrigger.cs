using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour {
    bool win = false;
    [SerializeField] GameObject[] winObjects;
    [SerializeField] bool freezeAfterWin;

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
                FreezeAfterWin();
                Debug.Log("Level completed by WinTrigger");
                LevelManager.Instance.LevelComplete();
            }
        }
    }

    public void SetWinObjects(GameObject[] winObjects) {
        this.winObjects = winObjects;
    }

    public void FreezeAfterWin() {
        if (!freezeAfterWin) return;

        foreach(var go in winObjects) {
            try {
                Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            } catch { }
        }

        try {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        } catch { }
    }
}
