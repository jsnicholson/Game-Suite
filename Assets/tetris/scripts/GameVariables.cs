using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameVariables : MonoBehaviour {

    public Vector2 gridDimensions = new Vector2(10, 20);

    public KeyCode moveLeft = KeyCode.LeftArrow;
    public KeyCode moveRight = KeyCode.RightArrow;
    public KeyCode rotate = KeyCode.UpArrow;
    public KeyCode drop = KeyCode.DownArrow;
}
