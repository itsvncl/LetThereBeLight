using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour
{
    private float timeTouchBegan = 0.0f;
    private float touchDuration = 0.0f;
    [SerializeField] private float longPressTreshold = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began) {
                timeTouchBegan = Time.time;
            }

            if (touch.phase == TouchPhase.Ended) {
                touchDuration = Time.time - timeTouchBegan;

                if (touchDuration < longPressTreshold) {
                    //Short touch
                    Debug.Log("Short");
                }
                else {
                    //Long touch
                    Debug.Log("Long");
                }
            }
        }
    }
}
