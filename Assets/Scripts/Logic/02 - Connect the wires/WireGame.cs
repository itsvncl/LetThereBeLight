using UnityEngine;

public class WireGame : MonoBehaviour
{
    private static WireGame Instance;
    [SerializeField] private int WireCount = 4;

    public static int ConnectedWireCount { get; private set; }

    private void Start() {
        Instance = this;
    }

    public static void WireConnected() {
        ConnectedWireCount++;

        if(ConnectedWireCount >= Instance.WireCount) {
            LevelManager.Instance.LevelComplete();
        }
    }
}
