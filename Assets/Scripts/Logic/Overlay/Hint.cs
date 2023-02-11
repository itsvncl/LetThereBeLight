using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hint : MonoBehaviour
{
    [SerializeField] private GameObject hintWindow;
    [SerializeField] private GameObject question;
    [SerializeField] private GameObject hint;

    bool gotHint = false;

    void Update() {
        if (Application.platform == RuntimePlatform.Android) {
            if (Input.GetKey(KeyCode.Escape)) {
                hintWindow.SetActive(false);
                TouchSystem.Instance.Enable();
            }
        }
    }

    public void OnNoClick() {
        hintWindow.SetActive(false);
        TouchSystem.Instance.Enable();
    }

    public void HintClick() {
        hintWindow.SetActive(true);

        if (gotHint) {
            question.SetActive(false);
            hint.SetActive(true);
        }
        else {
            question.SetActive(true);
            hint.SetActive(false);  
        }

        TouchSystem.Instance.Disable();
    }

    public void OnYesClick() {
        gotHint = true;
        question.SetActive(false);
        hint.SetActive(true);
    }
}
