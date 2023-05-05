using System;
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

    private Direction direction = Direction.Stationary;
    public Vector3 destination;
    Rigidbody2D rb;
    public int locationIndex;


    void Start() {
        //Start values are between 2 and 4. 4 has 1/4 chance of spawning
        rb = GetComponent<Rigidbody2D>();

        if (UnityEngine.Random.Range(0, 4) == 0) {
            tier = 1;
        }

        destination = transform.position;
        locationIndex = LocationToIndex();

        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateSprite(tier);

    }

    void FixedUpdate() {
        if (direction == Direction.Stationary) return;

        Vector3 newPos = transform.position;

        switch (direction) {
            case Direction.Up:
                newPos.y += movementSpeed;

                if (newPos.y >= destination.y) {
                    Settle(destination);
                    return;
                }
                break;
            case Direction.Down:
                newPos.y -= movementSpeed;

                if (newPos.y <= destination.y) {
                    Settle(destination);
                    return;
                }
                break;
            case Direction.Right:
                newPos.x += movementSpeed;

                if (newPos.x >= destination.x) {
                    Settle(destination);
                    return;
                }
                break;
            case Direction.Left:
                newPos.x -= movementSpeed;

                if (newPos.x <= destination.x) {
                    Settle(destination);
                    return;
                }
                break;

        }

        transform.position = newPos;
    }

    void OnTriggerStay2D(Collider2D collider) {
        HandleCollision(collider.gameObject);
    }

    void HandleCollision(GameObject collision) {
        NumberBlock other = collision.GetComponent<NumberBlock>();
        //Meaning that the other is not a number
        if (other == null) {
            //Settle(destination);
            return;
        }

        if (other.tier == this.tier && !merged) {
            if (other.direction == Direction.Stationary) {
                Destroy(other.gameObject);
                Merge();
            } else if (direction == Direction.Stationary) {
                Destroy(this.gameObject);
                other.Merge();
            }
        }
    }

    void Merge() {
        if (merged) return;
        tier++;
        UpdateSprite(tier);
        merged = true;
    }

    void UpdateSprite(int tier) {
        try {
            spriteRenderer.sprite = numberSprites[tier];
        } catch {
        }
    }

    void Settle(Vector3 newPos) {
        transform.position = newPos;

        direction = Direction.Stationary;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        locationIndex = LocationToIndex();
    }

    //Returns the object that is occuping the point. Returns null if this!
    GameObject GetOccupant(Vector3 loc) {
        List<Collider2D> colliders = new ();
        ContactFilter2D filter = new ();
        filter.NoFilter();

        Physics2D.OverlapPoint(loc, filter, colliders);

        GameObject number = null;
        foreach(var collider in colliders) {
            if (collider.gameObject.CompareTag("Player")) number = collider.gameObject;
        }

        return number == this ? null : number;
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
            return;
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
            else {
                previousNumber = current;
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
    public bool HasDestionation(TouchUtil.DragDirection direction) {
        Vector3 currentPos = transform.position;
        List<GameObject> occupancyList = new List<GameObject>();

        if (direction == TouchUtil.DragDirection.Up) {
            while (currentPos.y < 1.5f) {
                currentPos.y += 1f;
                occupancyList.Add(GetOccupant(currentPos));
            }
        }
        if (direction == TouchUtil.DragDirection.Down) {
            while (currentPos.y > -1.5f) {
                currentPos.y -= 1f;
                occupancyList.Add(GetOccupant(currentPos));
            }
        }
        if (direction == TouchUtil.DragDirection.Right) {
            while (currentPos.x < 1.5f) {
                currentPos.x += 1f;
                occupancyList.Add(GetOccupant(currentPos));
            }
        }
        if (direction == TouchUtil.DragDirection.Left) {
            while (currentPos.x > -1.5f) {
                currentPos.x -= 1f;
                occupancyList.Add(GetOccupant(currentPos));
            }
        }

        //Calculate with the list
        if (occupancyList.Count == 0) {
            destination = transform.position;
            return false;
        }

        occupancyList.Reverse();
        NumberBlock previousNumber = null;

        foreach (var occupant in occupancyList) {
            if (occupant == null) {
                return true;
            }

            NumberBlock current = occupant?.GetComponent<NumberBlock>();

            if (previousNumber == null) {
                previousNumber = current;
                continue;
            }

            if (previousNumber.tier == current.tier) {
                return true;
            }
            else {
                previousNumber = current;
            }
        }

        if (previousNumber != null && previousNumber.tier == tier) {
            return true;
        }

        return false;
    }

    public int LocationIndex {
        get { return locationIndex; }
    }

    public int LocationToIndex() {
        float[] positions = NumberGameLogic.validPos;
        int x = 0;
        int y = 0;

        for(int i = 0; i < positions.Length; i++) {
            if(positions[i] == destination.x) {
                x = i;
            }
            if (positions[i] == destination.y) {
                y = i;
            }
        }

        return (12 - y * 4) + x;
    }

    public int Tier {
        get { return tier; }
    }

    public Direction Dir { 
        get { return direction; }
    }

}
