using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// マップ、地形生成に関するデータ
/// </summary>
[CreateAssetMenu(fileName = "MapData", menuName = "ScriptableObjects/CreateMapData")]
public class MapData : ScriptableObject
{
    [SerializeField]
    float chipWidth;
    [SerializeField]
    int splitX;
    [SerializeField]
    float noiseLoopSize;

    [SerializeField]
    float maxHeight;
    [SerializeField, Range(0.0f, 1.0f)]
    float down;

    [SerializeField]
    LengthAndHeight height;

    [Serializable]
    public class LengthAndHeight
    {
        public float length;
        [Range(0.0f, 1.0f)]
        public float height;
    }

    public float ChipWidth => chipWidth;

    public int SplitX => splitX;

    public float NoiseLoopSize => noiseLoopSize;

    public float MaxHeight => maxHeight;

    public float Down => down;

    public LengthAndHeight Height => height;
}
