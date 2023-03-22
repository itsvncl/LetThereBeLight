using UnityEngine;

public class Zoom : MonoBehaviour, IDraggable, ITouchBeginEvent, ITouchEndEvent {
    [SerializeField] private GameObject whiteness;
    [SerializeField] private float zoomMultiplier;

    private Vector3 touch1Before = Vector3.zero;
    private Vector3 touch1 = Vector3.zero;
    private Vector3 touch2Before = Vector3.zero;
    private Vector3 touch2 = Vector3.zero;

    private bool touch1Active = false;
    private bool touch2Active = false;

    private float previousDistanceBetweenTouches = 0.0f;

    private float verticalBoundary;
    private float horizontalBoundary;

    private bool levelComplete = false;

    void Start() {
        verticalBoundary = ScreenManager.SM.ScreenHeight / 2.0f;
        horizontalBoundary = ScreenManager.SM.ScreenWidth / 2.0f;
    }

    void Update() {
        Vector3 whitePos = whiteness.transform.position;
        float whiteScale = whiteness.transform.localScale.x;
        
        if(ScreenManager.SM.ScreenHeight - (whiteScale - Mathf.Abs(whitePos.y) * 2) <= 0 && !levelComplete) {
            levelComplete = true;

            LevelManager.Instance.LevelComplete();
            TouchSystem.Instance.Disable();
        }
    }

    public void OnDrag(TouchData touchData) {
        touch1Before = touch1;
        touch2Before = touch2;

        if (touchData.FingerID == 0) {
            touch1 = touchData.Position;
        }
        if (touchData.FingerID == 1) {
            touch2 = touchData.Position;
        }

        if (touch1Active && touch2Active) {
            float currentDistanceBetweenTouches = Vector2.Distance(touch1, touch2);
            float scale = (currentDistanceBetweenTouches - previousDistanceBetweenTouches)  * zoomMultiplier;
            AdjustObjectScale(whiteness, scale);

            Vector3 centerBefore = GetCenterOfVectors(touch1Before, touch2Before);
            Vector3 center = GetCenterOfVectors(touch1, touch2);
            AdjustObjectPosition(whiteness, center - centerBefore);

            previousDistanceBetweenTouches = currentDistanceBetweenTouches;
        }
        else {
            Vector3 activeFingerPos = touch1Active ? touch1 : touch2;
            Vector3 activeFingerBeforePos = touch1Active ? touch1Before : touch2Before;

            AdjustObjectPosition(whiteness, activeFingerPos - activeFingerBeforePos);
        }
    }

    public void OnTouchBegin(TouchData touchData) {
        if (touchData.FingerID == 0) {
            touch1 = touchData.Position;
            touch1Active = true;
        }
        if (touchData.FingerID == 1) {
            touch2 = touchData.Position;
            touch2Active = true;
        }

        if (touch1Active && touch2Active) {
            previousDistanceBetweenTouches = Vector2.Distance(touch2, touch1);
        }
    }

    public void OnTouchEnd(TouchData touchData) {
        if (touchData.FingerID == 0) {
            touch1Active = false;
        }
        if (touchData.FingerID == 1) {
            touch2Active = false;
        }
    }

    private void AdjustObjectScale(GameObject go, float scale) {
        Vector3 initialSacle = go.transform.localScale;
        if(initialSacle.x + scale < 0) {
            go.transform.localScale = new Vector3(0, 0, initialSacle.z);
        }
        else {
            go.transform.localScale = new Vector3(initialSacle.x + scale, initialSacle.y + scale, initialSacle.z);
        }
    }

    private void AdjustObjectPosition(GameObject go, Vector3 offset) {
        Vector3 initialPosition = go.transform.position;

        float newX = initialPosition.x + offset.x;
        float newY = initialPosition.y + offset.y;

        if(newX > horizontalBoundary) {
            newX = horizontalBoundary;
        }else if(newX < -horizontalBoundary) {
            newX = -horizontalBoundary;
        }

        if(newY > verticalBoundary) {
            newY = verticalBoundary;
        }else if (newY < -verticalBoundary) {
            newY = -verticalBoundary;
        }

        go.transform.position = new Vector3(newX, newY, initialPosition.z);
    }

    private Vector3 GetCenterOfVectors(Vector3 t1, Vector3 t2) {
        return Vector3.Lerp(t1, t2, 0.5f);
    }
}
