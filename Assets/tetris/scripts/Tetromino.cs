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
            Move(Vector2.left);           
        
        // if right key is pressed
        } else if (Input.GetKeyDown(GAME_VARIABLES.moveRight)) {
            // move right
            Move(Vector2.right);
        
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
    private void Move(Vector3 translation) {
        this.transform.position += translation;

        // check if current position is valid
        if (ValidPosition()) {
            // if so update the grid
            UpdateGrid();
        } else {
            // if not move this piece back
            this.transform.position += -translation;
        }
    }

    /// <summary>
    /// rotates the piece 90deg clockwise
    /// if rotation causes a collision it performs a 'wall kick' to try keep the piece in play
    /// </summary>
    private void Rotate() {
        this.transform.Rotate(0, 0, 90);

        // this code here tries to 'WALL KICK' to move the piece back into play
        // if we try to rotate and collide with something, we try moving left and right to get the piece to fit
        if (!ValidPosition()) {
            // try moving left
            this.transform.position += Vector3.left;
            if (!ValidPosition()) {
                // if dont fit try moving right
                this.transform.position += 2 * Vector3.right;
                if (!ValidPosition()) {
                    // cant wall kick
                    this.transform.position += Vector3.left;
                }
            }
        }

        UpdateGrid();

        // we do this additional rotation for each mino because the sprite uses lighting on the top and left sides
        // without this rotation, the 'lighting' changes when the object is rotated which does not look right
        foreach (Transform mino in this.transform) {
            mino.Rotate(0, 0, -90);
        }
    }

    private void Fall() {
        // move down
        this.transform.position += Vector3.down;
        // check if current position is valid
        if (ValidPosition()) {
            // if so update the grid
            UpdateGrid();          
        } else {
            enabled = false;
        }

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

    private bool ValidPosition() {
        // get dimensions of the current grid
        Vector2 dimensions = GRID.GetDimensions();

        // loop for each mino within this tetromino
        foreach (Transform mino in this.transform) {
            // calculate the grid position of this mino
            Vector2 minoGridPos = GRID.WorldToGrid(mino.position);

            if (minoGridPos.y < 0) {
                return false;
            }

            if (minoGridPos.x < 0 || minoGridPos.x >= dimensions.x) {
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