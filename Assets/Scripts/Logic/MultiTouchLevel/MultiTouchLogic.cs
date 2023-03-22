using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTouchLogic : MonoBehaviour
{
    [SerializeField] private GameObject singleTouch;
    [SerializeField] private GameObject[] twoTouch;
    [SerializeField] private GameObject[] fiveTouch;

    private Collider2D[] collidersInRange;
    bool win = false;

    void Awake() {
        if (AndoridActivityManager.Instance.DeviceHasFiveTouchSupport()){
            foreach(var go in fiveTouch) {
                go.SetActive(true);
            }
        }
        else if (AndoridActivityManager.Instance.DeviceHasTwoTouchSupport()){
            foreach (var go in twoTouch) {
                go.SetActive(true);
            }
        }
        else {
            singleTouch.SetActive(true);
        }
    }

    void Start() {
        collidersInRange = Physics2D.OverlapCircleAll(Vector2.zero, 5);
    }

    void Update() {

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
