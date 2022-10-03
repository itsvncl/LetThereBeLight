using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionLogic : MonoBehaviour {
    [SerializeField] private Collider2D target;

    [SerializeField] private GameObject tempWire;
    [SerializeField] private GameObject finalWire;

    private SpriteRenderer tempWireRenderer;
    private SpriteRenderer finalWireRenderer;

    [SerializeField] private GameObject exposedWireLeft;
    [SerializeField] private GameObject exposedWireRight;

    private Vector3 touchPos;
    private bool touched = false;
    
    private ConnectionCounter counter;
    private Collider2D col;

    void Start() {
        col = GetComponent<Collider2D>();
        counter = transform.parent.GetComponent<ConnectionCounter>();
        
        tempWireRenderer = tempWire.GetComponent<SpriteRenderer>();
        finalWireRenderer = finalWire.GetComponent<SpriteRenderer>();
    }

    void Update() {
        if (Input.touchCount > 0) {
            
            Touch touch = Input.GetTouch(0); //Getting only the first touch
            touchPos = Camera.main.ScreenToWorldPoint(touch.position); //Transforming the touch coordinates to unit coordinates
            touchPos.z = transform.position.z;

            //Start of touch
            if (touch.phase == TouchPhase.Began && col == Physics2D.OverlapPoint(touchPos)) {
                touched = true;
                
                tempWireRenderer.sortingOrder = 15 + counter.ConnectedWires;
                tempWire.SetActive(true);
                UpdateTempWireTransformation();
            }

            //End of touch
            if (touch.phase == TouchPhase.Ended && touched)  {
                tempWire.SetActive(false);
                
                if (target != Physics2D.OverlapPoint(touchPos)) {
                    return;
                }

                finalWireRenderer.sortingOrder = 15 + counter.ConnectedWires;
                finalWire.SetActive(true);

                exposedWireLeft.SetActive(false);
                exposedWireRight.SetActive(false);
                tempWire.SetActive(false);


                counter.ConnectWire();
            }
        }
    }

    private void FixedUpdate() {
        if (touched) {
            UpdateTempWireTransformation();
        }
    }

    private void UpdateTempWireTransformation() {
        //Setting the X stretch of the tempWire
        float distance = Vector2.Distance(tempWire.transform.position, touchPos);
        tempWire.transform.localScale = new Vector3(distance * (1 / transform.localScale.x), tempWire.transform.localScale.y, tempWire.transform.localScale.z);

        //Setting the Z rotation of the tempWire
        Vector3 point = touchPos - tempWire.transform.position;
        float rotationZ = Mathf.Atan2(point.y, point.x) * Mathf.Rad2Deg;
        tempWire.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
    }
}
