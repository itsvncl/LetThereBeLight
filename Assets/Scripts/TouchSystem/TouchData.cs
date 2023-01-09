using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchData
{
    public Vector3 Position { get; private set; }
    public Vector3 BeginPosition { get; private set; }
    public Vector3 EndPosition { get; private set; }
    public int TapCount { get; private set; }
    public int FingerID { get; private set; }
    
    //Represents the initially touched gameobject
    public GameObject gameObject { get; private set; }

    public TouchData(Vector3 position, int fingerID, GameObject gameObject, int tapCount) {
        BeginPosition = position;
        Position = position;
        FingerID = fingerID;
        TapCount = tapCount;
        this.gameObject = gameObject;
    }

    public TouchData(Vector3 position, Vector3 beginPosition, Vector3 endPosition, int fingerID, GameObject gameObject, int tapCount) {
        BeginPosition = beginPosition;
        EndPosition = endPosition;
        Position = position;
        FingerID = fingerID;
        TapCount = tapCount;
        this.gameObject = gameObject;
    }
    public TouchData(Vector3 position, Vector3 beginPosition, int fingerID, GameObject gameObject, int tapCount) {
        Position = position;
        BeginPosition = beginPosition;
        FingerID = fingerID;
        TapCount = tapCount;
        this.gameObject = gameObject;
    }
}
