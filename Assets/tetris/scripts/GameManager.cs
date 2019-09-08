using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameGrid grid;
    public Spawner spawner;

    private AudioSource audioSource;
    // audio clips
    public AudioClip audioLineClear;

    public void PieceLanded() {
        spawner.SpawnTetromino();
        ClearLines();
    }

    private void ClearLines() {
        audioSource.clip = audioLineClear;
        audioSource.Play();
    }
}