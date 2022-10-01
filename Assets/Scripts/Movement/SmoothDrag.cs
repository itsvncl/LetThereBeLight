using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothDrag : MonoBehaviour {
    private Vector3 touchOffset;
    private Vector3 touchPos;
    private Vector3 touchPosBefore;
    private Vector3 direction;

    private bool touched = false;
    private bool smoothing = false;
    private float distance;
    private float currentSpeed;

    private Collider2D col;
    private Rigidbody2D rb;

    [SerializeField] private float smoothingSpeed = 15;
    [SerializeField] private float followSpeed = 20;
    [SerializeField] private float cutoffSpeed = 0.05f;

    [SerializeField] private bool lockX = false;
    [SerializeField] private bool lockY = false;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        touchPos = Vector3.zero;
    }

    void Update() {
        if (Input.touchCount > 0) {
            
            Touch touch = Input.GetTouch(0);
            touchPosBefore = touchPos;
            touchPos = Camera.main.ScreenToWorldPoint(touch.position); //Transforming the touch coordinates to unit coordinates
            touchPos.z = transform.position.z;


            //Checking if the object is directly touched initally
            if (!touched && touch.phase == TouchPhase.Began && col == Physics2D.OverlapPoint(touchPos)) {
                touchOffset = transform.position - touchPos;
                smoothing = false;
                touched = true;
            }


            if (touched && touch.phase == TouchPhase.Ended) {
                smoothing = true;
                touched = false;

                distance = Vector3.Distance(touchPos, touchPosBefore);
                currentSpeed = Mathf.Clamp(distance / Time.deltaTime, 0, 50);
            }
        }
    }

    private void FixedUpdate() {
        
        if (touched) {
            direction = (touchPos + touchOffset) - (transform.position);

            if (lockX) direction.x = 0;
            if (lockY) direction.y = 0;

            rb.velocity = new Vector2(direction.x, direction.y) * followSpeed;
        }

        if (smoothing) {
            currentSpeed = Mathf.Lerp(currentSpeed, 0, smoothingSpeed * Time.deltaTime);
            rb.velocity = new Vector2(direction.x, direction.y) * currentSpeed;

            if (currentSpeed <= cutoffSpeed) {
                rb.velocity = Vector2.zero;
                smoothing = false;
            }
        }
    }

    public void SetLockX(bool val) {
        lockX = val;
        rb.velocity = Vector2.zero;
        smoothing = false;
    }
    public void SetLockY(bool val) {
        lockY = val;
        rb.velocity = Vector2.zero;
        smoothing = false;
    }
}
