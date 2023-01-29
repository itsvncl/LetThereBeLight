using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject levelSelector;

    public void StartGame() {
        //Load the last saved level



        //Placeholder:
        LevelManager.Instance.SwitchToLevel(1);
    }

    public void OpenLevelSelector() {
        mainMenu.SetActive(false);
        levelSelector.SetActive(true);
    }

    public void CloseLevelSelector() {
        levelSelector.SetActive(false);
        mainMenu.SetActive(true);
    }
}
