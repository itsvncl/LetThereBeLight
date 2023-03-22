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
    }

    void Update()
    {
        transform.localRotation = gyro.attitude * new Quaternion(0,0,1,0);
    }
}
