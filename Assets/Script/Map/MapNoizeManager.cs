using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNoizeManager
{
    public Vector2 noiseStartPos = new Vector2(1000,1000);

    public void Move(Vector2 move)
    {
        MapData d = StageDatabase.MapData;
        float x = (noiseStartPos.x + move.x / d.NoiseLoopSize * 256) % 256;
        float y = (noiseStartPos.y + move.y / d.NoiseLoopSize * 256) % 256;
        noiseStartPos = new Vector2(x, y);
    }
}
