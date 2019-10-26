using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector2Math {

    public static Vector2 RoundVec2(Vector2 vec) {
        return new Vector2(Mathf.Floor(vec.x), Mathf.Floor(vec.y));
    }

    /// <summary>
    /// rotate point around pivot by angle
    /// </summary>
    /// <param name="point"></param>
    /// <param name="pivot"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static Vector2 RotateAround(Vector2 point, Vector2 pivot, float angle) {
        float s = Mathf.Sin(angle * Mathf.Deg2Rad);
        float c = Mathf.Sin(angle * Mathf.Deg2Rad);

        point -= pivot;

        // rotate point
        float xnew = point.x * c - point.y * s;
        float ynew = point.x * s + point.y * c;

        // translate point back:
        point.x = xnew + pivot.x;
        point.y = ynew + pivot.y;
        return point;
    }

    /// <summary>
    /// returns the vector in the format "x,y"
    /// makes it easier to print vectors in debugging
    /// </summary>
    /// <param name="vec">the vector to print</param>
    /// <returns>string "x,y"</returns>
    public static string VecToString(Vector2 vec) {
        return (vec.x + "," + vec.y);
    }

} // ~VectorUtil
