using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour {

    public static int gridWidth = 10;
    public static int gridHeight = 20;

    public Transform[,] grid = new Transform[gridWidth, gridHeight];

    void OnDrawGizmos() {
        Gizmos.color = Color.black;
        for (int i = 0; i <= gridWidth; i++) {
            Gizmos.DrawLine(new Vector3(transform.position.x + i, transform.position.y, transform.position.z),
                            new Vector3(transform.position.x + i, transform.position.y + gridHeight, transform.position.z));
        }

        for (int j = 0; j <= gridHeight; j++) {
            Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y + j, transform.position.z),
                            new Vector3(transform.position.x + gridWidth, transform.position.y + j, transform.position.z));
        }
    }
}
