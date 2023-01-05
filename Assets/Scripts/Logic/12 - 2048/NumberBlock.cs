using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberBlock : MonoBehaviour {
    public enum Direction {
        Up, Down, Left, Right, Stationary,
    }


    private int tier = 0;
    SpriteRenderer spriteRenderer;

    [SerializeField] private Sprite[] numberSprites;
    [SerializeField] private float movementSpeed;

    Direction direction = Direction.Stationary;
    Rigidbody2D rb;
    public int locationIndex;


    void Start() {
        //Start values are between 2 and 4. 4 has 1/4 chance of spawning
        rb = GetComponent<Rigidbody2D>();

        if (Random.Range(0, 4) == 0) {
            tier = 1;
        }

        locationIndex = LocationToIndex();

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = numberSprites[tier];
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (direction == Direction.Stationary) return;

        Vector3 newPos = transform.position;
        Vector3 occupancyTest = newPos;
        float scaleOffsetX = transform.localScale.x / 2;
        float scaleOffsetY = transform.localScale.x / 2;

        switch (direction) {
            case Direction.Up:
                newPos.y += movementSpeed;
                occupancyTest.y += movementSpeed + scaleOffsetY;
                break;
            case Direction.Down:
                newPos.y -= movementSpeed;
                occupancyTest.y -= movementSpeed + scaleOffsetY;
                break;
            case Direction.Right:
                newPos.x += movementSpeed;
                occupancyTest.x += movementSpeed + scaleOffsetX;
                break;
            case Direction.Left:
                newPos.x -= movementSpeed;
                occupancyTest.x -= movementSpeed + scaleOffsetX;
                break;
        }

        if (IsOccupied(occupancyTest)) {
            Settle();
        }
        else {
            transform.position = newPos;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log(collision.gameObject.name);


        Settle();
    }

    void Merge() {
        tier++;
        spriteRenderer.sprite = numberSprites[tier];
    }

    void Settle() {
        //Helyre kell rakni
        float[] positions = NumberGameLogic.validPos;
        Vector3 newPos = transform.position;

        if(direction == Direction.Left || direction == Direction.Right) {
            foreach (float pos in positions) {
                if (Mathf.Abs(pos - newPos.x) < 0.5) {
                    newPos.x = pos;
                    break;
                }
            }
        }
        else {
            foreach (float pos in positions) {
                if (Mathf.Abs(pos - newPos.y) < 0.5) {
                    newPos.y = pos;
                    break;
                }
            }
        }

        transform.position = newPos;

        direction = Direction.Stationary;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        locationIndex = LocationToIndex();
    }

    bool IsOccupied(Vector3 loc) {
        Collider2D overlap = Physics2D.OverlapPoint(loc);

        if (overlap == GetComponent<Collider2D>()) return false;

        return overlap != null;
    }

    public void MoveInDirection(TouchUtil.DragDirection dir) {
        switch (dir) {
            case TouchUtil.DragDirection.Up:
                direction = Direction.Up;
                rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                break;
            case TouchUtil.DragDirection.Down:
                direction = Direction.Down;
                rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                break;
            case TouchUtil.DragDirection.Right:
                direction = Direction.Right;
                rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                break;
            case TouchUtil.DragDirection.Left:
                direction = Direction.Left;
                rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                break;
        }
    }

    public int LocationIndex {
        get { return locationIndex; }
    }

    public int LocationToIndex() {
        float[] positions = NumberGameLogic.validPos;
        int x = 0;
        int y = 0;

        for(int i = 0; i < positions.Length; i++) {
            if(positions[i] == transform.position.x) {
                x = i;
            }
            if (positions[i] == transform.position.y) {
                y = i;
            }
        }

        return (12 - y * 4) + x;
    }

}
