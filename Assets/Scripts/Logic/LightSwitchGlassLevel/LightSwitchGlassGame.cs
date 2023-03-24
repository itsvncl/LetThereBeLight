using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitchGlassGame : MonoBehaviour
{
    [SerializeField] public Sprite[] glassTextures;
    [SerializeField] public GameObject glass;
    [SerializeField] public int tapPerLevel = 10;
    [SerializeField] public int levels = 4;

    public int tapCount = 0;
    public int currentLevel = 0;
    public int maxTaps;
    public SpriteRenderer glassRenderer;

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
