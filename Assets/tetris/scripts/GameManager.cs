using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameGrid grid;
    public Spawner spawner;

    private AudioSource audioSource;
    // audio clips
    public AudioClip audioLineClear;

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    public void PieceLanded() {
        spawner.SpawnTetromino();
        ClearLines();
    }

    private void ClearLines() {
        bool b_lineFull = true;
        for (int i = 0; i < grid.GetDimensions().x; i++) {
            if(grid.GetGridAt(new Vector2(i, 0)) == null)
                b_lineFull = false;
            
        }

        if(b_lineFull) {
            audioSource.clip = audioLineClear;
            audioSource.Play();
        }     
    }
}