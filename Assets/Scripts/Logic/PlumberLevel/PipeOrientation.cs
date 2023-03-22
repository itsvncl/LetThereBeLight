using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PipeOrientation {
    Up, Right, Down, Left,
}

static class PipeOrientationMethods {

    public static PipeOrientation[] GetOrientations() {
        return new PipeOrientation[] {PipeOrientation.Up, PipeOrientation.Down, PipeOrientation.Left, PipeOrientation.Right};
    }
}
