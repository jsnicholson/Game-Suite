using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameGrid grid;
    public Spawner spawner;

    private AudioSource audio_source;
    // audio clips
    public AudioClip audio_lineClear;

    void Start() {
        audio_source = GetComponent<AudioSource>();
    }

    public void PieceLanded() {
        ClearLines();
        spawner.SpawnTetromino(); 
    }

    /// <summary>
    /// if the any lines are full then remove them
    /// </summary>
    private void ClearLines() {
        // list of line to clear
        List<int> lint_linesToClear = new List<int>();
        bool b_anyCleared = false;

        // loop over grid
        for (int j = 0; j < grid.GetDimensions().y; j++) {
            // assume line is full
            bool b_lineFull = true;
            for(int i = 0; i < grid.GetDimensions().x; i++) {
                // if grid at x,y is null, then line is not full
                if (grid.GetGridAt(new Vector2(i, j)) == null)
                    b_lineFull = false;   
            }

            // if line was full
            if (b_lineFull) {
                Debug.Log("Line full");
                // add line to list to be cleared
                lint_linesToClear.Add(j);
                // flag
                b_anyCleared = true;
            }
        }

        if(b_anyCleared) {
            // remove bottom line
            foreach(int line in lint_linesToClear) {
                for(int i = 0; i < grid.GetDimensions().x; i++) {
                    Destroy(grid.GetGridAt(new Vector2(i, line)));
                    grid.SetGridAt(new Vector2(i, line), null);
                }
            }
            // move all other minos down
            audio_source.clip = audio_lineClear;
            audio_source.Play();
        }     
    }
}