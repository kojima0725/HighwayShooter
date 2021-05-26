using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 道路本体、自動で生成され続ける
/// RoadObjectを継承している
/// </summary>
public class RoadChip : MonoBehaviour
{

    /// <summary>
    /// 自身の最後端の位置
    /// </summary>
    [SerializeField]
    private Transform end;

    /// <summary>
    /// マップチップ(自分自身のプレハブ)
    /// </summary>
    [SerializeField]
    GameObject chip;

    bool sonIsMaked = false;

    /// <summary>
    /// 道の終端の位置を渡す
    /// </summary>
    /// <returns></returns>
    public Transform GetEnd()
    {
        return end;
    }
}
