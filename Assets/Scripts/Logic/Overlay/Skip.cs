using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skip : MonoBehaviour
{
    public void SkipLevel() {
        LevelManager.Instance.LevelComplete();
    }
}
