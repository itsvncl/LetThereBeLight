using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class PremissionRequest : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.HasKey("permissionsRequested")) return;

        if (!Permission.HasUserAuthorizedPermission("android.permission.READ_EXTERNAL_STORAGE")) {
            Permission.RequestUserPermission("android.permission.READ_EXTERNAL_STORAGE");
        }

        if (!Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS")) {
            Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS");
        }

        PlayerPrefs.SetInt("permissionsRequested", 1);
    }
}