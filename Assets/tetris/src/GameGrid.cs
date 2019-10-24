using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class GameGrid : MonoBehaviour {

    private GameVariables m_gameVariables;
    protected readonly Vector2 m_gridDimensions = new Vector2(10, 22);

    protected GameObject[,] arrGrid;

    // DEBUG
    private Text m_gridText;

// ===== CALLED BY UNITY =====
    public virtual void OnDrawGizmos() {
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

// ===== PROTECTED =====
    protected void InitialiseGrid() {
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

    public GameObject GetGridAt(int x, int y) {
        return arrGrid[y, x];
    }

    public GameObject GetGridAt(Vector2 gridPos) {
        return arrGrid[(int) gridPos.y, (int) gridPos.x];
    }

    public void SetGridAt(int x, int y, GameObject value) {
        arrGrid[y, x] = value;
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

    public Vector2 WorldToGrid(Vector3 worldPos) {
        Vector3 relativePos = worldPos - this.transform.position;
        return Vector2Math.RoundVec2((Vector2)relativePos);
    }

    public Vector3 GridToWorld(Vector2 gridPos) {
        return this.transform.position + new Vector3(gridPos.x, gridPos.y, 0);
    }

    /// <summary>
    /// determines whether a given position is within the grid
    /// </summary>
    /// <param name="pos">point to check</param>
    /// <returns>true if in grid, false if not</returns>
    public bool InGrid(int x, int y) {
        if (y < 0 || x < 0 || x >= m_gridDimensions.x) {
            return false;
        } else {
            return true;
        }
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
                    m_gridText.text += ".";
                }
            }

            m_gridText.text += '\n';
        }
    }

} // ~GameGrid