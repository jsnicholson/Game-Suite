using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour {

// ===== PRIVATE MEMBERS =====
    private GameManager m_gameManager;
    private GameVariables m_gameVariables;
    private GameGrid m_gameGrid;

    private Vector2[] m_minoGridPositions;

    // in seconds
    private float m_lastFallTime;
    private float timeBetweenFalls = 1.0f;

    private float timeTillFastDrop = 0.5f;
    private float timeBetweenFastFall = 0.05f;
    private float pressTime;

// ===== PUBLIC =====
    /// <summary>
    /// stand in for constructor as apparently Unity doesnt like that
    /// </summary>
    /// <param name="_manager"></param>
    /// <param name="_variables"></param>
    /// <param name="_grid"></param>
    public void Initialise(GameManager _manager, GameVariables _variables, TetrisGrid _grid) {
        m_gameManager = _manager;
        m_gameVariables = _variables;
        m_gameGrid = _grid;

        m_minoGridPositions = new Vector2[this.transform.childCount];
        for (int i = 0; i < m_minoGridPositions.Length; i++) {
            m_minoGridPositions[i] = m_gameGrid.WorldToGrid(this.transform.GetChild(i).position);
        }

        m_lastFallTime = Time.time;

        UpdateGridPosition();
    }

    public void Update() {
        CheckForInput();
    }

    /// <summary>
    /// check for any player input
    /// </summary>
    private void CheckForInput() {
        // if left key is pressed
        if (Input.GetKeyDown(m_gameVariables.moveLeft)) {
            // move left
            if (Move(Vector2.left)) {
                UpdateGridPosition();
            }

        // if right key is pressed
        } else if (Input.GetKeyDown(m_gameVariables.moveRight)) {
            // move right
            if (Move(Vector2.right)) {
                UpdateGridPosition();
            }
        }

        // if rotate key is pressed
        if (Input.GetKeyDown(m_gameVariables.rotate)) {
            // rotate 90 deg clockwise
            Rotate();
        }

        if (Input.GetKeyDown(m_gameVariables.drop) || Time.time - m_lastFallTime >= timeBetweenFalls) {
            Fall();
            pressTime = Time.time;
        } else if (Input.GetKey(m_gameVariables.drop) && (Time.time - pressTime >= timeTillFastDrop) && (Time.time - m_lastFallTime >= timeBetweenFastFall))  {
            Fall();
        }
    }

// ===== PRIVATE =====
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
            this.transform.position -= translation;
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
            UpdateGridPosition();

            // we do this additional rotation for each mino because the sprite uses lighting on the top and left sides
            // without this rotation, the 'lighting' changes when the object is rotated which does not look right
            foreach (Transform mino in this.transform) {
                mino.Rotate(0, 0, -90);
            }
        } else {
            this.transform.Rotate(0, 0, -90);
        }
    }

    private void Fall() {
        if (!Move(Vector3.down)) {
            m_gameManager.PieceLanded();
            enabled = false;
        }
        else {
            UpdateGridPosition();
            m_lastFallTime = Time.time;
        }  
    }

    private bool ValidPosition() {
        // loop for each mino within this tetromino
        foreach (Transform mino in this.transform) {
            // calculate the grid position of this mino
            Vector2 minoGridPos = m_gameGrid.WorldToGrid(mino.position);

            if (!m_gameGrid.InGrid(minoGridPos)) {
                return false;
            }

            if (m_gameGrid.GetGridAt(minoGridPos) != null && m_gameGrid.GetGridAt(minoGridPos).transform.parent != this.transform) {
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
    private void UpdateGridPosition() {
        // remove all old positions
        for(int i = 0; i < m_minoGridPositions.Length; i++) {
            m_gameGrid.SetGridAt(m_minoGridPositions[i], null);       
        }

        // update to new positions
        for (int i = 0; i < m_minoGridPositions.Length; i++) {
            Transform currentMino = this.transform.GetChild(i);
            Vector2 minoGridPos = m_gameGrid.WorldToGrid(currentMino.position);

            m_minoGridPositions[i] = minoGridPos;

            m_gameGrid.SetGridAt(minoGridPos, this.transform.GetChild(i).gameObject);
        }

        m_gameGrid.DrawGrid();
    }

} // ~Tetromino