using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchUtil : MonoBehaviour {

    private DragDirection _dragDirection;
    private Vector3 _beginPosition;
    private Vector3 _endPosition;
    private TouchState _state;
    private float _swipeDistance;
    private bool _touchCompleted;

    public enum DragDirection {
        Up, Down, Left, Right
    }

    public enum TouchState {
        Ended, Began
    }

    public static bool IsTouching(Touch touch, Collider2D col) {
        Vector3 wp = Camera.main.ScreenToWorldPoint(touch.position);
        Vector2 touchPosition = new Vector2(wp.x, wp.y);

        return col == Physics2D.OverlapPoint(touchPosition);
    }

    public static DragDirection GetDragDirection(Vector2 directionVector) {

        if (Mathf.Abs(directionVector.x) > Mathf.Abs(directionVector.y)) {
            return directionVector.x > 0 ? DragDirection.Right : DragDirection.Left;
        }
        else {
            return directionVector.y > 0 ? DragDirection.Up : DragDirection.Down;
        }

    }

    void Update() {
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position); //Transforming the touch coordinates to unit coordinates
            touchPos.z = transform.position.z;

            if (touch.phase == TouchPhase.Began) {
                _state = TouchState.Began;
                _beginPosition = touch.position;
            }

            if (touch.phase == TouchPhase.Ended) {
                _state = TouchState.Ended;
                _endPosition = touch.position;
                _touchCompleted = true;
                _swipeDistance = Vector2.Distance(_beginPosition, _endPosition);

                _dragDirection = GetDragDirection((_endPosition - _beginPosition).normalized);
            }
        }
    }

    public DragDirection SwipeDirection {
        get { return _dragDirection; }
    }

    public TouchState State {
        get { return _state; }
    }

    public bool TouchCompleted { 
        get {
            bool value = _touchCompleted;
            _touchCompleted = false;
            return value;
        }
    }

    public float SwipeDistance { 
        get { return _swipeDistance; }
    }
}
