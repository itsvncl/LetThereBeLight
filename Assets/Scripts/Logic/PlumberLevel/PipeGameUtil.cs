using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeGameUtil : MonoBehaviour
{
    public static PipeGameUtil PU;

    private Dictionary<PipeType, Dictionary<PipeOrientation, List<PipeOrientation>>> _directions;

    public Dictionary<PipeType, Dictionary<PipeOrientation, List<PipeOrientation>>> Directions { get { return _directions; } }

    void Awake() {
        PU = this;

        
        //Initing directions
        Dictionary<PipeOrientation, List<PipeOrientation>> bendDirections = new() {
            { PipeOrientation.Up, new List<PipeOrientation>() { PipeOrientation.Up, PipeOrientation.Right } },
            { PipeOrientation.Right, new List<PipeOrientation>() { PipeOrientation.Right, PipeOrientation.Down } },
            { PipeOrientation.Down, new List<PipeOrientation>() { PipeOrientation.Left, PipeOrientation.Down } },
            { PipeOrientation.Left, new List<PipeOrientation>() { PipeOrientation.Up, PipeOrientation.Left } },
        };

        Dictionary<PipeOrientation, List<PipeOrientation>> straightDirections = new() {
            { PipeOrientation.Up, new List<PipeOrientation>() { PipeOrientation.Up, PipeOrientation.Down } },
            { PipeOrientation.Right, new List<PipeOrientation>() { PipeOrientation.Right, PipeOrientation.Left } },
            { PipeOrientation.Down, new List<PipeOrientation>() { PipeOrientation.Up, PipeOrientation.Down } },
            { PipeOrientation.Left, new List<PipeOrientation>() { PipeOrientation.Right, PipeOrientation.Left } },
        };

        _directions = new() {
            { PipeType.Bending, bendDirections},
            { PipeType.Straight, straightDirections}
        };
    }
}
