using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour, IDraggable, ITouchBeginEvent, ITouchEndEvent
{
    public void OnDrag(TouchData touchData) {
        Debug.Log(touchData.FingerID);
    }

    public void OnTouchBegin(TouchData touchData) {
        throw new System.NotImplementedException();
    }

    public void OnTouchEnd(TouchData touchData) {
        throw new System.NotImplementedException();
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
