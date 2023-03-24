using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RubGame : MonoBehaviour, IDraggable, ITouchBeginEvent {

    private Vector3 prevPos;
    [SerializeField] float decreaseSpeed = 0.04f;
    [SerializeField] public float increaseSpeed = 0.05f;

    bool positive = true;
    public float lightLevel = 0.0f;
    bool win = false;

    [SerializeField] Image lightImage;
    
    public void OnDrag(TouchData touchData) {
        if (touchData.FingerID != 0) return;

        if (positive && touchData.Position.y > prevPos.y) {
            lightLevel += increaseSpeed;
            positive = false;
        }
        else if (!positive && touchData.Position.y < prevPos.y) {
            lightLevel += increaseSpeed;
            positive = true;
        }
        
        prevPos = touchData.Position;
    }

    public void OnTouchBegin(TouchData touchData) {
        prevPos = touchData.Position;
    }

    void FixedUpdate() {
        if(lightLevel > decreaseSpeed) {
            lightLevel -= decreaseSpeed;
        }
        else {
            lightLevel = 0.0f;
        }
    }

    void Update() {
        UpdateImage();

        if (lightLevel >= 1.0f && !win) {
            win = true;

            try {
                LevelManager.Instance.LevelComplete();
            } catch {
                Debug.Log("LevelManager not inited");
            }
        }
    }

    private void UpdateImage() {
        try {
            Color c = lightImage.color;
            c.a = lightLevel;
            lightImage.color = c;
        } catch {
            Debug.Log("Image is not loaded");
        }
    }
}
