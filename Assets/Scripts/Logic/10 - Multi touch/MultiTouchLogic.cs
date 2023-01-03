using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTouchLogic : MonoBehaviour
{
    private Collider2D[] collidersInRange;
    bool win = false;

    void Start() {
        collidersInRange = Physics2D.OverlapCircleAll(Vector2.zero, 5);
    }

    void Update() {
        if (Input.touchCount < 7) return;

        bool touched = false;
        foreach( Collider2D col in collidersInRange) {
            touched = false;

            for(int i = 0; i < Input.touchCount; i++) {
                if(TouchUtil.IsTouching(Input.GetTouch(i), col)) {
                    touched = true;
                    break;
                }
            }

            if (!touched) return;
        }

        if (!win && touched) {
            win = true;
            LevelManager.Instance.LevelComplete();
        }
    }
}
