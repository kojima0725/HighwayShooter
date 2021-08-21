using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMaker : MonoBehaviour
{
    [SerializeField]
    private MapChip mapChipPrefab;

    public MapChip MakeChip()
    {
        MapData d = StageDatabase.MapData;
        MapChip maked = Instantiate(mapChipPrefab);
        maked.Init(d.ChipWidth, d.ChipHeight, d.SplitX, d.SplitY, d.NoiseLoopSize, 1000, 1000);
        return maked;
    }
}
