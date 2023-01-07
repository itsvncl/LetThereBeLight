using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowGame : MonoBehaviour
{
    public enum FlowColor {
        Red, Green, Blue, Orange, Yellow, LightBlue, Pink
    }

    public static FlowGame Instance { get; private set; }

    public FlowCell CurrentCell { get; set; }
    public FlowCell PreviousCell { get; set; }
    public List<FlowCell> Cells { get; set; }
    public FlowColor CursorColorID { get; private set; }
    public Color CursorColor { get; private set; }
    public bool CanDraw { get; set; }

    public Dictionary<FlowColor, List<FlowCell>> routes = new();

    void Awake() {
        Instance = this;
    }


    public void SetCursorColor(FlowEntity cell) {
        CursorColorID = cell.ColorID;
        CursorColor = cell.Color;
    }

    public static Color IDToColor(FlowColor colorID) {
        switch (colorID) {
            case FlowColor.Red: return FlowLoader.FL.Red; 
            case FlowColor.Green: return FlowLoader.FL.Green; 
            case FlowColor.Blue: return FlowLoader.FL.Blue;
            case FlowColor.Orange: return FlowLoader.FL.Orange;
            case FlowColor.Yellow: return FlowLoader.FL.Yellow;
            case FlowColor.LightBlue: return FlowLoader.FL.LightBlue; 
            case FlowColor.Pink: return FlowLoader.FL.Pink;
        }

        return new Color(4, 4, 4);
    }
}
