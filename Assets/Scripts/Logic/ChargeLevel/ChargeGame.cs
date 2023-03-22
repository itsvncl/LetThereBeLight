using UnityEngine;

public class ChargeGame : MonoBehaviour
{
    bool win = false;
    public GameObject charge;
    public GameObject fullCharge;

    void FixedUpdate()
    {
        if (win) return;

        if( ((int)Time.time) % 2 == 0) {
            charge.SetActive(false);
        }
        else {
            charge.SetActive(true);
        }

        if(SystemInfo.batteryStatus == BatteryStatus.Charging) {
            charge.SetActive(false);
            fullCharge.SetActive(true);
            win = true;
            LevelManager.Instance.LevelComplete();
        }
    }
}
