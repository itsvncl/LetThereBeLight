using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class PremissionRequest : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.HasKey("permissionsRequested")) return;

        StartCoroutine(RequestPermissions());
    }

    IEnumerator RequestPermissions()
    {
        if (!Permission.HasUserAuthorizedPermission("android.permission.READ_EXTERNAL_STORAGE"))
        {
            Permission.RequestUserPermission("android.permission.READ_EXTERNAL_STORAGE");
        }

        yield return new WaitForSeconds(0.5f);

        if (!Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS"))
        {
            Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS");
        }

        yield return new WaitForSeconds(0.5f);

        if (!Permission.HasUserAuthorizedPermission("android.permission.READ_MEDIA_IMAGES"))
        {
            Permission.RequestUserPermission("android.permission.READ_MEDIA_IMAGES");
        }

        PlayerPrefs.SetInt("permissionsRequested", 1);
    }
}
