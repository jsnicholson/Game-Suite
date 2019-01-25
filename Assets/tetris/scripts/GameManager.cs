using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameGrid grid;
    public Spawner spawner;

    public void PieceLanded() {
        spawner.SpawnTetromino();
    }
}