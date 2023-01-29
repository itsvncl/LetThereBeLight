using UnityEngine;

public class Rubbable : MonoBehaviour, ITouchBeginEvent, ITouchEndEvent, IDraggable
{
    [SerializeField] private int rubCount = 30;

    private Vector3 rubCenter = Vector3.zero;
    private bool positive = true;

    public void OnDrag(TouchData touchData) {
        if(positive && touchData.Position.y > rubCenter.y) {
            rubCount--;
            positive = false;
        }else if (!positive && touchData.Position.y < rubCenter.y) {
            rubCount--;
            positive= true;
        }
    }

    public void OnTouchBegin(TouchData touchData) {
        rubCenter = touchData.Position;
    }

    public void OnTouchEnd(TouchData touchData) {
        
    }
}
