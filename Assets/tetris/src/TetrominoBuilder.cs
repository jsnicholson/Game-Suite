using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this class stores information on all tetromino shapes
/// </summary>
public static class TetrominoBuilder {
    private static Vector2[] vectors_i = { new Vector2(-0.24f, 0), new Vector2(-0.08f, 0), new Vector2(0.08f, 0), new Vector2(0.24f, 0) };
    private static Vector2[] vectors_j = { new Vector2(-0.16f, 0.16f), new Vector2(-0.16f, 0), new Vector2(0, 0), new Vector2(0.16f, 0) };
    private static Vector2[] vectors_l = { new Vector2(-0.16f, 0), new Vector2(0, 0), new Vector2(0.16f, 0), new Vector2(0.16f, 0.16f) };
    private static Vector2[] vectors_o = { new Vector2(-0.08f, 0.8f), new Vector2(0.08f, 0.08f), new Vector2(0.08f, -0.08f), new Vector2(-0.08f, -0.08f) };
    private static Vector2[] vectors_s = { new Vector2(-0.16f, 0), new Vector2(0, 0), new Vector2(0, 0.16f), new Vector2(0.16f, 0.16f) };
    private static Vector2[] vectors_t = { new Vector2(-0.16f, 0), new Vector2(0, 0), new Vector2(0.16f, 0), new Vector2(0, 0.16f) };
    private static Vector2[] vectors_z = { new Vector2(-0.16f, 0.16f), new Vector2(0, 0.16f), new Vector2(0, 0), new Vector2(0.16f, 0) };

    /// <summary>
    /// creates and returns an array of all tetromino shapes
    /// </summary>
    /// <returns></returns>
    public static GameObject[] CreateTetrominos(Sprite tileSprite) {
        GameObject[] tetrominoes = new GameObject[7];
        Vector2[][] shapes = { vectors_i, vectors_j, vectors_l, vectors_o, vectors_s, vectors_t, vectors_z };

        for(int i = 0; i < 7; i++) {
            GameObject newTetromino = new GameObject();
            for (int j = 0; j < 4; j++) {
                GameObject tile = new GameObject("Tile (" + j + ")");
                tile.transform.parent = newTetromino.transform;
                SpriteRenderer renderer = tile.AddComponent<SpriteRenderer>();
                renderer.sprite = tileSprite;

                tile.transform.position = new Vector3(shapes[i][j].x, shapes[i][j].y, 0);
            }
            tetrominoes[i] = newTetromino;
        }

        return tetrominoes;
    }
}