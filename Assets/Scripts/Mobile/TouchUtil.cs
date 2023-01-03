using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchUtil : MonoBehaviour
{
    public static bool IsTouching(Touch touch, Collider2D col) {
        Vector3 wp = Camera.main.ScreenToWorldPoint(touch.position);
        Vector2 touchPosition = new Vector2(wp.x, wp.y);

        return col == Physics2D.OverlapPoint(touchPosition);
    }
}
