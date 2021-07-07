using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの車に関するデータ
/// </summary>
[CreateAssetMenu(fileName = "PlayerCarData", menuName = "ScriptableObjects/PlayerCarData")]
public class PlayerCarData : ScriptableObject
{
    [SerializeField]
    private float rollSensitivity;
    [SerializeField]
    private float handleInputSensitivity;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float accelerationPower;
    [SerializeField]
    private float brakePower;
    [SerializeField]
    private float firstSpeed;


    /// <summary>
    /// 車体を傾ける感度;
    /// </summary>
    public float RollSensitivity => rollSensitivity;

    /// <summary>
    /// ハンドルの入力感度(入力にどれだけ素直に反応するか)
    /// </summary>
    public float HandleInputSensitivity => handleInputSensitivity;

    /// <summary>
    /// 最大速度
    /// </summary>
    public float MaxSpeed => maxSpeed;

    /// <summary>
    /// 加速力
    /// </summary>
    public float AccelerationPower => accelerationPower;

    /// <summary>
    /// 減速力
    /// </summary>
    public float BrakePower => brakePower;

    /// <summary>
    /// 開始時のスピード
    /// </summary>
    public float FirstSpeed => firstSpeed;
}
