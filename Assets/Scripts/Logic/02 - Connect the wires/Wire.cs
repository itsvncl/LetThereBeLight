using UnityEngine;

public class Wire : MonoBehaviour, IDraggable, ITouchBeginEvent, ITouchEndEvent
{
    [SerializeField] private WireColor _color;
    private bool _connected = false;

    [SerializeField] private GameObject exposedWire;
    [SerializeField] private GameObject finalWire;
    [SerializeField] private GameObject tempWire;

    void Start()
    {
        exposedWire = transform.GetChild(0).gameObject;
        finalWire = transform.GetChild(1).gameObject;
        tempWire = transform.GetChild(2).gameObject;
    }

    public void OnDrag(TouchData touchData) {
        if (_connected) return;

        UpdateTempwireTransformation(touchData.Position);
    }

    public void OnTouchBegin(TouchData touchData) {
        if (_connected) return;

        UpdateTempwireTransformation(touchData.Position);
        tempWire.SetActive(true);
    }

    public void OnTouchEnd(TouchData touchData) {
        if (_connected) return;

        tempWire.SetActive(false);

        if (touchData.gameObject != this.gameObject && touchData.gameObject != null) {
            Wire otherWire = touchData.gameObject.GetComponent<Wire>();

            if (otherWire == null || otherWire._color != _color) return;

            //Sorting order should be adjusted
            exposedWire.SetActive(false);
            finalWire.SetActive(true);
            finalWire.GetComponent<SpriteRenderer>().sortingOrder = 15 + WireGame.ConnectedWireCount;

            _connected = true;

            otherWire.exposedWire.SetActive(false);
            otherWire._connected = true;

            WireGame.WireConnected();
        }
    }

    void UpdateTempwireTransformation(Vector3 position) {
        float distance = Vector2.Distance(tempWire.transform.position, position);
        tempWire.transform.localScale = new Vector3(distance * (1 / transform.localScale.x), tempWire.transform.localScale.y, tempWire.transform.localScale.z);

        //Setting the Z rotation of the tempWire
        Vector3 point = position - tempWire.transform.position;
        float rotationZ = Mathf.Atan2(point.y, point.x) * Mathf.Rad2Deg;
        tempWire.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
    }
}
