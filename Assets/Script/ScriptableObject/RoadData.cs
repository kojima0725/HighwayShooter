using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 道路に関するデータ
/// </summary>
[CreateAssetMenu(fileName = "RoadData", menuName = "ScriptableObjects/CreateRoadData")]
public class RoadData : ScriptableObject
{
    
    [SerializeField]
    private float length;
 
    [SerializeField]
    private float width;
    
    [SerializeField]
    private float limitDistance;

    /// <summary>
    /// ロードチップの長さ
    /// </summary>
    public float Length
    {
        get => length;
    }
    /// <summary>
    /// ロードチップの幅
    /// </summary>
    public float Width
    {
        get => width;
    }
    /// <summary>
    /// 道路の生成限界
    /// </summary>
    public float LimitDistance
    {
        get => limitDistance;
    }

}
