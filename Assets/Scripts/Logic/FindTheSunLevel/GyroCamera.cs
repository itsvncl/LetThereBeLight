using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroCamera : MonoBehaviour
{
    Gyroscope gyro;

    void Start()
    {
        gyro = Input.gyro;
        gyro.enabled = true;
        gyro.updateInterval = 0.005f;
    }

    void Update()
    {
        transform.localRotation = gyro.attitude * new Quaternion(0,0,1,0);
    }
}
