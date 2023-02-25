using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroController : MonoBehaviour
{
    private Gyroscope gyro;
    private Rigidbody2D rb;

    [SerializeField] private float velocity = 50.0f;
    [SerializeField] private float drag = 1.0f;

    private Vector2 calucaltedVelocity = Vector2.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gyro = Input.gyro;
        gyro.enabled = true;
    }

    void Update()
    {
        float xGain = Mathf.Abs(gyro.gravity.x) > 0.05f ? gyro.gravity.x : 0f;
        float yGain = Mathf.Abs(gyro.gravity.y) > 0.05f ? gyro.gravity.y : 0f;

        calucaltedVelocity.x = Mathf.Lerp(calucaltedVelocity.x, xGain, drag * Time.deltaTime);
        calucaltedVelocity.y = Mathf.Lerp(calucaltedVelocity.y, yGain, drag * Time.deltaTime);

        rb.velocity = calucaltedVelocity * velocity;
    }
}
