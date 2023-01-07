using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowEntity{
    public enum FlowType {
        Origin, Connection
    }

    public FlowEntity(FlowType type, FlowGame.FlowColor colorID) {
        Type = type;
        ColorID = colorID;
    } 

    private FlowGame.FlowColor _colorID;
    public FlowType Type { get; set; }
    public FlowGame.FlowColor ColorID { get { return _colorID; } set { SetColor(value); } }
    public Color Color { get; private set; }

    private void SetColor(FlowGame.FlowColor colorID) {
        _colorID = colorID;
        Color = FlowGame.IDToColor(colorID);
    }
}
