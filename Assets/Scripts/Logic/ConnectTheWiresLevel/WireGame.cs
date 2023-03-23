using UnityEngine;

public class WireGame : MonoBehaviour
{
    private static WireGame Instance;
    [SerializeField] private int WireCount = 4;

    public static int ConnectedWireCount { get; private set; }

    private void Start() {
        Instance = this;
        ConnectedWireCount = 0;
    }

    public static void WireConnected() {
        ConnectedWireCount++;

        if(ConnectedWireCount == Instance.WireCount) {
            try {
                LevelManager.Instance.LevelComplete();
            } catch {
                Debug.Log("LevelManager not inited");
            }
        }
    }

    public void SetWireCount(int count) {
        WireCount = count;
    }
}
