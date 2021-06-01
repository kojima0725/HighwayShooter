using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 道路の設計に関するデータ
/// </summary>
[CreateAssetMenu(fileName = "RoadDesignDocument", menuName = "ScriptableObjects/CreateRoadDesignDocument")]
public class RoadDesignDocument : ScriptableObject
{
    
    [SerializeField]
    private float straightLengthMax;
    
    [SerializeField]
    private float straightLengthMin;

    [SerializeField]
    private float curveMax;
    
    [SerializeField]
    private float curveMin;

    [SerializeField]
    private float curveStrengthMax;

    [SerializeField]
    private float curveStrengthMin;

    /// <summary>
    /// 直線距離の最大値
    /// </summary>
    public float StraightLengthMax
    {
        get => straightLengthMax;
    }
    /// <summary>
    /// 直線距離の最小値
    /// </summary>
    public float StraightLengthMin
    {
        get => straightLengthMin;
    }
    /// <summary>
    /// カーブの最大値
    /// </summary>
    public float CurveMax
    {
        get => curveMax;
    }
    /// <summary>
    /// カーブの最小値
    /// </summary>
    public float CurveMin
    {
        get => curveMin;
    }
    /// <summary>
    /// カーブのキツさの最大値
    /// </summary>
    public float CurveStrengthMax
    {
        get => curveStrengthMax;
    }
    /// <summary>
    /// カーブのキツさの最小値
    /// </summary>
    public float CurveStrengthMin
    {
        get => curveStrengthMin;
    }
}
