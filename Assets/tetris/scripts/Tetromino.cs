using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tetromino : MonoBehaviour {

    private GameManager GAME_MANAGER;
    private GameVariables GAME_VARIABLES;
    private GameGrid GRID;

    private Vector2[] minoGridPositions;

    private float timeSinceFall = 0;
    // in seconds
    private float timeBetweenFalls = 1;

    public Text gridText;

    void Start() {
        GAME_MANAGER = GameObject.FindWithTag("GM").GetComponent<GameManager>();
        GAME_VARIABLES = GameObject.FindWithTag("GM").GetComponent<GameVariables>();
        GRID = GAME_MANAGER.grid;
        gridText = GameObject.Find("Text").GetComponent<Text>();

        minoGridPositions = new Vector2[this.transform.childCount];
        for(int i = 0; i < minoGridPositions.Length; i++) {
            minoGridPositions[i] = GRID.WorldToGrid(this.transform.GetChild(i).position);
        }
        
        UpdateGrid();
    }

    void Update() {
        CheckInput();
    }

    /// <summary>
    /// check for any player input
    /// </summary>
    void CheckInput() {
        // if left key is pressed
        if (Input.GetKeyDown(GAME_VARIABLES.moveLeft)) {
            // move left
            if (Move(Vector2.left)) {
                UpdateGrid();
            }           
        
        // if right key is pressed
        } else if (Input.GetKeyDown(GAME_VARIABLES.moveRight)) {
            // move right
            if (Move(Vector2.right)) {
                UpdateGrid();
            }
        
        // if rotate key is pressed
        } else if (Input.GetKeyDown(GAME_VARIABLES.rotate)) {
            // rotate 90 deg clockwise
            Rotate();

        } else if (Input.GetKeyDown(GAME_VARIABLES.drop) || Time.time - timeSinceFall >= timeBetweenFalls) {
            Fall();
        }
    }

    /// <summary>
    /// move this piece by the translation given
    /// </summary>
    /// <param name="translation"></param>
    private bool Move(Vector3 translation) {
        this.transform.position += translation;

        // check if current position is valid
        if (ValidPosition()) {
            // if so update the grid
            return true;
        } else {
            // if not move this piece back
            this.transform.position += -translation;
            return false;
        }
    }

    /// <summary>
    /// rotates the piece 90deg clockwise
    /// if rotation causes a collision it performs a 'wall kick' to try keep the piece in play
    /// </summary>
    private void Rotate() {
        bool rotated = false;

        this.transform.Rotate(0, 0, 90);

        if (ValidPosition()) {
            rotated = true;
        } else {
            if (Move(Vector3.left) || Move(Vector3.right) || Move(Vector3.up)) {
                rotated = true;
            }
        }

        if (rotated) {
            UpdateGrid();

            // we do this additional rotation for each mino because the sprite uses lighting on the top and left sides
            // without this rotation, the 'lighting' changes when the object is rotated which does not look right
            foreach (Transform mino in this.transform) {
                mino.Rotate(0, 0, -90);
            }
        }
    }

    private void Fall() {
        if (!Move(Vector3.down)) {
            Debug.Log("landed!");

            enabled = false;
        }

        UpdateGrid();
        timeSinceFall = Time.time;
    }

    /// <summary>
    /// returns the vector in the format "x,y"
    /// makes it easier to print vectors in debugging
    /// </summary>
    /// <param name="vec">the vector to print</param>
    /// <returns>string "x,y"</returns>
    private string VecToString(Vector2 vec) {
        return (vec.x + "," + vec.y);
    }

    /// <summary>
    /// determines whether  a given position is within the grid
    /// </summary>
    /// <param name="pos">vector2 as grid position</param>
    /// <returns>true if in grid, false if not</returns>
    private bool InGrid(Vector2 pos) {
        if (pos.y < 0 || pos.x < 0 || pos.x >= GRID.GetDimensions().x) {
            return false;
        } else {
            return true;
        }
    }

    private bool ValidPosition() {
        // get dimensions of the current grid
        Vector2 dimensions = GRID.GetDimensions();

        // loop for each mino within this tetromino
        foreach (Transform mino in this.transform) {
            // calculate the grid position of this mino
            Vector2 minoGridPos = GRID.WorldToGrid(mino.position);

            if (!InGrid(minoGridPos)) {
                return false;
            }

            if (GRID.GetGridAt(minoGridPos) != null && GRID.GetGridAt(minoGridPos).parent != this.transform) {
                Debug.Log("hit existing mino");
                return false;
            }
        }

        // if return hasnt already been called, we haven't collided at all, so return 0
        return true;
    }

    /// <summary>
    /// clean up old occupied grid space and set new occupied space to true in the grid
    /// </summary>
    private void UpdateGrid() {
        // remove all old positions
        for(int i = 0; i < minoGridPositions.Length; i++) {
            GRID.SetGridAt(minoGridPositions[i], null);       
        }

        // update to new positions
        for (int i = 0; i < minoGridPositions.Length; i++) {
            Transform currentMino = this.transform.GetChild(i);
            Vector2 minoGridPos = GRID.WorldToGrid(currentMino.position);

            minoGridPositions[i] = minoGridPos;

            GRID.SetGridAt(minoGridPos, this.transform.GetChild(i));
        }

        DrawGrid();
    }

    private void DrawGrid() {
        Vector2 dimensions = GRID.GetDimensions();
        gridText.text = "";

        for (int y = (int) dimensions.y - 1; y > -1; y--) {
            for (int x = 0; x < dimensions.x; x++) {
                if (GRID.GetGridAt(new Vector2(x, y)) != null) {
                    gridText.text += "#";
                } else {
                    gridText.text += "e";
                }
            }

            gridText.text += '\n';
        }
    }
}