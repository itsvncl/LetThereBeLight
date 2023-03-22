using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Localization;

public class NotificationGame : MonoBehaviour
{
    [SerializeField] private LocalizedString notificationTitle;
    [SerializeField] private LocalizedString notificationText;

    void Start()
    {
        if (!Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS")) {
            Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS");
        }

        if (!Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS")) {
            Debug.Log("Baj van");
        }

        var channel = new AndroidNotificationChannel() {
            Id = "LetThereBeLight",
            Name = "Game",
            Importance = Importance.Low,
            Description = "Game completion notification",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);

        var notification = new AndroidNotification();
        notification.Title = notificationTitle.GetLocalizedString();
        notification.Text = notificationText.GetLocalizedString();
        notification.IntentData = "LevelCompletionTrigger";
        notification.FireTime = System.DateTime.Now;
        notification.ShouldAutoCancel = true;

        AndroidNotificationCenter.SendNotification(notification, "LetThereBeLight");
    }

    public void Win(string s1) {
        LevelManager.Instance.LevelComplete();
    }

    private void OnDestroy() {
        AndroidNotificationCenter.CancelAllNotifications();
        AndroidNotificationCenter.CancelAllDisplayedNotifications();
    }
}
