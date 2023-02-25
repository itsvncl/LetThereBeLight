using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintGame : MonoBehaviour
{
    [SerializeField] int gameSize = 4;
    [SerializeField] GameObject slicePrefab;

    private void Start() {
        GenerateSlices();
    }

    private void GenerateSlices() {
        float sliceWidth = ScreenManager.SM.ScreenWidth / gameSize;
        float sliceHeight = ScreenManager.SM.ScreenHeight / gameSize;

        for(int i = 0; i < gameSize; i++) {
            for(int j = 0; j < gameSize; j++) {
                GameObject newSlice = Instantiate(slicePrefab, Vector3.zero, Quaternion.identity);


                newSlice.transform.localScale = new Vector3(sliceWidth, sliceHeight, 1);
            }
        }
    }
}
