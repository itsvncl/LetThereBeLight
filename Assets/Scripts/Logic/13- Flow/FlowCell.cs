using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowCell : MonoBehaviour {
    [SerializeField] private FlowCell LeftSibling;
    [SerializeField] private FlowCell RightSibling;
    [SerializeField] private FlowCell TopSibling;
    [SerializeField] private FlowCell BottomSibling;

    private SpriteRenderer originRenderer;
    private SpriteRenderer connectRenderer;

    private FlowEntity origin;
    private FlowEntity connect;

    [SerializeField] private int _row;
    [SerializeField] private int _col;
    public int Row { get { return _row; } set { _row = value; } }
    public int Col { get { return _col; } set { _col = value; } }

    public FlowEntity Origin {
        get { return origin; }
        set {
            this.origin = value;
            originRenderer.sprite = GetSprite(origin.Type);
            originRenderer.color = origin.Color;
        }
    }
    public FlowEntity Connect {
        get { return connect; }
        set {
            this.connect = value;

            if (value == null) {
                connectRenderer.sprite = null;
                return;
            }

            connectRenderer.sprite = GetSprite(connect.Type);
            connectRenderer.color = connect.Color;
            //The radius is 30, so an extra 30 is necessary for smooth connection
            connectRenderer.size = new Vector2(1.3f, 1.0f);
        }
    }

    void Awake() {
        originRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        connectRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        FlowGame.Instance.CurrentCell = this;

        if (connect != null) {
            FlowGame.Instance.CanDraw = true;
            FlowGame.Instance.CursorColor = connect.Color;
            CleanRoute(this);
        }

        if (origin != null) {
            FlowGame.Instance.CanDraw = true;
            FlowGame.Instance.CursorColor = origin.Color;
            CleanRoute(this);
        }

        if (FlowGame.Instance.PreviousCell == null) {
            FlowGame.Instance.PreviousCell = this;
            return;
        }

        if (FlowGame.Instance.CanDraw) {
            FlowCell previousCell = FlowGame.Instance.PreviousCell;

            if(Connect != null || Origin != null) CleanRoute(this);

            if (IsCross(previousCell)) {
                OnCross();
            }
            else {
                CreateConnection(previousCell, this);
            }

            FlowGame.Instance.PreviousCell = this;
        }
    }

    Sprite GetSprite(FlowEntity.FlowType type) {
        return type == FlowEntity.FlowType.Origin ? FlowLoader.FL.OriginSprite : FlowLoader.FL.FlowSprite;
    }
    public void GetSiblings() {
        var cells = FlowGame.Instance.Cells;
        int size = (int)Mathf.Sqrt(cells.Count);

        if (Row == 0) {
            TopSibling = null;
        }
        else {
            TopSibling = cells[size * (Row - 1) + Col];
        }

        if (Row == size - 1) {
            BottomSibling = null;
        }
        else {
            BottomSibling = cells[size * (Row + 1) + Col];
        }

        if (Col == 0) {
            LeftSibling = null;
        }
        else {
            LeftSibling = cells[size * Row + Col - 1];
        }

        if (Col == size - 1) {
            RightSibling = null;
        }
        else {
            RightSibling = cells[size * Row + Col + 1];
        }
    }

    //Ez nem jó, vezetni kell, hogy mi után mi következett egy tömbbe a FlowGame-be
    void CleanRouteRecursion(FlowCell cell, FlowCell previousCell, List<FlowCell> cellsToFree, bool originStart) {
        if (cell.Origin == null && cell.Connect == null) return;

        Color color;

        if (previousCell.Origin != null) color = previousCell.Origin.Color;
        else color = previousCell.Connect.Color;

        if (originStart || cell.Connect != null && cell.Connect.Color.Equals(color)) {
            Debug.Log("Igen:)");
            cellsToFree.Add(cell);
            if (previousCell != cell.RightSibling) CleanRouteRecursion(cell.RightSibling, cell, cellsToFree, false);
            if (previousCell != cell.LeftSibling) CleanRouteRecursion(cell.LeftSibling, cell, cellsToFree, false);
            if (previousCell != cell.TopSibling) CleanRouteRecursion(cell.TopSibling, cell, cellsToFree, false);
            if (previousCell != cell.BottomSibling) CleanRouteRecursion(cell.BottomSibling, cell, cellsToFree, false);
        }

        if (cell.Origin != null && cell.Origin.Color.Equals(color)) {
            Debug.Log("Whytf");
            cellsToFree.Clear();
        }

        return;
    }
    void CleanRoute(FlowCell cell) {
        //Vencel, ha kör keletkezne húzás közeben, akkor azt még a create connection elõtt cleanRoutold;
        List<FlowCell> cellsToFree = new List<FlowCell>();
        bool isOriginStart = Origin != null;

        CleanRouteRecursion(cell.RightSibling, cell, cellsToFree, isOriginStart);
        if (cellsToFree.Count == 0) CleanRouteRecursion(cell.LeftSibling, cell, cellsToFree, isOriginStart);
        if (cellsToFree.Count == 0) CleanRouteRecursion(cell.TopSibling, cell, cellsToFree, isOriginStart);
        if (cellsToFree.Count == 0) CleanRouteRecursion(cell.BottomSibling, cell, cellsToFree, isOriginStart);

        Debug.Log(cellsToFree.Count);

        foreach (FlowCell c in cellsToFree) {
            FreeCell(c);
        }
    }

    void FreeCell(FlowCell cell) {
        cell.Connect = null;
    }

    void CreateConnection(FlowCell previousCell, FlowCell currentCell) {
        if (previousCell == null) {
            return;
        }

        Vector3 newPos = Vector3.zero;
        bool rotate = false;

        if (previousCell.Row < currentCell.Row) {
            newPos.y += 0.5f;
            rotate = true;
        }
        if (previousCell.Row > currentCell.Row) {
            newPos.y -= 0.5f;
            rotate = true;
        }
        if (previousCell.Col < currentCell.Col) {
            newPos.x -= 0.5f;
        }
        if (previousCell.Col > currentCell.Col) {
            newPos.x += 0.5f;
        }

        currentCell.connectRenderer.gameObject.transform.localPosition = newPos;
        currentCell.connectRenderer.gameObject.transform.localRotation = rotate ? Quaternion.Euler(0, 0, 90) : Quaternion.identity;

        currentCell.Connect = new FlowEntity(FlowEntity.FlowType.Connection, FlowGame.Instance.CursorColor);
    }
    bool IsCross(FlowCell other) {
        return other.Row != Row && other.Col != Col;
    }
    void OnCross() {
        FlowCell previousCell = FlowGame.Instance.PreviousCell;
        List<FlowCell> board = FlowGame.Instance.Cells;

        FlowCell skippedCell = null;

        //For indexing
        int size = (int)Mathf.Sqrt(FlowGame.Instance.Cells.Count);
        //Top left from previous
        if (previousCell.Row < Row && previousCell.Col > Col) {
            skippedCell = board[Row * size + Col + 1];
        }
        //Top right from previous
        if (previousCell.Row < Row && previousCell.Col < Col) {
            skippedCell = board[Row * size + Col - 1];
        }
        //Bottom left from previous
        if (previousCell.Row > Row && previousCell.Col > Col) {
            skippedCell = board[Row * size + Col + 1];
        }
        //Bottom right from previous
        if (previousCell.Row > Row && previousCell.Col < Col) {
            skippedCell = board[Row * size + Col - 1];
        }

        CreateConnection(previousCell, skippedCell);
        CreateConnection(skippedCell, this);
    }


}
