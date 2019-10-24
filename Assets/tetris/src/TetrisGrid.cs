using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class TetrisGrid : GameGrid {

// ===== CALLED BY UNITY =====
    public override void OnDrawGizmos() {
        base.OnDrawGizmos();
    }

    /// <summary>
    /// if the any lines are full then remove them
    /// </summary>
    public bool ClearLines() {
        bool b_anyCleared = false;

        for (int y = 0; y < m_gridDimensions.y; y++) {
            if (IsLineFull(y)) {
                RemoveLine(y);
                ReduceLinesAbove(y);
                b_anyCleared = true;
            }
        }

        DrawGrid();
        return b_anyCleared;
    }

// ===== PRIVATE =====
    private bool IsLineFull(int y) {
        Assert.IsTrue(InGrid(0, y));

        bool b_IsFull = true;

        for(int x = 0; x < m_gridDimensions.x; x++) {
            if(GetGridAt(x, y) == null) {
                b_IsFull = false;
            }
        }

        return b_IsFull;
    }

    private void RemoveLine(int y) {
        Assert.IsTrue(InGrid(0, y));

        for(int x = 0; x < m_gridDimensions.x; x++) {
            Destroy(GetGridAt(x, y));
            SetGridAt(x, y, null);
        }
    }

    private void ReduceLine(int y) {
        Assert.IsTrue(InGrid(0, y - 1));

        for(int x = 0; x < m_gridDimensions.x; x++) {
            GameObject mino = GetGridAt(x, y);

            if(mino != null) {
                SetGridAt(x, y - 1, mino);
                SetGridAt(x, y, null);
                mino.transform.position = new Vector3(mino.transform.position.x, mino.transform.position.y - 1, mino.transform.position.z);
            }
        }
    }

    private void ReduceLinesAbove(int y) {
        Assert.IsTrue(InGrid(0, y + 1));

        for(int j = y + 1; j < m_gridDimensions.y; j++) {
            ReduceLine(j);
        }
    }

} // ~TetrisGrid
