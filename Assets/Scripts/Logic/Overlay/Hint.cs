using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hint : MonoBehaviour
{
    [SerializeField] private GameObject hintWindow;
    [SerializeField] private GameObject noButton;
    [SerializeField] private GameObject yesButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
        TouchSystem.Instance.Disable();
    }
}
