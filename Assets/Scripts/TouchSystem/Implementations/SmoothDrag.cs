using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothDrag : MonoBehaviour, IDraggable, ITouchBeginEvent, ITouchEndEvent
{
    private Rigidbody2D rb;

    private Vector3 touchOffset;
    private Vector3 touchPosBefore;
    private Vector3 direction;

    [SerializeField] private float smoothingSpeed = 15;
    [SerializeField] private float followSpeed = 20;
    [SerializeField] private float cutoffSpeed = 0.05f;

    [SerializeField] private bool lockX = false;
    [SerializeField] private bool lockY = false;

    //Variables for smoothing phase
    private bool smoothingPhase = false;
    private float distance;
    private float currentSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if(rb == null) {
            rb = this.gameObject.AddComponent<Rigidbody2D>();
        }

        rb.gravityScale = 0.0f;
    }

    void Update()
    {
        if (smoothingPhase) {
            currentSpeed = Mathf.Lerp(currentSpeed, 0, smoothingSpeed * Time.deltaTime);
            rb.velocity = new Vector2(direction.x, direction.y) * currentSpeed;

            if (currentSpeed <= cutoffSpeed) {
                rb.velocity = Vector2.zero;
                smoothingPhase = false;
            }
        }
    }

    public void OnDrag(TouchData touchData) {
        direction = (touchData.Position + touchOffset) - (transform.position);

        if (lockX) direction.x = 0;
        if (lockY) direction.y = 0;

        rb.velocity = new Vector2(direction.x, direction.y) * followSpeed;
        touchPosBefore = touchData.Position;
    }

    public void OnTouchBegin(TouchData touchData) {
        touchOffset = transform.position - touchData.Position;
        touchPosBefore = touchData.BeginPosition;
        smoothingPhase = false;
    }

    public void OnTouchEnd(TouchData touchData) {
        smoothingPhase = true;
        distance = Vector3.Distance(touchData.EndPosition, touchPosBefore);
        currentSpeed = Mathf.Clamp(distance / Time.deltaTime, 0, 50);
    }


    public void SetLockX(bool val) {
        lockX = val;
        rb.velocity = Vector2.zero;
        smoothingPhase = false;
    }
    public void SetLockY(bool val) {
        lockY = val;
        rb.velocity = Vector2.zero;
        smoothingPhase = false;
    }
}
