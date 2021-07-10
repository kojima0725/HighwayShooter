using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 一般車に関するデータ
/// </summary>
[CreateAssetMenu(fileName = "NomalCarData", menuName = "ScriptableObjects/CreateNomalCarData")]
public class NomalCarData : ScriptableObject
{
    [SerializeField]
    private float speedKmH;

    [SerializeField]
    private float carSpawnLength;

    [SerializeField]
    private float betweenMax;

    [SerializeField]
    private float betweenMin;

    /// <summary>
    /// 移動速度(M/秒)
    /// </summary>
    public float SpeedMS => MathKoji.KmHToMS(speedKmH);

    /// <summary>
    /// 次の車のスポーン距離のしきい値
    /// </summary>
    public float CarSpawnLength => carSpawnLength;

    /// <summary>
    /// スポーン時の車間距離の最大値
    /// </summary>
    public float BetweenMax => betweenMax;

    /// <summary>
    /// スポーン時の車間距離の最小値
    /// </summary>
    public float BetweenMin => betweenMin; 
}
