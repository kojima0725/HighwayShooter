using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵車のデータ
/// </summary>
[CreateAssetMenu(fileName = "EnemyCarData", menuName = "ScriptableObjects/CreateEnemyCarData")]
public class EnemyCarData : ScriptableObject
{
    [SerializeField]
    private EnemyCar carPrefab;
    [SerializeField]
    private EnemyCarMovementData movementData;
    [SerializeField]
    private float hp;

    public EnemyCarMovementData MovementData => movementData;

    public float HP => hp;

    /// <summary>
    /// 車を生成する
    /// </summary>
    /// <returns>生成された車</returns>
    public EnemyCar GenerateEnemyCar()
    {
        EnemyCar maked = Instantiate(carPrefab);
        maked.SetEnemyCarData(this);
        return maked;
    }
}
