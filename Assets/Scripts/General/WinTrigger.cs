using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour {
    public bool win = false;
    [SerializeField] private GameObject[] winObjects;
    [SerializeField] private bool freezeAfterWin;

    void OnTriggerEnter2D(Collider2D col) {
        Debug.Log("Collision with gameobject");
        WinLogic(col.gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log("Collision with gameobject");
        WinLogic(collision.gameObject);
    }

    void WinLogic(GameObject gameObject) {
        if (win) return;

        foreach (GameObject go in winObjects) {
            if (go.Equals(gameObject)) {
                win = true;
                FreezeAfterWin();
                Debug.Log("Level completed by WinTrigger");

                try {
                    LevelManager.Instance.LevelComplete();
                } catch {
                    Debug.Log("Level manager is not inited.");
                }
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
