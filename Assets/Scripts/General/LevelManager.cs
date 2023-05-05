using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public static LevelManager Instance;

    private int maxLevel;
    private int progression = 1;
    private int lastLevel = 1;
    private int currentLevel = 0;

    [SerializeField] private Animator animator;
    [SerializeField] private float transitionTime = 1.5f;

    private void Awake() {

        if (Instance != null && Instance != this) {
            Destroy(this);
            return;
        }
        else {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            Debug.Log("LevelManager inited");
        }

        maxLevel = SceneManager.sceneCountInBuildSettings-1;

        if (PlayerPrefs.HasKey("progression")) {
            progression = PlayerPrefs.GetInt("progression");
        }
        else {
            PlayerPrefs.SetInt("progression", 1);
            progression = 1;
        }

        if (PlayerPrefs.HasKey("lastLevelPlayed")) {
            lastLevel = PlayerPrefs.GetInt("lastLevelPlayed");
        }
        else {
            PlayerPrefs.SetInt("lastLevelPlayed", 1);
            lastLevel = 1;
        }
    }

    IEnumerator Load() {
        Debug.Log("Level loading");
        animator.SetTrigger("FadeOut");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(currentLevel);
        animator.SetTrigger("FadeIn");
    }


    public void LevelComplete() {
        currentLevel++;

        if (currentLevel > progression) {
            progression = currentLevel;
            PlayerPrefs.SetInt("progression", progression);
        }

        if (currentLevel > maxLevel) {
            throw new System.Exception("Level index is out of bounds!");
        }

        if (currentLevel != maxLevel) {
            PlayerPrefs.SetInt("lastLevelPlayed", currentLevel);
            lastLevel = currentLevel;
        }


        StartCoroutine(Load());
    }

    public void SwitchToLevel(int level) {
        if (level > maxLevel) {
            Debug.Log(level);
            throw new System.Exception("Level index is out of bounds!");
        }
        currentLevel = level;

        if (level != 0) {
            PlayerPrefs.SetInt("lastLevelPlayed", level);
            lastLevel = level;
        }

        StartCoroutine(Load());
    }

    public void Continue() {
        if (lastLevel > maxLevel) {
            throw new System.Exception("Level index is out of bounds!");
        }

        SwitchToLevel(lastLevel);
    }

    public int GetMaxLevel() {
        return maxLevel;
    }

    public int GetCurrentLevel() {
        return currentLevel;
    }

}
