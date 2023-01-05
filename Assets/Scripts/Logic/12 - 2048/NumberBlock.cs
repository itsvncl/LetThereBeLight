using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberBlock : MonoBehaviour {
    public enum Direction {
        Up, Down, Left, Right, Stationary,
    }


    private int tier = 0;
    SpriteRenderer spriteRenderer;
    private bool merged = false;

    [SerializeField] private Sprite[] numberSprites;
    [SerializeField] private float movementSpeed;

    Direction direction = Direction.Stationary;
    public Vector3 destination;
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

        GameObject occupant = GetOccupant(occupancyTest);

        if (occupant != null) {
            HandleCollision(occupant);
        }
        else {
            transform.position = newPos;
        }

    }

    //Ez meg se hívódik
    private void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log("Ilyen");
        HandleCollision(collision.gameObject);
    }

    void HandleCollision(GameObject collision) {
        NumberBlock other = collision.GetComponent<NumberBlock>();
        //Meaning that the other is not a number
        if(other == null) {
            Settle();
            return;
        }

        if (other.tier == this.tier && !merged) {
            Destroy(other.gameObject);
            Merge();
        }
        else {
            Settle();
        }
    }

    void Merge() {
        if (merged) return;
        tier++;
        spriteRenderer.sprite = numberSprites[tier];
        merged = true;
    }

    void Settle() {
        //Helyre kell rakni
        float[] positions = NumberGameLogic.validPos;
        Vector3 newPos = transform.position;

        if(direction == Direction.Left || direction == Direction.Right) {
            foreach (float pos in positions) {
                if (Mathf.Abs(pos - newPos.x) <= 0.5) {
                    newPos.x = pos;
                    break;
                }
            }
        }
        else {
            foreach (float pos in positions) {
                if (Mathf.Abs(pos - newPos.y) <= 0.5) {
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

    //Returns the object that is occuping the point. Returns null if this!
    GameObject GetOccupant(Vector3 loc) {
        Collider2D overlap = Physics2D.OverlapPoint(loc);

        if (overlap == GetComponent<Collider2D>()) return null;

        return overlap?.gameObject;
    }

    public void MoveInDirection(TouchUtil.DragDirection dir) {
        merged = false;
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

        CalculateDestination();
    }

    private void CalculateDestination() {
        Vector3 currentPos = transform.position;
        List <GameObject> occupancyList = new List<GameObject>();
        int step = 0;

        if(direction == Direction.Up) {
            while(currentPos.y < 1.5f) {
                currentPos.y += 1f;
                occupancyList.Add(GetOccupant(currentPos));
            }
        }
        if (direction == Direction.Down) {
            while (currentPos.y > -1.5f) {
                currentPos.y -= 1f;
                occupancyList.Add(GetOccupant(currentPos));
            }
        }
        if (direction == Direction.Right) {
            while (currentPos.x < 1.5f) {
                currentPos.x += 1f;
                occupancyList.Add(GetOccupant(currentPos));
            }
        }
        if (direction == Direction.Left) {
            while (currentPos.x > -1.5f) {
                currentPos.x -= 1f;
                occupancyList.Add(GetOccupant(currentPos));
            }
        }

        //Calculate with the list
        if(occupancyList.Count == 0) {
            destination = transform.position;
        }
        
        occupancyList.Reverse();
        NumberBlock previousNumber = null;

        foreach(var occupant in occupancyList) {
            if(occupant == null) {
                step++;
                continue;
            }

            NumberBlock current = occupant?.GetComponent<NumberBlock>();

            if (previousNumber == null) {
                previousNumber = current;
                continue;
            }

            if (previousNumber.tier == current.tier) {
                step++;
                previousNumber = null;
                continue;
            }
        }

        if(previousNumber != null && previousNumber.tier == tier) {
            step++;
        }

        //Assigning the destination
        if (direction == Direction.Up) {
            destination = new Vector3(transform.position.x, transform.position.y + (step * 1f),transform.position.z);
        }
        if (direction == Direction.Down) {
            destination = new Vector3(transform.position.x, transform.position.y - (step * 1f), transform.position.z);
        }
        if (direction == Direction.Right) {
            destination = new Vector3(transform.position.x + (step * 1f), transform.position.y, transform.position.z);
        }
        if (direction == Direction.Left) {
            destination = new Vector3(transform.position.x - (step * 1f), transform.position.y, transform.position.z);
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
