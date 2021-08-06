using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の動きに関するデータ
/// </summary>
[CreateAssetMenu(fileName = "EnemyCarMovementData", menuName = "ScriptableObjects/CreateEnemyCarMovementData")]
public class EnemyCarMovementData : ScriptableObject
{
    [SerializeField]
    private float addSpeed;
    [SerializeField]
    private float acceleratorPower;

    [SerializeField]
    private float removeSpeed;
    [SerializeField]
    private float brakePower;

    [SerializeField]
    private float stayAngle;
    [SerializeField]
    private float stayLengthMin;
    [SerializeField]
    private float stayLengthMax;

    [SerializeField]
    private float sarchLength;

    /// <summary>
    /// プレイヤーに追いつく時にどれぐらいスピードを上げるか
    /// </summary>
    public float AddSpeedMS => KMath.KmHToMS(addSpeed);
    /// <summary>
    /// アクセル力
    /// </summary>
    public float AcceleraratorPower => KMath.KmHToMS(acceleratorPower);

    /// <summary>
    /// プレイヤーに前から近づく時にどれぐらいスピードを下げるか
    /// </summary>
    public float RemoveSpeedMS => KMath.KmHToMS(removeSpeed);
    /// <summary>
    /// ブレーキ力
    /// </summary>
    public float BrakePower => KMath.KmHToMS(brakePower);

    /// <summary>
    /// プレイヤーの前方のどの角度以内に留まるか
    /// </summary>
    public float StayAngle => stayAngle;

    /// <summary>
    /// プレイヤーの前方に留まる距離の最小値
    /// </summary>
    public float StayLengthMin => stayLengthMin;

    /// <summary>
    /// プレイヤーの前方に留まる距離の最大値
    /// </summary>
    public float StayLengthMax => stayLengthMax;

    /// <summary>
    /// 移動時にどのくらい前方まで探索するか
    /// </summary>
    public float SarchLength => sarchLength;
}
