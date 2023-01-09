using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObject : MonoBehaviour, IClickable, ITouchBeginEvent, ITouchEndEvent, IDraggable
{
    public void OnClick(TouchData touchData) {
        Debug.Log("I got clicked");
    }

    public void OnDrag(TouchData touchData) {
        Debug.Log("Being moved");
        transform.position = touchData.Position;
    }

    public void OnTouchBegin(TouchData touchData) {
        Debug.Log("Touch began on this object");
    }

    public void OnTouchEnd(TouchData touchData) {
        Debug.Log("Touch ended on object");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
