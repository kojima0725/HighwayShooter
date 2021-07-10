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
    private float maxEnemyCount;
}
