using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 道路に関するデータ
/// </summary>
[CreateAssetMenu(fileName = "RoadData", menuName = "ScriptableObjects/CreateRoadData")]
public class RoadData : ScriptableObject
{
    /// <summary>
    /// ロードチップの長さ
    /// </summary>
    [SerializeField]
    private float length;
    /// <summary>
    /// ロードチップの幅
    /// </summary>
    [SerializeField]
    private float width;
    /// <summary>
    /// 道路の生成限界
    /// </summary>
    [SerializeField]
    private float limitDistance;
    /// <summary>
    /// カーブのキツさの最大値
    /// </summary>
    [SerializeField]
    private float curveMax;

    public float Length
    {
        get => length;
    }

    public float Width
    {
        get => width;
    }

    public float LimitDistance
    {
        get => limitDistance;
    }

    public float CurveMax
    {
        get => curveMax;
    }
}
