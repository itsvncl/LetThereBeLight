using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    Vector3 outOfBound = new Vector3(999,999,1);
    SpriteRenderer spriteRenderer;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        if (TouchUtil.TU.TouchInProgress) {
            transform.position = TouchUtil.TU.CurrentPosition + new Vector3(0,0, -5);
        }
        else {
            FlowGame.Instance.CanDraw = false;
            FlowGame.Instance.PreviousCell = null;
            transform.position = outOfBound;
        }
    }

    public void SetColor(Color color) {
        spriteRenderer.color = color;
    }
}
