using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberGameLogic : MonoBehaviour {

    [SerializeField] private TouchUtil touchUtil;
    [SerializeField] private GameObject number;
    [SerializeField] private float lockTime;

    public List<GameObject> Board = new List<GameObject>(16);

    public static float[] validPos = { -1.5f, -0.5f, 0.5f, 1.5f };
    private bool boardLocked = false;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 16; i++) {
            Board.Add(null);
        }

        //Adding the 2 starter numbers
        CreateNewNumber();
        CreateNewNumber();
    }

    // Update is called once per frame
    void Update()
    {
        if (touchUtil.TouchCompleted && !boardLocked && touchUtil.SwipeDistance > 70) {
            //Move numbers
            MoveNumbers();
            StartCoroutine(PlayTurn(lockTime));
        }
    }

    private Vector3 indexToPos(int index) {
        Debug.Log(index + "  " + index % 4 + "  " + (3 - index / 4));

        Vector3 pos = new Vector3(validPos[index%4], validPos[3-index/4], -1);

        return pos;
    }

    private void CreateNewNumber() {
        List<int> freePos = FreePositions();

        int posIndex = freePos[Random.Range(0, freePos.Count)];
        Board[posIndex] = Instantiate(number, indexToPos(posIndex), Quaternion.identity);
    }

    private List<int> FreePositions() {
        List<int> freePos = new List<int>();

        for(int i = 0; i < 16; i++) {
            if(Board[i] == null) {
                freePos.Add(i);
            }
        }

        return freePos;
    }

    void MoveNumbers() {
        foreach(var number in Board){
            if(number != null) {
                number.gameObject.GetComponent<NumberBlock>().MoveInDirection(touchUtil.SwipeDirection);
            }
        }
    }

    void SyncPositions() {
        List<GameObject> newBoard = new List<GameObject>(16);

        for (int i = 0; i < 16; i++) {
            newBoard.Add(null);
        }

        foreach(var number in Board) {
            if (number == null) continue;

            int index = number.GetComponent<NumberBlock>().LocationIndex;
            Debug.Log(index);
            newBoard[index] = number;
        }

        Board = newBoard;
    }

    IEnumerator PlayTurn(float time) {
        boardLocked = true;

        yield return new WaitForSeconds(time);

        SyncPositions();
        CreateNewNumber();
        boardLocked = false;
    }
}
