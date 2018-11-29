using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour {

    public KeyCode moveLeft = KeyCode.LeftArrow;
    public KeyCode moveRight = KeyCode.RightArrow;
    public KeyCode rotate = KeyCode.UpArrow;
    public KeyCode drop = KeyCode.DownArrow;

    void Update() {
        CheckInput();
    }

    void CheckInput() {
        if (Input.GetKeyDown(moveLeft)) {
            if (PositionIsValid(Vector3.left)) {
                Move(Vector3.left);
            }    
        } else if (Input.GetKeyDown(moveRight)) {
            if (PositionIsValid(Vector3.right)) {
                Move(Vector3.right);
            }
        } else if (Input.GetKeyDown(rotate)) {
            Rotate();
        }
    }

    void Move(Vector3 translation) {
        this.transform.position += translation;
    }

    void Rotate() {
        this.transform.Rotate(0, 0, 90);
        // we do this additional rotation for each mino because the sprite uses lighting on the top and left sides
        // without this rotation, the 'lighting' changes when the object is rotated which does not look right
        foreach(Transform mino in this.transform) {
            mino.Rotate(0, 0, -90);
        }
    }

    void Fall() {

    }

    Vector2 FloorVec2(Vector2 vec) {
        return new Vector2(Mathf.Floor(vec.x), Mathf.Floor(vec.y));
    }

    /// <summary>
    /// check if when transformation pos is applied, the tetromino will be in the grid
    /// </summary>
    /// <param name="pos">the position of the parent tetromino</param>
    /// <returns>returns a bool</returns>
    public bool PositionIsValid(Vector3 pos) {
        foreach(Transform mino in this.transform) {
            Vector3 newMinoPos = mino.position + pos;
            Vector2 gridPos = FloorVec2(new Vector2(newMinoPos.x, newMinoPos.y));
            Debug.Log("minoGridPos(" + gridPos.x + "," + gridPos.y + ")");
            if (!(newMinoPos.x >= 0 && newMinoPos.x < GameVariables.gridWidth && newMinoPos.y >= 0)) {
                return false;
            }
        }

        return true;
    }
}
