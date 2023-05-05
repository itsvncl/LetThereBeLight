using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour {
    [SerializeField] private int size = 10;

    [SerializeField] private int beginPosX = 5;
    [SerializeField] private int beginPosY = 0;
    [SerializeField] private int endPosX = 5;
    [SerializeField] private int endPosY = 9;

    [SerializeField] private GameObject TilePrefab;
    [SerializeField] private GameObject BorderPrefab;
    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private GameObject EndPrefab;

    private GameObject Player;

    private List<List<MazeNode>> nodes = new List<List<MazeNode>>();
    private List<MazeNode> visitedNodesWithUnvisitedNeighbours = new List<MazeNode>();
    private List<GameObject> generatedObjects = new List<GameObject>();

    public void Generate() {
        if (generatedObjects.Count != 0) return;

        InitNodes();
        GenereateMainRoute();
        GenerateSubRoutes();
        PlaceEntities();
        FitToScreen();
    }

    public void ResetGame() {
        nodes.Clear();
        visitedNodesWithUnvisitedNeighbours.Clear();
        
        foreach(var obj in generatedObjects) {
            Destroy(obj);
        }

        generatedObjects.Clear();

        transform.localScale = Vector3.one;
        transform.position = new Vector3(0, 0, -4.0f);
    }

    private void InitNodes() {
        //Generating the nodes
        for (int i = 0; i < size; i++) {
            List<MazeNode> newRow = new List<MazeNode>();

            for (int j = 0; j < size; j++) {
                newRow.Add(new MazeNode(j, i));
            }

            nodes.Add(newRow);
        }

        //Setting the neighbours
        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                MazeNode node = nodes[i][j];

                if (i > 0) node.BottomNeighbour = nodes[i - 1][j];
                if (i < size - 1) node.TopNeighbour = nodes[i + 1][j];
                if (j > 0) node.LeftNeighbour = nodes[i][j - 1];
                if (j < size - 1) node.RightNeighbour = nodes[i][j + 1];
            }
        }
    }

    private void GenereateMainRoute() {
        MazeNode currentNode = nodes[beginPosY][beginPosX];

        while (currentNode != nodes[endPosY][endPosX]) {
            Visit(currentNode);

            visitedNodesWithUnvisitedNeighbours.Add(currentNode);
            UpdateVisitedNodesWithUnvisitedNeighbours();

            if (visitedNodesWithUnvisitedNeighbours.Count == 0) break;

            if (!visitedNodesWithUnvisitedNeighbours.Contains(currentNode)) {
                currentNode = GetRandomVisitedNodeWithUnvisitedNeighbours();
            }

            currentNode = Travel(currentNode);
        }

        //This way the end node wont get added to the visitedNodesWithUnvisitedNeighbours array, witch is good.
        Visit(currentNode);
    }

    private void GenerateSubRoutes() {
        MazeNode currentNode = GetRandomVisitedNodeWithUnvisitedNeighbours();

        while (visitedNodesWithUnvisitedNeighbours.Count > 0) {
            Visit(currentNode);

            visitedNodesWithUnvisitedNeighbours.Add(currentNode);
            UpdateVisitedNodesWithUnvisitedNeighbours();

            if (visitedNodesWithUnvisitedNeighbours.Count == 0) break;

            if (!visitedNodesWithUnvisitedNeighbours.Contains(currentNode)) {
                currentNode = visitedNodesWithUnvisitedNeighbours[Random.Range(0, visitedNodesWithUnvisitedNeighbours.Count)];
            }

            currentNode = Travel(currentNode);
        }
    }

    private void PlaceEntities() {
        Player = Instantiate(PlayerPrefab, new Vector3(beginPosX, beginPosY, -6.0f), Quaternion.identity);
        Player.transform.SetParent(this.transform);

        GameObject endTrigger = Instantiate(EndPrefab, new Vector3((float)endPosX, (float)endPosY, -6.0f), Quaternion.identity);
        endTrigger.transform.SetParent(this.transform);

        BorderPrefab.transform.position = new Vector3(size / 2.0f - 0.5f, size / 2.0f - 0.5f, -4.0f);
        BorderPrefab.transform.localScale = new Vector3(size, size, 1.0f);

        generatedObjects.Add(endTrigger);
        generatedObjects.Add(Player);

        WinTrigger trigger = endTrigger.GetComponent<WinTrigger>();
        trigger.SetWinObjects(new GameObject[] { Player });
    }

    private void FitToScreen() {

        float scale = ScreenManager.SM.ScreenWidth / size;
        transform.localScale = new Vector3(scale, scale, 1.0f);

        float pos = (-(size / 2.0f) + 0.5f) * scale;
        transform.position = new Vector3(pos, pos, -4.0f);
    }


    public enum Direction {
        Up, Down, Left, Right 
    }
    private bool HasUnvisitedNeighbour(MazeNode node) {
        if (node.TopNeighbour != null && !node.TopNeighbour.IsVisited) return true;
        if (node.BottomNeighbour != null && !node.BottomNeighbour.IsVisited) return true;
        if (node.LeftNeighbour != null && !node.LeftNeighbour.IsVisited) return true;
        if (node.RightNeighbour != null && !node.RightNeighbour.IsVisited) return true;

        return false;
    }
    private void UpdateVisitedNodesWithUnvisitedNeighbours() {
        List<MazeNode> toRemove = new();

        foreach (MazeNode node in visitedNodesWithUnvisitedNeighbours) {
            if (!HasUnvisitedNeighbour(node)) toRemove.Add(node);
        }

        foreach (MazeNode node in toRemove) {
            visitedNodesWithUnvisitedNeighbours.Remove(node);
        }
    }

    private List<Direction> GetAvailableDirections(MazeNode node) {
        List<Direction> availableDirections = new List<Direction>();

        if (node.TopNeighbour != null && !node.TopNeighbour.IsVisited) availableDirections.Add(Direction.Up);
        if (node.BottomNeighbour != null && !node.BottomNeighbour.IsVisited) availableDirections.Add(Direction.Down);
        if (node.LeftNeighbour != null && !node.LeftNeighbour.IsVisited) availableDirections.Add(Direction.Left);
        if (node.RightNeighbour != null && !node.RightNeighbour.IsVisited) availableDirections.Add(Direction.Right);

        return availableDirections;
    }
    private Direction GetRandomDirection(List<Direction> directions) {
        return directions[Random.Range(0, directions.Count)];
    }
    private MazeNode GetRandomVisitedNodeWithUnvisitedNeighbours() {
        return visitedNodesWithUnvisitedNeighbours[Random.Range(0, visitedNodesWithUnvisitedNeighbours.Count)];
    }
    private void Visit(MazeNode node) {
        node.IsVisited = true;
    }

    private MazeNode Travel(MazeNode currentNode) {
        List<Direction> availableDirections = GetAvailableDirections(currentNode);
        Direction travelDirection = GetRandomDirection(availableDirections);

        GameObject newTile = null;

        switch (travelDirection) {
            case Direction.Up:
                newTile = Instantiate(TilePrefab, new Vector3(currentNode.XIndex * 1.0f, currentNode.YIndex + 0.5f, -5.0f), Quaternion.identity);
                newTile.transform.SetParent(this.transform);
                newTile.transform.localScale = new Vector3(0.8f, 1.8f, 1.0f);

                currentNode = currentNode.TopNeighbour;
                break;
            case Direction.Down:
                newTile = Instantiate(TilePrefab, new Vector3(currentNode.XIndex * 1.0f, currentNode.YIndex - 0.5f, -5.0f), Quaternion.identity);
                newTile.transform.SetParent(this.transform);
                newTile.transform.localScale = new Vector3(0.8f, 1.8f, 1.0f);

                currentNode = currentNode.BottomNeighbour;
                break;
            case Direction.Left:
                newTile = Instantiate(TilePrefab, new Vector3(currentNode.XIndex - 0.5f, currentNode.YIndex * 1.0f, -5.0f), Quaternion.identity);
                newTile.transform.SetParent(this.transform);
                newTile.transform.localScale = new Vector3(1.8f, 0.8f, 1.0f);

                currentNode = currentNode.LeftNeighbour;
                break;
            case Direction.Right:
                newTile = Instantiate(TilePrefab, new Vector3(currentNode.XIndex + 0.5f, currentNode.YIndex * 1.0f, -5.0f), Quaternion.identity);
                newTile.transform.SetParent(this.transform);
                newTile.transform.localScale = new Vector3(1.8f, 0.8f, 1.0f);

                currentNode = currentNode.RightNeighbour;
                break;
        }

        generatedObjects.Add(newTile);

        return currentNode;
    }
}
