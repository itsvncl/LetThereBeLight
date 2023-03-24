using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightFlickering : MonoBehaviour
{
    public Image light;

    public bool on = true;

    public float minTimeOff = 0.05f;
    public float maxTimeOff = 0.2f;

    public float maxTimeOn = 0.5f;
    public float minTimeOn = 2.0f;


    public float switchTime;


    private void Start() {
        switchTime = Time.time + Random.Range(minTimeOff, maxTimeOff);
    }

    void FixedUpdate() {
        if(Time.time > switchTime) {
            light.gameObject.SetActive(on);
            on = !on;

            if (on) {
                switchTime = Time.time + Random.Range(minTimeOff, maxTimeOff);
            }
            else {
                switchTime = Time.time + Random.Range(minTimeOn, maxTimeOn);
            }
        }
    }
}
