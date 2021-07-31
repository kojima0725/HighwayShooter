using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 「車」のインターフェース
/// </summary>
public interface ICar
{
    /// <summary>
    /// 現在走行中のレーン
    /// </summary>
    /// <returns></returns>
    int CurrentLane { get; }

    /// <summary>
    /// 速度
    /// </summary>
    /// <returns></returns>
    float SpeedMS { get; }

}
