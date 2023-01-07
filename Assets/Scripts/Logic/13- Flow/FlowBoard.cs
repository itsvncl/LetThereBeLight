using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowBoard : MonoBehaviour {

    struct Origin {
        public int index;
        public Color color;
    }

    [SerializeField] private int SIZE;
    [SerializeField] private float PADDING;
    [SerializeField] private float Y_OFFSET;
    [SerializeField] private GameObject CELL_PREFAB;

    private List<FlowCell> cells = new();
    private Origin[] origins;

    void Start() {
        origins = new Origin[]{
        new Origin { index = 3, color = FlowLoader.FL.Yellow },
        new Origin { index = 10, color = FlowLoader.FL.Pink },
        new Origin { index = 18, color = FlowLoader.FL.Orange },
        new Origin { index = 23, color = FlowLoader.FL.Green },
        new Origin { index = 24, color = FlowLoader.FL.Blue },
        new Origin { index = 27, color = FlowLoader.FL.Blue },
        new Origin { index = 28, color = FlowLoader.FL.Pink },
        new Origin { index = 30, color = FlowLoader.FL.Yellow },
        new Origin { index = 45, color = FlowLoader.FL.Orange },
        new Origin { index = 52, color = FlowLoader.FL.Red },
        new Origin { index = 56, color = FlowLoader.FL.Red },
        new Origin { index = 60, color = FlowLoader.FL.Green },
        new Origin { index = 63, color = FlowLoader.FL.LightBlue },
        new Origin { index = 75, color = FlowLoader.FL.LightBlue },
        };

        GenerateBoard();
        AddOrigins();
    }


    void GenerateBoard() {
        float boardAreaSize = ScreenManager.SM.ScreenWidth - 2 * PADDING;

        float cellSize = boardAreaSize / SIZE;
        float startPoint = -(boardAreaSize / 2.0f) + cellSize / 2.0f;

        for (int y = SIZE; y > 0; y--) {
            for (int x = 0; x < SIZE; x++) {
                GameObject newCellGo = Instantiate(CELL_PREFAB, new Vector3(startPoint + cellSize * x, startPoint + cellSize * y + Y_OFFSET, 0), Quaternion.identity);
                newCellGo.transform.localScale = new Vector3(cellSize, cellSize, 1);

                FlowCell newCell = newCellGo.GetComponent<FlowCell>();
                newCell.Row = SIZE - y;
                newCell.Col = x;
                cells.Add(newCell);
            }
        }

        FlowGame.Instance.Cells = cells;
        foreach(FlowCell cell in cells) {
           cell.GetSiblings();
        }
    }

    void AddOrigins() {
        foreach (Origin origin in origins) {
            cells[origin.index].Origin = new(FlowEntity.FlowType.Origin, origin.color);
        }
    }
}
