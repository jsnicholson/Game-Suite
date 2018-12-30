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

    void Start() {
        tetrominoList = new GameObject[] { tetromino_i, tetromino_j , tetromino_l, tetromino_o,
                                            tetromino_s, tetromino_t, tetromino_z};

        Tetromino tetromino = Instantiate(tetromino_t, new Vector3(2.5f, 6.5f, 0), Quaternion.identity).GetComponent<Tetromino>();
    }

    void SpawnTetromino() {

    }
}
