using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using SunCalcNet;
using System;
using UnityEngine.Networking;
using System.Globalization;

public class SunGame : MonoBehaviour {
    [SerializeField] private GameObject sun;
    [SerializeField] private Image lightImage;
    [SerializeField] private float increaseSpeed = 0.5f;
    [SerializeField] private float defaultAzimuth = 264f;
    [SerializeField] private float defaultAltitude = 64f;
    [SerializeField] private float sunDistance = 5.0f;

    [SerializeField] private GameObject unplayableOverlay;

    private Plane[] cameraFrustum;
    private Collider sunCollider;

    float lightLevel = 0.0f;
    bool win = false;

    void Awake() {
        if (!AndoridActivityManager.Instance.DeviceHasMagnetometer()) {
            unplayableOverlay.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    private void Start() {
        sunCollider = sun.GetComponent<Collider>();
        sun.transform.position = AzimuthToVector3(Mathf.Deg2Rad * defaultAzimuth, Mathf.Deg2Rad * defaultAltitude, sunDistance);

        PositionSun();
    }

    public void PositionSun() {
        StartCoroutine(GetGeoLocation());
    }

    IEnumerator GetGeoLocation() {
        var uwr = UnityWebRequest.Get("http://ip-api.com/json");

        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.Success) {
            String resp = uwr.downloadHandler.text;

            String lonString = resp.Substring(resp.IndexOf("\"lon\":") + 6, 7);
            String latString = resp.Substring(resp.IndexOf("\"lat\":") + 6, 7);

            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            provider.NumberGroupSeparator = ",";

            double lon = Convert.ToDouble(lonString, provider);
            double lat = Convert.ToDouble(latString, provider);

            DateTime current = DateTime.Now;

            var sunPos = SunCalc.GetSunPosition(current, lat, lon);

            sun.transform.position = AzimuthToVector3((float)(sunPos.Azimuth + Math.PI), (float)sunPos.Altitude, sunDistance);
        }
    }

    public static Vector3 AzimuthToVector3(float azimuth, float altitude, float distance) {

        float x = distance * (Mathf.Cos(altitude) * Mathf.Sin(azimuth));
        float y = distance * (Mathf.Cos(altitude) * Mathf.Cos(azimuth));
        float z = distance * Mathf.Sin(altitude);

        Vector3 point = new Vector3(x, y, z);

        return point * -1.0f;
    }

    void FixedUpdate()
    {
        var bounds = sunCollider.bounds;
        cameraFrustum = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        if (GeometryUtility.TestPlanesAABB(cameraFrustum, bounds)){
            lightLevel += increaseSpeed;
        }
        else {
            lightLevel -= increaseSpeed;
        }

        if(lightLevel < 0) lightLevel = 0;
    }

    void Update() {
        Color c = lightImage.color;
        c.a = lightLevel;
        lightImage.color = c;

        if (lightLevel >= 1.0f && !win) {
            win = true;
            LevelManager.Instance.LevelComplete();
        }
    }
}
