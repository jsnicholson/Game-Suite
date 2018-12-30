using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour {

    private GameVariables GAME_VARIABLES;
    private Vector2 gridDimensions = new Vector2(10, 20);

    private Transform[,] arrGrid;

    void Awake() {
        GAME_VARIABLES = GameObject.FindWithTag("GM").GetComponent<GameVariables>();
        gridDimensions = GAME_VARIABLES.gridDimensions;
    }

    void Start() {
        arrGrid = new Transform[(int) gridDimensions.y, (int) gridDimensions.x];
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.black;
        for (int i = 0; i <= gridDimensions.x; i++) {
            Gizmos.DrawLine(new Vector3(transform.position.x + i, transform.position.y, transform.position.z),
                            new Vector3(transform.position.x + i, transform.position.y + gridDimensions.y, transform.position.z));
        }

        for (int j = 0; j <= gridDimensions.y; j++) {
            Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y + j, transform.position.z),
                            new Vector3(transform.position.x + gridDimensions.x, transform.position.y + j, transform.position.z));
        }
    }

    private void InitialiseGrid() {
        for(int j = 0; j < gridDimensions.y; j++) {
            for(int i = 0; i < gridDimensions.x; i++) {
                arrGrid[j, i] = null;
            }
        }
    }

    public Vector2 GetDimensions() {
        return gridDimensions;
    }

    public Transform[,] GetGrid() {
        return arrGrid;
    }

    public Transform GetGridAt(Vector2 gridPos) {
        return arrGrid[(int) gridPos.y, (int) gridPos.x];
    }

    public void SetGridAt(Vector2 pos, Transform value) {
        arrGrid[(int) pos.y,  (int) pos.x] = value;
    }

    public static Vector2 FloorVec2(Vector2 vec) {
        return new Vector2(Mathf.Floor(vec.x), Mathf.Floor(vec.y));
    }

    public Vector2 WorldToGrid(Vector2 screenVec) {
        Vector2 relativePos = screenVec - (Vector2)this.transform.position;
        return FloorVec2(relativePos);
    }
}