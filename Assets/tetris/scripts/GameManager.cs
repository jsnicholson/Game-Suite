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

    /// <summary>
    /// returns the vector in the format "x,y"
    /// makes it easier to print vectors in debugging
    /// </summary>
    /// <param name="vec">the vector to print</param>
    /// <returns>string "x,y"</returns>
    public string VecToString(Vector2 vec) {
        return (vec.x + "," + vec.y);
    }

    public void PieceLanded() {
        ClearLines();
        spawner.SpawnTetromino(); 
    }

    /// <summary>
    /// if the any lines are full then remove them
    /// </summary>
    private void ClearLines() {
        // we call this a lot so may as well just do it once here
        Vector2 vec_gridDimensions = grid.GetDimensions();
        // list of line to clear
        List<int> lint_linesToClear = new List<int>();
        bool b_anyCleared = false;

        // loop over grid
        for (int j = 0; j < vec_gridDimensions.y; j++) {
            // assume line is full
            bool b_lineFull = true;
            for(int i = 0; i < vec_gridDimensions.x; i++) {
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
                for(int i = 0; i < vec_gridDimensions.x; i++) {
                    Destroy(grid.GetGridAt(new Vector2(i, line)));
                    grid.SetGridAt(new Vector2(i, line), null);
                }
            }

            // move all other minos down
            // start from second row up
            for(int j = 1; j < vec_gridDimensions.y; j++) {
                for(int i = 0; i < vec_gridDimensions.x; i++) {
                    // get obj at pos
                    GameObject obj_mino = grid.GetGridAt(new Vector2(i, j));
                    // if a mino is there
                    if(obj_mino != null) {
                        // reference it in the row below
                        grid.SetGridAt(new Vector2(i, j - lint_linesToClear.Count), obj_mino);
                        // remove from previous location
                        grid.SetGridAt(new Vector2(i, j), null);

                        // unity is a pain
                        Vector3 vec_objPosition = obj_mino.transform.position;
                        vec_objPosition.y -= lint_linesToClear.Count;
                        obj_mino.transform.position = vec_objPosition;
                    }
                }
            }

            // play sound
            audio_source.clip = audio_lineClear;
            audio_source.Play();
        }     
    }
}