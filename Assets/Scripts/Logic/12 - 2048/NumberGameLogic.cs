using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberGameLogic : MonoBehaviour {

    [SerializeField] private TouchUtil touchUtil;
    [SerializeField] private GameObject number;
    [SerializeField] private float lockTime;
    [SerializeField] private int winTier;
    [SerializeField] private Collider2D retryButton;
    [SerializeField] private Collider2D playArea;

    public List<GameObject> Board = new List<GameObject>(16);

    public static float[] validPos = { -1.5f, -0.5f, 0.5f, 1.5f };
    private bool boardLocked = false;

    // Start is called before the first frame update
    void Start() {
        boardLocked = false;

        for (int i = 0; i < 16; i++) {
            Board.Add(null);
        }

        //Adding the 2 starter numbers
        CreateNewNumber();
        CreateNewNumber();
    }

    // Update is called once per frame
    void Update() {

        bool touchCompleted = touchUtil.TouchCompleted;

        //Retry button
        if (touchCompleted && !boardLocked && touchUtil.IsTouching(retryButton) && touchUtil.SwipeDistance < 20) {
            Reset();

            return;
        }
        //Play
        if (touchCompleted && !boardLocked && touchUtil.BeaganTouching(playArea) && touchUtil.SwipeDistance > 70) {
            if (!IsValidMove()) return;

            //Move numbers
            MoveNumbers();
            StartCoroutine(PlayTurn(lockTime));
        }
    }

    private bool IsWin() {

        foreach (var go in Board) {
            NumberBlock num = go?.GetComponent<NumberBlock>();
            if (num == null) continue;
            if (num.Tier == winTier) return true;
        }
        return false;
    }

    private void OnWin() {
        Debug.Log("Game won");

        boardLocked = true;
        LevelManager.Instance.LevelComplete();
    }

    private void Reset() {
        foreach (var go in Board) {
            Destroy(go);
        }

        Board.Clear();

        Start();
    }

    private Vector3 indexToPos(int index) {
        Vector3 pos = new Vector3(validPos[index % 4], validPos[3 - index / 4], -1);

        return pos;
    }

    private bool IsValidMove() {

        foreach (var go in Board) {
            NumberBlock num = go?.GetComponent<NumberBlock>();
            if (num == null) continue;
            if (num.HasDestionation(touchUtil.SwipeDirection)) return true;
        }

        return false;
    }

    private void CreateNewNumber() {
        List<int> freePos = FreePositions();

        int posIndex = freePos[Random.Range(0, freePos.Count)];

        Board[posIndex] = Instantiate(number, indexToPos(posIndex), Quaternion.identity);
    }

    private List<int> FreePositions() {
        List<int> freePos = new List<int>();

        for (int i = 0; i < 16; i++) {
            if (Board[i] == null) {
                freePos.Add(i);
            }
        }

        return freePos;
    }

    //Depending on the direction, the objects start 
    void MoveNumbers() {

        if (true) {
            foreach (var number in Board) {
                if (number != null) {
                    number.gameObject.GetComponent<NumberBlock>().MoveInDirection(touchUtil.SwipeDirection);
                }
            }
        }
    }

    void SyncPositions() {
        List<GameObject> newBoard = new List<GameObject>(16);

        for (int i = 0; i < 16; i++) {
            newBoard.Add(null);
        }

        foreach (var number in Board) {
            if (number == null) continue;

            int index = number.GetComponent<NumberBlock>().LocationIndex;
            newBoard[index] = number;
        }

        Board = newBoard;
    }

    private bool IsMoving() {
        foreach (var number in Board) {
            if (number != null) {
                if(number.gameObject.GetComponent<NumberBlock>().Dir != NumberBlock.Direction.Stationary) {
                    return true;
                }
            }
        }
        return false;
    }

    IEnumerator PlayTurn(float time) {
        boardLocked = true;

        yield return new WaitForSeconds(time);

        while (IsMoving()) {
            Debug.Log("Was moving");
            yield return new WaitForSeconds(0.05f);
        }

        SyncPositions();
        CreateNewNumber();


        boardLocked = false;

        if (IsWin()) {
            OnWin();
        }
    }
}
