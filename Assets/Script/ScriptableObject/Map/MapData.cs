using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// マップ、地形生成に関するデータ
/// </summary>
[CreateAssetMenu(fileName = "MapData", menuName = "ScriptableObjects/CreateMapData")]
public class MapData : ScriptableObject
{
    [SerializeField]
    float chipWidth;
    [SerializeField]
    float chipHeight;
    [SerializeField]
    int splitX;
    [SerializeField]
    int splitY;
    [SerializeField]
    float noiseLoopSize;

    public float ChipWidth => chipWidth;
    public float ChipHeight => chipHeight;

    public int SplitX => splitX;
    public int SplitY => splitY;

    public float NoiseLoopSize => noiseLoopSize;
}
