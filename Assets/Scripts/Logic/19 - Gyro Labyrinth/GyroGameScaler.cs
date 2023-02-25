using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroGameScaler : MonoBehaviour
{
    void Start()
    {
        float width = ScreenManager.SM.ScreenWidth;
        float scale = Mathf.Clamp(width / 5.63f, 0.1f, 1.0f);

        transform.localScale = new Vector3(scale, scale, 1.0f);
    }
}
