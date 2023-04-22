using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Localization;

public class NotificationGame : MonoBehaviour
{
    private bool win = false;
    [SerializeField] private LocalizedString notificationTitle;
    [SerializeField] private LocalizedString notificationText;

    [SerializeField] private GameObject unplayableOverlay;

    void Start()
    {
        if (AndroidActivityManager.getAPILevel() < 33) return;

        if (!Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS"))
        {
            Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS");
        }

        StartCoroutine(InitGame());
    }

    public void sendNotification()
    {
        var channel = new AndroidNotificationChannel()
        {
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
        if (win) return;
        win = true;

        LevelManager.Instance.LevelComplete();
    }

    private void OnDestroy() {
        AndroidNotificationCenter.CancelAllNotifications();
        AndroidNotificationCenter.CancelAllDisplayedNotifications();
    }



    IEnumerator InitGame() {
        yield return new WaitForSeconds(0.5f);

        if (!Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS")) {
            unplayableOverlay.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            sendNotification();
        }
    }
}
