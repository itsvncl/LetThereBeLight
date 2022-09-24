using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothDrag : MonoBehaviour
{
    private Vector3 touchOffset;
    private Vector3 touchPos;
    private Vector3 initialTouch;
    private Vector3 touchPosBefore;
    private Vector3 dircetion;
    private bool touched;

    private Vector3 smoothingEnd;
    private bool smoothing;

    private Collider2D col;
    private Rigidbody2D rb;  


    [SerializeField] private float smoothingSpeed = 10;
    [SerializeField] private float smoothingDistance = 3;
    [SerializeField] private float followSpeed = 20;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        touchPos = Vector3.zero;
        touched = false;
        smoothing = false;
    }

    void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            touchPosBefore = touchPos;
            touchPos = Camera.main.ScreenToWorldPoint(touch.position); //Transforming the touch coordinates to unit coordinates
            touchPos.z = transform.position.z;

            //Getting the offset of the touch from the objects center
            if(touch.phase == TouchPhase.Began)
            {
                initialTouch = transform.position;
                touchOffset = transform.position - touchPos;
            }
            
            //Checking if the object is directly touched initally
            if(!touched && touch.phase == TouchPhase.Began && col == Physics2D.OverlapPoint(touchPos))
            {
                smoothing = false;
                touched = true;
            }

            if (touched)
            {
                dircetion = (touchPos + touchOffset) - (transform.position);
                rb.velocity = new Vector2(dircetion.x, dircetion.y) * followSpeed;
            }


            if (touched && touch.phase == TouchPhase.Ended)
            {
                if (touched) StartCoroutine(startSmoothing());
                touched = false;
                rb.velocity = Vector2.zero;


                //Getting the distance between the location of the sprite, and the location of the lifted finger
                float distance = Vector3.Distance(touchPos, touchPosBefore);
                smoothingEnd = calculateEndPosition(touchPosBefore, touchPos);
            }
        }

        if (smoothing) transform.position = Vector3.Lerp(transform.position, smoothingEnd, Time.deltaTime * smoothingSpeed);
    }

    Vector3 calculateEndPosition(Vector3 before, Vector3 after)
    {
        Vector3 dir = after - before;
        
        float x = dir.x < 0f ? -1 : 1;
        float y = dir.y < 0f ? -1 : 1;

        Vector3 endPos = new Vector3();
        endPos.x = transform.position.x + (dir.x ) * smoothingDistance;
        endPos.y = transform.position.y + (dir.y ) * smoothingDistance;
        endPos.z = transform.position.z;

        return endPos;
    }

    IEnumerator startSmoothing()
    {
        smoothing = true;
        yield return new WaitForSeconds(4);
        smoothing = false;
    }
}
