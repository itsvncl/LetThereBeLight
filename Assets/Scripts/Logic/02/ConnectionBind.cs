using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionBind : MonoBehaviour
{
    [SerializeField] private Collider2D target;

    [SerializeField] private GameObject tempWire;
    [SerializeField] private GameObject finalWire;

    [SerializeField] private GameObject exposedWireLeft;
    [SerializeField] private GameObject exposedWireRight;

    private Collider2D col;
    private Vector3 touchPos;
    private bool touched = false;

    void Start()
    {
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            touchPos = Camera.main.ScreenToWorldPoint(touch.position); //Transforming the touch coordinates to unit coordinates
            touchPos.z = transform.position.z;

            if(touch.phase == TouchPhase.Began && col == Physics2D.OverlapPoint(touchPos))
            {
                touched = true;
                tempWire.SetActive(true);

            }

            if (touched)
            {
                Vector3 where = tempWire.transform.position - touchPos;
                tempWire.transform.localScale = new Vector3(-where.x * (1/transform.localScale.x), tempWire.transform.localScale.y, tempWire.transform.localScale.z);
            }

            if(touched && touch.phase == TouchPhase.Ended && target == Physics2D.OverlapPoint(touchPos))
            {
                Debug.Log("Working");
                finalWire.SetActive(true);
                exposedWireLeft.SetActive(false);
                exposedWireRight.SetActive(false);
                tempWire.SetActive(false);

            }
        }
    }
}
