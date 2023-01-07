using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowCell : MonoBehaviour {
    private SpriteRenderer originRenderer;
    private SpriteRenderer connectRenderer;
    private SpriteRenderer backgroundRenderer;

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


            backgroundRenderer.enabled = true;
            backgroundRenderer.color = new Color(origin.Color.r, origin.Color.g, origin.Color.b, 0.3f);
        }
    }
    public FlowEntity Connect {
        get { return connect; }
        set {
            this.connect = value;

            if (value == null) {
                connectRenderer.sprite = null;

                if (origin == null)
                    backgroundRenderer.enabled = false;
                return;
            }

            connectRenderer.sprite = GetSprite(connect.Type);
            connectRenderer.color = connect.Color;
            //The radius is 30, so an extra 30 is necessary for smooth connection
            connectRenderer.size = new Vector2(1.3f, 1.0f);

            backgroundRenderer.enabled = true;
            backgroundRenderer.color = new Color(connect.Color.r, connect.Color.g, connect.Color.b, 0.3f);
        }
    }

    void Awake() {
        originRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        connectRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>();
        backgroundRenderer = transform.GetChild(2).GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (FlowGame.Instance.Lock) return;

        FlowGame.Instance.CurrentCell = this;
        FlowCell previousCell = FlowGame.Instance.PreviousCell;

        if (previousCell == null) {
            FlowGame.Instance.SetCursorColor(this.GetColorID());
        }
        else {
            FlowGame.Instance.SetCursorColor(previousCell.GetColorID());
        }

        //Initial touch
        if (previousCell == null) {
            if (IsEmpty()) {
                return;
            }

            FlowGame.Instance.PreviousCell = this;
            FlowGame.Instance.CanDraw = true;
            FlowGame.Instance.CleanRoute(this);

            if (!FlowGame.Instance.InRoute(this)) {
                FlowGame.Instance.AddToRoute(this);
            }



            return;
        }

        if (!FlowGame.Instance.CanDraw || OutOfBounds(previousCell)) return;

        //Swipe - Has previusCell
        //If its empty we can connect worry free
        if (IsEmpty()) {

            HandleConnection(previousCell);
            FlowGame.Instance.PreviousCell = this;

            return;
        }

        //If its a flow
        if (connect != null) {
            //Same color
            if (connect.ColorID == previousCell.GetColorID()) {
                FlowGame.Instance.CleanRoute(this);
                FlowGame.Instance.PreviousCell = this;
                return;
            }

            if (origin != null) return;

            //Different color
            FlowGame.Instance.RemoveFromRoute(this);
            HandleConnection(previousCell);
            FlowGame.Instance.PreviousCell = this;

            return;
        }

        //If its an origin
        if (origin != null) {
            //Same color
            if (origin.ColorID == previousCell.GetColorID()) {
                if (FlowGame.Instance.InRoute(this)) {
                    FlowGame.Instance.CleanRoute(this);
                    FlowGame.Instance.AddToRoute(this);
                    FlowGame.Instance.PreviousCell = this;
                }
                else {
                    HandleConnection(previousCell);
                    FlowGame.Instance.PreviousCell = this;
                    FlowGame.Instance.CanDraw = false;
                    FlowGame.Instance.CheckForWin();
                }
            }
        }
    }
    public void HandleConnection(FlowCell previousCell) {
        if (IsCross(previousCell)) {
            OnCross();
        }
        else {
            CreateConnection(previousCell, this);
        }
    }

    Sprite GetSprite(FlowEntity.FlowType type) {
        return type == FlowEntity.FlowType.Origin ? FlowLoader.FL.OriginSprite : FlowLoader.FL.FlowSprite;
    }

    public void FreeCell() {
        Connect = null;
    }

    void CreateConnection(FlowCell previousCell, FlowCell currentCell) {
        if (previousCell == null) {
            return;
        }

        Vector3 newPos = new Vector3(0, 0, -1);
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

        currentCell.Connect = new FlowEntity(FlowEntity.FlowType.Connection, previousCell.GetColorID());

        FlowGame.Instance.AddToRoute(currentCell);
    }

    bool IsCross(FlowCell other) {
        return other.Row != Row && other.Col != Col;
    }
    bool OutOfBounds(FlowCell previousCell) {
        return Mathf.Abs(previousCell.Row - Row) > 1 || Mathf.Abs(previousCell.Col - Col) > 1;
    }
    void OnCross() {
        FlowCell previousCell = FlowGame.Instance.PreviousCell;
        List<FlowCell> board = FlowGame.Instance.Cells;

        FlowCell skippedCell = null;

        //For indexing
        int size = (int)Mathf.Sqrt(FlowGame.Instance.Cells.Count);
        //Bottom left
        if (previousCell.Row < Row && previousCell.Col > Col) {
            skippedCell = board[Row * size + Col + 1];

            if (!skippedCell.IsEmpty()) {
                skippedCell = board[(Row - 1) * size + Col];
            }
        }
        //Bottom right from previous
        if (previousCell.Row < Row && previousCell.Col < Col) {
            skippedCell = board[Row * size + Col - 1];

            if (!skippedCell.IsEmpty()) {
                skippedCell = board[(Row - 1) * size + Col];
            }
        }
        //Top left from previous
        if (previousCell.Row > Row && previousCell.Col > Col) {
            skippedCell = board[Row * size + Col + 1];

            if (!skippedCell.IsEmpty()) {
                skippedCell = board[(Row + 1) * size + Col];
            }
        }

        //Top right from previous
        if (previousCell.Row > Row && previousCell.Col < Col) {
            skippedCell = board[Row * size + Col - 1];

            if (!skippedCell.IsEmpty()) {
                skippedCell = board[(Row + 1) * size + Col];
            }
        }

        if (!skippedCell.IsEmpty()) {
            FlowGame.Instance.CanDraw = false;
            return;
        }

        CreateConnection(previousCell, skippedCell);
        CreateConnection(skippedCell, this);
    }

    public bool IsEmpty() {
        return Origin == null && Connect == null;
    }

    public FlowGame.FlowColor GetColorID() {
        if (Connect != null) return Connect.ColorID;
        else if (Origin != null) return Origin.ColorID;

        return FlowGame.FlowColor.NoColor;
    }

}
