using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMaker : MonoBehaviour
{
    [SerializeField]
    private MapChip mapChipPrefab;

    public MapChip MakeChip()
    {
        MapChip maked = Instantiate(mapChipPrefab);
        maked.Init();
        return maked;
    }
}
