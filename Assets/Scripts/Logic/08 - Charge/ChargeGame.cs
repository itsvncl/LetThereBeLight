using UnityEngine;

public class ChargeGame : MonoBehaviour
{
    bool win = false;

    void FixedUpdate()
    {
        if(SystemInfo.batteryStatus == BatteryStatus.Charging && !win) {
            win = true;
            LevelManager.Instance.LevelComplete();
        }
    }
}
