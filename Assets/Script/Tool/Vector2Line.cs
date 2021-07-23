
using UnityEngine;

public struct Vector2Line
{
    public Vector2 start;
    public Vector2 end;

    public Vector2Line(Vector2 start, Vector2 end)
    {
        this.start = start;
        this.end = end;
    }

    public Vector2Line(float sx, float sy, float ex, float ey)
    {
        this.start = new Vector2(sx, sy);
        this.end = new Vector2(ex, ey);
    }
}
