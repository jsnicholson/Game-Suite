using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour {

    private GameManager GAME_MANAGER;
    private GameVariables GAME_VARIABLES;
    private GameGrid GRID;

    private Vector2[] minoGridPositions;

    void Start() {
        GAME_MANAGER = GameObject.FindWithTag("GM").GetComponent<GameManager>();
        GAME_VARIABLES = GameObject.FindWithTag("GM").GetComponent<GameVariables>();
        GRID = GAME_MANAGER.grid;
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
            // check if current position is valid
            if (CollisionAtPosition() == 0) {
                // if so update the grid
                UpdateGrid();
            } else {
                // if not move this piece back
                Move(-Vector2.left);
            }
        
        // if right key is pressed
        } else if (Input.GetKeyDown(GAME_VARIABLES.moveRight)) {
            // move right
            Move(Vector2.right);
            // check if current position is valid
            if (CollisionAtPosition() == 0) {
                // if so update the grid
                UpdateGrid();
            } else {
                // if not move back
                Move(-Vector2.right);
            }
        
        // if rotate key is pressed
        } else if (Input.GetKeyDown(GAME_VARIABLES.rotate)) {
            // rotate 90 deg clockwise
            Rotate();
            // because CollisionAtPosition returns an int, this line will move the piece
            // in the appropriate direction if it is colliding with a wall in an action
            // known as a 'wall kick'.
            // this allows the piece to always rotate but will push it back into play if it against a wall
            Move(Vector2.left * CollisionAtPosition());
        }
    }

    /// <summary>
    /// set all the grid positions for the minos of this piece
    /// </summary>
    /// <param name="positions">a vector array containing the grid position of all minos</param>
    public void SetMinoGridPositions(Vector2[] positions) {
        minoGridPositions = positions;
    }

    /// <summary>
    /// move this piece by the translation given
    /// </summary>
    /// <param name="translation"></param>
    private void Move(Vector3 translation) {
        this.transform.position += translation;
    }

    /// <summary>
    /// rotates the piece 90deg clockwise
    /// </summary>
    private void Rotate() {
        this.transform.Rotate(0, 0, 90);

        // we do this additional rotation for each mino because the sprite uses lighting on the top and left sides
        // without this rotation, the 'lighting' changes when the object is rotated which does not look right
        foreach (Transform mino in this.transform) {
            mino.Rotate(0, 0, -90);
        }
    }

    private void Fall() {

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
    /// checks collision for a piece at this position
    /// </summary>
    /// <returns>-1 if left collision, 1 if right collision, 0 if no collision</returns>
    private int CollisionAtPosition() {
        // get dimensions of the current grid
        Vector2 dimensions = GRID.GetDimensions();

        // loop for each mino within this tetromino
        foreach (Transform mino in this.transform) {
            // calculate the grid position of this mino
            Vector2 minoGridPos = GRID.WorldToGrid(mino.position);

            // check for left and right border collision
            if (minoGridPos.x < 0) {
                return -1;
            } else if (minoGridPos.x >= dimensions.x) {
                return 1;
            }
        }

        // if return hasnt already been called, we haven't collided at all, so return 0
        return 0;
    }

    /// <summary>
    /// clean up old occupied grid space and set new occupied space to true in the grid
    /// </summary>
    private void UpdateGrid() {
        for(int i = 0; i < this.transform.childCount; i++) {
            Vector2 minoGridPos = GRID.WorldToGrid(this.transform.GetChild(i).position);

            if (minoGridPositions != null) {
                GRID.SetGridAt(minoGridPositions[i], false);
                minoGridPositions[i] = minoGridPos;
            }
         
            GRID.SetGridAt(minoGridPos, true);
        }
    }
}