using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitchGlassGame : MonoBehaviour
{
    [SerializeField] private Sprite[] glassTextures;
    [SerializeField] private GameObject glass;
    [SerializeField] private int tapPerLevel = 10;
    [SerializeField] private int levels = 4;

    private int tapCount = 0;
    private int currentLevel = 0;
    private int maxTaps;
    private SpriteRenderer glassRenderer;

    void Start() {
        maxTaps = levels * tapPerLevel;
        glassRenderer = glass.GetComponent<SpriteRenderer>();
        glassRenderer.sprite = glassTextures[currentLevel];
    }

    public void Tap() {
        tapCount++;

        if(tapCount == maxTaps) {
            glass.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
            return;
        }

        if(Mathf.Floor(tapCount / tapPerLevel) > currentLevel) {
            currentLevel++;
            glassRenderer.sprite = glassTextures[currentLevel];
        }
    }
}
