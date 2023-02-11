using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private GameObject levelPrefab;
    [SerializeField] private GameObject levelContainer;

    void Start() {
        int completedUntil = PlayerPrefs.GetInt("progression");

        Debug.Log(completedUntil);

        for (int i = 0; i < LevelManager.Instance.GetMaxLevel(); i++) {
            GameObject level = Instantiate(levelPrefab);
            level.transform.SetParent(levelContainer.transform, false);
            
            TextMeshProUGUI text = level.GetComponentInChildren<TextMeshProUGUI>();
            text.text = (i+1).ToString();
            
            Button button = level.GetComponent<Button>();
            button.onClick.AddListener( () => { LevelManager.Instance.SwitchToLevel(int.Parse(text.text)); } );

            if (i >= completedUntil) {
                button.interactable = false;
            }
        }
    }
}
