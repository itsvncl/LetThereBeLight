using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapInPlace : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D col;

    [SerializeField] private float snapSpeed = 0.05f;

    bool inSlot = true;
    bool touched = false;
    float slot = 0.0f;
    float startPos;
    float pos;

    private List<float> validShort = new() { 1.0f, 0.5f, 0.0f, -0.5f, -1.0f };
    private List<float> validLong = new() { 0.75f, 0.25f, -0.25f, -0.75f };

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        RestrictMovement();
        Snap();
    }

    void RestrictMovement() {

        if (Input.touchCount < 1) {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            return;
        }

        var touch = Input.GetTouch(0);

        if (TouchUtil.IsTouching(touch, col) && touch.phase == TouchPhase.Began) {
            touched = true;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation | (CompareTag("Vertical") ? RigidbodyConstraints2D.FreezePositionX : RigidbodyConstraints2D.FreezePositionY);
        }
        else {
            if (touch.phase == TouchPhase.Ended) touched = false;
            if (!touched) rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    void Snap() {

        //Finding out of place block
        if (!touched && !InSlot() && inSlot) {
            inSlot = false;
            slot = FindClosestSlot();
            startPos = CompareTag("Vertical") ? transform.position.y : transform.position.x;
        }

        //If out of place
        if (!inSlot) {
            float pos = CompareTag("Vertical") ? transform.position.y : transform.position.x;
            

            //Moving it toward the slot.
            Vector3 newPos = transform.position;
            if (CompareTag("Vertical")) {
                if (startPos < slot) newPos.y += snapSpeed * Time.deltaTime;
                else newPos.y -= snapSpeed * Time.deltaTime;
            }
            else {
                if (startPos < slot) newPos.x += snapSpeed * Time.deltaTime;
                else newPos.x -= snapSpeed * Time.deltaTime;
            }


            //If reached the slot, lock it in
            if (CompareTag("Vertical")) {
                if ((slot - newPos.y < 0 && startPos <= slot) || (slot - newPos.y > 0 && startPos >= slot)) {
                    transform.position = new Vector3(transform.position.x, slot, transform.position.z);
                    inSlot = true;
                    return;
                }
            }
            else {
                if ((slot - newPos.x < 0 && startPos <= slot) || (slot - newPos.x > 0 && startPos >= slot)) {
                    transform.position = new Vector3(slot, transform.position.y, transform.position.z);
                    inSlot = true;
                    return;
                }
            }

            transform.position = newPos;

        }
    }

    bool InSlot() {
        float pos = CompareTag("Vertical") ? transform.position.y : transform.position.x;
        List<float> positions = name.EndsWith("Long") ? validLong : validShort;

        return positions.Contains(pos);
    }

    float FindClosestSlot() {

        float currentPos = CompareTag("Vertical") ? transform.position.y : transform.position.x;
        List<float> positions = name.EndsWith("Long") ? validLong : validShort;

        foreach (float pos in positions) {
            float offset = Math.Abs(pos - currentPos);
            if (offset <= 0.25) {
                return pos;
            }
        }

        return 0.0f;
    }
}
