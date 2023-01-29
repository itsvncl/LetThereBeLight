using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public static LevelManager Instance;

    private int maxLevel = 32;
    private int progression = 0;
    private int currentLevel = 0;

    [SerializeField] private Animator animator;
    [SerializeField] private float transitionTime = 1.5f;

    private void Awake() {

        if (Instance != null && Instance != this) {
            Destroy(this);
        }
        else {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
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

        if (currentLevel > maxLevel) {
            throw new System.Exception("Level index is out of bounds!");
        }

        StartCoroutine(Load());
    }

    public void SwitchToLevel(int level) {
        if (level > maxLevel) {
            Debug.Log(level);
            throw new System.Exception("Level index is out of bounds!");
        }
        currentLevel = level;

        StartCoroutine(Load());
    }

    public void DebugNextLevel() {
        if (currentLevel < maxLevel) {
            currentLevel++;
        }
        SceneManager.LoadScene(currentLevel);
    }

    public void DebugPreviousLevel() {
        if (currentLevel > 0) {
            currentLevel--;
        }
        SceneManager.LoadScene(currentLevel);
    }

    public int GetMaxLevel() {
        return maxLevel;
    }

    public int GetProgression() { return progression; }
}
