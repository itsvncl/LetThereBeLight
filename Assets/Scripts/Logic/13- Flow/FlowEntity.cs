using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowEntity{
    public enum FlowType {
        Origin, Connection
    }

    public FlowEntity(FlowType type, Color color) {
        Type = type;
        Color = color;
    } 

    public FlowType Type { get; set; }
    public Color Color { get; set; }
}
