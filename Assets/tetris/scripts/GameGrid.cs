using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour {

    private GameVariables GAME_VARIABLES;
    private Vector2 gridDimensions = new Vector2(10, 20);

    private bool[,] arrGrid;

    void Awake() {
        GAME_VARIABLES = GameObject.FindWithTag("GM").GetComponent<GameVariables>();
        gridDimensions = GAME_VARIABLES.gridDimensions;
    }

    void Start() {
        arrGrid = new bool[(int) gridDimensions.y, (int) gridDimensions.x];
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
                arrGrid[j, i] = false;
            }
        }
    }

    public bool[,] GetGrid() {
        return arrGrid;
    }

    public void SetGridAt(Vector2 pos, bool value) {
        arrGrid[(int) pos.y,  (int) pos.x] = value;
    }

    public static Vector2 WorldToGrid(Vector2 screenVec) {
        return new Vector2(Mathf.Floor(screenVec.x), Mathf.Floor(screenVec.y));
    }
}