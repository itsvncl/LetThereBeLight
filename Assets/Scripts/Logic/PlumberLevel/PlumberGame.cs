using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlumberGame : MonoBehaviour {
    [SerializeField] private int SIZE;
    [SerializeField] private float PADDING;
    [SerializeField] private float Y_OFFSET;
    [SerializeField] private GameObject CELL_PREFAB;
    [SerializeField] private GameObject BendingPipePrefab;
    [SerializeField] private GameObject StraightPipePrefab;

    [SerializeField] private int startPos;
    [SerializeField] private int endPos;

    [SerializeField] private List<PipeType> pipeLayout;

    private List<List<Pipe>> board = new();

    private Pipe endPipe;

    private void Start() {
        GenerateBoard();
        RevealRoute();
    }

    void GenerateBoard() {
        float boardAreaSize = ScreenManager.SM.ScreenWidth - 2 * PADDING;

        float cellSize = boardAreaSize / SIZE;
        float startPoint = -(boardAreaSize / 2.0f) + cellSize / 2.0f;

        int idx = 0;

        for (int y = SIZE; y > 0; y--) {
            List<Pipe> row = new List<Pipe>();
            for (int x = 1; x < SIZE - 1; x++) {
                GameObject prefab = pipeLayout[idx] == PipeType.Straight ? StraightPipePrefab : BendingPipePrefab;

                GameObject newCellGo = Instantiate(prefab, new Vector3(startPoint + cellSize * x, startPoint + cellSize * y + Y_OFFSET, 0), Quaternion.identity);
                newCellGo.transform.localScale = new Vector3(cellSize, cellSize, 1);

                idx++;
                Pipe pi = newCellGo.GetComponent<Pipe>();
                pi.game = this;
                row.Add(pi);
            }
            board.Add(row);
        }

        GameObject newCell = Instantiate(StraightPipePrefab, new Vector3(startPoint + cellSize * 0, startPoint + cellSize * (SIZE - startPos) + Y_OFFSET, 0), Quaternion.identity);
        newCell.transform.localScale = new Vector3(cellSize, cellSize, 1);
        Pipe p = newCell.GetComponent<Pipe>();
        p.LockOrientation();
        p.SetOrientation(PipeOrientation.Right);
        p.game = this;
        p.SetHighlighted(true);

        newCell = Instantiate(StraightPipePrefab, new Vector3(startPoint + cellSize * (SIZE - 1), startPoint + cellSize * (SIZE - endPos) + Y_OFFSET, 0), Quaternion.identity);
        newCell.transform.localScale = new Vector3(cellSize, cellSize, 1);
        p = newCell.GetComponent<Pipe>();
        p.LockOrientation();
        p.SetOrientation(PipeOrientation.Right);
        p.game = this;
        p.SetHighlighted(false);

        endPipe = p;
    }



    private void routeRecursion(Pipe pipe, int row, int col) {
        pipe.SetHighlighted(true);

        var directions = PipeGameUtil.PU.GetDirections();

        foreach (var direction in directions[pipe.type][pipe.orientation]) {
            switch (direction) {
                case PipeOrientation.Up:
                    if (row > 0) {
                        var upPipe = board[row - 1][col];

                        if (upPipe.IsHighlighted == false && directions[upPipe.type][upPipe.orientation].Contains(PipeOrientation.Down)) {
                            routeRecursion(upPipe, row - 1, col);
                        }
                    }
                    break;
                case PipeOrientation.Down:
                    if (row < SIZE - 1) {
                        var downPipe = board[row + 1][col];

                        if (downPipe.IsHighlighted == false && directions[downPipe.type][downPipe.orientation].Contains(PipeOrientation.Up)) {
                            routeRecursion(downPipe, row + 1, col);
                        }
                    }
                    break;
                case PipeOrientation.Left:
                    if (col > 0) {
                        var leftPipe = board[row][col - 1];

                        if (leftPipe.IsHighlighted == false && directions[leftPipe.type][leftPipe.orientation].Contains(PipeOrientation.Right)) {
                            routeRecursion(leftPipe, row, col - 1);
                        }
                    }
                    break;
                case PipeOrientation.Right:
                    if (col < SIZE - 3) {
                        var rightPipe = board[row][col + 1];

                        if (rightPipe.IsHighlighted == false && directions[rightPipe.type][rightPipe.orientation].Contains(PipeOrientation.Left)) {
                            routeRecursion(rightPipe, row, col + 1);
                        }
                    }
                    break;
            }
        };
    }
    public void RevealRoute() {
        var pipe = board[startPos][0];

        foreach (var list in board) {
            foreach (var p in list) {
                p.SetHighlighted(false);
            }
        }

        if (pipe.type == PipeType.Straight && (pipe.orientation == PipeOrientation.Left || pipe.orientation == PipeOrientation.Right)) {
            routeRecursion(pipe, startPos, 0);
        } else if (pipe.type == PipeType.Bending && pipe.orientation == PipeOrientation.Down || pipe.orientation == PipeOrientation.Left) {
            routeRecursion(pipe, startPos, 0);
        }
    }

    public bool IsWin() {
        bool win = board[endPos][SIZE - 3].inFlow;

        endPipe.SetHighlighted(win);

        return win;
    }
}
