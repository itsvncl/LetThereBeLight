using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject levelSelector;
    public GameObject languageSelector;

    public void StartGame() {
        LevelManager.Instance.Continue();
    }

    public void OpenLevelSelector() {
        languageSelector.SetActive(false);
        mainMenu.SetActive(false);
        levelSelector.SetActive(true);
    }

    public void CloseLevelSelector() {
        levelSelector.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void OpenLanguageSelector() {
        languageSelector.SetActive(!languageSelector.activeSelf);
    }

}
