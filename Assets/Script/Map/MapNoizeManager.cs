using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNoizeManager
{
    public Vector2 noiseStartPos;


    public MapNoizeManager()
    {
        noiseStartPos = new Vector2(Random.Range(100.0f, 250.0f), Random.Range(100.0f, 250.0f));
    }

    public void Move(Vector2 move)
    {
        MapData d = StageDatabase.MapData;
        float size = d.NoiseLoopSize / 256;
        float x = (noiseStartPos.x + move.x / size);
        float y = (noiseStartPos.y + move.y / size);
        noiseStartPos = new Vector2(x, y);
    }

    public static Vector2 NoizePos(Vector2 from, float x, float y)
    {
        MapData d = StageDatabase.MapData;
        float size = d.NoiseLoopSize / 256;
        float yoko = (from.x + x / size);
        float tate = (from.y + y / size);
        return new Vector2(yoko, tate);
    }
}
