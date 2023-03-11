using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitchOrientationGame : MonoBehaviour
{
    [SerializeField] private Rigidbody2D lightSwitchRb;

    private void Update() {
        if(Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown) {
            lightSwitchRb.velocity = new Vector2(0.0f, 1.0f);
        }
    }
}
