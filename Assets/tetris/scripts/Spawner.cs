using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    [SerializeField]
    private GameObject tetromino_i;
    [SerializeField]
    private GameObject tetromino_j;
    [SerializeField]
    private GameObject tetromino_l;
    [SerializeField]
    private GameObject tetromino_o;
    [SerializeField]
    private GameObject tetromino_s;
    [SerializeField]
    private GameObject tetromino_t;
    [SerializeField]
    private GameObject tetromino_z;

    private GameObject[] tetrominoList;

    // used for tetrominos that have their pivot offset from any mino (ie. i, o)
    private Vector3 spawnPointCentral;
    // used for tetrominos that have their pivot at the centre of a mino (ie. j, l, s, t, z)
    private Vector3 spawnPointOffset;

    private GameGrid GRID;

    void Start() {
        tetrominoList = new GameObject[] { tetromino_i, tetromino_j , tetromino_l, tetromino_o,
                                            tetromino_s, tetromino_t, tetromino_z};

        GRID = GameObject.FindWithTag("GM").GetComponent<GameManager>().grid;

        Vector2 tempPoint = GRID.WorldToGrid(new Vector2(5f, 19f));
        // used for tetrominos with a pivot offset from any mino (ie. i, o)
        spawnPointCentral = new Vector3(tempPoint.x, tempPoint.y, 0);

        // used for tetrominos with a pivot centred on a mino (ie. j, l, s, t, z)
        spawnPointOffset = new Vector3(tempPoint.x - 0.5f, tempPoint.y - 0.5f, 0);

        SpawnTetromino();
    }

    public void SpawnTetromino() {
        int currentTetrominoIndex = Random.Range(0, tetrominoList.Length - 1);
        GameObject currentTetromino = tetrominoList[currentTetrominoIndex];
        Vector3 spawnPoint;

        if(currentTetrominoIndex == 0 || currentTetrominoIndex == 3) {
            spawnPoint = spawnPointCentral;
        } else {
            spawnPoint = spawnPointOffset;
        }

        Instantiate(currentTetromino, spawnPoint, Quaternion.identity).GetComponent<Tetromino>();
    }
}