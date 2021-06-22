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
    private int lane;
    
    [SerializeField]
    private float limitDistance;


    /// <summary>
    /// ロードチップの長さ
    /// </summary>
    public float Length => length;

    /// <summary>
    /// ロードチップの幅
    /// </summary>
    public float Width => width;

    /// <summary>
    /// 車線数
    /// </summary>
    public int Lane => lane;

    /// <summary>
    /// 道路の生成限界
    /// </summary>
    public float LimitDistance => limitDistance;

}
