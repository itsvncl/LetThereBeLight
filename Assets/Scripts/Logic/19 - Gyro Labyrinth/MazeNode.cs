using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeNode
{
    private bool isVisited = false;
    private MazeNode rightNeighbour = null;
    private MazeNode leftNeighbour = null;
    private MazeNode topNeighbour = null;
    private MazeNode bottomNeighbour = null;

    private int xIndex;
    private int yIndex;

    public MazeNode( int x, int y) {
        this.xIndex = x;
        this.yIndex = y;
    }

    public MazeNode RightNeighbour { get { return rightNeighbour; } set { rightNeighbour = value; } }
    public MazeNode LeftNeighbour { get { return leftNeighbour; } set { leftNeighbour = value; } }
    public MazeNode TopNeighbour { get { return topNeighbour; } set { topNeighbour = value; } }
    public MazeNode BottomNeighbour { get { return bottomNeighbour; } set { bottomNeighbour = value; } }
    public bool IsVisited { get { return isVisited; } set { isVisited = value; } }
    public int XIndex { get { return xIndex; } }
    public int YIndex { get { return yIndex; } }
}
