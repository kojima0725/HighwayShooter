using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵車のスポーンに関するデータ
/// </summary>
[CreateAssetMenu(fileName = "EnemyCarSpawnData", menuName = "ScriptableObjects/CreateEnemyCarSpawnData")]
public class EnemyCarSpawnData : ScriptableObject
{
    [SerializeField]
    private float spawnBackLength;
    [SerializeField]
    private float intervalTime;
    [SerializeField]
    private float firstIntervalTime;
    [SerializeField]
    private float maxCount;
    [SerializeField]
    private float firstAddSpeed;

    /// <summary>
    /// 車をプレイヤーの何メートル後ろにスポーンさせるか
    /// </summary>
    public float SpawnBackLength => spawnBackLength;

    /// <summary>
    /// スポーンのインターバル時間
    /// </summary>
    public float IntervalTime => intervalTime;

    /// <summary>
    /// ステージ開始時の最初の敵が湧くまでの時間
    /// </summary>
    public float FirstIntervalTime => firstIntervalTime;

    /// <summary>
    /// 同時に登場する敵の最大数
    /// </summary>
    public float MaxCount => maxCount;

    /// <summary>
    /// スポーン時にプレイヤーの速度よりどれぐらい速いか
    /// </summary>
    public float FirstAddSpeedMS => MathKoji.KmHToMS(firstAddSpeed);
}