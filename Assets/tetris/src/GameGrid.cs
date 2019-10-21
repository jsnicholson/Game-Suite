using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameGrid : MonoBehaviour {

    private GameVariables m_gameVariables;
    private readonly Vector2 m_gridDimensions = new Vector2(10, 22);

    private GameObject[,] arrGrid;

    // DEBUG
    private Text m_gridText;

// ===== CALLED BY UNITY =====
    public void OnDrawGizmos() {
        Gizmos.color = Color.black;
        for (int i = 0; i <= m_gridDimensions.x; i++) {
            Gizmos.DrawLine(new Vector3(transform.position.x + i, transform.position.y, transform.position.z),
                            new Vector3(transform.position.x + i, transform.position.y + m_gridDimensions.y, transform.position.z));
        }

        for (int j = 0; j <= m_gridDimensions.y; j++) {
            Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y + j, transform.position.z),
                            new Vector3(transform.position.x + m_gridDimensions.x, transform.position.y + j, transform.position.z));
        }
    }

// ===== PUBLIC =====
    public void Initialise(GameManager _manager)
	{
        m_gameVariables = _manager.GetVariables();
        arrGrid = new GameObject[(int)m_gridDimensions.y, (int)m_gridDimensions.x];
        InitialiseGrid();

        // ================== DEBUG ===================
        m_gridText = GameObject.Find("Text").GetComponent<Text>();
    }

// ===== PRIVATE =====
    private void InitialiseGrid() {
        for(int j = 0; j < m_gridDimensions.y; j++) {
            for(int i = 0; i < m_gridDimensions.x; i++) {
                arrGrid[j, i] = null;
            }
        }
    }

// ===== PUBLIC =====
    public Vector2 GetDimensions() {
        return m_gridDimensions;
    }

    public GameObject[,] GetGrid() {
        return arrGrid;
    }

    public GameObject GetGridAt(Vector2 gridPos) {
        return arrGrid[(int) gridPos.y, (int) gridPos.x];
    }

    public void SetGridAt(Vector2 pos, GameObject value) {
        arrGrid[(int) pos.y,  (int) pos.x] = value;
    }

    /// <summary>
    /// converts from world space to grid space
    /// </summary>
    /// <param name="worldPos">world position to convert</param>
    /// <returns></returns>
    public Vector2 WorldToGrid(Vector2 worldPos) {
        // explicitly cast to Vector2 as dealing with 2D grid
        Vector2 relativePos = worldPos - (Vector2)this.transform.position;
        return Vector2Math.RoundVec2(relativePos);
    }

    /// <summary>
    /// determines whether a given position is within the grid
    /// </summary>
    /// <param name="pos">point to check</param>
    /// <returns>true if in grid, false if not</returns>
    public bool InGrid(Vector2 pos) {
        if (pos.y < 0 || pos.x < 0 || pos.x >= m_gridDimensions.x) {
            return false;
        } else {
            return true;
        }
    }

    /// <summary>
    /// Debug draw grid
    /// </summary>
    public void DrawGrid() {
        Vector2 dimensions = GetDimensions();
        m_gridText.text = "";

        for (int y = (int)dimensions.y - 1; y > -1; y--) {
            for (int x = 0; x < dimensions.x; x++) {
                if (GetGridAt(new Vector2(x, y)) != null) {
                    m_gridText.text += "#";
                } else {
                    m_gridText.text += "e";
                }
            }

            m_gridText.text += '\n';
        }
    }

    /// <summary>
    /// if the any lines are full then remove them
    /// </summary>
    public bool ClearLines() {
        // list of line to clear
        List<int> lint_linesToClear = new List<int>();
        bool b_anyCleared = false;

        // loop over grid
        for (int j = 0; j < m_gridDimensions.y; j++) {
            // assume line is full
            bool b_lineFull = true;
            for (int i = 0; i < m_gridDimensions.x; i++) {
                // if grid at x,y is null, then line is not full
                if (GetGridAt(new Vector2(i, j)) == null)
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

        if (b_anyCleared) {
            // remove bottom line
            foreach (int line in lint_linesToClear) {
                for (int i = 0; i < m_gridDimensions.x; i++) {
                    Destroy(GetGridAt(new Vector2(i, line)));
                    SetGridAt(new Vector2(i, line), null);
                }
            }

            // move all other minos down
            // start from second row up
            for (int j = 1; j < m_gridDimensions.y; j++) {
                for (int i = 0; i < m_gridDimensions.x; i++) {
                    // get obj at pos
                    GameObject obj_mino = GetGridAt(new Vector2(i, j));
                    // if a mino is there
                    if (obj_mino != null) {
                        // reference it in the row below
                        SetGridAt(new Vector2(i, j - lint_linesToClear.Count), obj_mino);
                        // remove from previous location
                        SetGridAt(new Vector2(i, j), null);

                        // unity is a pain
                        Vector3 vec_objPosition = obj_mino.transform.position;
                        vec_objPosition.y -= lint_linesToClear.Count;
                        obj_mino.transform.position = vec_objPosition;
                    }
                }
            }
        }

        return b_anyCleared;
    }

} // ~GameGrid