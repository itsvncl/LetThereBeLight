using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragStrech : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(t.position);

            Vector3 vDistance = transform.position - touchPos;

            transform.localScale = new Vector3(-vDistance.x, transform.localScale.y, transform.localScale.z);
        }
    }
}
