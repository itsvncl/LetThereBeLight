using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorseLogic : MonoBehaviour {
   
    private float timeTouchBegan = 0.0f;
    private float touchDuration = 0.0f;
    [SerializeField] private float longPressTreshold = 1.0f;

    [SerializeField] private GameObject dot;
    [SerializeField] private GameObject dash;
    [SerializeField] private float gap = 0.2f;

    private List<GameObject> morseCode = new List<GameObject>();
    private int dotCount = 0;
    private int dashCount = 0;

    [SerializeField] private Collider2D retryCollider;

    void Start() {

    }

    void Update() {
        handleTouch();
    }

    void handleTouch() {

        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began) {
                timeTouchBegan = Time.time;
            }

            if (touch.phase == TouchPhase.Ended) {
                touchDuration = Time.time - timeTouchBegan;
                
                Vector3 wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                Vector2 touchPosition = new Vector2(wp.x, wp.y);

                if (retryCollider == Physics2D.OverlapPoint(touchPosition)) {
                    Reset();
                    return;
                }

                if (touchDuration < longPressTreshold) {
                    morseCode.Add(Instantiate(dot, transform.position, Quaternion.identity));
                    dotCount++;
                }
                else {
                    morseCode.Add(Instantiate(dash, transform.position, Quaternion.identity));
                    dashCount++;
                }

                centerCode();
                if (isWin()) {
                    LevelManager.Instance.LevelComplete();
                }
            }
        }
    }

    private void Reset() {
        dotCount = 0;
        dashCount = 0;

        foreach(GameObject go in morseCode) {
            Destroy(go, 0);
        }

        morseCode.Clear();
    }

    bool isWin() {
        if(morseCode.Count > 10) Reset();
        if(morseCode.Count != 9) return false;
        else {
            int index = 0;

            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    if(morseCode[index].name == dot.name + "(Clone)" && i == 1) {
                        return false;
                    }
                    if (morseCode[index].name == dash.name + "(Clone)" && i != 1) {
                        return false;
                    }
                    index++;
                }
            }
        }
        
        return true;
    }

    void centerCode() {
        float dotInc = dot.transform.localScale.x / 2.0f;
        float dashInc = dash.transform.localScale.x / 2.0f;
        
        float start = (dotCount * -dot.transform.localScale.x + dashCount * -dash.transform.localScale.x + (dotCount + dashCount - 1) * -gap) / 2.0f;
        float inc = 0;

        foreach (GameObject go in morseCode) {
            if (go.name == dot.name + "(Clone)") {
                inc += dotInc;
                go.transform.position = new Vector3(start + inc, go.transform.position.y, go.transform.position.z);
                inc += dotInc + gap;

            }
            else {
                inc += dashInc;
                go.transform.position = new Vector3(start + inc, go.transform.position.y, go.transform.position.z);
                inc += dashInc + gap;

            }
        }
    }
}
