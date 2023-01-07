using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowGame : MonoBehaviour
{
    public static FlowGame Instance { get; private set; }

    public FlowCell CurrentCell { get; set; }
    public FlowCell PreviousCell { get; set; }
    public List<FlowCell> Cells { get; set; }
    public Color CursorColor { get; set; }
    public bool CanDraw { get; set; }


    void Awake()
    {
        Instance = this;
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
