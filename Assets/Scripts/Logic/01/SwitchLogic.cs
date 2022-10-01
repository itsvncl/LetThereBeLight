using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/* On this level, the win condition is to touch the top collider */
public class SwitchLogic : MonoBehaviour {
    private Rigidbody2D rb;
    private BoxCollider2D self;
    [SerializeField] private Collider2D ceiling;
    [SerializeField] private SmoothDrag touchController;

    // Start is called before the first frame update
    void Start() {
        self = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (ceiling.IsTouching(self)) {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;

            LevelManager.Instance.LevelComplete();
        }
    }
}
