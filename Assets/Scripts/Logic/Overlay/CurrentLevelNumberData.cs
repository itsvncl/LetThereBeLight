using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentLevelNumberData : MonoBehaviour
{
    public string LevelNumber { get {
            return LevelManager.Instance.GetCurrentLevel().ToString();
        }
    }
}
