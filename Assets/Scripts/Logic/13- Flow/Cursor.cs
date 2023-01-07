using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    Vector3 outOfBound = new Vector3(999,999,1);


    void Update()
    {
        if (TouchUtil.TU.TouchInProgress) {
            transform.position = TouchUtil.TU.CurrentPosition;
        }
        else {
            FlowGame.Instance.CanDraw = false;
            FlowGame.Instance.PreviousCell = null;
            transform.position = outOfBound;
        }
    }
}
