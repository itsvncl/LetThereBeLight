using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowGame : MonoBehaviour {
    public enum FlowColor {
        Red, Green, Blue, Orange, Yellow, LightBlue, Pink
    }

    public static FlowGame Instance { get; private set; }

    [SerializeField] private Cursor Cursor;

    public FlowCell CurrentCell;
    public FlowCell PreviousCell;
    public List<FlowCell> Cells;
    public FlowColor CursorColorID;
    public Color CursorColor;
    public bool CanDraw;
    public bool Lock = false;

    public Dictionary<FlowColor, List<FlowCell>> routes = new();

    void Awake() {
        Instance = this;
        InitRoutes();
    }

    public void SetCursorColor(FlowColor colorID) {
        CursorColorID = colorID;
        Color c = IDToColor(colorID);
        CursorColor = new Color(c.r, c.g, c.b, 0.3f);

        Cursor.SetColor(CursorColor);
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

    public void InitRoutes() {
        foreach (FlowColor color in Enum.GetValues(typeof(FlowColor))) {
            routes.Add(color, new List<FlowCell>());
        }
    }

    public void AddToRoute(FlowCell cell) {
        routes[cell.GetColorID()].Add(cell);
    }

    public void RemoveFromRoute(FlowCell cell) {
        List<FlowCell> route = routes[cell.GetColorID()];

        for (int i = route.Count - 1; i > 0; i--) {
            route[i].FreeCell();
            if (route[i] == cell) {
                route.RemoveAt(i);
                return;
            }
            route.RemoveAt(i);
        }
    }

    public void CleanRoute(FlowCell cell) {
        List<FlowCell> route =  routes[cell.GetColorID()];

        if(cell.Origin != null) {
            foreach( var c in route) {
                c.FreeCell();
            }
            route.Clear();
            return;
        }
        
        for (int i = route.Count - 1; i > 0; i--) {
            if (route[i] == cell) {
                return;
            }
            route[i].FreeCell();
            route.RemoveAt(i);
        }

        foreach(var c in route) {
            Debug.Log(c.Row + " " + c.Col);
        }
    }

    public bool InRoute(FlowCell cell) {
        List<FlowCell> route = routes[cell.GetColorID()];

        return route.Contains(cell);
    }

    public void CheckForWin() {
        int origin = 0;
        int connectedOrigin = 0;
        foreach(var cell in Cells) {
            if (cell.IsEmpty()) return;

            if(cell.Origin != null) {
                origin++;
                if(cell.Connect != null) {
                    connectedOrigin++;
                }
            }
        }

        if(connectedOrigin *2 >= origin) {
            OnWin();
        }
    }
    public void OnWin() {
        Lock = true;
        LevelManager.Instance.LevelComplete();
    }

    void DebugPrintRoute() {

    }
}
