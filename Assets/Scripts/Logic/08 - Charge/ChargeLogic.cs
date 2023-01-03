using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeLogic : MonoBehaviour
{
    bool win = false;

    void Update()
    {
        if(SystemInfo.batteryStatus == BatteryStatus.Charging && !win) {
            win = true;
            LevelManager.Instance.LevelComplete();
        }
    }
}
