using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroController : MonoBehaviour {
    private Gyroscope gyro;
    private Rigidbody2D rb;

    [SerializeField] private float velocity = 50.0f;
    [SerializeField] private float drag = 1.0f;
    [SerializeField] private float tiltTreshold = 0.05f;

    private Vector2 calucaltedVelocity = Vector2.zero;

    private bool canMove = true;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        gyro = Input.gyro;
        gyro.enabled = true;
    }

    void Update() {
        if (!canMove) return;

        float xGain = Mathf.Abs(gyro.gravity.x) > tiltTreshold ? gyro.gravity.x : 0f;
        float yGain = Mathf.Abs(gyro.gravity.y) > tiltTreshold ? gyro.gravity.y : 0f;

        calucaltedVelocity.x = Mathf.Lerp(calucaltedVelocity.x, xGain, drag * Time.deltaTime);
        calucaltedVelocity.y = Mathf.Lerp(calucaltedVelocity.y, yGain, drag * Time.deltaTime);

        rb.velocity = calucaltedVelocity * velocity;
    }
}
