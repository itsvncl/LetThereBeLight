using System.Collections.Generic;
using UnityEngine;

public class MorseGame : MonoBehaviour
{
    public static MorseGame Instance;

    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private GameObject dashPrefab;

    [SerializeField] private float longPressTime = 0.5f;
    [SerializeField] private float morseGap = 0.2f;
    [SerializeField] private int maximumSize = 10;
    public float LongPressTime { get { return longPressTime; } private set { longPressTime = value;  } }

    private int dotCount = 0;
    private int dashCount = 0;

    private bool gameInProgress = true;

    public enum MorseType {
        DOT,DASH
    }

    void Start() {
        Instance = this;    
    }

    private List<MorseType> morseString = new List<MorseType>();
    private List<GameObject> morseObejcts = new List<GameObject>();

    public void AddMorse(MorseType type) {
        if (!gameInProgress) return;

        if(morseString.Count >= maximumSize) {
            ResetGame();
            return;
        }

        morseString.Add(type);

        if(type == MorseType.DOT) {
            GameObject go = Instantiate(dotPrefab, transform.position, Quaternion.identity);
            go.name = dotPrefab.name;
            morseObejcts.Add(go);
            dotCount++;
        }
        else {
            GameObject go = Instantiate(dashPrefab, transform.position, Quaternion.identity);
            go.name = dashPrefab.name;
            morseObejcts.Add(go);
            dashCount++;
        }

        centerCode();

        if (IsWin()) { 
            OnWin();
        }
    }

    void centerCode() {
        float dotInc = dotPrefab.transform.localScale.x / 2.0f;
        float dashInc = dashPrefab.transform.localScale.x / 2.0f;

        float start = (dotCount * -dotPrefab.transform.localScale.x + dashCount * -dashPrefab.transform.localScale.x + (dotCount + dashCount - 1) * -morseGap) / 2.0f;
        float inc = 0;

        foreach (GameObject go in morseObejcts) {
            if (go.name == dotPrefab.name) {
                inc += dotInc;
                go.transform.position = new Vector3(start + inc, go.transform.position.y, go.transform.position.z);
                inc += dotInc + morseGap;
            }
            else {
                inc += dashInc;
                go.transform.position = new Vector3(start + inc, go.transform.position.y, go.transform.position.z);
                inc += dashInc + morseGap;

            }
        }
    }

    public void ResetGame() {
        if (!gameInProgress) return;

        foreach (GameObject go in morseObejcts) {
            Destroy(go, 0);
        }

        morseObejcts.Clear();
        morseString.Clear();

        dotCount = 0;
        dashCount = 0;
    }

    private bool IsWin() {
        if(morseString.Count != 9) return false;

        int index = 0;

        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                if (morseString[index] == MorseType.DOT && i == 1) {
                    return false;
                }
                if (morseString[index] == MorseType.DASH && i != 1) {
                    return false;
                }
                index++;
            }
        }

        return true;
    }

    private void OnWin() {
        gameInProgress = false;
        LevelManager.Instance.LevelComplete();
    }
}
