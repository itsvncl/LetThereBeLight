using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathyrinthController : MonoBehaviour
{
    [SerializeField] private MazeGenerator generator;

    bool reset = false;

    void Start()
    {
        generator.Generate();
    }

    public void ResetGame() {
        if (reset) return;

        reset = true;
        generator.ResetGame();
        generator.Generate();


        reset = false;
    }
}
