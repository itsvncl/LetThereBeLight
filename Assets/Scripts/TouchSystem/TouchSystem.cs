using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchSystem : MonoBehaviour
{

    private Vector3 _worldPosition;
    private int _tapCount;
    private Dictionary<int, TouchData> _touchDictionary = new();

    //Adding raycaster to the camera, if not already present
    void Start() {
        Physics2DRaycaster physicsRaycaster = FindObjectOfType<Physics2DRaycaster>();
        if (physicsRaycaster == null) {
            Camera.main.gameObject.AddComponent<Physics2DRaycaster>();
        }
    }

    void Update() {
        if (Input.touchCount > 0) {

            //Multi touch support
            foreach (Touch touch in Input.touches) {
                _worldPosition = Camera.main.ScreenToWorldPoint(touch.position);
                _worldPosition.z = 0;
                _tapCount = touch.tapCount;

                //New touch
                if (touch.phase == TouchPhase.Began) {
                    HandleNewTouch(touch);
                }

                //Ongoing touch
                if (touch.phase != TouchPhase.Began && touch.phase != TouchPhase.Ended) {
                    HandleOngoingTouch(touch);
                }

                //Ended touch
                if (touch.phase == TouchPhase.Ended) {
                    HandleFinishedTouch(touch);
                }

                if (touch.phase == TouchPhase.Canceled) {
                    _touchDictionary.Remove(touch.fingerId);
                }
            }
        }

        void HandleNewTouch(Touch touch) {
            _touchDictionary.Add(touch.fingerId, new TouchData(_worldPosition, touch.fingerId, null, _tapCount));

            GameObject hitObject = GetTouchedObject();

            if (hitObject != null) {
                _touchDictionary[touch.fingerId] = new TouchData(_worldPosition, touch.fingerId, hitObject, _tapCount);
                ITouchBeginEvent beginEvent = hitObject.GetComponent<ITouchBeginEvent>();

                if (beginEvent != null) {
                    beginEvent.OnTouchBegin(_touchDictionary[touch.fingerId]);
                }
            }
        }
        void HandleOngoingTouch(Touch touch) {
            //Ongoing touch only gets called back to Draggables
            //Wont recognise newly touched objecs.
            TouchData previousTouchData = _touchDictionary[touch.fingerId];
            TouchData newTouchData = CreateMovingPosData(previousTouchData);
            _touchDictionary[touch.fingerId] = newTouchData;

            GameObject beginGameObject = previousTouchData.gameObject;

            if (beginGameObject != null) {
                IDraggable dragEvent = beginGameObject.GetComponent<IDraggable>();

                if (dragEvent != null) {
                    dragEvent.OnDrag(newTouchData);
                }
            }
        }
        void HandleFinishedTouch(Touch touch) {
            TouchData previousTouchData = _touchDictionary[touch.fingerId];
            TouchData endTouchData = CreateEndPosData(previousTouchData);
            _touchDictionary[touch.fingerId] = endTouchData;

            GameObject beginGameObject = previousTouchData.gameObject;
            GameObject endObject = GetTouchedObject();

            //If the touch began touching a gameobject
            if (beginGameObject != null) {

                //Normal end event
                ITouchEndEvent endEvent = beginGameObject.GetComponent<ITouchEndEvent>();
                if (endEvent != null) {
                    endEvent.OnTouchEnd(endTouchData);
                }

                //Swipe event
                ISwipeEvent swipeEvent = beginGameObject.GetComponent<ISwipeEvent>();
                if (swipeEvent != null) {
                    swipeEvent.OnSwipe(GetSwipeDirection(endTouchData));
                }

                //OnClick event
                //Currently the OnClick is not restricted to distance
                if (beginGameObject.Equals(endObject)) {
                    IClickable clickEvent = beginGameObject.GetComponent<IClickable>();
                    if(clickEvent != null) {
                        clickEvent.OnClick(endTouchData);
                    }
                }
            }

            //If the touch began without touching a game object
            if (endObject != null) {
                ITouchEndEvent endEvent = endObject.GetComponent<ITouchEndEvent>();
                if (endEvent != null) {
                    endEvent.OnTouchEnd(endTouchData);
                }
            }

            //Removing the touch from the map
            _touchDictionary.Remove(touch.fingerId);
        }

        GameObject GetTouchedObject() {
            RaycastHit2D hit = Physics2D.Raycast(_worldPosition, Camera.main.transform.forward);

            if (hit.collider == null) return null;

            GameObject hitObject = hit.collider.gameObject;

            if (gameObject == null) {
                throw new System.Exception("Touched GameObject is null");
            }

            return hitObject;
        }

        TouchData CreateEndPosData(TouchData data) {
            return new TouchData(_worldPosition, data.BeginPosition, _worldPosition, data.FingerID, data.gameObject, _tapCount);
        }
        TouchData CreateMovingPosData(TouchData data) {
            return new TouchData(_worldPosition, data.BeginPosition, data.FingerID, data.gameObject, _tapCount);
        }
        SwipeDirection GetSwipeDirection(TouchData data) {
            Vector2 directionVector = (data.EndPosition - data.BeginPosition).normalized;

            if (Mathf.Abs(directionVector.x) > Mathf.Abs(directionVector.y)) {
                return directionVector.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
            }
            else {
                return directionVector.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;
            }
        }
    }
}
