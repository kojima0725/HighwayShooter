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
    [SerializeField]
    private float gunRange;
    [SerializeField]
    private float gunAccurate;
    [SerializeField]
    private int gunRPM;
    private float gunSpeed;
    [SerializeField]
    private float gunShootInterval;
    [SerializeField]
    private int gunShootAtOnce;
    

    public EnemyCarMovementData MovementData => movementData;

    public float HP => hp;
    public float GunRange => gunRange;
    public float GunAccurate => gunAccurate;
    public float GunSpeed => gunSpeed;
    public float GunShootInterval => gunShootInterval;
    public int GunShooAtOnce => gunShootAtOnce;

    private void OnValidate()
    {
        gunSpeed = 60.0f / gunRPM;
    }

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
