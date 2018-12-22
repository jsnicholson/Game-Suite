using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour {

    private GameManager GAME_MANAGER;
    private GameVariables GAME_VARIABLES;
    private GameGrid GRID;

    private Vector2 gridPos;

    void Start() {
        GAME_MANAGER = GameObject.FindWithTag("GM").GetComponent<GameManager>();
        GAME_VARIABLES = GameObject.FindWithTag("GM").GetComponent<GameVariables>();
        GRID = GAME_MANAGER.grid;
    }

    void Update() {
        CheckInput();
    }

    void CheckInput() {
        if (Input.GetKeyDown(GAME_VARIABLES.moveLeft)) {
            if (CanMoveInDirection(Vector2.left)) {
                Move(Vector3.left);
            }    
        } else if (Input.GetKeyDown(GAME_VARIABLES.moveRight)) {
            if (CanMoveInDirection(Vector2.right)) {
                Move(Vector3.right);
            }
        } else if (Input.GetKeyDown(GAME_VARIABLES.rotate)) {
            Rotate();
        }
    }

    private void Move(Vector3 translation) {
        this.transform.position += translation;
    }

    private void Rotate() {
        this.transform.Rotate(0, 0, 90);
        // we do this additional rotation for each mino because the sprite uses lighting on the top and left sides
        // without this rotation, the 'lighting' changes when the object is rotated which does not look right
        foreach(Transform mino in this.transform) {
            mino.Rotate(0, 0, -90);
        }
    }

    private void Fall() {

    }

    private Vector2 FloorVec2(Vector2 vec) {
        return new Vector2(Mathf.Floor(vec.x), Mathf.Floor(vec.y));
    }

    private string VecToString(Vector2 vec) {
        return (vec.x + "," + vec.y);
    }

    private bool CanMoveInDirection(Vector2 direction) {
        return PositionIsValid((Vector2) this.transform.position + direction);
    }

    private bool PositionIsValid(Vector2 pos) {
        Vector2 translation = pos - (Vector2) this.transform.position;

        foreach(Transform mino in this.transform) {
            Vector2 newMinoPos = (Vector2) mino.position + translation;

            if (!(InsideBorder(newMinoPos))) {
                return false;
            }
        }

        return true;
    }

    private bool InsideBorder(Vector2 pos) {
        if (!(pos.x >= GRID.transform.position.x && pos.x < GRID.transform.position.x + GAME_VARIABLES.gridDimensions.x && pos.y >= 0)) {
            return false;
        }

        return true;
    }
}
